using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace MyStateless
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class MyStateless : StatelessService, IMyStateLess
    {
        public MyStateless(StatelessServiceContext context)
            : base(context)
        {
        }

        public Task<string> Test()
        {
            return Task.FromResult(DateTimeOffset.UtcNow.ToString("HH:mm:ss.ffffff"));
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => new FabricTransportServiceRemotingListener(context, this))
            };
        }
    }
}
