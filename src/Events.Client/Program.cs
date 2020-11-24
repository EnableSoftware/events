using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Events.Client.Services.Api;
using Events.Client.State;

namespace Events.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(builder);
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddHttpClient("Events.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Events.Server"));

            var defaultAccessTokenScope = builder.Configuration.GetValue<string>("AzureAD:DefaultAccessTokenScope");
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAD", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(defaultAccessTokenScope);
            });

            builder.Services.AddScoped<CategoryApiService>();
            builder.Services.AddScoped<EventApiService>();
            builder.Services.AddScoped<UserApiService>();
            builder.Services.AddSingleton<UserState>();
        }
    }
}
