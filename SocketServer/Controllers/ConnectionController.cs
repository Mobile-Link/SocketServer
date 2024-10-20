using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Entities;
using SocketServer.Hubs;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ConnectionController(
    ConnectionService connectionService) : ControllerBase
{
    [HttpGet("GetConnectedDevices")]
    public ActionResult<List<int>> GetConnectedDevice(int userId)//TODO get from auth
    {
        var connectedDevices = connectionService.GetConnectedDevices(userId);
        return connectedDevices ?? [];
    }
}