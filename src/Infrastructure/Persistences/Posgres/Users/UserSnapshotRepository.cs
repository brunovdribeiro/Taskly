using Application.Features.Users.Interfaces;
using Domain.Aggregates;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Persistences.Posgres.Users;

public class UserSnapshotRepository : IUserSnapshotRepository
{
    private readonly ApplicationDbContext _context;

    public UserSnapshotRepository(
        ApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken
    )
    {
        var snapshot = await _context.UserSnapshots
            .Where(u => u.Id == id.Value)
            .OrderByDescending(u => u.Version)
            .FirstOrDefaultAsync(cancellationToken);

        if (snapshot == null)
            return null;

        return User.Create(
            UserId.From(snapshot.Id),
            snapshot.Email,
            snapshot.Name,
            snapshot.IsActive,
            snapshot.CreatedAt,
            snapshot.LastModified
        );
    }

    public async Task SaveSnapshotAsync(
        User user,
        CancellationToken cancellationToken
    )
    {
        var snapshot = new UserSnapshot
        {
            Id = user.Id.Value,
            Email = user.Email,
            Name = user.Name,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified,
            Version = user.Version
        };

        await _context.UserSnapshots.AddAsync(snapshot, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}