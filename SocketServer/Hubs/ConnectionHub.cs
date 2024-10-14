using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Hubs;

public class ConnectionHub(DeviceService deviceService) : Hub
{

    public async Task AddToGroup(string groupName)
    {
        var userId = Context.ConnectionId;
        var userName = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"Usuário {userName} de ID {userId} conectado!");
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        
        await Clients.Group(groupName).SendAsync("UserConnected", userId, "entrou no grupo");
    }
    
    public override Task OnConnectedAsync()
    {
        var httpContext = Context?.GetHttpContext();
        var deviceId = httpContext?.Request.Query["deviceId"];
        if (deviceId.Value.Count == 0 || deviceId.Value == "0")
        {
            return base.OnConnectedAsync();
        }
        var device = deviceService.GetDeviceById(int.Parse(deviceId.Value));
        if (device == null)
        {
            return base.OnConnectedAsync();
        }
        Context?.Features.Set<Device>(device);
        AddToGroup(groupName: device.User.IdUser.ToString());
        
        return base.OnConnectedAsync();
    }
    
    public async Task RemoveGroup(string groupName)
    {
        var userId = Context.ConnectionId;
        var userName = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"Usuário {userName} de ID {userId} desconectado!");
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        
        await Clients.Group(groupName).SendAsync("UserDisconnected", userId, "saiu do grupo");
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        RemoveGroup(groupName: "Users");
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendFile(string userId, string fileName)
    {
        await Clients.Group(userId).SendAsync("ReceiveFile", fileName);
    }
    
    
    // public override Task OnConnectedAsync()
    // {
    //     var connectionId = Context.ConnectionId;
    //     
    //     Console.WriteLine($"User {connectionId} conectado");
    //     _connectionManagerService.AddUsers(connectionId);
    //     
    //     return base.OnConnectedAsync();
    // }
    //
    // public override Task OnDisconnectedAsync(Exception? exception)
    // {
    //     var connectionId = Context.ConnectionId;
    //     
    //     Console.WriteLine($"User {connectionId} desconectado");
    //     _connectionManagerService.RemoveUser(connectionId);
    //     
    //     return base.OnDisconnectedAsync(exception);
    // }
    // public async Task SendFile(string userId, string fileName, byte[] chunk)
    // {
    //     if (!_transferringFiles.ContainsKey(userId))
    //     {
    //         _transferringFiles[userId] = new List<string>();
    //     }
    //     if (!_transferringFiles[userId].Contains(fileName))
    //     {
    //         _transferringFiles[userId].Add(fileName);
    //     }
    //         
    //     await Clients.User(userId).SendAsync("ReceiveFileChunk", fileName, chunk);
    //     
    //     Console.WriteLine($"User {userId} enviou o arquivo {fileName}");
    // }
    //
    // public async Task CompleteTransfer(string userId, string fileName, long fileSize)
    // {
    //     if(_transferringFiles.ContainsKey(userId) && _transferringFiles[userId].Contains(fileName))
    //     {
    //         _transferringFiles[userId].Remove(fileName);
    //     }
    //         
    //     await Clients.User(userId).SendAsync("FileTransferComplete", fileName, fileSize);
    //     
    //     await _transferService.DeleteTempFile(fileName);
    // }
    //     
    // public async Task StartTransfer(string userId, string fileName, long fileSize)
    // {
    //     await Clients.User(userId).SendAsync("StartFileTransfer", fileName, fileSize);
    // }
    //
    // public async Task ReceiveFile(string connectionId,string fileName, byte[] chunk)
    // {
    //     await _transferService.ReceiveFile(fileName, chunk);
    // }
}