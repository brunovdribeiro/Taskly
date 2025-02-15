namespace Domain.ValueObjects;

public record TaskId
{
    public Guid Value { get; }

    private TaskId(Guid value)
    {
        Value = value;
    }

    public static TaskId New() => new(Guid.NewGuid());
    public static TaskId From(Guid value) => new(value);
}