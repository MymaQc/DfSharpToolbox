using Dragonfly;
using Toolbox.Diagnostics;

namespace Toolbox.Entities;

public sealed class CustomEntityType<TMarker> : World.NetworkEntityType
{
    private readonly EntityProperties _defaults = new();
    private readonly List<TickHandler> _tickHandlers = [];
    private Func<EntityProperties, Cube.BBox> _boundingBox =
        _ => Cube.Box(-0.3, 0, -0.3, 0.3, 1.8, 0.3);
    private Action<EntityContext>? _closeHandler;

    internal CustomEntityType(string identifier, string networkIdentifier)
    {
        Identifier = ValidateIdentifier(identifier, nameof(identifier));
        NetworkIdentifier = ValidateIdentifier(networkIdentifier, nameof(networkIdentifier));
    }

    public string Identifier { get; }
    public string NetworkIdentifier { get; }
    public bool IsRegistered { get; private set; }

    public CustomEntityType<TMarker> BoundingBox(Cube.BBox boundingBox)
    {
        EnsureNotRegistered();
        _boundingBox = _ => boundingBox;
        return this;
    }

    public CustomEntityType<TMarker> BoundingBox(Func<EntityProperties, Cube.BBox> boundingBox)
    {
        EnsureNotRegistered();
        _boundingBox = boundingBox ?? throw new ArgumentNullException(nameof(boundingBox));
        return this;
    }

    public CustomEntityType<TMarker> DefaultProperty<T>(string name, T value)
    {
        EnsureNotRegistered();
        _defaults.Set(name, value);
        return this;
    }

    public CustomEntityType<TMarker> OnTick(Action<EntityTickContext> handler) => TickEvery(1, handler);

    public CustomEntityType<TMarker> TickEvery(int interval, Action<EntityTickContext> handler)
    {
        EnsureNotRegistered();
        if (interval < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(interval), "Tick interval must be at least 1.");
        }
        ArgumentNullException.ThrowIfNull(handler);
        _tickHandlers.Add(new TickHandler(interval, handler));
        return this;
    }

    public CustomEntityType<TMarker> OnClose(Action<EntityContext> handler)
    {
        EnsureNotRegistered();
        _closeHandler += handler ?? throw new ArgumentNullException(nameof(handler));
        return this;
    }

    public CustomEntityType<TMarker> Register()
    {
        EnsureNotRegistered();
        World.RegisterEntityType(this);
        IsRegistered = true;
        return this;
    }

    public EntitySpawnBuilder<TMarker> Spawn(World.Tx transaction) => new(this, transaction);

    public World.Entity Open(World.Tx tx, World.EntityHandle handle, World.EntityData data)
    {
        var properties = data.Data as EntityProperties ?? _defaults.Clone();
        data.Data = properties;
        return new ManagedEntity(this, tx, handle, data, properties);
    }

    public string EncodeEntity() => Identifier;

    public string NetworkEncodeEntity() => NetworkIdentifier;

    public Cube.BBox BBox(World.Entity entity)
    {
        return entity is ManagedEntity managed
            ? _boundingBox(managed.Properties)
            : _boundingBox(_defaults);
    }

    public void DecodeNBT(Dictionary<string, object?> values, World.EntityData data)
    {
        ArgumentNullException.ThrowIfNull(values);
        var properties = _defaults.Clone();
        properties.Merge(values);
        data.Data = properties;
    }

    public Dictionary<string, object?> EncodeNBT(World.EntityData data)
    {
        return data.Data is EntityProperties properties ? properties.Snapshot() : _defaults.Snapshot();
    }

    internal EntityProperties CreateProperties() => _defaults.Clone();

    internal void EnsureRegistered()
    {
        if (!IsRegistered)
        {
            throw new InvalidOperationException($"Entity type '{Identifier}' must be registered in the plugin constructor before it can be spawned.");
        }
    }

    private void EnsureNotRegistered()
    {
        if (IsRegistered)
        {
            throw new InvalidOperationException($"Entity type '{Identifier}' can no longer be changed after registration.");
        }
    }

    private static string ValidateIdentifier(string identifier, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier, parameterName);
        var separator = identifier.IndexOf(':');
        if (separator <= 0 || separator == identifier.Length - 1 ||
            identifier.IndexOf(':', separator + 1) >= 0 ||
            !IsValidIdentifierPart(identifier.AsSpan(0, separator), allowSlash: false) ||
            !IsValidIdentifierPart(identifier.AsSpan(separator + 1), allowSlash: true))
        {
            throw new ArgumentException(
                "An entity identifier must use the lowercase namespace:name format.",
                parameterName);
        }
        return identifier;
    }

    private static bool IsValidIdentifierPart(ReadOnlySpan<char> value, bool allowSlash)
    {
        if (value.IsEmpty || !IsLowercaseLetterOrDigit(value[0]))
        {
            return false;
        }
        foreach (var character in value[1..])
        {
            if (!IsLowercaseLetterOrDigit(character) && character is not '_' and not '.' and not '-' &&
                (!allowSlash || character != '/'))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsLowercaseLetterOrDigit(char character) =>
        character is >= 'a' and <= 'z' or >= '0' and <= '9';

    private sealed class ManagedEntity(
        CustomEntityType<TMarker> type,
        World.Tx transaction,
        World.EntityHandle handle,
        World.EntityData data,
        EntityProperties properties) : World.TickerEntity
    {
        private const long GameTickTicks = TimeSpan.TicksPerSecond / 20;
        private bool _closed;
        private long _ticksLived = System.Math.Max(0, data.Age.Ticks / GameTickTicks);

        internal EntityProperties Properties { get; } = properties;

        public void Close()
        {
            if (_closed)
            {
                return;
            }
            _closed = true;
            if (type._closeHandler is { } handler)
            {
                ToolboxLogger.Try(() => handler(CreateContext(transaction)), $"Entity close ({type.Identifier})");
            }
            transaction.RemoveEntity(this).Close();
        }

        public World.EntityHandle H() => handle;
        public Vector3 Position() => data.Pos;
        public Rotation Rotation() => data.Rot;

        public void Tick(World.Tx tx, long current)
        {
            _ticksLived++;
            data.Age += TimeSpan.FromMilliseconds(50);
            if (data.FireDuration > TimeSpan.Zero)
            {
                var remaining = data.FireDuration - TimeSpan.FromMilliseconds(50);
                data.FireDuration = remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
            }

            foreach (var tickHandler in type._tickHandlers)
            {
                if (_ticksLived % tickHandler.Interval != 0)
                {
                    continue;
                }
                var context = new EntityTickContext(
                    tx, handle, data, Properties, Close, current, _ticksLived);
                ToolboxLogger.Try(
                    () => tickHandler.Handler(context),
                    $"Entity tick ({type.Identifier})");
                if (_closed)
                {
                    break;
                }
            }
        }

        private EntityContext CreateContext(World.Tx tx) =>
            new(tx, handle, data, Properties, Close);
    }

    private sealed record TickHandler(int Interval, Action<EntityTickContext> Handler);
}
