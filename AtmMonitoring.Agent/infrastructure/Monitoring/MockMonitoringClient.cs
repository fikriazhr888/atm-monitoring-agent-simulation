using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Domain.Enums;
using AtmMonitoring.Agent.Domain.Models;

namespace AtmMonitoring.Agent.Infrastructure.Monitoring;

public class MockMonitoringClient : IMonitoringClient
{
    private readonly Random _random = new();


    public async Task<SendResult> SendStatusAsync(AtmStatus status,bool isRetry = false)
    {
        await Task.Delay(1000);

        if (!isRetry &&
            (status.OverallStatus == "OFFLINE" ||
             status.NetworkStatus == "OFFLINE"))
        {
            return new SendResult
            {
                Status = SendStatus.Failed,
                Message = "Failed because ATM offline"
            };
        }

        //retry pending
        var networkAvailable = _random.Next(1, 5) != 1;

        if (!networkAvailable)
        {
            return new SendResult
            {
                Status = SendStatus.Failed,
                Message = "Network unavailable"
            };
        }

        return new SendResult
        {
            Status = SendStatus.Success,
            Message = "Status sent successfully"
        };
    }

    //public async Task<SendResult> SendStatusAsync(AtmStatus status)
    //{
    //    await Task.Delay(1000);

    //    var networkAvailable = _random.Next(1, 5) != 1;

    //    if (!networkAvailable)
    //    {
    //        return new SendResult
    //        {
    //            Status = SendStatus.Failed,
    //            Message = "Network unavailable"
    //        };
    //    }

    //    return new SendResult
    //    {
    //        Status = SendStatus.Success,
    //        Message = "Status sent successfully"
    //    };
    //}

    //public async Task<SendResult> SendStatusAsync(AtmStatus status)
    //{
    //    await Task.Delay(1000);

    //    if (status.OverallStatus == "OFFLINE" ||
    //        status.NetworkStatus == "OFFLINE")
    //    {
    //        return new SendResult
    //        {
    //            Status = SendStatus.Failed,
    //            Message = "Failed to send status because ATM is offline"
    //        };
    //    }

    //    var randomFailure = _random.Next(1, 10);

    //    if (randomFailure == 1)
    //    {
    //        return new SendResult
    //        {
    //            Status = SendStatus.Failed,
    //            Message = "Random network failure occurred"
    //        };
    //    }

    //    return new SendResult
    //    {
    //        Status = SendStatus.Success,
    //        Message = "Status sent successfully"
    //    };
    //}
}