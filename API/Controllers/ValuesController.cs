using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using MyStateless;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly TelemetryClient _telemetryClient;

        public ValuesController(TelemetryConfiguration configuration)
        {
            // To keep it simple this example uses the TelemetryClient directly to log and uses AspNET Core Dependency injection. 
            // The TelemetryConfiguration is received and used to create a new client.
            // Note: that a TelemetryClient injected directly without additional steps will not contain the correct configuration.
            _telemetryClient = new TelemetryClient(configuration);
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            _telemetryClient.TrackTrace("Trace message from the API");
            var proxyFactory = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());

            var service = proxyFactory.CreateServiceProxy<IMyStateLess>(new Uri("fabric:/AI_Logging/MyStateless"));
            
            return new[] { DateTimeOffset.UtcNow.ToString("HH:mm:ss.ffffff"), await service.Test() };
        }
    }
}
