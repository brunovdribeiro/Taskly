using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Events;

public record TaskCreatedEvent(
    TaskId TaskId,
    string Title,
    string Description
) : IEvent;

public record TaskAssignedEvent(
    TaskId TaskId,
    UserId AssignedTo
) : IEvent;

public record TaskStatusUpdatedEvent(
    TaskId TaskId,
    TaskStatus NewStatus
) : IEvent;

public record TaskPriorityUpdatedEvent(
    TaskId TaskId,
    TaskPriority NewPriority
) : IEvent;