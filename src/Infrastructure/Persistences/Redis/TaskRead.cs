using Application.Common.Interfaces;
using Application.Features.Tasks.Dtos;
using Domain.Enums;
using Infrastructure.Persistences.Redis.Documents;
using Redis.OM;
using Redis.OM.Searching;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Infrastructure.Persistences.Redis;

public class TaskRead : ITaskRead
{
    private readonly RedisConnectionProvider _provider;
    private readonly IRedisCollection<TaskDocument> _tasks;

    public TaskRead(
        RedisConnectionProvider provider
    )
    {
        _provider = provider;
        _tasks = _provider.RedisCollection<TaskDocument>();
    }

    public async Task<TaskDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var task = await _tasks.FindByIdAsync(id.ToString());

        if (task == null)
            return null;

        return new TaskDto
        {
            Id = Guid.Parse(task.Id),
            Title = task.Title,
            Description = task.Description,
            Status = Enum.Parse<TaskStatus>(task.Status),
            Priority = Enum.Parse<TaskPriority>(task.Priority),
            CreatedAt = task.CreatedAt,
            LastModified = task.LastModified
        };
    }
}