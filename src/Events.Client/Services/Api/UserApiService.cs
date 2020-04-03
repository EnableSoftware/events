using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Events.Client.Services.Api
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IEnumerable<UserModel>> Get()
        {
            return await _httpClient.GetJsonAsync<IEnumerable<UserModel>>("api/user");
        }

        public async Task<UserModel> Get(int id)
        {
            return await _httpClient.GetJsonAsync<UserModel>($"api/user/{id}");
        }

        public async Task<int> Post(UserModel model)
        {
            return await _httpClient.PostJsonAsync<int>("api/user", model);
        }

        public async Task Put(int id, UserModel model)
        {
            await _httpClient.PutJsonAsync($"api/user/{id}", model);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/user/{id}");
        }

        public async Task<Uri> GetProfileImageUrl()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var email = authenticationState.User.FindFirst(ClaimTypes.Upn);

            if (email != null)
            {
                #pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
                using (var md5 = MD5.Create())
                #pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
                {
                    var inputBytes = Encoding.ASCII.GetBytes(email.Value);

                    var hashBytes = md5.ComputeHash(inputBytes);

                    var sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(value: hashBytes[i].ToString("X2"));
                    }

                    var hash = sb.ToString().ToLower();

                    return new Uri($"https://www.gravatar.com/avatar/{hash}");
                }
            }

            // TODO better default image
            return new Uri("https://www.gravatar.com/avatar/");
        }
    }
}
