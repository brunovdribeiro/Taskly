using Application.Features.Tasks.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Aggregates.Task;

namespace Infrastructure.Persistence.Postgres.Tasks;

public class TaskSnapshotRepository(
    ApplicationDbContext context
) : ITaskSnapshotRepository
{
    public async Task<Task?> GetLatestSnapshotAsync(
        TaskId id,
        CancellationToken cancellationToken
    )
    {
        var snapshot = await context.TaskSnapshots
            .Where(t => t.Id == id.Value)
            .OrderByDescending(t => t.Version)
            .FirstOrDefaultAsync(cancellationToken);

        return snapshot == null ? null : MapToDomain(snapshot);
    }

    public async System.Threading.Tasks.Task SaveSnapshotAsync(
        Task task,
        CancellationToken cancellationToken
    )
    {
        var snapshot = new TaskSnapshot
        {
            Id = task.Id.Value,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            AssignedTo = task.AssignedTo?.Value,
            CreatedAt = task.CreatedAt,
            LastModified = task.LastModified,
            Version = task.Version
        };

        await context.TaskSnapshots.AddAsync(snapshot, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static Task MapToDomain(
        TaskSnapshot snapshot
    )
    {
        return Task.Create(
            TaskId.From(snapshot.Id),
            snapshot.Title,
            snapshot.Description,
            snapshot.Status,
            snapshot.Priority,
            snapshot.AssignedTo.HasValue ? UserId.From(snapshot.AssignedTo.Value) : null,
            snapshot.CreatedAt,
            snapshot.LastModified,
            snapshot.Version
        );
    }
}