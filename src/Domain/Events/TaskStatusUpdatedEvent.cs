using Domain.Common;
using Domain.ValueObjects;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Events;

public record TaskStatusUpdatedEvent(
    TaskId TaskId,
    TaskStatus NewStatus
) : IEvent;