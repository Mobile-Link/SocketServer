using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Hubs;

namespace SocketServer.Services;

public class ConnectionService(IHubContext<ConnectionHub> hubContext)
{
    public async Task<IActionResult> GetUserConnections(long idUser)
    {
        // hubContext.Clients.Group(idUser.ToString()).GetConnection;
        return new OkObjectResult("");
    }
}