using AtmMonitoring.Agent.Domain.Models;

namespace AtmMonitoring.Agent.Application.Interfaces;

public interface IPendingStorageService
{
    Task SavePendingAsync(AtmStatus status);
    Task<List<AtmStatus>> GetPendingAsync();
    Task RemovePendingAsync(string statusId);
}
