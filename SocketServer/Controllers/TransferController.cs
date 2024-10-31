using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Enums;
using SocketServer.Hubs;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "Authorized")]
public class TransferController(
    TransferService transferService,
    IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpPost("startTransference")]
    public async Task<ActionResult<int?>> StartTransference([FromBody] StartTransference request)
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }

        var transferenceId = await transferService.StartTransference(request, int.Parse(idClaim.Value));
        if (transferenceId == null)
        {
            return StatusCode(500);
        }
        return new OkObjectResult(transferenceId);
    }
    [HttpPost("sendFileChunk")]
    public async Task<ActionResult> SendFileChunk([FromBody] SendFileChunk request)
    {
        var success = await transferService.SendFileChunk(request);
        if (!success)
        {
            return StatusCode(500);
        }
        return new OkResult();
    }
    
    [HttpGet("getTransfer")]
    public async Task<ActionResult> GetTransfer([FromQuery] int idTransfer)
    {
        var transfer = transferService.GetTransfer(idTransfer);
        if (transfer == null)
        {
            return StatusCode(404);
        }
        return new OkObjectResult(transfer);
    }
    
    [HttpGet("getTransferChunks")]
    public async Task<ActionResult> GetTransferChunks([FromQuery] int idTransfer)
    {
        var transfer = transferService.GetTransfer(idTransfer);
        if (transfer == null)
        {
            return StatusCode(404);
        }
        var chunks = transferService.GetTransferChunks(idTransfer);
        return new OkObjectResult(chunks);
    }
    
    [HttpGet("checkTransferChunksCompletion")]
    public async Task<ActionResult> CheckTransferChunksCompletion([FromQuery] int idTransfer)
    {
        var transfer = transferService.GetTransfer(idTransfer);
        if (transfer == null)
        {
            return StatusCode(404);
        }
        var completed = transferService.GetTransferChunks(idTransfer);
        return new OkObjectResult(completed);
    }
}