using System.Text.Json;
using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Domain.Models;

namespace AtmMonitoring.Agent.Infrastructure.Storage;

public class JsonPendingStorageService : IPendingStorageService
{
    private readonly string _filePath = "storage/pending-status.json";

    public JsonPendingStorageService()
    {
        var directory = Path.GetDirectoryName(_filePath);

        Console.WriteLine($"Creating directory: {directory}");

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public async Task SavePendingAsync(AtmStatus status)
    {
        var pendingStatuses = await GetPendingAsync();

        var exists = pendingStatuses.Any(x => x.StatusId == status.StatusId);

        if (exists)
        {
            return;
        }

        pendingStatuses.Add(status);

        var json = JsonSerializer.Serialize(pendingStatuses,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<List<AtmStatus>> GetPendingAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<AtmStatus>();
        }

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<AtmStatus>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<AtmStatus>>(json) ?? new List<AtmStatus>();
        }
        catch
        {
            return new List<AtmStatus>();
        }
    }

    public async Task RemovePendingAsync(string statusId)
    {
        var pendingStatuses = await GetPendingAsync();

        var filtered = pendingStatuses
            .Where(x => x.StatusId != statusId)
            .ToList();

        var json = JsonSerializer.Serialize(
            filtered,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(_filePath, json);
    }
}