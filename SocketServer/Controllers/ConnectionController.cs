using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Entities;
using SocketServer.Hubs;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "Authorized")]
public class ConnectionController(
    ConnectionService connectionService, IHttpContextAccessor httpContextAccessor, DeviceService deviceService) : ControllerBase
{
    [HttpGet("GetConnectedDevices")]
    public ActionResult<List<int>> GetConnectedDevice()//TODO get from auth
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }

        var user = deviceService.GetUserByDevice(int.Parse(idClaim.Value));
        if (user == null)
        {
            return new StatusCodeResult(500);
        }
        var connectedDevices = connectionService.GetConnectedDevices(user.IdUser);
        return connectedDevices ?? [];
    }
}