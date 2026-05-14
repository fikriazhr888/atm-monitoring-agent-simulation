using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Domain.Enums;

namespace AtmMonitoring.Agent;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAtmStatusProvider _atmStatusProvider;
    private readonly IMonitoringClient _monitoringClient;
    private readonly IPendingStorageService _pendingStorageService;

    public Worker(ILogger<Worker> logger, IAtmStatusProvider atmStatusProvider, IMonitoringClient monitoringClient, IPendingStorageService pendingStorageService)
    {
        _logger = logger;
        _atmStatusProvider = atmStatusProvider;
        _monitoringClient = monitoringClient;
        _pendingStorageService = pendingStorageService;
    }

    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ATM Monitoring Agent Started");

        await RetryPendingStatusesAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {


                var status = await _atmStatusProvider.GetStatusAsync();

                var sendResult = await _monitoringClient.SendStatusAsync(status);

                if (sendResult.Status == SendStatus.Success)
                {
                    _logger.LogInformation(
                        "Send Success: {Message}",
                        sendResult.Message);
                }
                else
                {
                    _logger.LogWarning(
                        "Send Failed: {Message}",
                        sendResult.Message);

                    await _pendingStorageService.SavePendingAsync(status);

                    _logger.LogWarning(
                        "Status saved to pending storage: {StatusId}",
                        status.StatusId);
                }

                _logger.LogInformation(
                    """
                    =========================================================
                    Status Collected:
                    StatusId: {StatusId}
                    TerminalId: {TerminalId}
                    OverallStatus: {OverallStatus}
                    Message: {Message}
                    =========================================================
                    """,
                    status.StatusId,
                    status.TerminalId,
                    status.OverallStatus,
                    status.Message
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to collect ATM status");
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
    private async Task RetryPendingStatusesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retry pending statuses started");

        var pendingStatuses = await _pendingStorageService.GetPendingAsync();

        if (!pendingStatuses.Any())
        {
            _logger.LogInformation("No pending statuses found");

            return;
        }

        foreach (var pendingStatus in pendingStatuses)
        {
            var result = await _monitoringClient.SendStatusAsync(pendingStatus,true);

            if (result.Status == SendStatus.Success)
            {
                await _pendingStorageService.RemovePendingAsync(
                    pendingStatus.StatusId);

                _logger.LogInformation(
                    "Retry pending success: {StatusId}",
                    pendingStatus.StatusId);
            }
            else
            {
                _logger.LogWarning(
                    "Retry pending failed: {StatusId}",
                    pendingStatus.StatusId);
            }
            await Task.Delay(5000, cancellationToken);
        }
    }
}