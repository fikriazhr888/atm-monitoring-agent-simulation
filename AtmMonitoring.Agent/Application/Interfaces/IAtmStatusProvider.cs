using AtmMonitoring.Agent.Domain.Models;

namespace AtmMonitoring.Agent.Application.Interfaces;

public interface IAtmStatusProvider
{
    Task<AtmStatus> GetStatusAsync();
}
