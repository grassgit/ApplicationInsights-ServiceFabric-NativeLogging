using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ApplicationInsights.ServiceFabric.Module;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.ServiceFabric.Services.Runtime;

namespace MyStateless
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("MyStatelessType", context =>
                {
                    // Pull the configuration from the config package.
                    var cfg = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
                    var appInsightsID = cfg.Settings.Sections["Diagnostics"].Parameters["ApplicationInsightsID"].Value;

                    // Create the telemetry configuration
                    var config = TelemetryConfiguration.CreateDefault();
                    config.InstrumentationKey = appInsightsID;

                    if (System.Diagnostics.Debugger.IsAttached)
                        config.TelemetryChannel.DeveloperMode = true;

                    // Add initializes for the basics
                    config.TelemetryInitializers.Add(new CodePackageVersionTelemetryInitializer());
                    config.TelemetryInitializers.Add(FabricTelemetryInitializerExtension.CreateFabricTelemetryInitializer(context));

                    new AzureInstanceMetadataTelemetryModule().Initialize(config);
                    new ServiceRemotingDependencyTrackingTelemetryModule().Initialize(config);
                    new ServiceRemotingRequestTrackingTelemetryModule().Initialize(config);

                    // Pass the configuration for use
                    // Note this could benefit from Dependency Injection but this is not setup by default so to keep the example 
                    // this is not used.
                    return new MyStateless(context, new Microsoft.ApplicationInsights.TelemetryClient(config));
                }).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(MyStateless).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
