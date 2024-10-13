using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocketServer.Services;

namespace SocketServer.Controllers;

[ApiController]
public class TransferController(TransferService transferService) : ControllerBase
{
    private readonly TransferService _transferService = transferService;
    
    
}