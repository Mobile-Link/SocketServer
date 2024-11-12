namespace SocketServer.Services;

public class TransferenceMonitorService(IConfiguration configuration) : BackgroundService
{
    private readonly Dictionary<int, DateTime> _lastChunkReceivedByTransaction = new Dictionary<int, DateTime>();
    private Action<int>? _timeoutFunction;
    public void ChunkReceived(int idTransference, Action<int>? timeoutFunction = null)
    {
        if (timeoutFunction != null && _timeoutFunction == null)
        {
            _timeoutFunction = timeoutFunction;
        }
        _lastChunkReceivedByTransaction[idTransference] = DateTime.Now;
    }

    public void RemoveMonitor(int idTransference)
    {
        _lastChunkReceivedByTransaction.Remove(idTransference);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var kvp in _lastChunkReceivedByTransaction)
            {
                int idTransference = kvp.Key;
                var minutes = int.Parse(configuration["TransferenceChunkTimeoutMinutes"] ?? "30"); 
                if (DateTime.Now - kvp.Value > TimeSpan.FromMinutes(minutes))
                {
                    _lastChunkReceivedByTransaction.Remove(idTransference);
                    _timeoutFunction?.Invoke(idTransference);
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}