using Dragonfly;

namespace Toolbox.Sounds;

public readonly record struct ToolboxSound(World.Sound Value)
{
    public World.Sound ToDragonfly()
    {
        return Value;
    }

    public static ToolboxSound FromDragonflySound(World.Sound sound)
    {
        ArgumentNullException.ThrowIfNull(sound);
        return new ToolboxSound(sound);
    }

    public static ToolboxSound AnvilBreak => FromDragonflySound(new Sound.AnvilBreak());
    public static ToolboxSound AnvilLand => FromDragonflySound(new Sound.AnvilLand());
    public static ToolboxSound AnvilUse => FromDragonflySound(new Sound.AnvilUse());
    public static ToolboxSound ArrowHit => FromDragonflySound(new Sound.ArrowHit());
    public static ToolboxSound BarrelClose => FromDragonflySound(new Sound.BarrelClose());
    public static ToolboxSound BarrelOpen => FromDragonflySound(new Sound.BarrelOpen());
    public static ToolboxSound BlastFurnaceCrackle => FromDragonflySound(new Sound.BlastFurnaceCrackle());
    public static ToolboxSound BowShoot => FromDragonflySound(new Sound.BowShoot());
    public static ToolboxSound Burning => FromDragonflySound(new Sound.Burning());
    public static ToolboxSound Burp => FromDragonflySound(new Sound.Burp());
    public static ToolboxSound CampfireCrackle => FromDragonflySound(new Sound.CampfireCrackle());
    public static ToolboxSound ChestClose => FromDragonflySound(new Sound.ChestClose());
    public static ToolboxSound ChestOpen => FromDragonflySound(new Sound.ChestOpen());
    public static ToolboxSound Click => FromDragonflySound(new Sound.Click());
    public static ToolboxSound ComposterEmpty => FromDragonflySound(new Sound.ComposterEmpty());
    public static ToolboxSound ComposterFill => FromDragonflySound(new Sound.ComposterFill());
    public static ToolboxSound ComposterFillLayer => FromDragonflySound(new Sound.ComposterFillLayer());
    public static ToolboxSound ComposterReady => FromDragonflySound(new Sound.ComposterReady());
    public static ToolboxSound CopperScraped => FromDragonflySound(new Sound.CopperScraped());
    public static ToolboxSound CrossbowShoot => FromDragonflySound(new Sound.CrossbowShoot());
    public static ToolboxSound DecoratedPotInsertFailed => FromDragonflySound(new Sound.DecoratedPotInsertFailed());
    public static ToolboxSound Deny => FromDragonflySound(new Sound.Deny());
    public static ToolboxSound DoorCrash => FromDragonflySound(new Sound.DoorCrash());
    public static ToolboxSound Drowning => FromDragonflySound(new Sound.Drowning());
    public static ToolboxSound EnderChestClose => FromDragonflySound(new Sound.EnderChestClose());
    public static ToolboxSound EnderChestOpen => FromDragonflySound(new Sound.EnderChestOpen());
    public static ToolboxSound Experience => FromDragonflySound(new Sound.Experience());
    public static ToolboxSound Explosion => FromDragonflySound(new Sound.Explosion());
    public static ToolboxSound FireCharge => FromDragonflySound(new Sound.FireCharge());
    public static ToolboxSound FireExtinguish => FromDragonflySound(new Sound.FireExtinguish());
    public static ToolboxSound FireworkBlast => FromDragonflySound(new Sound.FireworkBlast());
    public static ToolboxSound FireworkHugeBlast => FromDragonflySound(new Sound.FireworkHugeBlast());
    public static ToolboxSound FireworkLaunch => FromDragonflySound(new Sound.FireworkLaunch());
    public static ToolboxSound FireworkTwinkle => FromDragonflySound(new Sound.FireworkTwinkle());
    public static ToolboxSound Fizz => FromDragonflySound(new Sound.Fizz());
    public static ToolboxSound FurnaceCrackle => FromDragonflySound(new Sound.FurnaceCrackle());
    public static ToolboxSound GhastShoot => FromDragonflySound(new Sound.GhastShoot());
    public static ToolboxSound GhastWarning => FromDragonflySound(new Sound.GhastWarning());
    public static ToolboxSound GlassBreak => FromDragonflySound(new Sound.GlassBreak());
    public static ToolboxSound Ignite => FromDragonflySound(new Sound.Ignite());
    public static ToolboxSound ItemAdd => FromDragonflySound(new Sound.ItemAdd());
    public static ToolboxSound ItemBreak => FromDragonflySound(new Sound.ItemBreak());
    public static ToolboxSound ItemFrameRemove => FromDragonflySound(new Sound.ItemFrameRemove());
    public static ToolboxSound ItemFrameRotate => FromDragonflySound(new Sound.ItemFrameRotate());
    public static ToolboxSound ItemThrow => FromDragonflySound(new Sound.ItemThrow());
    public static ToolboxSound LecternBookPlace => FromDragonflySound(new Sound.LecternBookPlace());
    public static ToolboxSound LevelUp => FromDragonflySound(new Sound.LevelUp());
    public static ToolboxSound LightningExplode => FromDragonflySound(new Sound.LightningExplode());
    public static ToolboxSound LightningThunder => FromDragonflySound(new Sound.LightningThunder());
    public static ToolboxSound MusicDiscEnd => FromDragonflySound(new Sound.MusicDiscEnd());
    public static ToolboxSound Pop => FromDragonflySound(new Sound.Pop());
    public static ToolboxSound PotionBrewed => FromDragonflySound(new Sound.PotionBrewed());
    public static ToolboxSound PowerOff => FromDragonflySound(new Sound.PowerOff());
    public static ToolboxSound PowerOn => FromDragonflySound(new Sound.PowerOn());
    public static ToolboxSound SignWaxed => FromDragonflySound(new Sound.SignWaxed());
    public static ToolboxSound SmokerCrackle => FromDragonflySound(new Sound.SmokerCrackle());
    public static ToolboxSound StopUsingSpyglass => FromDragonflySound(new Sound.StopUsingSpyglass());
    public static ToolboxSound TNT => FromDragonflySound(new Sound.TNT());
    public static ToolboxSound Teleport => FromDragonflySound(new Sound.Teleport());
    public static ToolboxSound Thunder => FromDragonflySound(new Sound.Thunder());
    public static ToolboxSound Totem => FromDragonflySound(new Sound.Totem());
    public static ToolboxSound UseSpyglass => FromDragonflySound(new Sound.UseSpyglass());
    public static ToolboxSound WaxRemoved => FromDragonflySound(new Sound.WaxRemoved());
    public static ToolboxSound WaxedSignFailedInteraction => FromDragonflySound(new Sound.WaxedSignFailedInteraction());
    public static ToolboxSound ShulkerBoxOpen => FromDragonflySound(new Sound.ShulkerBoxOpen());
    public static ToolboxSound ShulkerBoxClose => FromDragonflySound(new Sound.ShulkerBoxClose());
    public static ToolboxSound EnderEyePlaced => FromDragonflySound(new Sound.EnderEyePlaced());
    public static ToolboxSound EndPortalCreated => FromDragonflySound(new Sound.EndPortalCreated());

    public static ToolboxSound CreateAttackSound(bool damage = true)
    {
        return FromDragonflySound(new Sound.Attack(damage));
    }

    public static ToolboxSound CreateFallSound(double distance)
    {
        return FromDragonflySound(new Sound.Fall(distance));
    }

    public static ToolboxSound CreateBlockPlaceSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.BlockPlace(block));
    }

    public static ToolboxSound CreateBlockBreakingSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.BlockBreaking(block));
    }

    public static ToolboxSound CreateDoorOpenSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.DoorOpen(block));
    }

    public static ToolboxSound CreateDoorCloseSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.DoorClose(block));
    }

    public static ToolboxSound CreateTrapdoorOpenSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.TrapdoorOpen(block));
    }

    public static ToolboxSound CreateTrapdoorCloseSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.TrapdoorClose(block));
    }

    public static ToolboxSound CreateFenceGateOpenSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.FenceGateOpen(block));
    }

    public static ToolboxSound CreateFenceGateCloseSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.FenceGateClose(block));
    }

    public static ToolboxSound CreateNoteSound(Sound.Instrument instrument, int pitch)
    {
        return FromDragonflySound(new Sound.Note(instrument, pitch));
    }

    public static ToolboxSound CreateMusicDiscSound(Sound.DiscType discType)
    {
        return FromDragonflySound(new Sound.MusicDiscPlay(discType));
    }

    public static ToolboxSound CreateDecoratedPotInsertedSound(double progress)
    {
        return FromDragonflySound(new Sound.DecoratedPotInserted(progress));
    }

    public static ToolboxSound CreateItemUseOnSound(World.Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        return FromDragonflySound(new Sound.ItemUseOn(block));
    }

    public static ToolboxSound CreateEquipItemSound(World.Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return FromDragonflySound(new Sound.EquipItem(item));
    }

    public static ToolboxSound CreateBucketFillSound(World.Liquid liquid)
    {
        ArgumentNullException.ThrowIfNull(liquid);
        return FromDragonflySound(new Sound.BucketFill(liquid));
    }

    public static ToolboxSound CreateBucketEmptySound(World.Liquid liquid)
    {
        ArgumentNullException.ThrowIfNull(liquid);
        return FromDragonflySound(new Sound.BucketEmpty(liquid));
    }

    public static ToolboxSound CreateCrossbowLoadSound(int stage, bool quickCharge = false)
    {
        return FromDragonflySound(new Sound.CrossbowLoad(stage, quickCharge));
    }

    public static ToolboxSound CreateGoatHornSound(Sound.Horn horn)
    {
        return FromDragonflySound(new Sound.GoatHorn(horn));
    }
}
