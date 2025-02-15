using Application.Common.Interfaces;
using Application.Features.Tasks.Dtos;
using Redis.OM;
using Redis.OM.Searching;

namespace Infrastructure.Redis;

public class TaskRead : ITaskRead
{
    private readonly RedisConnectionProvider _provider;
    private readonly IRedisCollection<TaskDocument> _tasks;

    public TaskRead(RedisConnectionProvider provider)
    {
        _provider = provider;
        _tasks = _provider.RedisCollection<TaskDocument>();
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _tasks.FindByIdAsync(id.ToString());
        
        if (task == null)
            return null;

        return new TaskDto
        {
            Id = Guid.Parse(task.Id),
            Title = task.Title,
            Description = task.Description,
            Status = Enum.Parse<Domain.Enums.TaskStatus>(task.Status),
            Priority = Enum.Parse<Domain.Enums.TaskPriority>(task.Priority),
            CreatedAt = task.CreatedAt,
            LastModified = task.LastModified
        };
    }
}