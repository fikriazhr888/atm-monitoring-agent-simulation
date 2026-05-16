using AtmMonitoring.Agent.Application.Interfaces;
using AtmMonitoring.Agent.Domain.Models;
using Microsoft.Data.Sqlite;

namespace AtmMonitoring.Agent.Infrastructure.Storage;

public class SqlitePendingStorageService : IPendingStorageService
{
    private readonly string _connectionString ="Data Source=storage/pending-status.db";

    public SqlitePendingStorageService()
    {
        InitializeDatabase();
    }

    //database dan table dibuat otomatis
    //saat service pertama kali dijalankan.
    private void InitializeDatabase()
    {
        Directory.CreateDirectory("storage");

        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();

        // StatusId dijadikan PK ,untuk mencegah duplicate pending data.
        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS PendingStatuses
            (

                StatusId TEXT PRIMARY KEY,
                TerminalId TEXT,
                Timestamp TEXT,
                NetworkStatus TEXT,
                CashStatus TEXT,
                PrinterStatus TEXT,
                CardReaderStatus TEXT,
                OverallStatus TEXT,
                Message TEXT
            );
        ";

        command.ExecuteNonQuery();
    }

    public async Task SavePendingAsync(AtmStatus status)
    {
        using var connection = new SqliteConnection(_connectionString);

        await connection.OpenAsync();

        var checkCommand = connection.CreateCommand();

        // duplicate check dilakukan sebelum insert agar pending status tidak tersimpan berulang
        checkCommand.CommandText =
        @"
            SELECT COUNT(*)
            FROM PendingStatuses
            WHERE StatusId = $statusId
        ";

        checkCommand.Parameters.AddWithValue("$statusId",status.StatusId);

        var exists =Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;

        if (exists)
        {
            return;
        }
        var command = connection.CreateCommand();

        //status gagal akan disimpan ke SQLite, agar dapat di-retry pada cycle berikutnya
        command.CommandText =
        @"
            INSERT INTO PendingStatuses(
                StatusId,
                TerminalId,
                Timestamp,
                NetworkStatus,
                CashStatus,
                PrinterStatus,
                CardReaderStatus,
                OverallStatus,
                Message
            )
            VALUES(
                $statusId,
                $terminalId,
                $timestamp,
                $networkStatus,
                $cashStatus,
                $printerStatus,
                $cardReaderStatus,
                $overallStatus,
                $message
            );
        ";

        command.Parameters.AddWithValue("$statusId", status.StatusId);
        command.Parameters.AddWithValue("$terminalId", status.TerminalId);
        command.Parameters.AddWithValue("$timestamp", status.Timestamp.ToString("O"));
        command.Parameters.AddWithValue("$networkStatus", status.NetworkStatus);
        command.Parameters.AddWithValue("$cashStatus", status.CashStatus);
        command.Parameters.AddWithValue("$printerStatus", status.PrinterStatus);
        command.Parameters.AddWithValue("$cardReaderStatus", status.CardReaderStatus);
        command.Parameters.AddWithValue("$overallStatus", status.OverallStatus);
        command.Parameters.AddWithValue("$message", status.Message);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<AtmStatus>> GetPendingAsync()
    {
        var result = new List<AtmStatus>();
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        var command = connection.CreateCommand();


        command.CommandText =
        @"
            SELECT *
            FROM PendingStatuses
        ";

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new AtmStatus
            {
                StatusId = reader.GetString(0),
                TerminalId = reader.GetString(1),
                Timestamp = DateTime.Parse(reader.GetString(2)),
                NetworkStatus = reader.GetString(3),
                CashStatus = reader.GetString(4),
                PrinterStatus = reader.GetString(5),
                CardReaderStatus = reader.GetString(6),
                OverallStatus = reader.GetString(7),
                Message = reader.GetString(8)
            });
        }



        return result;
    }

    //pending status dihapus setelah retry berhasil
    public async Task RemovePendingAsync(string statusId)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText =
        @"
            DELETE FROM PendingStatuses
            WHERE StatusId = $statusId
        ";

        command.Parameters.AddWithValue("$statusId",statusId);


        await command.ExecuteNonQueryAsync();
    }
}