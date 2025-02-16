namespace Domain.Common;

public abstract class AggregateRoot<TId>
{
    private readonly List<IEvent> _events = new();

    public TId Id { get; protected set; }
    public int Version { get; protected set; }

    public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

    protected void AddEvent(
        IEvent @event
    )
    {
        _events.Add(@event);
        Apply(@event);
        Version++;
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected abstract void Apply(
        IEvent @event
    );

    public void Load(
        IEnumerable<IEvent> history
    )
    {
        foreach (var @event in history)
        {
            Apply(@event);
            Version++;
        }
    }
}