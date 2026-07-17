using Dragonfly;
using Toolbox;
using Toolbox.Events;
using Toolbox.Events.Player.Blocks;
using Toolbox.Events.Player.Combat;
using Toolbox.Events.Player.Commands;
using Toolbox.Events.Player.Diagnostics;
using Toolbox.Events.Player.Interaction;
using Toolbox.Events.Player.Items;
using Toolbox.Events.Player.Lifecycle;
using Toolbox.Events.Player.Movement;
using Toolbox.Events.Player.Network;
using Toolbox.Events.Player.State;
using Toolbox.Events.World.Blocks;
using Toolbox.Events.World.Entities;
using Toolbox.Events.World.Explosions;
using Toolbox.Events.World.Lifecycle;
using Toolbox.Events.World.Liquid;
using Toolbox.Events.World.Redstone;
using Toolbox.Events.World.Sound;
using Toolbox.Players;
using Toolbox.Worlds;

namespace ToolboxExample;

internal static class ExampleEventDiagnostics
{
    public static void Register(ToolboxPlugin plugin, ExampleState state)
    {
        var events = plugin.GetEvents();

        events.On<PlayerJumpEvent>(ev => LogPlayer(state, ev.Player, "jump"));
        events.On<PlayerTeleportEvent>(ev => LogPlayer(state, ev.Player, $"teleport pos={ev.Position}"));
        events.On<PlayerChangeWorldEvent>(ev => LogPlayer(state, ev.Player, $"change-world before={ev.Before?.Name() ?? "none"} after={WorldApi.GetName(ev.After)}"));
        events.On<PlayerToggleSprintEvent>(ev => LogPlayer(state, ev.Player, $"sprint={ev.Sprinting}"));
        events.On<PlayerToggleSneakEvent>(ev => LogPlayer(state, ev.Player, $"sneak={ev.Sneaking}"));

        events.On<PlayerAttackEntityEvent>(ev => LogPlayer(state, ev.Player, $"attack-entity force={ev.KnockbackForce:0.00} height={ev.KnockbackHeight:0.00} critical={ev.Critical}"));
        events.On<PlayerDamageEvent>(ev => LogPlayer(state, ev.Player, $"damage amount={ev.Damage:0.00} immune={ev.Immune}"));
        events.On<PlayerHealEvent>(ev => LogPlayer(state, ev.Player, $"heal amount={ev.Amount:0.00}"));
        events.On<PlayerDeathEvent>(ev => LogPlayer(state, ev.Player, $"death keepInventory={ev.KeepInventory}"));
        events.On<PlayerRespawnEvent>(ev => LogPlayer(state, ev.Player, $"respawn pos={ev.Position} world={WorldApi.GetName(ev.World)}"));
        events.On<PlayerSkinChangeEvent>(ev => LogPlayer(state, ev.Player, "skin-change"));

        events.On<PlayerFireExtinguishEvent>(ev => LogPlayer(state, ev.Player, $"fire-extinguish pos={ev.Position}"));
        events.On<PlayerStartBreakEvent>(ev => LogPlayer(state, ev.Player, $"start-break pos={ev.Position}"));
        events.On<PlayerBlockPlaceEvent>(ev => LogPlayer(state, ev.Player, $"block-place pos={ev.Position} block={ev.Block.GetType().Name}"));
        events.On<PlayerBlockPickEvent>(ev => LogPlayer(state, ev.Player, $"block-pick pos={ev.Position} block={ev.Block.GetType().Name}"));
        events.On<PlayerItemUseOnBlockEvent>(ev => LogPlayer(state, ev.Player, $"item-use-on-block pos={ev.Position} face={ev.Face}"));

        events.On<PlayerItemUseEvent>(ev => LogPlayer(state, ev.Player, "item-use"));
        events.On<PlayerItemUseOnEntityEvent>(ev => LogPlayer(state, ev.Player, $"item-use-on-entity entity={ev.Entity.GetType().Name}"));
        events.On<PlayerItemReleaseEvent>(ev => LogPlayer(state, ev.Player, $"item-release duration={ev.Duration.TotalSeconds:0.00}s"));
        events.On<PlayerItemConsumeEvent>(ev => LogPlayer(state, ev.Player, $"item-consume item={ItemName(ev.Item)}"));
        events.On<PlayerItemDamageEvent>(ev => LogPlayer(state, ev.Player, $"item-damage damage={ev.Damage} item={ItemName(ev.Item)}"));
        events.On<PlayerItemPickupEvent>(ev => LogPlayer(state, ev.Player, $"item-pickup item={ItemName(ev.Item)}"));
        events.On<PlayerHeldSlotChangeEvent>(ev => LogPlayer(state, ev.Player, $"held-slot {ev.From}->{ev.To}"));
        events.On<PlayerItemDropEvent>(ev => LogPlayer(state, ev.Player, $"item-drop item={ItemName(ev.Item)}"));

        events.On<PlayerCommandExecutionEvent>(ev => LogPlayer(state, ev.Player, $"command-execution /{ev.Command.Name()} {string.Join(" ", ev.Arguments)}"));
        events.On<PlayerDiagnosticsEvent>(ev => LogPlayer(state, ev.Player, "diagnostics"));
        events.On<PlayerPunchAirEvent>(ev => LogPlayer(state, ev.Player, "punch-air"));
        events.On<PlayerSignEditEvent>(ev => LogPlayer(state, ev.Player, $"sign-edit pos={ev.Position} front={ev.FrontSide} old='{ev.OldText}' new='{ev.NewText}'"));
        events.On<PlayerSleepEvent>(ev => LogPlayer(state, ev.Player, $"sleep sendReminder={ev.SendReminder}"));
        events.On<PlayerLecternPageTurnEvent>(ev => LogPlayer(state, ev.Player, $"lectern-page pos={ev.Position} {ev.OldPage}->{ev.NewPage}"));
        events.On<PlayerTransferEvent>(ev => LogPlayer(state, ev.Player, $"transfer address={ev.Address}"));
        events.On<PlayerExperienceGainEvent>(ev => LogPlayer(state, ev.Player, $"xp-gain amount={ev.Amount}"));

        events.On<BlockBurnEvent>(ev => LogWorld(state, ev.World, $"block-burn pos={ev.Position}"));
        events.On<CropTrampleEvent>(ev => LogWorld(state, ev.World, $"crop-trample pos={ev.Position}"));
        events.On<FireSpreadEvent>(ev => LogWorld(state, ev.World, $"fire-spread {ev.From}->{ev.To}"));
        events.On<LeavesDecayEvent>(ev => LogWorld(state, ev.World, $"leaves-decay pos={ev.Position}"));
        events.On<EntitySpawnEvent>(ev => LogWorld(state, ev.World, $"entity-spawn type={ev.Entity.GetType().Name}"));
        events.On<EntityDespawnEvent>(ev => LogWorld(state, ev.World, $"entity-despawn type={ev.Entity.GetType().Name}"));
        events.On<WorldExplosionEvent>(ev => LogWorld(state, ev.World, $"explosion pos={ev.Position} blocks={ev.Blocks.Length} entities={ev.Entities.Length}"));
        events.On<WorldCloseEvent>(ev => LogWorld(state, ev.World, "world-close"));
        events.On<LiquidFlowEvent>(ev => LogWorld(state, ev.World, $"liquid-flow {ev.From}->{ev.Into} liquid={ev.Liquid.GetType().Name}"));
        events.On<LiquidDecayEvent>(ev => LogWorld(state, ev.World, $"liquid-decay pos={ev.Position} before={ev.Before.GetType().Name} after={ev.After?.GetType().Name ?? "none"}"));
        events.On<LiquidHardenEvent>(ev => LogWorld(state, ev.World, $"liquid-harden pos={ev.Position} new={ev.NewBlock.GetType().Name}"));
        events.On<RedstoneUpdateEvent>(ev => LogWorld(state, ev.World, $"redstone-update type={ev.Update.GetType().Name}"));
        events.On<WorldSoundEvent>(ev => LogWorld(state, ev.World, $"sound type={ev.Sound.GetType().Name} pos={ev.Position}"));
    }

    private static void LogPlayer(ExampleState state, Player player, string message)
    {
        Log(state, $"{PlayerApi.GetName(player)}: {message}");
        if (state.EventDiagnosticsEnabled)
        {
            PlayerApi.SendTip(player, $"event: {message}");
        }
    }

    private static void LogWorld(ExampleState state, World world, string message)
    {
        Log(state, $"{WorldApi.GetName(world)}: {message}");
    }

    private static void Log(ExampleState state, string message)
    {
        state.RecordEvent(message);
        if (state.EventDiagnosticsEnabled)
        {
            Console.WriteLine($"[ToolboxExample/Event] {message}");
        }
    }

    private static string ItemName(Item.Stack stack)
    {
        return stack.Item()?.GetType().Name ?? "Air";
    }
}
