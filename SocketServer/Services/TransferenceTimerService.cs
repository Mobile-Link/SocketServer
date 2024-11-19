using System.Timers;

namespace SocketServer.Services;

public class TransferenceTimerService(IConfiguration configuration)
{
    private readonly Dictionary<int, System.Timers.Timer?> _lastChunkReceivedByTransaction = new Dictionary<int, System.Timers.Timer?>();
    private Action<int>? _timeoutFunction;
    public void ChunkReceived(int idTransference, Action<int>? timeoutFunction = null)
    {
        if (timeoutFunction != null && _timeoutFunction == null)
        {
            _timeoutFunction = timeoutFunction;
        }

        if (!_lastChunkReceivedByTransaction.ContainsKey(idTransference))
        {
            var minutes = int.Parse(configuration["TransferenceChunkTimeoutMinutes"] ?? "30"); 
            _lastChunkReceivedByTransaction[idTransference] = new System.Timers.Timer(new TimeSpan(0, minutes, 0));    
            _lastChunkReceivedByTransaction[idTransference].Elapsed += (object? o, ElapsedEventArgs e) =>
            {
                _timeoutFunction?.Invoke(idTransference);
            };
        }
        _lastChunkReceivedByTransaction[idTransference]?.Stop();
        _lastChunkReceivedByTransaction[idTransference]?.Start();
    }

    public void RemoveMonitor(int idTransference)
    {
        _lastChunkReceivedByTransaction[idTransference]?.Stop();
        _lastChunkReceivedByTransaction.Remove(idTransference);
    }
}