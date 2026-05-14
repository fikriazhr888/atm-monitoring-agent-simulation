using AtmMonitoring.Agent.Domain.Models;

namespace AtmMonitoring.Agent.Application.Interfaces;

public interface IMonitoringClient
{
    Task<SendResult> SendStatusAsync(AtmStatus status,bool isRetry = false);
}