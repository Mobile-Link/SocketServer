using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Entities;
using SocketServer.Enums;
using SocketServer.Infra;

namespace SocketServer.Services;

public class DeviceService(AppDbContext context, ExpirationDbContext expirationDbContext)
{
    public async Task<Device> CreateDevice(User user, string deviceName)
    {
        var device = new Device()
        {
            IdUser = user.IdUser,
            IsDeleted = false,
            LastLocation = "",
            AvailableSpace = 0,
            OccupiedSpace = 0,
            Name = deviceName,
            CreationDate = DateTime.Now,
            AlterationDate = DateTime.Now,
            EnDeviceOs = EnDeviceOs.Windows,
        };
        await context.Devices.AddAsync(device);
        await context.SaveChangesAsync();
        return device;
    }
    
     public async Task<DeviceToken> CreateDeviceToken(Device device)
    {
        var token = new DeviceToken
        {
            IdDevice = device.IdDevice,
            Token = GenerateCode.GenerateJwtToken(device.IdDevice.ToString()), //TODO use IdDevice as claim
            InsertionDate = DateTime.Now,
        };
        expirationDbContext.DeviceTokens.Add(token);
        await expirationDbContext.SaveChangesAsync();
        return token;
    }

    public Device? GetDeviceById(int deviceId)
    {
        return context.Devices
            .AsNoTracking()
            .FirstOrDefault(device => device.IdDevice == deviceId);
    }
    
    public User? GetUserByDevice(int deviceId)
    {
        return context.Devices
            .AsNoTracking()
            .Include(device => device.User)
            .FirstOrDefault(device => device.IdDevice == deviceId)
            ?.User;
    }
}