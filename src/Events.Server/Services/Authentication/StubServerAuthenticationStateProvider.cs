using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Events.Server.Services.Authentication
{
    public class StubServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new System.Security.Claims.ClaimsPrincipal()));
        }
    }
}
