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
    
    public async Task<History> CreateHistory(EnActions enAction, string description, int idDevice, int idUser)
    {
        var history = new History
        {
            IdDevice = idDevice,
            EnAction = enAction,
            Description = description,
            Date = DateTime.Now,
            IdUser = idUser
        };
        
        context.Histories.Add(history);
        await context.SaveChangesAsync();
        
        return history;
    }
}