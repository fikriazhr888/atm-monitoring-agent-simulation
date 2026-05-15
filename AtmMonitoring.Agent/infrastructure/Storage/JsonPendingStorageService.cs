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
        //pada proses ini akan membuat folder bernama storage untuk menampung data pending json.
        if (!string.IsNullOrEmpty(directory))
        {
            //seaindainya folder sudah ada, CreateDirectory tidak akan membuat folder lagi, do nothing
            Directory.CreateDirectory(directory);
        }
    }

    public async Task SavePendingAsync(AtmStatus status)
    {
        var pendingStatuses = await GetPendingAsync();


        //disini saya menggunnakan statusId untuk menghindari duplikat data yang akan di save
        
        var exists = pendingStatuses.Any(x => x.StatusId == status.StatusId);

        //jika true, maka akan ke return.
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
        //ini proses untuk menghapus remove pending
        //dengan cara get data kecuali data yang sudah sukses, lalu di rewrite
        await File.WriteAllTextAsync(_filePath, json);
    }
}