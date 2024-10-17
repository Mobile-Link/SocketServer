using Microsoft.AspNetCore.SignalR;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Hubs;

public class TransferHub(TransferService transferService) : Hub
{
    
    private readonly TransferService _transferService = transferService;
    
    public async Task StartTransference(int idDevice, string filePath, long fileSize, string destinationPath)
    {
        var transference = await transferService.StartFileTransfer(new Transference 
        {
            DeviceDestination = new Device(){IdDevice = idDevice},
            FilePath = filePath,
            Size = fileSize,
            DestinationPath = destinationPath
        });
        
        var transferId = transference.IdTranference;
        
        await Clients.User(idDevice.ToString()).SendAsync("StartTransfer", transferId, filePath, fileSize);
        
        Console.WriteLine("Transferência iniciada");
    }
    
    public async Task SendFileChunk(long idTransfer, long startByteIndex, byte[] byteArray)
    {
        await Clients.User("").SendAsync("SendPacket",idTransfer, startByteIndex, byteArray);
    }
    
    public async Task CompleteFileTransfer(int transferId, string receiverId, string fileName, long fileSize)
    {
        
    }
}