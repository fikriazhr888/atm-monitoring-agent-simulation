using AtmMonitoring.Agent.Domain.Models;
using AtmMonitoring.Agent.Infrastructure.Storage;
using FluentAssertions;

namespace AtmMonitoring.Tests.Storage;

public class JsonPendingStorageServiceTests
{
    [Fact]
    public async Task SavePendingAsync_Should_Save_Status()
    {
        // Arrange
        var service = new JsonPendingStorageService();

        var status = new AtmStatus
        {
            StatusId = Guid.NewGuid().ToString(),
            TerminalId = "ATM-0001",
            Timestamp = DateTime.Now,
            NetworkStatus = "OFFLINE",
            CashStatus = "NORMAL",
            PrinterStatus = "NORMAL",
            CardReaderStatus = "NORMAL",
            OverallStatus = "OFFLINE",
            Message = "Test pending"
        };

        // Act
        await service.SavePendingAsync(status);

        var pending = await service.GetPendingAsync();

        // Assert
        pending.Should().Contain(x => x.StatusId == status.StatusId);
    }

    [Fact]
    public async Task RemovePendingAsync_Should_Remove_Status()
    {
        var service = new JsonPendingStorageService();

        var status = new AtmStatus
        {
            StatusId = Guid.NewGuid().ToString(),
            TerminalId = "ATM-0001",
            Timestamp = DateTime.Now,
            NetworkStatus = "OFFLINE",
            CashStatus = "NORMAL",
            PrinterStatus = "NORMAL",
            CardReaderStatus = "NORMAL",
            OverallStatus = "OFFLINE",
            Message = "Test remove"
        };

        await service.SavePendingAsync(status);
        await service.RemovePendingAsync(status.StatusId);

        var pending = await service.GetPendingAsync();

        pending.Should().NotContain(x => x.StatusId == status.StatusId);
    }
}