using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Hubs;

public class ConnectionHub(DeviceService deviceService, TransferService transferService, ConnectionService connectionService) : Hub
{
    private readonly TransferService _transferService = transferService;

    public async Task AddToGroup(int idUser, int idDevice)
    {
        Console.WriteLine($"Usuário {idUser} conectou o dispositivo {idDevice}!");


        if (connectionService.ConnectedDevices.ContainsKey(idUser))
        {
            connectionService.ConnectedDevices[idUser].Add(idDevice);
        }
        else
        {
            connectionService.ConnectedDevices[idUser] = new List<int>()
            {
                idDevice
            };
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, idUser.ToString());
    }

    public override Task OnConnectedAsync()
    {
        var httpContext = Context?.GetHttpContext();
        var deviceIdQuery = httpContext?.Request.Query["deviceId"];
        Console.WriteLine($"Usuário {deviceIdQuery} de ID conectado!");
        if (deviceIdQuery.Value.Count == 0)
        {
            return base.OnConnectedAsync();
        }

        var deviceId = int.Parse(deviceIdQuery.Value);
        var user = deviceService.GetUserByDevice(deviceId);
        if (user == null)
        {
            return base.OnConnectedAsync();
        }

        var device = deviceService.GetDeviceById(deviceId);
        if (device == null)
        {
            return base.OnConnectedAsync();
        }

        device.IdUser = user.IdUser;
        Context?.Features.Set<Tuple<int, int>>(new Tuple<int, int>(user.IdUser, deviceId));
        AddToGroup(user.IdUser, deviceId);

        return base.OnConnectedAsync();
    }

    private async Task RemoveFromGroup()
    {
        var connectionId = Context.ConnectionId;
        Console.WriteLine($"ID {connectionId} desconectado!");
        if (Context?.ConnectionId == null)
        {
            return;
        }
        var ids = Context.Features.Get<Tuple<int, int>>();
        if (ids == null)
        {
            return;
        }

        if (!connectionService.ConnectedDevices.ContainsKey(ids.Item1))
        {
            return;
        }
        Console.WriteLine($"Dispositivo {ids.Item1} do usuário {ids.Item2}");
        connectionService.ConnectedDevices[ids.Item1].RemoveAll(item => item == ids.Item2);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ids.Item1.ToString());
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