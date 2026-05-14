using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Domain.Models;

namespace AtmMonitoring.Agent.Device.Providers;

public class MockAtmStatusProvider : IAtmStatusProvider
{
    private readonly Random _random = new();

    public async Task<AtmStatus> GetStatusAsync()
    {
        await Task.Delay(500);

        var overallStatuses = new[]
        {
            "HEALTHY",
            "DEGRADED",
            "OFFLINE"
        };

        var selectedStatus = overallStatuses[_random.Next(overallStatuses.Length)];
        //var selectedStatus = "OFFLINE";
        //var selectedStatus = "HEALTHY";

        return new AtmStatus
        {
            StatusId = $"STAT-{Guid.NewGuid()}",
            TerminalId = "ATM-0001",
            Timestamp = DateTime.Now,
            NetworkStatus = selectedStatus == "OFFLINE" ? "OFFLINE" : "ONLINE",
            CashStatus = "NORMAL",
            PrinterStatus = "NORMAL",
            CardReaderStatus = "NORMAL",

            OverallStatus = selectedStatus,
            Message = selectedStatus switch
            {
                "HEALTHY" => "ATM operating normally",
                "DEGRADED" => "ATM partially degraded",
                "OFFLINE" => "ATM network offline",
                _ => "Unknown status"
            }
        };
    }
}