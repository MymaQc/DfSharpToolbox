using Dragonfly;
using Toolbox.Events.Player;
using Toolbox.Events.Player.Blocks;
using Toolbox.Events.Player.Chat;
using Toolbox.Events.Player.Combat;
using Toolbox.Events.Player.Commands;
using Toolbox.Events.Player.Diagnostics;
using Toolbox.Events.Player.Interaction;
using Toolbox.Events.Player.Items;
using Toolbox.Events.Player.Lifecycle;
using Toolbox.Events.Player.Movement;
using Toolbox.Events.Player.Network;
using Toolbox.Events.Player.State;
using Toolbox.Events.Packets;
using Toolbox.Events.Server;
using Toolbox.Events.World;
using Toolbox.Events.World.Blocks;
using Toolbox.Events.World.Entities;
using Toolbox.Events.World.Explosions;
using Toolbox.Events.World.Lifecycle;
using Toolbox.Events.World.Liquid;
using Toolbox.Events.World.Redstone;
using Toolbox.Events.World.Sound;
using DPacketContext = Dragonfly.Packet.Context;
using DPacket = Dragonfly.Packet.Packet;
using DPlayer = Dragonfly.Player;
using DWorld = Dragonfly.World;

namespace Toolbox.Events;

public sealed class ToolboxEventDispatcher(EventManager events)
{

    private EventManager Events { get; } = events;

    private TEvent Call<TEvent>(TEvent ev) where TEvent : Event
    {
        Events.Call(ev);
        return ev;
    }

    public (string Message, bool Allowed) Allow(Net.Addr address, Login.IdentityData identity, Login.ClientData client)
    {
        var ev = Call(new ConnectionPreLoginEvent(address, identity, client));
        return ev.IsCancelled() ? (ev.KickMessage, false) : (string.Empty, true);
    }

