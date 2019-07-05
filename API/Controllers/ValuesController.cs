using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ValuesController()
        {
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var proxyFactory = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());

            var service = proxyFactory.CreateServiceProxy<IMyStateLess>(new Uri("fabric:/AI_Logging/MyStateless"));
            
            return new[] { DateTimeOffset.UtcNow.ToString("HH:mm:ss.ffffff"), await service.Test() };
        }
    }
}
