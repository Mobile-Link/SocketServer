using Microsoft.EntityFrameworkCore;
using SocketServer.Data;

namespace SocketServer.Services;

public class CodeExpirationService
{
    private readonly ILogger<CodeExpirationService> _logger;
    private readonly AppDbContext _context;
    private readonly UserService _userService;
    private Timer _timer;
    
    public CodeExpirationService(ILogger<CodeExpirationService> logger, AppDbContext context, UserService userService)
    {
        _logger = logger;
        _context = context;
        _userService = userService;
    }
    
    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CodeExpirationService is starting.");
        
        _timer = new Timer(DeleteExpiredCodes, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CodeExpirationService is stopping.");
        
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        
        return Task.CompletedTask;
    }
    
    private async void DeleteExpiredCodes(object state)
    {
        _logger.LogInformation("CodeExpirationService is deleting expired codes.");
        
        var verificationCodes = await _context.VerificationCodes.Where(v => v.ExpirationDate < DateTime.Now).ToListAsync();

        foreach (var verificationCode in verificationCodes)
        {
            _logger.LogInformation($"Deleting code for user {verificationCode.Email}");
            await _userService.DeleteVerificationCode(verificationCode.Email);
        }
    }
    
    public void Dispose()
    {
        _timer?.Dispose();
    }
}