using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Entities;
using SocketServer.Enums;
using SocketServer.Hubs;
using SocketServer.Models;

namespace SocketServer.Services;

public class TransferService(
    AppDbContext context,
    DeviceService deviceService,
    ConnectionService connectionService,
    IHubContext<ConnectionHub> hubContext, 
    IConfiguration configuration)
{
    private readonly AppDbContext _context = context;

    public async Task<Transference> AddFileTransfer(Transference transference)
    {
        _context.Transfers.Add(transference);
        await _context.SaveChangesAsync();

        return transference;
    }

    private async Task<TransferenceChunk> AddTransferenceChunk(TransferenceChunk chunk)
    {
        _context.TransferenceChunks.Add(chunk);
        await _context.SaveChangesAsync();

        return chunk;
    }

    private async Task UpdateTransferenceChunk(TransferenceChunk transferenceChunk)
    {
        _context.TransferenceChunks.Update(transferenceChunk);
        await _context.SaveChangesAsync();
    }
    
    private async Task UpdateTransference(Transference transference)
    {
        _context.Transfers.Update(transference);
        await _context.SaveChangesAsync();
    }

    public Transference? GetTransfer(int idTransference)
    {
        return _context.Transfers
            .AsNoTracking()
            .FirstOrDefault((transference => transference.IdTranference == idTransference));
    }
    
    public List<TransferenceChunk> GetTransferChunks(int idTransference)
    {
        return _context.TransferenceChunks
            .AsNoTracking()
            .Where((chunk => chunk.IdTransference == idTransference)).ToList();
    }

    public bool CheckAllChunksOnStatus(int idTransference, EnChunkStatus chunkStatus)
    {
        return !_context.TransferenceChunks.Any((chunk) => chunk.IdTransference == idTransference && chunk.EnChunkStatus != chunkStatus);
    }
    
    public async Task<int?> StartTransference(StartTransference request, int idDeviceOrigin)
    {
        var deviceDestination = deviceService.GetDeviceById(request.IdDevice);
        var user = deviceService.GetUserByDevice(request.IdDevice);
        if (user == null || deviceDestination == null)
        {
            return null;
        }

        var transference = await AddFileTransfer(new Transference
        {
            IdUser = user.IdUser,
            IdDeviceOrigin = idDeviceOrigin,
            IdDeviceDestination = deviceDestination.IdDevice,
            FilePath = request.FilePath,
            Size = request.FileSize,
            DestinationPath = request.DestinationPath
        });

        var transferId = transference.IdTranference;

        var totalChunks = (int)Math.Ceiling((double)request.FileSize / (1024 * 1024));
        for (var index = 0; index < totalChunks; index++)
        {
            await AddTransferenceChunk(new TransferenceChunk()
            {
                IdTransference = transferId,
                EnChunkStatus = EnChunkStatus.Pending,
                startByteIndex = index * (1024 * 1024)
            });
        }

        var connectionDestination = connectionService.findDeviceConnection(user.IdUser, deviceDestination.IdDevice);
        if (connectionDestination == null)
        {
            return transferId;
        }

        await hubContext.Clients.Client(connectionDestination)
            .SendAsync("ReceiveNewTransference", transferId, request.FilePath, request.FileSize);
        return transferId;
    }

    public async Task<bool> SendFileChunk(SendFileChunk request)
    {
        var transference = GetTransfer(request.IdTransfer);
        if (transference == null)
        {
            return false;
        }

        var user = deviceService.GetUserByDevice(transference.IdUser);
        if (user == null)
        {
            return false;
        }

        var transferenceChunk = await _context.TransferenceChunks.FirstOrDefaultAsync((chunk) =>
            chunk.IdTransference == request.IdTransfer && chunk.startByteIndex ==
            request.StartByteIndex);
        if (transferenceChunk == null)
        {
            //TODO error
            return false;
        }
        
        transferenceChunk.EnChunkStatus = EnChunkStatus.Received;
        await UpdateTransferenceChunk(transferenceChunk);

        var directory = Path.Combine(configuration["ChunkUploadPath"] ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), transference.IdTranference.ToString());
        var chunkPath = Path.Combine(directory, $"{request.StartByteIndex}.bin");
        Directory.CreateDirectory(directory);
        await System.IO.File.WriteAllBytesAsync(chunkPath, request.ByteArray);

        
        var connectionDestination =
            connectionService.findDeviceConnection(user.IdUser, transference.IdDeviceDestination);
        if (connectionDestination == null)
        {
            //TODO get when device connects
            return true;
        }

        if (CheckAllChunksOnStatus(transference.IdTranference, EnChunkStatus.Received))
        {
            transference.EnStatus = EnStatus.InCloud;
            await UpdateTransference(transference);
        }
        
        await hubContext.Clients.Client(connectionDestination).SendAsync("ReceiveFileChunk", request.IdTransfer,
            request.StartByteIndex, request.ByteArray);
        
        return true;
    }
}