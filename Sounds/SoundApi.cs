using Dragonfly;

namespace Toolbox.Sounds;

public static class SoundApi
{
    private static World.Sound From(ToolboxSound sound)
    {
        return sound.ToDragonfly();
    }

    public static ToolboxSound FromDragonflySound(World.Sound sound)
    {
        return ToolboxSound.FromDragonflySound(sound);
    }

    public static void PlaySound(World.Tx tx, Vector3 position, ToolboxSound sound)
    {
        PlayDragonflySound(tx, position, From(sound));
    }

    private static void PlayDragonflySound(World.Tx tx, Vector3 position, World.Sound sound)
    {
        ArgumentNullException.ThrowIfNull(tx);
        ArgumentNullException.ThrowIfNull(sound);
        tx.PlaySound(position, sound);
    }

    public static void PlaySound(Player player, ToolboxSound sound)
    {
        PlayDragonflySound(player, From(sound));
    }

    private static void PlayDragonflySound(Player player, World.Sound sound)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(sound);
        player.PlaySound(sound);
    }

    public static ToolboxSound CreateAttackSound(bool damage = true)
    {
        return ToolboxSound.CreateAttackSound(damage);
    }

    public static ToolboxSound CreateFallSound(double distance)
    {
        return ToolboxSound.CreateFallSound(distance);
    }

    public static ToolboxSound CreateBlockPlaceSound(World.Block block)
    {
        return ToolboxSound.CreateBlockPlaceSound(block);
    }

    public static ToolboxSound CreateBlockBreakingSound(World.Block block)
    {
        return ToolboxSound.CreateBlockBreakingSound(block);
    }

    public static ToolboxSound CreateDoorOpenSound(World.Block block)
    {
        return ToolboxSound.CreateDoorOpenSound(block);
    }

    public static ToolboxSound CreateDoorCloseSound(World.Block block)
    {
        return ToolboxSound.CreateDoorCloseSound(block);
    }

    public static ToolboxSound CreateTrapdoorOpenSound(World.Block block)
    {
        return ToolboxSound.CreateTrapdoorOpenSound(block);
    }

    public static ToolboxSound CreateTrapdoorCloseSound(World.Block block)
    {
        return ToolboxSound.CreateTrapdoorCloseSound(block);
    }

    public static ToolboxSound CreateFenceGateOpenSound(World.Block block)
    {
        return ToolboxSound.CreateFenceGateOpenSound(block);
    }

    public static ToolboxSound CreateFenceGateCloseSound(World.Block block)
    {
        return ToolboxSound.CreateFenceGateCloseSound(block);
    }

    public static ToolboxSound CreateNoteSound(Sound.Instrument instrument, int pitch)
    {
        return ToolboxSound.CreateNoteSound(instrument, pitch);
    }

    public static ToolboxSound CreateMusicDiscSound(Sound.DiscType discType)
    {
        return ToolboxSound.CreateMusicDiscSound(discType);
    }

    public static ToolboxSound CreateDecoratedPotInsertedSound(double progress)
    {
        return ToolboxSound.CreateDecoratedPotInsertedSound(progress);
    }

    public static ToolboxSound CreateItemUseOnSound(World.Block block)
    {
        return ToolboxSound.CreateItemUseOnSound(block);
    }

    public static ToolboxSound CreateEquipItemSound(World.Item item)
    {
        return ToolboxSound.CreateEquipItemSound(item);
    }

    public static ToolboxSound CreateBucketFillSound(World.Liquid liquid)
    {
        return ToolboxSound.CreateBucketFillSound(liquid);
    }

    public static ToolboxSound CreateBucketEmptySound(World.Liquid liquid)
    {
        return ToolboxSound.CreateBucketEmptySound(liquid);
    }

    public static ToolboxSound CreateCrossbowLoadSound(int stage, bool quickCharge = false)
    {
        return ToolboxSound.CreateCrossbowLoadSound(stage, quickCharge);
    }

    public static ToolboxSound CreateGoatHornSound(Sound.Horn horn)
    {
        return ToolboxSound.CreateGoatHornSound(horn);
    }
}
