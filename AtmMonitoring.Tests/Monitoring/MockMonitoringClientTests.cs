using AtmMonitoring.Agent.Domain.Enums;
using AtmMonitoring.Agent.Domain.Models;
using AtmMonitoring.Agent.Infrastructure.Monitoring;
using FluentAssertions;

namespace AtmMonitoring.Tests.Monitoring;

public class MockMonitoringClientTests
{
    [Fact]
    public async Task SendStatusAsync_Should_Return_Result()
    {
        var client = new MockMonitoringClient();

        var status = new AtmStatus
        {
            StatusId = "TEST-001",
            TerminalId = "ATM-0001",
            Timestamp = DateTime.Now,
            NetworkStatus = "ONLINE",
            CashStatus = "NORMAL",
            PrinterStatus = "NORMAL",
            CardReaderStatus = "NORMAL",
            OverallStatus = "HEALTHY",
            Message = "ATM operating normally"
        };

        var result = await client.SendStatusAsync(status);

        result.Should().NotBeNull();

        result.Status.Should().BeOneOf(
            SendStatus.Success,
            SendStatus.Failed);
    }
}