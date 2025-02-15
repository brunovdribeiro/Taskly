using Domain.Aggregates;
using Task = System.Threading.Tasks.Task;

namespace Application.Features.Users.Interfaces;

public interface IUserSnapshotRepository
{
    Task SaveSnapshotAsync(User user, CancellationToken cancellationToken);
}