namespace Toolbox.Events;

public sealed class EventManager
{

    private readonly Dictionary<Type, List<Delegate>> _handlers = [];

    public void On<TEvent>(Action<TEvent> handler) where TEvent : Event
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);
        if (!_handlers.TryGetValue(type, out var handlers))
        {
            handlers = [];
            _handlers[type] = handlers;
        }

        handlers.Add(handler);
    }

    public void Register(Listener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        listener.Register(this);
    }

    public void Call<TEvent>(TEvent ev) where TEvent : Event
    {
        ArgumentNullException.ThrowIfNull(ev);
        if (!_handlers.TryGetValue(ev.GetType(), out var exactHandlers))
        {
            return;
        }

        foreach (var handler in exactHandlers)
        {
            ((Action<TEvent>)handler)(ev);
        }
    }

    public void Clear()
    {
        _handlers.Clear();
    }
}
