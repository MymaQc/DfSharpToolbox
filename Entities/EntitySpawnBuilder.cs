using Dragonfly;

namespace Toolbox.Entities;

public sealed record SpawnedEntity(
    World.EntityHandle Handle,
    World.Entity Entity,
    EntityProperties Properties)
{
    public bool Despawn(World.Tx transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        var (entity, found) = Handle.Entity(transaction);
        if (!found || entity is null)
        {
            return false;
        }
        entity.Close();
        return true;
    }
}

public sealed class EntitySpawnBuilder<TMarker>
{
    private readonly CustomEntityType<TMarker> _type;
    private readonly World.Tx _transaction;
    private readonly World.EntitySpawnOpts _options = new();
    private readonly EntityProperties _properties;
    private TimeSpan _fireDuration;
    private TimeSpan _age;
    private bool _spawned;

    internal EntitySpawnBuilder(CustomEntityType<TMarker> type, World.Tx transaction)
    {
        _type = type;
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        _properties = type.CreateProperties();
    }

    public EntitySpawnBuilder<TMarker> At(Vector3 position)
    {
        _options.Position = position;
        return this;
    }

    public EntitySpawnBuilder<TMarker> Rotated(Rotation rotation)
    {
        _options.Rotation = rotation;
        return this;
    }

    public EntitySpawnBuilder<TMarker> Moving(Vector3 velocity)
    {
        _options.Velocity = velocity;
        return this;
    }

    public EntitySpawnBuilder<TMarker> UniqueId(Guid id)
    {
        _options.ID = id;
        return this;
    }

    public EntitySpawnBuilder<TMarker> NameTag(string nameTag)
    {
        _options.NameTag = nameTag ?? string.Empty;
        return this;
    }

    public EntitySpawnBuilder<TMarker> OnFire(TimeSpan duration)
    {
        _fireDuration = duration < TimeSpan.Zero ? TimeSpan.Zero : duration;
        return this;
    }

    public EntitySpawnBuilder<TMarker> WithAge(TimeSpan age)
    {
        _age = age < TimeSpan.Zero ? TimeSpan.Zero : age;
        return this;
    }

    public EntitySpawnBuilder<TMarker> Property<T>(string name, T value)
    {
        _properties.Set(name, value);
        return this;
    }

    public SpawnedEntity Create()
    {
        if (_spawned)
        {
            throw new InvalidOperationException("This entity spawn builder has already been used.");
        }
        _type.EnsureRegistered();
        _spawned = true;

        var handle = _options.New(_type, new SpawnConfig(_properties, _fireDuration, _age));
        var entity = _transaction.AddEntity(handle);
        return new SpawnedEntity(handle, entity, _properties);
    }

    private sealed class SpawnConfig(
        EntityProperties properties,
        TimeSpan fireDuration,
        TimeSpan age) : World.EntityConfig
    {
        public void Apply(World.EntityData data)
        {
            data.Data = properties;
            data.FireDuration = fireDuration;
            data.Age = age;
        }
    }
}
