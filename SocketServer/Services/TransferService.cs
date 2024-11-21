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
    TransferenceTimerService transferenceTimerService,
    IHubContext<ConnectionHub> hubContext, 
    IConfiguration configuration)
{
    public async Task<Transference> AddFileTransfer(Transference transference)
    {
        context.Transfers.Add(transference);
        await context.SaveChangesAsync();

        return transference;
    }

    private async Task<TransferenceChunk> AddTransferenceChunk(TransferenceChunk chunk)
    {
        context.TransferenceChunks.Add(chunk);
        await context.SaveChangesAsync();

        return chunk;
    }

    private async Task UpdateTransferenceChunk(TransferenceChunk transferenceChunk)
    {
        context.TransferenceChunks.Update(transferenceChunk);
        await context.SaveChangesAsync();
    }
    
    private async Task UpdateTransference(Transference transference)
    {
        context.Transfers.Update(transference);
        await context.SaveChangesAsync();
    }

    public Transference? GetTransfer(int idTransference)
    {
        return context.Transfers
            .AsNoTracking()
            .FirstOrDefault((transference => transference.IdTransference == idTransference));
    }
    
    public List<TransferenceChunk> GetTransferChunks(int idTransference)
    {
        return context.TransferenceChunks
            .AsNoTracking()
            .Where((chunk => chunk.IdTransference == idTransference)).ToList();
    }
    
    public TransferenceChunk? GetTransferChunk(int idChunk)
    {
        return context.TransferenceChunks
            .AsNoTracking()
            .FirstOrDefault((chunk => chunk.IdTransferenceChunk == idChunk));
    }

    public bool CheckAllChunksOnStatus(int idTransference, EnChunkStatus chunkStatus)
    {
        return !context.TransferenceChunks.Any((chunk) => chunk.IdTransference == idTransference && chunk.EnChunkStatus != chunkStatus);
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
            FileNameExtension = request.FileNameExtension,
            Size = request.FileSize,
            DestinationPath = request.DestinationPath,
            EnStatus = EnStatus.NotStarted
        });

        var transferId = transference.IdTransference;

        var totalChunks = (int)Math.Ceiling((double)request.FileSize / (1024 * 1024));
        for (var index = 0; index < totalChunks; index++)
        {
            await AddTransferenceChunk(new TransferenceChunk()
            {
                IdTransference = transferId,
                EnChunkStatus = EnChunkStatus.Pending,
                StartByteIndex = index * (1024 * 1024)
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

        var transferenceChunk = await context.TransferenceChunks.FirstOrDefaultAsync((chunk) =>
            chunk.IdTransference == request.IdTransfer && chunk.StartByteIndex ==
            request.StartByteIndex);
        if (transferenceChunk == null)
        {
            //TODO error
            return false;
        }
        
        transferenceChunk.EnChunkStatus = EnChunkStatus.Received;
        await UpdateTransferenceChunk(transferenceChunk);

        var directory = Path.Combine(configuration["ChunkUploadPath"] ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), transference.IdTransference.ToString());
        var chunkPath = Path.Combine(directory, $"{request.StartByteIndex}.bin");
        Directory.CreateDirectory(directory);
        await File.WriteAllBytesAsync(chunkPath, request.ByteArray);

        if (CheckAllChunksOnStatus(transference.IdTransference, EnChunkStatus.Received))
        {
            transferenceTimerService.RemoveMonitor(transference.IdTransference);
            transference.EnStatus = EnStatus.InCloud;
            await UpdateTransference(transference);
        }
        else
        {
            transferenceTimerService.ChunkReceived(transference.IdTransference);
            transference.EnStatus = EnStatus.InProgress;
            await UpdateTransference(transference);
        }
        
        var connectionDestination =
            connectionService.findDeviceConnection(user.IdUser, transference.IdDeviceDestination);
        if (connectionDestination == null)
        {
            return true;
        }

        await hubContext.Clients.Client(connectionDestination).SendAsync("ReceiveFileChunk", request.IdTransfer,
            request.StartByteIndex, request.ByteArray);
        
        return true;
    }
    
    public async Task<byte[]?> GetChunkBytes(TransferenceChunk chunk)
    {
        var directory = Path.Combine(configuration["ChunkUploadPath"] ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), chunk.IdTransference.ToString());
        var chunkPath = Path.Combine(directory, $"{chunk.StartByteIndex}.bin");
        return await File.ReadAllBytesAsync(chunkPath);
    }
    
    public List<Transference> GetTransfersNotOnDestination(int idDestination)
    {
        return context.Transfers
            .Where((transference => transference.IdDeviceDestination == idDestination))
            .Where((transference => transference.EnStatus == EnStatus.InCloud))
            .AsNoTracking()
            .ToList();
    }
    
    public List<Transference> GetTransfersNotOnServer(int idDeviceOrigin)
    {
        return context.Transfers
            .Where((transference => transference.IdDeviceOrigin == idDeviceOrigin))
            .Where((transference => transference.EnStatus == EnStatus.ReceivingStalled))
            .AsNoTracking()
            .ToList();
    }

    public void TimeoutTransference(int idTransference)
    {
        var transfer = GetTransfer(idTransference);
        if (transfer == null)
        {
            return;
        }

        if (CheckAllChunksOnStatus(transfer.IdTransference, EnChunkStatus.Received))
        {
            if (transfer.EnStatus == EnStatus.InProgress)
            {
                transfer.EnStatus = EnStatus.InCloud;
                UpdateTransference(transfer).ContinueWith(_ => {});
            }
            return;
        }
        
        transfer.EnStatus = EnStatus.ReceivingStalled;
        UpdateTransference(transfer).ContinueWith(_ => {});
        
        var connectionOrigin = connectionService.findDeviceConnection(transfer.IdUser, transfer.IdDeviceOrigin);
        if (connectionOrigin == null)
        {
            return;
        }
        hubContext.Clients.Client(connectionOrigin).SendAsync("ReSendChunks", transfer.IdTransference);
    }

    public TransferenceChunk? GetChunkWithTransference(int idChunk)
    {
        return context.TransferenceChunks
            .AsNoTracking()
            .Include(chunk => chunk.Transference)
            .FirstOrDefault((chunk => chunk.IdTransferenceChunk == idChunk));
    }

    public async Task<bool> FinishTransfer(int idTransfer)
    {
        var transfer = GetTransfer(idTransfer);
        if (transfer == null)
        {
            return false;
        }

        var chunks = GetTransferChunks(idTransfer);
        var directory = Path.Combine(configuration["ChunkUploadPath"] ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), transfer.IdTransference.ToString());
        foreach (var chunk in chunks)
        {
            var chunkPath = Path.Combine(directory, $"{chunk.StartByteIndex}.bin");
            File.Delete(chunkPath);
        }
        Directory.Delete(directory);
        transfer.EnStatus = EnStatus.Finished;
        await UpdateTransference(transfer);
        return true;
    }
}