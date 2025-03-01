using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Events;

public record TaskPriorityUpdatedEvent(
    TaskId TaskId,
    TaskPriority NewPriority
) : IEvent;