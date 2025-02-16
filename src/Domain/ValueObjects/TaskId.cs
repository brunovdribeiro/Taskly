namespace Domain.ValueObjects;

public record TaskId
{
    private TaskId(
        Guid value
    )
    {
        Value = value;
    }

    public Guid Value { get; }

    public static TaskId New()
    {
        return new TaskId(Guid.NewGuid());
    }

    public static TaskId From(
        Guid value
    )
    {
        return new TaskId(value);
    }
}