using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
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
        private readonly TelemetryClient _client;

        public MyStateless(StatelessServiceContext context, TelemetryClient client)
            : base(context)
        {
            _client = client;
        }

        public Task<string> Test()
        {
            _client.TrackTrace("Message from stateless");

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
