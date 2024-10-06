using Microsoft.EntityFrameworkCore;
using SocketServer.Entities;

namespace SocketServer.Data;

public class ExpirationDbContext(DbContextOptions<ExpirationDbContext> options) : DbContext(options)
{
    public DbSet<VerificationCode> VerificationCodes { get; set; }
};