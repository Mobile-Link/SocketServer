using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using SocketServer.Services;

namespace SocketServer.ChatHub;

public class ChatHub : Hub
{
    private readonly ConnectionManagerService _connectionManagerService;
    
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
}