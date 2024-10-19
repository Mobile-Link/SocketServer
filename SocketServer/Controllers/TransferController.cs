using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Services;

namespace SocketServer.Controllers;

[ApiController]
[Authorize]
public class TransferController(TransferService transferService) : ControllerBase
{
    private readonly TransferService _transferService = transferService;
    
    
}