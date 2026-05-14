using AtmMonitoring.Agent.Domain.Enums;

namespace AtmMonitoring.Agent.Domain.Models;

public class SendResult
{
    public SendStatus Status { get; set; }
    public string? Message { get; set; }
}