using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using SocketServer.Services;

namespace SocketServer.ChatHub;

public class TransferHub : Hub
{
    private readonly ConnectionManagerService _connectionManagerService;
    private readonly FileTransferService _fileTransferService;
    private readonly Dictionary<string, List<string>> _transferringFiles = new Dictionary<string, List<string>>();
    
    public TransferHub(ConnectionManagerService connectionManagerService, FileTransferService fileTransferService)
    {
        _fileTransferService = fileTransferService;
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
    
    public async Task CompleteTransfer(string userId, string fileName, long fileSize)
    {
        if(_transferringFiles.ContainsKey(userId) && _transferringFiles[userId].Contains(fileName))
        {
            _transferringFiles[userId].Remove(fileName);
        }
            
        await Clients.User(userId).SendAsync("FileTransferComplete", fileName, fileSize);
        
        await _fileTransferService.DeleteTempFile(fileName);
    }
        
    public async Task StartTransfer(string userId, string fileName, long fileSize)
    {
        await Clients.User(userId).SendAsync("StartFileTransfer", fileName, fileSize);
    }
    
    public async Task ReceiveFile(string connectionId,string fileName, byte[] chunk)
    {
        await _fileTransferService.ReceiveFile(fileName, chunk);
    }
}