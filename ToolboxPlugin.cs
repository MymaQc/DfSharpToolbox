using Dragonfly;
using Toolbox.Diagnostics;
using Toolbox.Events;
using DPacketContext = Dragonfly.Packet.Context;
using DPacket = Dragonfly.Packet.Packet;

namespace Toolbox;

public abstract class ToolboxPlugin : Plugin
{
    private readonly ToolboxEventDispatcher _eventDispatcher;
    private readonly EventManager _events;
    private bool _disabled;

    protected ToolboxPlugin()
    {
        ToolboxLogger.Initialize();
        _events = new EventManager();
        _eventDispatcher = new ToolboxEventDispatcher(_events);
    }

    public EventManager GetEvents()
    {
        return _events;
    }

    public bool IsToolboxDisabled()
    {
        return _disabled;
    }

    public override void OnDisable()
    {
        DisableToolbox();
    }

    private void DisableToolbox()
    {
        if (_disabled)
        {
            return;
        }

        _events.ClearListeners();
        _disabled = true;
    }

    public override (string Message, bool Allowed) Allow(Net.Addr addr, Login.IdentityData d, Login.ClientData c)
    {
        return _eventDispatcher.Allow(addr, d, c);
    }

    public override void OnJoin(Player.Context ctx)
    {
        _eventDispatcher.OnJoin(ctx);
    }

    public override void HandleMove(Player.Context ctx, Vector3 newPos, Rotation newRot)
    {
        _eventDispatcher.HandleMove(ctx, newPos, newRot);
    }

    public override void HandleJump(Player player)
    {
        _eventDispatcher.HandleJump(player);
    }

    public override void HandleTeleport(Player.Context ctx, Vector3 pos)
    {
        _eventDispatcher.HandleTeleport(ctx, pos);
    }

    public override void HandleChangeWorld(Player player, World? before, World after)
    {
        _eventDispatcher.HandleChangeWorld(player, before, after);
    }

    public override void HandleToggleSprint(Player.Context ctx, bool after)
    {
        _eventDispatcher.HandleToggleSprint(ctx, after);
    }

    public override void HandleToggleSneak(Player.Context ctx, bool after)
    {
        _eventDispatcher.HandleToggleSneak(ctx, after);
    }

    public override void HandleChat(Player.Context ctx, ref string message)
    {
        _eventDispatcher.HandleChat(ctx, ref message);
    }

    public override void HandleFoodLoss(Player.Context ctx, int from, ref int to)
    {
        _eventDispatcher.HandleFoodLoss(ctx, from, ref to);
    }

    public override void HandleHeal(Player.Context ctx, ref double health, World.HealingSource src)
    {
        _eventDispatcher.HandleHeal(ctx, ref health, src);
    }

    public override void HandleHurt(Player.Context ctx, ref double damage, bool immune, ref TimeSpan attackImmunity, World.DamageSource src)
    {
        _eventDispatcher.HandleHurt(ctx, ref damage, immune, ref attackImmunity, src);
    }

    public override void HandleDeath(Player player, World.DamageSource src, ref bool keepInv)
    {
        _eventDispatcher.HandleDeath(player, src, ref keepInv);
    }

    public override void HandleRespawn(Player player, ref Vector3 pos, ref World world)
    {
        _eventDispatcher.HandleRespawn(player, ref pos, ref world);
    }

    public override void HandleSkinChange(Player.Context ctx, ref Skin skin)
    {
        _eventDispatcher.HandleSkinChange(ctx, ref skin);
    }

    public override void HandleFireExtinguish(Player.Context ctx, Cube.Pos pos)
    {
        _eventDispatcher.HandleFireExtinguish(ctx, pos);
    }

    public override void HandleStartBreak(Player.Context ctx, Cube.Pos pos)
    {
        _eventDispatcher.HandleStartBreak(ctx, pos);
    }

    public override void HandleBlockBreak(Player.Context ctx, Cube.Pos pos, ref Item.Stack[] drops, ref int xp)
    {
        _eventDispatcher.HandleBlockBreak(ctx, pos, ref drops, ref xp);
    }

    public override void HandleBlockPlace(Player.Context ctx, Cube.Pos pos, World.Block block)
    {
        _eventDispatcher.HandleBlockPlace(ctx, pos, block);
    }

    public override void HandleBlockPick(Player.Context ctx, Cube.Pos pos, World.Block block)
    {
        _eventDispatcher.HandleBlockPick(ctx, pos, block);
    }

    public override void HandleItemUse(Player.Context ctx)
    {
        _eventDispatcher.HandleItemUse(ctx);
    }

    public override void HandleItemUseOnBlock(Player.Context ctx, Cube.Pos pos, Cube.Face face, Vector3 clickPos)
    {
        _eventDispatcher.HandleItemUseOnBlock(ctx, pos, face, clickPos);
    }

    public override void HandleItemUseOnEntity(Player.Context ctx, World.Entity entity)
    {
        _eventDispatcher.HandleItemUseOnEntity(ctx, entity);
    }

    public override void HandleItemRelease(Player.Context ctx, Item.Stack item, TimeSpan duration)
    {
        _eventDispatcher.HandleItemRelease(ctx, item, duration);
    }

    public override void HandleItemConsume(Player.Context ctx, Item.Stack item)
    {
        _eventDispatcher.HandleItemConsume(ctx, item);
    }

