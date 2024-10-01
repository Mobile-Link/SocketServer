using Microsoft.AspNetCore.SignalR;

namespace SocketServer.Services;

public class ConnectionManagerService
{
    private readonly Dictionary<string, string> _activeConnections = new Dictionary<string, string>();
    private readonly IHubContext<TransferHub.TransferHub> _hubContext;
    
    public ConnectionManagerService(IHubContext<TransferHub.TransferHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public void AddUsers(string connectionId)
    {
        if (!_activeConnections.ContainsKey(connectionId))
        {
            _activeConnections[connectionId] = connectionId;
            
            _hubContext.Clients.All.SendAsync("UserConnected", connectionId);
        }
    }
    
    public void RemoveUser(string connectionId)
    {
        if (_activeConnections.ContainsKey(connectionId))
        {
            _activeConnections.Remove(connectionId);
            
            _hubContext.Clients.All.SendAsync("UserDisconnected", connectionId);
        }
    }
    
    public string GetConnectionId(string connectionId)
    {
        return connectionId;
    }
    
    public bool IsUserConnected(string connectionId)
    {
        return _activeConnections.ContainsKey(connectionId);
    }
    
    public Dictionary<string, string> GetActiveConnections()
    {
        return _activeConnections;
    }
}