using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Services;

namespace SocketServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceController(DeviceService deviceService, HistoryService historyService)
{
    // [HttpGet]
    // public async Task<IEnumerable<Device>> GetAllDevices()
    // {
    //     return await _deviceService.GetAllDevices();
    // }
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
    
    [HttpPost("device/history")]
    public async Task DeviceHistory([FromBody] int deviceId)
    {
        await historyService.GetHistoryById(deviceId);
    }
}
