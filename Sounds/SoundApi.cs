using Dragonfly;

namespace Toolbox.Sounds;

public static class SoundApi
{
    private static World.Sound From(ToolboxSound sound)
    {
        return sound.ToDragonfly();
    }

    public static ToolboxSound From(World.Sound sound)
    {
        return ToolboxSound.From(sound);
    }

    public static void Play(World.Tx tx, Vector3 position, ToolboxSound sound)
    {
        Play(tx, position, From(sound));
    }

    private static void Play(World.Tx tx, Vector3 position, World.Sound sound)
    {
        ArgumentNullException.ThrowIfNull(tx);
        ArgumentNullException.ThrowIfNull(sound);
        tx.PlaySound(position, sound);
    }

    public static void Play(Player player, ToolboxSound sound)
    {
        Play(player, From(sound));
    }

    private static void Play(Player player, World.Sound sound)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(sound);
        player.PlaySound(sound);
    }

    public static ToolboxSound Attack(bool damage = true)
    {
        return ToolboxSound.Attack(damage);
    }

    public static ToolboxSound Fall(double distance)
    {
        return ToolboxSound.Fall(distance);
    }

    public static ToolboxSound BlockPlace(World.Block block)
    {
        return ToolboxSound.BlockPlace(block);
    }

    public static ToolboxSound BlockBreaking(World.Block block)
    {
        return ToolboxSound.BlockBreaking(block);
    }

    public static ToolboxSound DoorOpen(World.Block block)
    {
        return ToolboxSound.DoorOpen(block);
    }

    public static ToolboxSound DoorClose(World.Block block)
    {
        return ToolboxSound.DoorClose(block);
    }

    public static ToolboxSound TrapdoorOpen(World.Block block)
    {
        return ToolboxSound.TrapdoorOpen(block);
    }

    public static ToolboxSound TrapdoorClose(World.Block block)
    {
        return ToolboxSound.TrapdoorClose(block);
    }

    public static ToolboxSound FenceGateOpen(World.Block block)
    {
        return ToolboxSound.FenceGateOpen(block);
    }

    public static ToolboxSound FenceGateClose(World.Block block)
    {
        return ToolboxSound.FenceGateClose(block);
    }

    public static ToolboxSound Note(Sound.Instrument instrument, int pitch)
    {
        return ToolboxSound.Note(instrument, pitch);
    }

    public static ToolboxSound MusicDisc(Sound.DiscType discType)
    {
        return ToolboxSound.MusicDisc(discType);
    }

    public static ToolboxSound DecoratedPotInserted(double progress)
    {
        return ToolboxSound.DecoratedPotInserted(progress);
    }

    public static ToolboxSound ItemUseOn(World.Block block)
    {
        return ToolboxSound.ItemUseOn(block);
    }

    public static ToolboxSound EquipItem(World.Item item)
    {
        return ToolboxSound.EquipItem(item);
    }

    public static ToolboxSound BucketFill(World.Liquid liquid)
    {
        return ToolboxSound.BucketFill(liquid);
    }

    public static ToolboxSound BucketEmpty(World.Liquid liquid)
    {
        return ToolboxSound.BucketEmpty(liquid);
    }

    public static ToolboxSound CrossbowLoad(int stage, bool quickCharge = false)
    {
        return ToolboxSound.CrossbowLoad(stage, quickCharge);
    }

    public static ToolboxSound GoatHorn(Sound.Horn horn)
    {
        return ToolboxSound.GoatHorn(horn);
    }
}
