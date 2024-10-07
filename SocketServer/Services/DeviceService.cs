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
            User = user,
            IsDeleted = false,
            LastLocation = "",
            AvailableSpace = 0,
            OccupiedSpace = 0,
            Name = deviceName,
            CreationDate = DateTime.Now,
            AlterationDate = DateTime.Now,
            EnDeviceOsType = EnDeviceOSType.Windows,
        };
        await context.Devices.AddAsync(device);
        await context.SaveChangesAsync();
        return device;
    }
    
     public async Task<DeviceToken> CreateDeviceToken(Device device, User user)
    {
        var token = new DeviceToken
        {
            IdDevice = device.IdDevice,
            Token = GenerateCode.GenerateJwtToken(user.Email),
            InsertionDate = DateTime.Now,
        };
        expirationDbContext.DeviceTokens.Add(token);
        await expirationDbContext.SaveChangesAsync();
        return token;
    }
}