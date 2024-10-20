using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Hubs;

[Authorize(Policy = "Authorized")]
public class ConnectionHub(DeviceService deviceService, TransferService transferService, ConnectionService connectionService) : Hub
{
    public async Task AddToGroup(int idUser, int idDevice)
    {
        Console.WriteLine($"Usuário {idUser} conectou o dispositivo {idDevice}!");


        connectionService.Add(idUser, idDevice, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, idUser.ToString());
    }

    public override async Task OnConnectedAsync()
    {
        var idDeviceClaim = Context.User.FindFirst(claim => claim.Type == "IdDevice");
        if (idDeviceClaim == null)
        {
            return;
        }

        Console.WriteLine($"Usuário {idDeviceClaim.Value} de ID conectado!");
        var deviceId = int.Parse(idDeviceClaim.Value);
        var user = deviceService.GetUserByDevice(deviceId);
        if (user == null)
        {
            return;
        }

        var device = deviceService.GetDeviceById(deviceId);
        if (device == null)
        {
            return;
        }

        device.IdUser = user.IdUser;
        await AddToGroup(user.IdUser, deviceId);
        await base.OnConnectedAsync();
    }

    private async Task RemoveFromGroup()
    {
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"ID {connectionId} desconectado!");
        if (Context?.ConnectionId == null)
        {
            return;
        }
        var idDeviceClaim = Context.User.FindFirst(claim => claim.Type == "IdDevice");
        if (idDeviceClaim == null)
        {
            return;
        }
        var user = deviceService.GetUserByDevice(int.Parse(idDeviceClaim.Value));
        if (user == null)
        {
            return;
        }

        connectionService.Remove(user.IdUser, Context.ConnectionId);
        Console.WriteLine($"Dispositivo {user.IdUser} do usuário {idDeviceClaim.Value}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.IdUser.ToString());
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        RemoveFromGroup();
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendFile(string userId, string fileName)
    {
        await Clients.Group(userId).SendAsync("ReceiveFile", fileName);
    }

    public async Task StartTransference(int idDevice, string filePath, long fileSize, string destinationPath)
    {
        var deviceDestination = deviceService.GetDeviceById(idDevice);
        var user = deviceService.GetUserByDevice(idDevice);
        var deviceOrigin = Context.Features.Get<Device>();

        if (deviceDestination == null || deviceOrigin == null || user == null)
        {
            return;
        }

        var transference = await transferService.StartFileTransfer(new Transference
        {
            IdUser = user.IdUser,
            IdDeviceOrigin = deviceOrigin.IdDevice,
            IdDeviceDestination = deviceDestination.IdDevice,
            FilePath = filePath,
            Size = fileSize,
            DestinationPath = destinationPath
        });

        var transferId = transference.IdTranference;
        var device = Context?.Features.Get<Device>();

        await Clients.User(idDevice.ToString()).SendAsync("StartTransfer", transferId, filePath, fileSize);

        Console.WriteLine("Transferência iniciada");
    }

    public async Task SendFileChunk(long idTransfer, long startByteIndex, byte[] byteArray)
    {
        await Clients.User("").SendAsync("ReceivePackat", idTransfer, startByteIndex, byteArray);
    }

    public async Task CompleteFileTransfer(int transferId, string receiverId, string fileName, long fileSize)
    {
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