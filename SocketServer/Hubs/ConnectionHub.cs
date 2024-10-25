using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Hubs;
[Authorize]
public class ConnectionHub(
    DeviceService deviceService,
    ConnectionService connectionService) : Hub
{
    public async Task AddToGroup(int idUser, int idDevice)
    {
        Console.WriteLine($"Usuário {idUser} conectou o dispositivo {idDevice}!");


        connectionService.Add(idUser, idDevice, Context.ConnectionId);
        await Clients.OthersInGroup(idUser.ToString()).SendAsync("UpdateConnectedDevices",
            connectionService.GetConnectedDevices(idUser));
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
        await Clients.OthersInGroup(user.IdUser.ToString()).SendAsync("UpdateConnectedDevices",
            connectionService.GetConnectedDevices(user.IdUser));
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


    public async Task CompleteFileTransfer(int transferId, string receiverId, string fileName, long fileSize)
    {
    }
}