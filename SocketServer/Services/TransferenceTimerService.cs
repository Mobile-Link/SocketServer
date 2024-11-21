using System.Timers;

namespace SocketServer.Services;

public class TransferenceTimerService(IConfiguration configuration, IServiceProvider serviceProvider)
{
    private readonly Dictionary<int, System.Timers.Timer?> _lastChunkReceivedByTransaction = new Dictionary<int, System.Timers.Timer?>();
    public void ChunkReceived(int idTransference)
    {
        if (_lastChunkReceivedByTransaction.ContainsKey(idTransference))
        {
            _lastChunkReceivedByTransaction[idTransference]?.Stop();
            _lastChunkReceivedByTransaction[idTransference]?.Dispose();
        }

        var minutes = int.Parse(configuration["TransferenceChunkTimeoutMinutes"] ?? "30"); 
        var timer = new System.Timers.Timer(new TimeSpan(0, minutes, 0));
        timer.AutoReset = false;
        timer.Elapsed += (object? o, ElapsedEventArgs e) =>
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var transferService = scope.ServiceProvider.GetRequiredService<TransferService>();
                transferService.TimeoutTransference(idTransference);
                RemoveMonitor(idTransference);
            }
        };
        timer.Start();
        _lastChunkReceivedByTransaction[idTransference] = timer;
    }

    public void RemoveMonitor(int idTransference)
    {
        _lastChunkReceivedByTransaction[idTransference]?.Stop();
        _lastChunkReceivedByTransaction[idTransference]?.Dispose();
        _lastChunkReceivedByTransaction.Remove(idTransference);
    }
}