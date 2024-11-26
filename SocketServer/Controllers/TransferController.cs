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
    [HttpPost("failTransfer")]
    public async Task<ActionResult> FailTransfer([FromQuery] int idTransfer)
    {
        var success = await transferService.FailTransfer(idTransfer);
        if (!success)
        {
            return StatusCode(500);
        }
        return new OkResult();
    }
    [HttpPost("finishTransfer")]
    public async Task<ActionResult> FinishTransfer([FromQuery] int idTransfer)
    {
        var success = await transferService.FinishTransfer(idTransfer);
        if (!success)
        {
            return StatusCode(500);
        }
        return new OkResult();
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
    
    [HttpGet("getChunkWithTransference")]
    public ActionResult GetChunk([FromQuery] int idChunk)
    {
        var chunk = transferService.GetChunkWithTransference(idChunk);
        if (chunk == null)
        {
            return StatusCode(404);
        }
        return new OkObjectResult(chunk);
    }
    
    [HttpGet("getChunkBytes")]
    public async Task<ActionResult> GetChunkBytes([FromQuery] int idChunk)
    {
        var chunk = transferService.GetTransferChunk(idChunk);
        if (chunk == null)
        {
            return BadRequest();
        }

        var bytes = await transferService.GetChunkBytes(chunk);
        if (bytes == null)
        {
            return NotFound();//TODO emit to owner device requesting it to send the chunk (Validate necessity)
        }
        return new OkObjectResult(new { Chunk = chunk, Bytes = bytes});
    }
    
    [HttpGet("getTransfersNotOnServer")]
    public async Task<ActionResult> GetTransfersNotOnServer()
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        var bytes = transferService.GetTransfersNotOnServer(int.Parse(idClaim.Value));
        return new OkObjectResult(bytes);
    }
    
    [HttpGet("getTransfersNotOnDestination")]
    public async Task<ActionResult> GetTransfersNotOnDestination()
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        var bytes = transferService.GetTransfersNotOnDestination(int.Parse(idClaim.Value));
        return new OkObjectResult(bytes);
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