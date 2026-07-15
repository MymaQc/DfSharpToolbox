namespace Toolbox.Events;

public abstract class Event
{
    private DateTimeOffset CalledAt { get; } = DateTimeOffset.UtcNow;

    public DateTimeOffset GetCalledAt()
    {
        return CalledAt;
    }
}
