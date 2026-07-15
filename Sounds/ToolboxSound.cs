using Dragonfly;

namespace Toolbox.Sounds;

public readonly record struct ToolboxSound(World.Sound Value)
{
    public World.Sound ToDragonfly()
    {
        return Value;
    }

    public static ToolboxSound From(World.Sound sound)
    {
        ArgumentNullException.ThrowIfNull(sound);
        return new ToolboxSound(sound);
    }

    public static ToolboxSound AnvilBreak => From(new Sound.AnvilBreak());
    public static ToolboxSound AnvilLand => From(new Sound.AnvilLand());
    public static ToolboxSound AnvilUse => From(new Sound.AnvilUse());
    public static ToolboxSound ArrowHit => From(new Sound.ArrowHit());
    public static ToolboxSound BarrelClose => From(new Sound.BarrelClose());
    public static ToolboxSound BarrelOpen => From(new Sound.BarrelOpen());
    public static ToolboxSound BlastFurnaceCrackle => From(new Sound.BlastFurnaceCrackle());
    public static ToolboxSound BowShoot => From(new Sound.BowShoot());
    public static ToolboxSound Burning => From(new Sound.Burning());
    public static ToolboxSound Burp => From(new Sound.Burp());
    public static ToolboxSound CampfireCrackle => From(new Sound.CampfireCrackle());
    public static ToolboxSound ChestClose => From(new Sound.ChestClose());
    public static ToolboxSound ChestOpen => From(new Sound.ChestOpen());
    public static ToolboxSound Click => From(new Sound.Click());
    public static ToolboxSound ComposterEmpty => From(new Sound.ComposterEmpty());
    public static ToolboxSound ComposterFill => From(new Sound.ComposterFill());
    public static ToolboxSound ComposterFillLayer => From(new Sound.ComposterFillLayer());
    public static ToolboxSound ComposterReady => From(new Sound.ComposterReady());
    public static ToolboxSound CopperScraped => From(new Sound.CopperScraped());
    public static ToolboxSound CrossbowShoot => From(new Sound.CrossbowShoot());
    public static ToolboxSound DecoratedPotInsertFailed => From(new Sound.DecoratedPotInsertFailed());
    public static ToolboxSound Deny => From(new Sound.Deny());
    public static ToolboxSound DoorCrash => From(new Sound.DoorCrash());
    public static ToolboxSound Drowning => From(new Sound.Drowning());
    public static ToolboxSound EnderChestClose => From(new Sound.EnderChestClose());
    public static ToolboxSound EnderChestOpen => From(new Sound.EnderChestOpen());
    public static ToolboxSound Experience => From(new Sound.Experience());
    public static ToolboxSound Explosion => From(new Sound.Explosion());
    public static ToolboxSound FireCharge => From(new Sound.FireCharge());
    public static ToolboxSound FireExtinguish => From(new Sound.FireExtinguish());
    public static ToolboxSound FireworkBlast => From(new Sound.FireworkBlast());
    public static ToolboxSound FireworkHugeBlast => From(new Sound.FireworkHugeBlast());
    public static ToolboxSound FireworkLaunch => From(new Sound.FireworkLaunch());
    public static ToolboxSound FireworkTwinkle => From(new Sound.FireworkTwinkle());
    public static ToolboxSound Fizz => From(new Sound.Fizz());
    public static ToolboxSound FurnaceCrackle => From(new Sound.FurnaceCrackle());
    public static ToolboxSound GhastShoot => From(new Sound.GhastShoot());
    public static ToolboxSound GhastWarning => From(new Sound.GhastWarning());
    public static ToolboxSound GlassBreak => From(new Sound.GlassBreak());
    public static ToolboxSound Ignite => From(new Sound.Ignite());
    public static ToolboxSound ItemAdd => From(new Sound.ItemAdd());
    public static ToolboxSound ItemBreak => From(new Sound.ItemBreak());
    public static ToolboxSound ItemFrameRemove => From(new Sound.ItemFrameRemove());
    public static ToolboxSound ItemFrameRotate => From(new Sound.ItemFrameRotate());
    public static ToolboxSound ItemThrow => From(new Sound.ItemThrow());
    public static ToolboxSound LecternBookPlace => From(new Sound.LecternBookPlace());
    public static ToolboxSound LevelUp => From(new Sound.LevelUp());
    public static ToolboxSound LightningExplode => From(new Sound.LightningExplode());
    public static ToolboxSound LightningThunder => From(new Sound.LightningThunder());
    public static ToolboxSound MusicDiscEnd => From(new Sound.MusicDiscEnd());
    public static ToolboxSound Pop => From(new Sound.Pop());
    public static ToolboxSound PotionBrewed => From(new Sound.PotionBrewed());
    public static ToolboxSound PowerOff => From(new Sound.PowerOff());
    public static ToolboxSound PowerOn => From(new Sound.PowerOn());
    public static ToolboxSound SignWaxed => From(new Sound.SignWaxed());
    public static ToolboxSound SmokerCrackle => From(new Sound.SmokerCrackle());
    public static ToolboxSound StopUsingSpyglass => From(new Sound.StopUsingSpyglass());
    public static ToolboxSound TNT => From(new Sound.TNT());
    public static ToolboxSound Teleport => From(new Sound.Teleport());
    public static ToolboxSound Thunder => From(new Sound.Thunder());
    public static ToolboxSound Totem => From(new Sound.Totem());
    public static ToolboxSound UseSpyglass => From(new Sound.UseSpyglass());
    public static ToolboxSound WaxRemoved => From(new Sound.WaxRemoved());
    public static ToolboxSound WaxedSignFailedInteraction => From(new Sound.WaxedSignFailedInteraction());
    public static ToolboxSound ShulkerBoxOpen => From(new Sound.ShulkerBoxOpen());
    public static ToolboxSound ShulkerBoxClose => From(new Sound.ShulkerBoxClose());
    public static ToolboxSound EnderEyePlaced => From(new Sound.EnderEyePlaced());
    public static ToolboxSound EndPortalCreated => From(new Sound.EndPortalCreated());

    public static ToolboxSound Attack(bool damage = true)
    {
        return From(new Sound.Attack(damage));
    }

    public static ToolboxSound Fall(double distance)
    {
        return From(new Sound.Fall(distance));
    }

    public static ToolboxSound BlockPlace(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.BlockPlace(block));
    }

    public static ToolboxSound BlockBreaking(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.BlockBreaking(block));
    }

    public static ToolboxSound DoorOpen(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.DoorOpen(block));
    }

    public static ToolboxSound DoorClose(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.DoorClose(block));
    }

    public static ToolboxSound TrapdoorOpen(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.TrapdoorOpen(block));
    }

    public static ToolboxSound TrapdoorClose(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.TrapdoorClose(block));
    }

    public static ToolboxSound FenceGateOpen(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.FenceGateOpen(block));
    }

    public static ToolboxSound FenceGateClose(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.FenceGateClose(block));
    }

    public static ToolboxSound Note(Sound.Instrument instrument, int pitch)
    {
        return From(new Sound.Note(instrument, pitch));
    }

    public static ToolboxSound MusicDisc(Sound.DiscType discType)
    {
        return From(new Sound.MusicDiscPlay(discType));
    }

    public static ToolboxSound DecoratedPotInserted(double progress)
    {
        return From(new Sound.DecoratedPotInserted(progress));
    }

    public static ToolboxSound ItemUseOn(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return From(new Sound.ItemUseOn(block));
    }

    public static ToolboxSound EquipItem(World.Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return From(new Sound.EquipItem(item));
    }

    public static ToolboxSound BucketFill(World.Liquid liquid)
    {
        ArgumentNullException.ThrowIfNull(liquid);
        return From(new Sound.BucketFill(liquid));
    }

    public static ToolboxSound BucketEmpty(World.Liquid liquid)
    {
        ArgumentNullException.ThrowIfNull(liquid);
        return From(new Sound.BucketEmpty(liquid));
    }

    public static ToolboxSound CrossbowLoad(int stage, bool quickCharge = false)
    {
        return From(new Sound.CrossbowLoad(stage, quickCharge));
    }

    public static ToolboxSound GoatHorn(Sound.Horn horn)
    {
        return From(new Sound.GoatHorn(horn));
    }
}
