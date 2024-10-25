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
        var token = new DeviceToken
        {
            IdDevice = idDevice,
            Token = GenerateCode.GenerateJwtToken(idDevice.ToString()), //TODO use IdDevice as claim
            InsertionDate = DateTime.Now,
        };
        expirationDbContext.DeviceTokens.Add(token);
        await expirationDbContext.SaveChangesAsync();
        return token;
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
    
    public async Task<IActionResult> UpdateDevice(int deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device == null)
        {
            return new NotFoundObjectResult(new {error = "Dispositivo não encontrado"});
        }
        
        context.Devices.Update(device);
        await context.SaveChangesAsync();
        
        await historyService.CreateHistory(
            EnActions.ChangedDevice,
            $"O nome do dispositivo foi modificado para {device.Name} ",
            deviceId,
            device.IdUser
        );
        
        return new OkObjectResult(new {message = "Dispositivo atualizado com sucesso"});
    }
}