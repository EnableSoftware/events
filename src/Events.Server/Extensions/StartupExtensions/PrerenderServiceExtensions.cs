using Events.Server.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Events.Server.Extensions.StartupExtensions
{
    public static class PrerenderServiceExtensions
    {
        public static void AddCustomPrerenderServices(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, StubServerAuthenticationStateProvider>();
            services.AddScoped<HttpClient>();
        }
    }
}
