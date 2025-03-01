using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Events;

public record TaskAssignedEvent(
    TaskId TaskId,
    UserId AssignedTo
) : IEvent;