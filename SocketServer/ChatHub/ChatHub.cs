using Microsoft.AspNetCore.SignalR;

namespace SocketServer.ChatHub;

public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        
        return base.OnConnectedAsync();
    }

    public async Task SendFile(string userId, byte[] chunk)
    {
        await Clients.Others.SendAsync("ReceiveFileChunk", userId, chunk);
    }
    
    public async Task CompleteTransfer(string userId, string fileName)
    {
        await Clients.Others.SendAsync("FileTransferComplete", userId, fileName);
    }
}