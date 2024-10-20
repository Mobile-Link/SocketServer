using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Hubs;

namespace SocketServer.Services;

public class ConnectionService
{
    private Dictionary<int, List<Tuple<int,string>>> _connectedDevices = new();
    public List<int> GetConnectedDevices(int idUser)
    {
        if (!_connectedDevices.TryGetValue(idUser, out var device))
        {
            return [];
        }
        return device.Select((device) => device.Item1).ToList();
    }

    public void Add(int idUser, int idDevice, string connectionId)
    {
        var item = new Tuple<int, string>(idDevice, connectionId);
        if (_connectedDevices.TryGetValue(idUser, out List<Tuple<int, string>>? value))
        {
            value.Add(item);
        }
        else
        {
            _connectedDevices[idUser] = [item];
        }
    }
    public void Remove(int idUser, string connectionId)
    {
        if (!_connectedDevices.TryGetValue(idUser, out List<Tuple<int, string>>? value))
        {
            return;
        }

        value.RemoveAll(item => item.Item2 == connectionId);
    }
}