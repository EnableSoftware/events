using Events.Shared.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Server.Extensions.StartupExtensions
{
    public static class ClientConfigurationExtensions
    {
        public static void AddEventsClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventsClientConfiguration>(configuration.GetSection(EventsClientConfiguration.ClientConfiguration));
        }
    }
}
