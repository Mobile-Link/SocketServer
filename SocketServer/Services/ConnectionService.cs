using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Hubs;

namespace SocketServer.Services;

public class ConnectionService
{
    public Dictionary<int, List<int>> ConnectedDevices = new Dictionary<int, List<int>>();
    public List<int> GetConnectedDevices(int idUser)
    {
        if (!ConnectedDevices.TryGetValue(idUser, out var device))
        {
            return [];
        }
        return device.ToList();
    }
}