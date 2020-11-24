using System.Collections.Generic;

namespace Events.Shared.Models.Configuration
{
    public class EventsClientAuthenticationConfiguration
    {
        public const string ClientConfiguration = "AzureAD";

        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string DefaultAccessTokenScope { get; set; }
        public bool ValidateAuthority { get; set; }
    }
}
