using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ConnectionController(
    ConnectionManagerService connectionManagerService,
    IHubContext<TransferHub.TransferHub> hubContext) : ControllerBase
{
    [HttpGet("connections")]
    public IActionResult GetConnections()
    {
        return Ok(connectionManagerService.GetActiveConnections());
    }
    
    [HttpPost("connect")]
    public async Task<IActionResult> Connect(string connectionId)
    {
        if (!connectionManagerService.IsUserConnected(connectionId))
        {
            connectionManagerService.AddUsers(connectionId);
            
            await hubContext.Clients.All.SendAsync("UserConnected", connectionId);
            
            return Ok(new { message = "Conexão estabelecida com sucesso" });
        }
        else
        {
            return BadRequest(new { error = "Usuário já conectado" });
        }
    }
    
    [HttpPost("start")]
    public async Task<IActionResult> StartTransfer(string connectionId)
    {
        if (!connectionManagerService.IsUserConnected(connectionId))
        {
            return BadRequest("Conexão não estabelecida");
        }
        
        await hubContext.Clients.User(connectionId).SendAsync("StartTransfer");
        
        return Ok("Transferência iniciada");
    }
    
    [HttpDelete("disconnect/{connectionId}")]
    public async Task<IActionResult> Disconnect(string connectionId)
    {
        if (connectionManagerService.IsUserConnected(connectionId))
        {
            connectionManagerService.RemoveUser(connectionId);
            
            await hubContext.Clients.All.SendAsync("UserDisconnected", connectionId);
            
            return Ok(new { message = "Conexão encerrada com sucesso" });
        }
        else
        {
            return BadRequest(new { error = "Usuário não conectado" });
        }
    }
}