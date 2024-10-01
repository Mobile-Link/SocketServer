using Microsoft.EntityFrameworkCore;
using SocketServer.Entities;

namespace SocketServer.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<VerificationCode> VerificationCodes { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}