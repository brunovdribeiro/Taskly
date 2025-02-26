using System.Text.Json.Serialization;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Events;

public record UserCreatedEvent : IEvent
{
    [JsonConstructor]
    public UserCreatedEvent(
        Guid userId,
        string email,
        string name,
        bool isActive,
        DateTime createdAt,
        DateTime? lastModified
    )
    {
        UserId = userId;
        Email = email;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        LastModified = lastModified;
    }


    public UserCreatedEvent(
        UserId userId,
        string email,
        string name,
        bool isActive,
        DateTime createdAt,
        DateTime? lastModified
    )
    {
        UserId = userId.Value;
        Email = email;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        LastModified = lastModified;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string Name { get; }
    public bool IsActive { get; }
    public DateTime CreatedAt { get; }
    public DateTime? LastModified { get; }
}

public record UserDeactivatedEvent(
    Guid UserId
) : IEvent;

public record UserActivatedEvent(
    Guid UserId
) : IEvent;