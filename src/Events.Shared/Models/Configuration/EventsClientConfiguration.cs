using System.Collections.Generic;

namespace Events.Shared.Models.Configuration
{
    public class EventsClientConfiguration
    {
        public const string ClientConfiguration = "Events.Client";
        public EventsClientAuthenticationConfiguration AzureAD { get; set; }
    }
}
