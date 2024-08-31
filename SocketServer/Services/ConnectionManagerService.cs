using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace SocketServer.Services;

public class ConnectionManagerService
{
    private readonly Dictionary<string, string> _activeConnections = new Dictionary<string, string>();
    private readonly IHubContext<FileTransferHub.FileTransferHub> _hubContext;
    
    public ConnectionManagerService(IHubContext<FileTransferHub.FileTransferHub> hubContext)
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
}