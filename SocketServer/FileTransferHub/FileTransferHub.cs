using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using SocketServer.Services;

namespace SocketServer.FileTransferHub;

public class FileTransferHub : Hub
{
    private readonly Dictionary<string, List<string>> _transferringFiles = new Dictionary<string, List<string>>();
    private readonly FileTransferService _fileTransferService;
    private readonly ConnectionManagerService _connectionManagerService;
        
    public FileTransferHub(FileTransferService fileTransferService, ConnectionManagerService connectionManagerService)
    {
        _fileTransferService = fileTransferService;
        _connectionManagerService = connectionManagerService;
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