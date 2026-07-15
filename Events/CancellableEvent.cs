namespace Toolbox.Events;

public abstract class CancellableEvent : Event
{
    private bool _cancelled;

    public bool IsCancelled()
    {
        return _cancelled;
    }

    public void Cancel()
    {
        _cancelled = true;
    }

    public void Uncancel()
    {
        _cancelled = false;
    }
}
