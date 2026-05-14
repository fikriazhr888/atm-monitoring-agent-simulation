using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmMonitoring.Agent.Domain.Models;

public class AtmStatus
{
    public string StatusId { get; set; } = string.Empty;
    public string TerminalId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string NetworkStatus { get; set; } = string.Empty;
    public string CashStatus { get; set; } = string.Empty;
    public string PrinterStatus { get; set; } = string.Empty;
    public string CardReaderStatus { get; set; } = string.Empty;
    public string OverallStatus { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
