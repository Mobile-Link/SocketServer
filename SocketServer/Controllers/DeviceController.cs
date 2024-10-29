using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Services;
using System.Web;

namespace SocketServer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceController(DeviceService deviceService, IHttpContextAccessor httpContextAccessor)
{
    [HttpGet("GetUserDevices")]
    public ActionResult<List<Device>> GetUserDevices() //TODO get user from auth
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
        var devices = deviceService.GetUserDevices(user.IdUser);
        return devices ?? [];
    }
    
    [HttpDelete("DeleteDevice")]
    public async Task<IActionResult> DeleteDevice(int deviceId)
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
        var device = deviceService.GetDeviceById(deviceId);
        if (device == null)
        {
            return new StatusCodeResult(500);
        }
        if (device.IdUser != user.IdUser)
        {
            return new StatusCodeResult(500);
        }
        await deviceService.DeleteDeviceByUser(deviceId);
        
        return new OkObjectResult(new {message = "Dispositivo deletado com sucesso"});
    }
    
    // [HttpPost("DeleteDevice")]
    // public async Task<IActionResult> DeleteDevice()
    // {
    // }
}