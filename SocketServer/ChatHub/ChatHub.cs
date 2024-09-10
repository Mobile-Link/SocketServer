using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using SocketServer.Services;

namespace SocketServer.ChatHub;

public class ChatHub : Hub
{
    private readonly ConnectionManagerService _connectionManagerService;
    private readonly Dictionary<string, List<string>> _transferringFiles = new Dictionary<string, List<string>>();
    
    public ChatHub(ConnectionManagerService connectionManagerService)
    {
        _connectionManagerService = connectionManagerService;
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);

        Console.WriteLine($"User {user} enviou a mensagem: {message}");
    }
    
    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        
        Console.WriteLine($"User {connectionId} conectado");
        _connectionManagerService.AddUsers(connectionId);
        
        return base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        
        Console.WriteLine($"User {connectionId} desconectado");
        _connectionManagerService.RemoveUser(connectionId);
        
        return base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendFile(string userId, string fileName, byte[] chunk)
    {
        if (!_transferringFiles.ContainsKey(userId))
        {
            _transferringFiles[userId] = new List<string>();
        }
        if (!_transferringFiles[userId].Contains(fileName))
        {
            _transferringFiles[userId].Add(fileName);
        }
            
        await Clients.User(userId).SendAsync("ReceiveFileChunk", fileName, chunk);
        
        Console.WriteLine($"User {userId} enviou o arquivo {fileName}");
    }
}