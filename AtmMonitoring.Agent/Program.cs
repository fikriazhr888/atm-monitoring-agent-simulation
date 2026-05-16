using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Device.Providers;
using AtmMonitoring.Agent.Infrastructure.Monitoring;
using AtmMonitoring.Agent.Infrastructure.Storage;

namespace AtmMonitoring.Agent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);


            //Register 
            builder.Services.AddSingleton<IAtmStatusProvider, MockAtmStatusProvider>();
            builder.Services.AddSingleton<IPendingStorageService, SqlitePendingStorageService>();
            builder.Services.AddSingleton<IMonitoringClient, MockMonitoringClient>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}