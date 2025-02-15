namespace Domain.Common;

public interface IEvent
{
    Guid EventId => Guid.NewGuid();
    DateTime Timestamp => DateTime.UtcNow;
}