using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeviceController(DeviceService deviceService, HistoryService historyService)
{
    [HttpGet("GetUserDevices")]
    public ActionResult<List<Device>> GetUserDevices(int userId) //TODO get user from auth
    {
        var devices = deviceService.GetUserDevices(userId);
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