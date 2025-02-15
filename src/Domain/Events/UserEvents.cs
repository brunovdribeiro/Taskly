using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Events;

public record UserCreatedEvent : IEvent
{
    public UserId UserId { get; }
    public string Email { get; }
    public string Name { get; }
    public bool IsActive { get; }
    public DateTime CreatedAt { get; }
    public DateTime? LastModified { get; }

    public UserCreatedEvent(UserId userId, string email, string name, bool isActive, DateTime createdAt, DateTime? lastModified)
    {
        UserId = userId;
        Email = email;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        LastModified = lastModified;
    }
}
public record UserDeactivatedEvent(UserId UserId) : IEvent;
public record UserActivatedEvent(UserId UserId) : IEvent;