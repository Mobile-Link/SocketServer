using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Entities;
using SocketServer.Enums;

namespace SocketServer.Services;

public class HistoryService(AppDbContext context)
{
    public async Task<IEnumerable<History>> GetAllHistories()
    {
        return await context.Histories.ToListAsync();
    }
    
    public async Task<History?> GetHistoryById(int deviceId)
    {
        return await context.Histories.FindAsync(deviceId);
    }
    
    public async Task<History> CreateHistory(EnActionsType enAction, string description, DateTime date, User user, Device device)
    {
        var history = new History
        {
            EnActionType = enAction,
            Description = description,
            Date = date,
            User = user,
            Device = device
        };
        context.Histories.Add(history);
        await context.SaveChangesAsync();
        return history;
    }
    
    public async Task RegisterDeviceDeletion(Device device, User user)
    {
        var history = new History
        {
            EnActionType = EnActionsType.DeletedDevice,
            Description = $"O dispositivo {device.Name} foi deletado",
            Date = DateTime.Now,
            User = user,
            Device = device
        };
        context.Histories.Add(history);
        await context.SaveChangesAsync();
    }
    
    public async Task RegisterDeviceCreation(Device device, User user)
    {
        var history = new History
        {
            EnActionType = EnActionsType.AddedDevice,
            Description = $"O dispositivo {device.Name} foi adicionado",
            Date = DateTime.Now,
            User = user,
            Device = device
        };
        context.Histories.Add(history);
        await context.SaveChangesAsync();
    }
    
    public async Task RegisterPasswordChange(User user)
    {
        var history = new History
        {
            EnActionType = EnActionsType.ChangedPassword,
            Description = "A senha foi alterada",
            Date = DateTime.Now,
            User = user
        };
        context.Histories.Add(history);
        await context.SaveChangesAsync();
    }
    
    public async Task RegisterDeviceChange(Device device, User user)
    {
        var history = new History
        {
            EnActionType = EnActionsType.ChangedDevice,
            Description = $"O nome do dispositivo {device.Name} foi alterado",
            Date = DateTime.Now,
            User = user,
            Device = device
        };
        context.Histories.Add(history);
        await context.SaveChangesAsync();
    }
    
    public async Task RegisterUserChange(User user)
    {
        var history = new History
        {
            EnActionType = EnActionsType.ChangedUser,
            Description = "O nome do usuário foi alterado",
            Date = DateTime.Now,
            User = user
        };
        context.Histories.Add(history);
        await context.SaveChangesAsync();
    }
}