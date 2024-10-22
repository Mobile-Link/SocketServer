using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    [HttpPost("StartTransference")]
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
    [HttpPost("SendFileChunk")]
    public async Task<ActionResult> SendFileChunk([FromBody] SendFileChunk request)
    {
        var success = await transferService.SendFileChunk(request);
        if (!success)
        {
            return StatusCode(500);
        }
        return new OkResult();
    }
}