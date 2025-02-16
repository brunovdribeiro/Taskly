using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Aggregates;

public class Task : AggregateRoot<TaskId>
{
    private Task() { }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public UserId? AssignedTo { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModified { get; private set; }

    public static Task Create(
        TaskId id,
        string title,
        string description
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new TaskDomainException("Title cannot be empty");

        var task = new Task
        {
            Id = id,
            Title = title,
            Description = description,
            Status = TaskStatus.Todo,
            Priority = TaskPriority.Normal,
            CreatedAt = DateTime.UtcNow
        };

        task.AddEvent(new TaskCreatedEvent(id, title, description));
        return task;
    }

    public static Task Create(
        TaskId id,
        string title,
        string description,
        TaskStatus status,
        TaskPriority priority,
        UserId? assignedTo,
        DateTime createdAt,
        DateTime? lastModified,
        int version
    )
    {
        var task = new Task
        {
            Id = id,
            Title = title,
            Description = description,
            Status = status,
            Priority = priority,
            AssignedTo = assignedTo,
            CreatedAt = createdAt,
            LastModified = lastModified,
            Version = version
        };

        return task;
    }

    protected override void Apply(
        IEvent @event
    )
    {
        switch (@event)
        {
            case TaskCreatedEvent e:
                Id = e.TaskId;
                Title = e.Title;
                Description = e.Description;
                Status = TaskStatus.Todo;
                Priority = TaskPriority.Normal;
                CreatedAt = DateTime.UtcNow;
                break;
            case TaskAssignedEvent e:
                AssignedTo = e.AssignedTo;
                break;
            case TaskStatusUpdatedEvent e:
                Status = e.NewStatus;
                break;
            case TaskPriorityUpdatedEvent e:
                Priority = e.NewPriority;
                break;
        }
    }

    public void AssignTo(
        UserId userId
    )
    {
        LastModified = DateTime.UtcNow;
        AddEvent(new TaskAssignedEvent(Id, userId));
    }

    public void UpdateStatus(
        TaskStatus newStatus
    )
    {
        if (Status == newStatus)
            return;

        LastModified = DateTime.UtcNow;
        AddEvent(new TaskStatusUpdatedEvent(Id, newStatus));
    }

    public void UpdatePriority(
        TaskPriority newPriority
    )
    {
        if (Priority == newPriority)
            return;

        LastModified = DateTime.UtcNow;
        AddEvent(new TaskPriorityUpdatedEvent(Id, newPriority));
    }
}