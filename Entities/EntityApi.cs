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

    public static void CloseEntity(World.Entity entity)
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

    public static IEnumerable<World.Entity> GetEntities(World.Tx tx)
    {
        return tx.Entities();
    }

    public static IEnumerable<World.Entity> GetNearbyEntities(World.Tx tx, Vector3 center, double radius)
    {
        return tx.EntitiesWithin(Cube.Box(center.X - radius, center.Y - radius, center.Z - radius, center.X + radius, center.Y + radius, center.Z + radius));
    }

    public static IEnumerable<World.Entity> GetEntitiesWithin(World.Tx tx, Cube.BBox box)
    {
        return tx.EntitiesWithin(box);
    }

    private static World.VisibilityLevel ToDragonflyVisibility(EntityVisibility visibility)
    {
        return visibility switch
        {
            EntityVisibility.Public => World.PublicVisibility(),
            EntityVisibility.ForceInvisible => World.EnforceInvisible(),
            EntityVisibility.ForceVisible => World.EnforceVisible(),
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null),
        };
    }

    public static void HideEntityFromViewer(Player viewer, World.Entity entity)
    {
        viewer.HideEntity(entity);
    }

    public static void ShowEntityToViewer(Player viewer, World.Entity entity)
    {
        viewer.ShowEntity(entity);
    }

    public static void ViewNameTag(Player viewer, World.Entity entity, string nameTag)
    {
        viewer.ViewNameTag(entity, nameTag);
    }

    public static void ViewPublicNameTag(Player viewer, World.Entity entity)
    {
        viewer.ViewPublicNameTag(entity);
    }

    public static void ViewScoreTag(Player viewer, World.Entity entity, string scoreTag)
    {
        viewer.ViewScoreTag(entity, scoreTag);
    }

    public static void ViewPublicScoreTag(Player viewer, World.Entity entity)
    {
        viewer.ViewPublicScoreTag(entity);
    }

    private static void ViewVisibility(Player viewer, World.Entity entity, World.VisibilityLevel level)
    {
        viewer.ViewVisibility(entity, level);
    }

    public static void ViewVisibility(Player viewer, World.Entity entity, EntityVisibility visibility)
    {
        ViewVisibility(viewer, entity, ToDragonflyVisibility(visibility));
    }

    public static void RemoveViewLayer(Player viewer, World.Entity entity)
    {
        viewer.RemoveViewLayer(entity);
    }
}
