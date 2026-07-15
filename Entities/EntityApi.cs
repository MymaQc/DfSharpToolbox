using Dragonfly;

namespace Toolbox.Entities;

public static class EntityApi
{

    public static Vector3 GetPosition(World.Entity entity)
    {
        return entity.Position();
    }

    public static Rotation GetRotation(World.Entity entity)
    {
        return entity.Rotation();
    }

    public static World.EntityHandle GetHandle(World.Entity entity)
    {
        return entity.H();
    }

    public static void Close(World.Entity entity)
    {
        entity.Close();
    }

    public static bool IsClosed(World.EntityHandle handle)
    {
        return handle.Closed();
    }

    public static Guid GetUniqueId(World.EntityHandle handle)
    {
        return handle.UUID();
    }

    public static (World.Entity? Entity, bool Ok) GetEntity(World.EntityHandle handle, World.Tx tx)
    {
        return handle.Entity(tx);
    }

    public static World.Entity AddEntity(World.Tx tx, World.EntityHandle handle)
    {
        return tx.AddEntity(handle);
    }

    public static World.Entity AddEntity(World.Tx tx, World.EntityHandle handle, Vector3 position)
    {
        return tx.AddEntityAt(handle, position);
    }

    public static World.EntityHandle RemoveEntity(World.Tx tx, World.Entity entity)
    {
        return tx.RemoveEntity(entity);
    }

    public static IEnumerable<World.Entity> GetAll(World.Tx tx)
    {
        return tx.Entities();
    }

    public static IEnumerable<World.Entity> GetNearby(World.Tx tx, Vector3 center, double radius)
    {
        return tx.EntitiesWithin(Cube.Box(center.X - radius, center.Y - radius, center.Z - radius, center.X + radius, center.Y + radius, center.Z + radius));
    }
}
