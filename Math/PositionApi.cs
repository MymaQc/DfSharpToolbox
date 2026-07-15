using Dragonfly;

namespace Toolbox.Math;

public static class PositionApi
{
    public static bool IsFinite(Vector3 position)
    {
        return double.IsFinite(position.X) && double.IsFinite(position.Y) && double.IsFinite(position.Z);
    }

    public static bool IsFinite(Rotation rotation)
    {
        return double.IsFinite(rotation.Yaw) && double.IsFinite(rotation.Pitch);
    }

    public static bool IsInsideBorder(Vector3 position, double border, double minY = -64, double maxY = 320)
    {
        return IsFinite(position) && position.Y >= minY && position.Y <= maxY && System.Math.Abs(position.X) <= border && System.Math.Abs(position.Z) <= border;
    }

    public static Cube.Pos ToBlockPosition(Vector3 position)
    {
        return Cube.PosFromVec3(position);
    }

    public static Cube.Pos AddToBlockPosition(Cube.Pos position, int x = 0, int y = 0, int z = 0)
    {
        return position.Add(new Cube.Pos(x, y, z));
    }

    public static Cube.Pos GetSide(Cube.Pos position, Cube.Face face)
    {
        return position.Side(face);
    }

    public static Vector3 AddToVector(Vector3 position, double x = 0, double y = 0, double z = 0)
    {
        return new Vector3(position.X + x, position.Y + y, position.Z + z);
    }
}
