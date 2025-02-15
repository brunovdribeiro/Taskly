using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Events;

public record UserCreatedEvent(UserId UserId, string Email, string Name) : IEvent;
public record UserDeactivatedEvent(UserId UserId) : IEvent;
public record UserActivatedEvent(UserId UserId) : IEvent;