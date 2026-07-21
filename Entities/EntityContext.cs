using Dragonfly;

namespace Toolbox.Entities;

public class EntityContext
{
    private readonly World.EntityData _data;
    private readonly Action _despawn;

    internal EntityContext(
        World.Tx transaction,
        World.EntityHandle handle,
        World.EntityData data,
        EntityProperties properties,
        Action despawn)
    {
        Transaction = transaction;
        Handle = handle;
        _data = data;
        Properties = properties;
        _despawn = despawn;
    }

    public World.Tx Transaction { get; }
    public World.EntityHandle Handle { get; }
    public EntityProperties Properties { get; }

    public Vector3 Position
    {
        get => _data.Pos;
        set => _data.Pos = value;
    }

    public Vector3 Velocity
    {
        get => _data.Vel;
        set => _data.Vel = value;
    }

    public Rotation Rotation
    {
        get => _data.Rot;
        set => _data.Rot = value;
    }

    public string NameTag
    {
        get => _data.Name;
        set => _data.Name = value ?? string.Empty;
    }

    public TimeSpan FireDuration
    {
        get => _data.FireDuration;
        set => _data.FireDuration = value < TimeSpan.Zero ? TimeSpan.Zero : value;
    }

    public TimeSpan Age
    {
        get => _data.Age;
        set => _data.Age = value < TimeSpan.Zero ? TimeSpan.Zero : value;
    }

    public void Despawn() => _despawn();
}

public sealed class EntityTickContext : EntityContext
{
    internal EntityTickContext(
        World.Tx transaction,
        World.EntityHandle handle,
        World.EntityData data,
        EntityProperties properties,
        Action despawn,
        long currentTick,
        long ticksLived)
        : base(transaction, handle, data, properties, despawn)
    {
        CurrentTick = currentTick;
        TicksLived = ticksLived;
    }

    public long CurrentTick { get; }
    public long TicksLived { get; }
}
