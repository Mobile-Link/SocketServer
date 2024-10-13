using Microsoft.AspNetCore.SignalR;

namespace SocketServer.Services;

public class TransferService
{
    private readonly string _tempDirectory = Path.Combine(Path.GetTempPath(), "FileTransferTemp");
}