    public void OnJoin(DPlayer.Context ctx)
    {
        var ev = Call(new PlayerJoinEvent(ctx.Player()));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleMove(DPlayer.Context ctx, Vector3 newPos, Rotation newRot)
    {
        var ev = Call(new PlayerMoveEvent(ctx.Player(), newPos, newRot));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleJump(DPlayer player)
    {
        Call(new PlayerJumpEvent(player));
    }

    public void HandleTeleport(DPlayer.Context ctx, Vector3 pos)
    {
        var ev = Call(new PlayerTeleportEvent(ctx.Player(), pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleChangeWorld(DPlayer player, DWorld? before, DWorld after)
    {
        Call(new PlayerChangeWorldEvent(player, before, after));
    }

    public void HandleToggleSprint(DPlayer.Context ctx, bool after)
    {
        var ev = Call(new PlayerToggleSprintEvent(ctx.Player(), after));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleToggleSneak(DPlayer.Context ctx, bool after)
    {
        var ev = Call(new PlayerToggleSneakEvent(ctx.Player(), after));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleChat(DPlayer.Context ctx, ref string message)
    {
        var ev = Call(new PlayerChatEvent(ctx.Player(), message));
        message = ev.Message;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleFoodLoss(DPlayer.Context ctx, int from, ref int to)
    {
        var ev = Call(new PlayerFoodLossEvent(ctx.Player(), from, to));
        to = ev.To;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleHeal(DPlayer.Context ctx, ref double health, DWorld.HealingSource src)
    {
        var ev = Call(new PlayerHealEvent(ctx.Player(), health, src));
        health = ev.Amount;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleHurt(DPlayer.Context ctx, ref double damage, bool immune, ref TimeSpan attackImmunity, DWorld.DamageSource src)
    {
        var ev = Call(new PlayerDamageEvent(ctx.Player(), damage, immune, attackImmunity, src));
        damage = ev.Damage;
        attackImmunity = ev.AttackImmunity;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleDeath(DPlayer player, DWorld.DamageSource src, ref bool keepInv)
    {
        var ev = Call(new PlayerDeathEvent(player, src, keepInv));
        keepInv = ev.KeepInventory;
    }

    public void HandleRespawn(DPlayer player, ref Vector3 pos, ref DWorld world)
    {
        var ev = Call(new PlayerRespawnEvent(player, pos, world));
        pos = ev.Position;
        world = ev.World;
    }

    public void HandleSkinChange(DPlayer.Context ctx, ref Skin skin)
    {
        var ev = Call(new PlayerSkinChangeEvent(ctx.Player(), skin));
        skin = ev.Skin;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleFireExtinguish(DPlayer.Context ctx, Cube.Pos pos)
    {
        var ev = Call(new PlayerFireExtinguishEvent(ctx.Player(), pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleStartBreak(DPlayer.Context ctx, Cube.Pos pos)
    {
        var ev = Call(new PlayerStartBreakEvent(ctx.Player(), pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleBlockBreak(DPlayer.Context ctx, Cube.Pos pos, ref Item.Stack[] drops, ref int xp)
    {
        var ev = Call(new PlayerBlockBreakEvent(ctx.Player(), pos, drops, xp));
        drops = ev.Drops;
        xp = ev.Experience;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleBlockPlace(DPlayer.Context ctx, Cube.Pos pos, DWorld.Block block)
    {
        var ev = Call(new PlayerBlockPlaceEvent(ctx.Player(), pos, block));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleBlockPick(DPlayer.Context ctx, Cube.Pos pos, DWorld.Block block)
    {
        var ev = Call(new PlayerBlockPickEvent(ctx.Player(), pos, block));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemUse(DPlayer.Context ctx)
    {
        var ev = Call(new PlayerItemUseEvent(ctx.Player()));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemUseOnBlock(DPlayer.Context ctx, Cube.Pos pos, Cube.Face face, Vector3 clickPos)
    {
        var ev = Call(new PlayerItemUseOnBlockEvent(ctx.Player(), pos, face, clickPos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemUseOnEntity(DPlayer.Context ctx, DWorld.Entity entity)
    {
        var ev = Call(new PlayerItemUseOnEntityEvent(ctx.Player(), entity));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemRelease(DPlayer.Context ctx, Item.Stack item, TimeSpan duration)
    {
        var ev = Call(new PlayerItemReleaseEvent(ctx.Player(), item, duration));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemConsume(DPlayer.Context ctx, Item.Stack item)
    {
        var ev = Call(new PlayerItemConsumeEvent(ctx.Player(), item));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleAttackEntity(DPlayer.Context ctx, DWorld.Entity entity, ref double force, ref double height, ref bool critical)
    {
        var ev = Call(new PlayerAttackEntityEvent(ctx.Player(), entity, force, height, critical));
        force = ev.KnockbackForce;
        height = ev.KnockbackHeight;
        critical = ev.Critical;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleExperienceGain(DPlayer.Context ctx, ref int amount)
    {
        var ev = Call(new PlayerExperienceGainEvent(ctx.Player(), amount));
        amount = ev.Amount;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandlePunchAir(DPlayer.Context ctx)
    {
        var ev = Call(new PlayerPunchAirEvent(ctx.Player()));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleSignEdit(DPlayer.Context ctx, Cube.Pos pos, bool frontSide, string oldText, string newText)
    {
        var ev = Call(new PlayerSignEditEvent(ctx.Player(), pos, frontSide, oldText, newText));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleSleep(DPlayer.Context ctx, ref bool sendReminder)
    {
        var ev = Call(new PlayerSleepEvent(ctx.Player(), sendReminder));
        sendReminder = ev.SendReminder;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleLecternPageTurn(DPlayer.Context ctx, Cube.Pos pos, int oldPage, ref int newPage)
    {
        var ev = Call(new PlayerLecternPageTurnEvent(ctx.Player(), pos, oldPage, newPage));
        newPage = ev.NewPage;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemDamage(DPlayer.Context ctx, Item.Stack item, ref int damage)
    {
        var ev = Call(new PlayerItemDamageEvent(ctx.Player(), item, damage));
        damage = ev.Damage;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemPickup(DPlayer.Context ctx, ref Item.Stack item)
    {
        var ev = Call(new PlayerItemPickupEvent(ctx.Player(), item));
        item = ev.Item;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleHeldSlotChange(DPlayer.Context ctx, int from, int to)
    {
        var ev = Call(new PlayerHeldSlotChangeEvent(ctx.Player(), from, to));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleItemDrop(DPlayer.Context ctx, Item.Stack item)
    {
        var ev = Call(new PlayerItemDropEvent(ctx.Player(), item));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleTransfer(DPlayer.Context ctx, ref Net.UDPAddr addr)
    {
        var ev = Call(new PlayerTransferEvent(ctx.Player(), addr));
        addr = ev.Address;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleCommandExecution(DPlayer.Context ctx, Cmd.Command command, string[] args)
    {
        var ev = Call(new PlayerCommandExecutionEvent(ctx.Player(), command, args));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleQuit(DPlayer player)
    {
        Call(new PlayerQuitEvent(player));
    }

    public void HandleDiagnostics(DPlayer player, Session.Diagnostics diagnostics)
    {
        Call(new PlayerDiagnosticsEvent(player, diagnostics));
    }

    public void HandleLiquidFlow(DWorld.Context ctx, Cube.Pos from, Cube.Pos into, DWorld.Liquid liquid, DWorld.Block replaced)
    {
        var ev = Call(new LiquidFlowEvent(ctx.World(), from, into, liquid, replaced));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleLiquidDecay(DWorld.Context ctx, Cube.Pos pos, DWorld.Liquid before, DWorld.Liquid? after)
    {
        var ev = Call(new LiquidDecayEvent(ctx.World(), pos, before, after));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleLiquidHarden(DWorld.Context ctx, Cube.Pos hardenedPos, DWorld.Block liquidHardened, DWorld.Block otherLiquid, DWorld.Block newBlock)
    {
        var ev = Call(new LiquidHardenEvent(ctx.World(), hardenedPos, liquidHardened, otherLiquid, newBlock));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleSound(DWorld.Context ctx, DWorld.Sound sound, Vector3 pos)
    {
        var ev = Call(new WorldSoundEvent(ctx.World(), sound, pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleFireSpread(DWorld.Context ctx, Cube.Pos from, Cube.Pos to)
    {
        var ev = Call(new FireSpreadEvent(ctx.World(), from, to));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleBlockBurn(DWorld.Context ctx, Cube.Pos pos)
    {
        var ev = Call(new BlockBurnEvent(ctx.World(), pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleCropTrample(DWorld.Context ctx, Cube.Pos pos)
    {
        var ev = Call(new CropTrampleEvent(ctx.World(), pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleLeavesDecay(DWorld.Context ctx, Cube.Pos pos)
    {
        var ev = Call(new LeavesDecayEvent(ctx.World(), pos));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleEntitySpawn(DWorld.Tx tx, DWorld.Entity entity)
    {
        Call(new EntitySpawnEvent(tx.World(), entity));
    }

    public void HandleEntityDespawn(DWorld.Tx tx, DWorld.Entity entity)
    {
        Call(new EntityDespawnEvent(tx.World(), entity));
    }

    public void HandleExplosion(DWorld.Context ctx, Vector3 position, ref DWorld.Entity[] entities, ref Cube.Pos[] blocks, ref double itemDropChance, ref bool spawnFire)
    {
        var ev = Call(new WorldExplosionEvent(ctx.World(), position, entities, blocks, itemDropChance, spawnFire));
        entities = ev.Entities;
        blocks = ev.Blocks;
        itemDropChance = ev.ItemDropChance;
        spawnFire = ev.SpawnFire;
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleRedstoneUpdate(DWorld.Context ctx, DWorld.RedstoneUpdate update)
    {
        var ev = Call(new RedstoneUpdateEvent(ctx.World(), update));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleClose(DWorld.Tx tx)
    {
        Call(new WorldCloseEvent(tx.World()));
    }

    public void HandleClientPacket(DPacketContext ctx, DPacket packet)
    {
        var ev = Call(new ClientPacketEvent(ctx, packet));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }

    public void HandleServerPacket(DPacketContext ctx, DPacket packet)
    {
        var ev = Call(new ServerPacketEvent(ctx, packet));
        if (ev.IsCancelled())
        {
            ctx.Cancel();
        }
    }
}
