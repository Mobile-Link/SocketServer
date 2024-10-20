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
public class DeviceController(DeviceService deviceService, HistoryService historyService, IHttpContextAccessor httpContextAccessor)
{
    [HttpGet("GetUserDevices")]
    public ActionResult<List<Device>> GetUserDevices() //TODO get user from auth
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        var devices = deviceService.GetUserDevices(int.Parse(idClaim.Value));
        return devices ?? [];
    }
    //
    // [HttpGet("{id}")]
    // public async Task<Device?> GetDeviceById(int id)
    // {
    //     return await _deviceService.GetDeviceById(id);
    // }
    //
    // [HttpPost]
    // public async Task<Device> CreateDevice(Device device)
    // {
    //     return await _deviceService.CreateDevice(device);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<Device> UpdateDevice(int id, Device device)
    // {
    //     return await _deviceService.UpdateDevice(id, device);
    // }
    //
    // [HttpDelete("{id}")]
    // public async Task DeleteDevice(int id)
    // {
    //     await _deviceService.DeleteDevice(id);
    // }

    [HttpPost("history")]
    public async Task DeviceHistory([FromBody] int deviceId)
    {
        await historyService.GetHistoryById(deviceId);
    }
}