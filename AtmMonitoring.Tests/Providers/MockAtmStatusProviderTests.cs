using AtmMonitoring.Agent.Device.Providers;
using FluentAssertions;

namespace AtmMonitoring.Tests.Providers;

public class MockAtmStatusProviderTests
{
    [Fact]
    public async Task GetStatusAsync_Should_Return_Valid_Status()
    {
        var provider = new MockAtmStatusProvider();

        var result = await provider.GetStatusAsync();

        result.Should().NotBeNull();
        result.StatusId.Should().NotBeNullOrWhiteSpace();
        result.TerminalId.Should().Be("ATM-0001");
        result.OverallStatus.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetStatusAsync_Should_Generate_Unique_StatusId()
    {
        var provider = new MockAtmStatusProvider();

        var firstStatus = await provider.GetStatusAsync();

        var secondStatus = await provider.GetStatusAsync();

        firstStatus.StatusId.Should().NotBe(secondStatus.StatusId);
    }


}