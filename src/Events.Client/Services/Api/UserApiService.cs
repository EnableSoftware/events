using Events.Shared.Hashes.Md5;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserModel>>("api/user");
        }

        public async Task<UserModel> Get(int id)
        {
            return await _httpClient.GetFromJsonAsync<UserModel>($"api/user/{id}");
        }

        public async Task<int> Post(UserModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", model);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task Put(int id, UserModel model)
        {
            await _httpClient.PutAsJsonAsync($"api/user/{id}", model);
        }

        public async Task Delete(int id)
        {
            var uri = new Uri($"api/user/{id}");
            await _httpClient.DeleteAsync(uri);
        }

        public async Task<UserModel> GetSelf()
        {
            return await _httpClient.GetFromJsonAsync<UserModel>("api/account/get-self");
        }

        public async Task<Uri> GetProfileImageUrl()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var email = authenticationState.User.FindFirst("preferred_username");

            if (email != null && !string.IsNullOrEmpty(email.Value))
            {
                var md5 = new Md5Hash
                {
                    Value = email.Value
                };

                var hash = md5.FingerPrint.ToString().ToLower();

                return new Uri($"https://www.gravatar.com/avatar/{hash}");

            }

            return new Uri("https://www.gravatar.com/avatar/");
        }
    }
}
