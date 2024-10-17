using Microsoft.AspNetCore.SignalR;
using SocketServer.Data;
using SocketServer.Entities;

namespace SocketServer.Services;

public class TransferService(AppDbContext context)
{
    private readonly AppDbContext _context = context;
    
    public async Task<Transference> StartFileTransfer(Transference transference)
    {
        _context.Transfers.Add(transference);
        await _context.SaveChangesAsync();
        
        return transference;
    }

    // public async Task AddFileChunk(int transferId, byte[] chunk)
    // {
    //     var transfer = await _context.Transfers.FindAsync(transferId);
    //     
    //     if (transfer != null)
    //     {
    //         string chunk = 
    //     }
    // }
}