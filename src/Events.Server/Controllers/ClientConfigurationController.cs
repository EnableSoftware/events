using System;
using Events.Shared.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Events.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientConfigurationController : ControllerBase
    {
        private readonly EventsClientConfiguration _clientConfiguration;

        public ClientConfigurationController(IOptions<EventsClientConfiguration> clientConfiguration)
        {
            _clientConfiguration = clientConfiguration.Value ?? throw new ArgumentNullException(nameof(clientConfiguration));
        }


        [HttpGet]
        public ActionResult GetConfiguration()
        {
            return Ok(_clientConfiguration);
        }
    }
}