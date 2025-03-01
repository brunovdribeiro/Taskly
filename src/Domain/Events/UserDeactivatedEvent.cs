using Domain.Common;

namespace Domain.Events;

public record UserDeactivatedEvent(
    Guid UserId
) : IEvent;