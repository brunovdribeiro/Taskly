using Domain.Aggregates;
using Domain.ValueObjects;
using Task = System.Threading.Tasks.Task;

namespace Application.Features.Users.Interfaces;

public interface IUserSnapshotRepository
{
    Task<User?> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken
    );

    Task SaveSnapshotAsync(
        User user,
        CancellationToken cancellationToken
    );
}