    public override void HandleAttackEntity(Player.Context ctx, World.Entity entity, ref double force, ref double height, ref bool critical)
    {
        _eventDispatcher.HandleAttackEntity(ctx, entity, ref force, ref height, ref critical);
    }

    public override void HandleExperienceGain(Player.Context ctx, ref int amount)
    {
        _eventDispatcher.HandleExperienceGain(ctx, ref amount);
    }

    public override void HandlePunchAir(Player.Context ctx)
    {
        _eventDispatcher.HandlePunchAir(ctx);
    }

    public override void HandleSignEdit(Player.Context ctx, Cube.Pos pos, bool frontSide, string oldText, string newText)
    {
        _eventDispatcher.HandleSignEdit(ctx, pos, frontSide, oldText, newText);
    }

    public override void HandleSleep(Player.Context ctx, ref bool sendReminder)
    {
        _eventDispatcher.HandleSleep(ctx, ref sendReminder);
    }

    public override void HandleLecternPageTurn(Player.Context ctx, Cube.Pos pos, int oldPage, ref int newPage)
    {
        _eventDispatcher.HandleLecternPageTurn(ctx, pos, oldPage, ref newPage);
    }

    public override void HandleItemDamage(Player.Context ctx, Item.Stack item, ref int damage)
    {
        _eventDispatcher.HandleItemDamage(ctx, item, ref damage);
    }

    public override void HandleItemPickup(Player.Context ctx, ref Item.Stack item)
    {
        _eventDispatcher.HandleItemPickup(ctx, ref item);
    }

    public override void HandleHeldSlotChange(Player.Context ctx, int from, int to)
    {
        _eventDispatcher.HandleHeldSlotChange(ctx, from, to);
    }

    public override void HandleItemDrop(Player.Context ctx, Item.Stack item)
    {
        _eventDispatcher.HandleItemDrop(ctx, item);
    }

    public override void HandleTransfer(Player.Context ctx, ref Net.UDPAddr addr)
    {
        _eventDispatcher.HandleTransfer(ctx, ref addr);
    }

    public override void HandleCommandExecution(Player.Context ctx, Cmd.Command command, string[] args)
    {
        _eventDispatcher.HandleCommandExecution(ctx, command, args);
    }

    public override void HandleQuit(Player player)
    {
        _eventDispatcher.HandleQuit(player);
    }

    public override void HandleDiagnostics(Player player, Session.Diagnostics diagnostics)
    {
        _eventDispatcher.HandleDiagnostics(player, diagnostics);
    }

    public override void HandleLiquidFlow(World.Context ctx, Cube.Pos from, Cube.Pos into, World.Liquid liquid, World.Block replaced)
    {
        _eventDispatcher.HandleLiquidFlow(ctx, from, into, liquid, replaced);
    }

    public override void HandleLiquidDecay(World.Context ctx, Cube.Pos pos, World.Liquid before, World.Liquid? after)
    {
        _eventDispatcher.HandleLiquidDecay(ctx, pos, before, after);
    }

    public override void HandleLiquidHarden(World.Context ctx, Cube.Pos hardenedPos, World.Block liquidHardened, World.Block otherLiquid, World.Block newBlock)
    {
        _eventDispatcher.HandleLiquidHarden(ctx, hardenedPos, liquidHardened, otherLiquid, newBlock);
    }

    public override void HandleSound(World.Context ctx, World.Sound sound, Vector3 pos)
    {
        _eventDispatcher.HandleSound(ctx, sound, pos);
    }

    public override void HandleFireSpread(World.Context ctx, Cube.Pos from, Cube.Pos to)
    {
        _eventDispatcher.HandleFireSpread(ctx, from, to);
    }

    public override void HandleBlockBurn(World.Context ctx, Cube.Pos pos)
    {
        _eventDispatcher.HandleBlockBurn(ctx, pos);
    }

    public override void HandleCropTrample(World.Context ctx, Cube.Pos pos)
    {
        _eventDispatcher.HandleCropTrample(ctx, pos);
    }

    public override void HandleLeavesDecay(World.Context ctx, Cube.Pos pos)
    {
        _eventDispatcher.HandleLeavesDecay(ctx, pos);
    }

    public override void HandleEntitySpawn(World.Tx tx, World.Entity entity)
    {
        _eventDispatcher.HandleEntitySpawn(tx, entity);
    }

    public override void HandleEntityDespawn(World.Tx tx, World.Entity entity)
    {
        _eventDispatcher.HandleEntityDespawn(tx, entity);
    }

    public override void HandleExplosion(World.Context ctx, Vector3 position, ref World.Entity[] entities, ref Cube.Pos[] blocks, ref double itemDropChance, ref bool spawnFire)
    {
        _eventDispatcher.HandleExplosion(ctx, position, ref entities, ref blocks, ref itemDropChance, ref spawnFire);
    }

    public override void HandleRedstoneUpdate(World.Context ctx, World.RedstoneUpdate update)
    {
        _eventDispatcher.HandleRedstoneUpdate(ctx, update);
    }

    public override void HandleClose(World.Tx tx)
    {
        _eventDispatcher.HandleClose(tx);
    }

    public override void HandleClientPacket(DPacketContext ctx, DPacket packet)
    {
        _eventDispatcher.HandleClientPacket(ctx, packet);
    }

    public override void HandleServerPacket(DPacketContext ctx, DPacket packet)
    {
        _eventDispatcher.HandleServerPacket(ctx, packet);
    }
}
