using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Entities;
using SocketServer.Enums;
using SocketServer.Infra;

namespace SocketServer.Services;

public class DeviceService(AppDbContext context, ExpirationDbContext expirationDbContext, HistoryService historyService)
{
    public async Task<Device> CreateDevice(User user, string deviceName)
    {
        var dateAccess = DateTime.Now;
        
        var device = new Device()
        {
            IdUser = user.IdUser,
            IsDeleted = false,
            LastLocation = "",
            AvailableSpace = 0,
            OccupiedSpace = 0,
            Name = deviceName,
            CreationDate = dateAccess,
            AlterationDate = dateAccess,
            EnDeviceOs = EnDeviceOs.Windows,
            LastAccessDate = dateAccess
        };
        await context.Devices.AddAsync(device);
        await context.SaveChangesAsync();

        await historyService.CreateHistory(
            EnActions.AddedDevice,
            $"Novo dispositivo adicionado ao usuário {user.Username}",
            device.IdDevice,
            user.IdUser
        );
        
        return device;
    }
    
     public async Task<DeviceToken> CreateDeviceToken(int idDevice)
    {
        var existingToken = GetDeviceToken(idDevice);
        if (existingToken != null)
        {
            await DeleteDeviceTokens(idDevice);
        }
        var token = new DeviceToken
        {
            IdDevice = idDevice,
            Token = GenerateCode.GenerateJwtToken(idDevice.ToString()),
            InsertionDate = DateTime.Now,
        };
        expirationDbContext.DeviceTokens.Add(token);
        await expirationDbContext.SaveChangesAsync();
        return token;
    }

    public async Task DeleteDeviceTokens(int idDevice)
    {
        var tokens = expirationDbContext.DeviceTokens.Where(token => token.IdDevice == idDevice);
        foreach (var token in tokens)
        {
            expirationDbContext.Remove(token);
            await expirationDbContext.SaveChangesAsync();
        }
    }

    public DeviceToken? GetDeviceToken(int deviceId)
    {
        return expirationDbContext.DeviceTokens
            .AsNoTracking()
            .FirstOrDefault(token => token.IdDevice == deviceId);
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
    
    public List<Device>? GetUserDevices(int userId)
    {
        return context.Devices
            .AsNoTracking()
            .Where(device => device.IdUser == userId)
            .ToList();
    }
    
    public async Task<IActionResult> DeleteDeviceByUser(int deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device == null)
        {
            return new NotFoundObjectResult(new {error = "Dispositivo não encontrado"});
        }
        
        device.IsDeleted = true;
        context.Devices.Update(device);
        context.SaveChanges();
        
        await historyService.CreateHistory(
            EnActions.DeletedDevice,
            $"Dispositivo {device.Name} do usuário {device.User.Username} deletado",
            deviceId,
            device.IdUser
        );
        
        return new OkObjectResult(new {message = "Dispositivo deletado com sucesso"});
    }
    
    public async Task UpdateDevice(Device device)
    {
        context.Devices.Update(device);
        await context.SaveChangesAsync();
    }
    
    public async Task LastAccess(Device device)
    {
        var dateAccess = DateTime.Now;

        device.LastAccessDate = dateAccess;
        
        var lastAccess = new AccessLog
        {
            IdUser = device.IdUser,
            IdDevice = device.IdDevice,
            Date = dateAccess,
            AccessLocation = "", //TODO pegar localização
        };

        context.Devices.Update(device);
        await context.SaveChangesAsync();
        
        context.AccessLogs.Update(lastAccess);
        await context.SaveChangesAsync();
    }
    
    public AccessLog? GetLastAccess(int deviceId)
    {
        return context.AccessLogs
            .AsNoTracking()
            .OrderByDescending(accessLog => accessLog.Date)
            .FirstOrDefault(accessLog => accessLog.IdDevice == deviceId);
    }
}