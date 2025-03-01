using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Events;

public record TaskCreatedEvent(
    TaskId TaskId,
    string Title,
    string Description
) : IEvent;