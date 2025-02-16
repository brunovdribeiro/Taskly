namespace Domain.ValueObjects;

public record UserId
{
    protected UserId() { }

    private UserId(
        Guid value
    )
    {
        Value = value;
    }

    public Guid Value { get; }

    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId From(
        Guid value
    )
    {
        return new UserId(value);
    }
}