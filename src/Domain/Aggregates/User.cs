using Domain.Common;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public class User : AggregateRoot<UserId>
{
    private User() { }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModified { get; private set; } = DateTime.UtcNow;

    public static User Create(
        UserId id,
        string email,
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new UserDomainException("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new UserDomainException("Name cannot be empty");

        var now = DateTime.UtcNow;

        var user = new User
        {
            Id = id,
            Email = email,
            Name = name,
            IsActive = true,
            CreatedAt = now
        };

        user.AddEvent(new UserCreatedEvent(id, email, name, true, now, now));
        return user;
    }

    public static User Create(
        UserId id,
        string email,
        string name,
        bool isActive,
        DateTime createdAt,
        DateTime? lastModified
    )
    {
        var user = Create(id, email, name);

        user.IsActive = isActive;
        user.CreatedAt = createdAt;
        user.LastModified = lastModified;

        user.AddEvent(new UserCreatedEvent(id, email, name, isActive, createdAt, lastModified ?? createdAt));
        return user;
    }

    protected override void Apply(
        IEvent @event
    )
    {
        switch (@event)
        {
            case UserCreatedEvent e:
                Id = UserId.From(e.UserId);
                Email = e.Email;
                Name = e.Name;
                IsActive = true;
                CreatedAt = DateTime.UtcNow;
                break;
            case UserDeactivatedEvent:
                IsActive = false;
                LastModified = DateTime.UtcNow;
                break;
            case UserActivatedEvent:
                IsActive = true;
                LastModified = DateTime.UtcNow;
                break;
        }
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        LastModified = DateTime.UtcNow;
        AddEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        if (IsActive)
            return;

        LastModified = DateTime.UtcNow;
        AddEvent(new UserActivatedEvent(Id));
    }
}