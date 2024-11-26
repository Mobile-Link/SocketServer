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
        if (!_connectedDevices.TryGetValue(idUser, out var devices))
        {
            return [];
        }
        return devices.Select((device) => device.Item1).ToList();
    }

    public string? findDeviceConnection(int idUser, int idDevice)
    {
        if (!_connectedDevices.TryGetValue(idUser, out var devices))
        {
            return null;
        }

        return devices.FirstOrDefault((device) => device.Item1 == idDevice)?.Item2;
    }
    
    public int? findDeviceByConnection(int idUser, string connectionId)
    {
        if (!_connectedDevices.TryGetValue(idUser, out var devices))
        {
            return null;
        }

        return devices.FirstOrDefault((device) => device.Item2 == connectionId)?.Item1;
    }

    public void Add(int idUser, int idDevice, string connectionId)
    {
        var item = new Tuple<int, string>(idDevice, connectionId);
        if (_connectedDevices.TryGetValue(idUser, out List<Tuple<int, string>>? value))
        {
            var index = value.FindIndex((tuple => tuple.Item1 == idDevice));
            if (index != -1)
            {
                _connectedDevices[idUser][index] = item;
                return;
            }

            value.Add(item);
            return;
        }
        
        _connectedDevices[idUser] = [item];
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