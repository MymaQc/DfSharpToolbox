using Dragonfly;

namespace Toolbox.Blocks;

public readonly record struct CustomBlockBox(float MinX, float MinY, float MinZ, float MaxX, float MaxY, float MaxZ)
{
    public static CustomBlockBox Full { get; } = new(0, 0, 0, 1, 1, 1);
}

public readonly record struct CustomBlockVector(float X, float Y, float Z);

public enum CustomBlockTextureFace { All, Up, Down, North, South, East, West }

public sealed class CustomBlockInteractionContext(Player.Context context, Cube.Pos position, Cube.Face face, Vector3 clickPosition)
{
    public Player Player => context.Player();
    public World.Tx Transaction => context;
    public Cube.Pos Position => position;
    public Cube.Face Face => face;
    public Vector3 ClickPosition => clickPosition;
    public bool Cancelled => context.Cancelled();
    public void Cancel() => context.Cancel();
}

public sealed class CustomBlockPlacementContext(Player.Context context, Cube.Pos position, Block.Custom block)
{
    public Player Player => context.Player();
    public World.Tx Transaction => context;
    public Cube.Pos Position => position;
    public Block.Custom Block => block;
    public bool Cancelled => context.Cancelled();
    public void Cancel() => context.Cancel();
}
