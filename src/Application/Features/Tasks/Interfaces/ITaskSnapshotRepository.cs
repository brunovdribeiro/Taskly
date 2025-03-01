using Domain.ValueObjects;

namespace Application.Features.Tasks.Interfaces;

public interface ITaskSnapshotRepository
{
    Task SaveSnapshotAsync(
        Domain.Aggregates.Task task,
        CancellationToken cancellationToken
    );

    Task<Domain.Aggregates.Task?> GetLatestSnapshotAsync(
        TaskId taskId,
        CancellationToken cancellationToken
    );
}