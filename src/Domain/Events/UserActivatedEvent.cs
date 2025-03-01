using Domain.Common;

namespace Domain.Events;

public record UserActivatedEvent(
    Guid UserId
) : IEvent;