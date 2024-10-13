using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Hubs;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ConnectionController(
    IHubContext<ConnectionHub> hubContext) : ControllerBase
{
    
}