using Events.Client.State;
using Events.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Events.Client.Services.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly UserState _userState;

        public CustomAuthenticationStateProvider(HttpClient httpClient, UserState userState)
        {
            _httpClient = httpClient;
            _userState = userState;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal user;

            // TODO gracefully handle non authorised users, prevent post to https://login.microsoftonline.com, cors issue
            try
            {
                var result = await _httpClient.GetJsonAsync<UserModel>("api/account/get-self");
                _userState.SetUserInfo(result);
                var identity = new ClaimsIdentity(new[]
                {
                   new Claim(ClaimTypes.Name, result.Name),
                   new Claim(ClaimTypes.Upn, result.Email),
                }, "AzureADAuth");

                if (!string.IsNullOrEmpty(result.Role))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, result.Role));
                }

                user = new ClaimsPrincipal(identity);
            }
            catch (HttpRequestException)
            {
                user = new ClaimsPrincipal();
            }

            return new AuthenticationState(user);
        }
    }
}
