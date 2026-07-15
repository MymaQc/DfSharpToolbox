using Dragonfly;
using Toolbox.Blocks;
using Toolbox.Commands;
using Toolbox.Effects;
using Toolbox.Entities;
using Toolbox.Forms;
using Toolbox.Inventories;
using Toolbox.Items;
using Toolbox.Math;
using Toolbox.Players;
using Toolbox.Players.Controls;
using Toolbox.Presentation;
using Toolbox.Sounds;
using Toolbox.Tasks;
using Toolbox.Timing;
using Toolbox.Worlds;
using Toolbox.Worlds.States;

namespace ToolboxExample;

internal static class ExampleCommands
{
    public static void Register(ExampleState state)
    {
        ToolboxKitCommand.State = state;
        ToolboxEffectCommand.State = state;

        CommandApi.RegisterCommand("tbxhelp", "Affiche les commandes de test Toolbox.", ShowHelp, "tbx");
        CommandApi.RegisterPlayerCommand("tbxprofile", "Teste PlayerApi et les snapshots joueur.", ShowProfile);
        CommandApi.RegisterPlayerCommand("tbxitems", "Teste ItemFactory, ItemNbtApi, InventoryApi et ItemCooldownApi.", (_, player) => GiveItems(player));
        CommandApi.RegisterPlayerCommand("tbxforms", "Ouvre les forms Toolbox.", (_, player) => ExampleForms.OpenMain(player, state));
        CommandApi.RegisterPlayerCommand("tbxui", "Teste TitleApi, ToastApi, ScoreboardApi et PlayerControlApi.", (_, player) => ShowUi(player));
        CommandApi.RegisterPlayerCommand("tbxworld", "Teste WorldApi, BlockApi, DimensionApi et EntityApi.", World);
        CommandApi.RegisterPlayerCommand("tbxtask", "Teste TaskApi.", (ctx, player) => Tasks(ctx, player, state));
        CommandApi.RegisterPlayerCommand("tbxpacketlog", "Active ou desactive le log des packets.", (_, player) => TogglePacketLog(player, state));

        CommandApi.CreateCommandBuilder("tbxbuilder", "Exemple de CommandBuilder.")
            .AddAlias("tbxb")
            .OnExecute(ctx => ctx.SendMessage("CommandBuilder fonctionne. Essaie aussi /tbxkit 3 Test ou /tbxeffect speed 10."))
            .RegisterCommand();

        CommandApi.RegisterPlayerCommand<ToolboxKitCommand>("tbxkit", "Commande a parametres: <count> [name].", "tbxgive");
        CommandApi.RegisterPlayerCommand<ToolboxEffectCommand>("tbxeffect", "Commande a parametres: <effect> [seconds].");
        CommandApi.RegisterCommand<ToolboxEchoCommand>("tbxecho", "Commande raw text: <message...>.");
    }

    private static void ShowHelp(CommandContext ctx)
    {
        ctx.SendMessage("ToolboxExample:");
        ctx.SendMessage("/tbxprofile - infos joueur");
        ctx.SendMessage("/tbxitems - items, NBT, inventaire, cooldown");
        ctx.SendMessage("/tbxforms - simple/modal/custom forms");
        ctx.SendMessage("/tbxui - title, toast, scoreboard, HUD/input");
        ctx.SendMessage("/tbxworld - monde, blocks, entites, dimensions");
        ctx.SendMessage("/tbxtask - task immediate/later/repeating");
        ctx.SendMessage("/tbxpacketlog - inspecte les packets via events");
        ctx.SendMessage("/tbxkit <count> [name] - commande a parametres");
        ctx.SendMessage("/tbxeffect <speed|jumpboost|nightvision> [seconds]");
        ctx.SendMessage("/tbxecho <message...>");
    }

    private static void ShowProfile(CommandContext ctx, Player player)
    {
        var identity = PlayerApi.GetIdentitySnapshot(player);
        var movement = PlayerApi.GetMovementState(player);
        var physical = PlayerApi.GetPhysicalState(player);
        var food = PlayerApi.GetFoodState(player);
        var xp = PlayerApi.GetExperienceState(player);

        ctx.SendMessage($"Name={identity.Name} UUID={identity.UniqueId} XUID={identity.Xuid}");
        ctx.SendMessage($"Position={PlayerApi.GetPosition(player)} Ping={PlayerApi.GetPing(player).TotalMilliseconds:0}ms");
        ctx.SendMessage($"Health={PlayerApi.GetHealth(player):0.0}/{PlayerApi.GetMaxHealth(player):0.0} Food={food.Food} XP={xp.Experience}");
        ctx.SendMessage($"Move sprint={movement.Sprinting} sneak={movement.Sneaking} flying={movement.Flying}");
        ctx.SendMessage($"Ground={physical.OnGround} fall={physical.FallDistance:0.0} breathing={physical.Breathing}");
    }

    public static void GiveItems(Player player)
    {
        var sword = ItemFactory.CreateBuilder(new Item.Sword(Item.ToolTierDiamond))
            .SetCustomName("Toolbox sword")
            .SetLore("Cree avec ItemStackBuilder", "NBT via ItemNbtApi")
            .AddEnchantment(Item.Sharpness, 3, force: true)
            .AddEnchantment(Item.Unbreaking, 2, force: true)
            .SetUnbreakable()
            .SetTag("toolbox.owner", PlayerApi.GetName(player))
            .SetTag("toolbox.demo", true)
            .Build();

        var apples = ItemFactory.CreateBuilder(new Item.GoldenApple(), 4)
            .SetCustomName("Toolbox apples")
            .SetTag("toolbox.kind", "food")
            .Build();

        var owner = ItemNbtApi.GetString(sword, "toolbox.owner", "inconnu");
        InventoryApi.GiveItem(player, sword);
        InventoryApi.GiveItem(player, apples);
        InventoryApi.GiveItem(player, ItemFactory.CreateTotem(2));
        InventoryApi.SetArmor(player, ItemFactory.CreateDiamondArmor());
        ItemCooldownApi.SetCooldown(player, apples, TimeSpan.FromSeconds(3));

        PlayerApi.Feed(player);
        PlayerApi.HealFull(player);
        PlayerApi.AddExperience(player, 15);
        SoundApi.PlaySound(player, ToolboxSound.LevelUp);
        PlayerApi.SendMessage(player, $"Items envoyes. NBT owner={owner}, cooldown golden apple={ItemCooldownApi.HasCooldown(player, apples)}");
    }

    public static void ShowUi(Player player)
    {
        var title = TitleApi.CreateTitleWithText("Toolbox");
        title = TitleApi.SetSubtitle(title, "TitleApi");
        title = TitleApi.SetActionText(title, "Action text");
        title = TitleApi.SetDurationTicks(title, 60);
        TitleApi.SendTitle(player, title);
        ToastApi.SendToast(player, "Toolbox", "ToastApi fonctionne.");

        var scoreboard = ScoreboardApi.CreateScoreboard("Toolbox")
            .SetLine(0, $"Player: {PlayerApi.GetName(player)}")
            .SetLine(1, $"Health: {PlayerApi.GetHealth(player):0.0}")
            .SetLine(2, $"XP: {PlayerApi.GetExperience(player)}")
            .SetLine(3, $"Food: {PlayerApi.GetFood(player)}")
            .AddLine("AddLine fonctionne");
        ScoreboardApi.SendScoreboard(player, scoreboard);

        PlayerControlApi.HideHud(player, PlayerHudElement.ItemText);
        var hidden = PlayerControlApi.IsHudHidden(player, PlayerHudElement.ItemText);
        PlayerControlApi.ShowHud(player, PlayerHudElement.ItemText);
        PlayerControlApi.LockInput(player, PlayerInputLock.Jump);
        var locked = PlayerControlApi.IsInputLocked(player, PlayerInputLock.Jump);
        PlayerControlApi.UnlockInput(player, PlayerInputLock.Jump);
        PlayerControlApi.ShowCoordinates(player);
        PlayerControlApi.SendSleepingIndicator(player, 1, 10);
        PlayerControlApi.RemoveBossBar(player);

        PlayerApi.SendMessage(player, $"UI test: itemTextHidden={hidden} jumpLocked={locked}");
    }

    private static void World(CommandContext ctx, Player player)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var world = WorldApi.GetWorld(tx);
        var position = PlayerApi.GetBlockPosition(player);
        var target = PositionApi.AddToBlockPosition(position, 1, 0, 0);
        var oldBlock = BlockApi.GetBlock(tx, target);
        var newBlock = BlockFactory.RequireBlockByName("minecraft:gold_block");

        BlockApi.SetBlock(tx, target, newBlock, BlockApi.CreateSetOptions(disableRedstoneUpdates: true));
        SoundApi.PlaySound(tx, PlayerApi.GetPosition(player), SoundApi.CreateBlockPlaceSound(newBlock));

        var nearbyEntities = EntityApi.GetNearbyEntities(tx, PlayerApi.GetPosition(player), 8).Count();
        var nearbyGold = BlockApi.FindNearbyBlocks(tx, target, 4, newBlock).Count();
        var overworld = DimensionApi.GetPresetDimension(DimensionPreset.Overworld);
        var custom = DimensionApi.CreateNetherLikeDimension();

        ctx.SendMessage($"World={WorldApi.GetName(world)} Tick={WorldApi.GetCurrentTick(tx)} Range={WorldApi.GetRange(tx)}");
        ctx.SendMessage($"Block remplace a {target}: {oldBlock.GetType().Name} -> {newBlock.GetType().Name}, goldNearby={nearbyGold}");
        ctx.SendMessage($"Entities radius 8={nearbyEntities}, overworldRange={DimensionApi.GetRange(overworld)}, netherWaterEvaporates={DimensionApi.DoesWaterEvaporate(custom)}");
    }

    private static void Tasks(CommandContext ctx, Player player, ExampleState state)
    {
        var tx = ctx.GetTransaction();
        if (tx is null)
        {
            ctx.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var world = WorldApi.GetWorld(tx);
        TaskApi.RunTask(world, taskTx => WorldApi.BroadcastMessage(taskTx, "Task immediate via Toolbox."));
        TaskApi.RunTaskLaterTicks(world, 40, taskTx => WorldApi.BroadcastMessage(taskTx, "Task delayed 40 ticks."));

        if (state.HasRepeatingTask())
        {
            ExampleForms.OpenConfirmReset(player, state);
            return;
        }

        state.SetRepeatingTask(TaskApi.RunTaskTimerTicks(world, 20, 20, taskTx =>
        {
            var run = state.IncrementRepeatingTaskRuns();
            WorldApi.BroadcastMessage(taskTx, $"Repeating task #{run}");
            if (run >= 5)
            {
                state.StopRepeatingTask();
            }
        }));

        PlayerApi.SendMessage(player, $"Task convert: 40 ticks = {TimeApi.ConvertGameTicksToDuration(40).TotalSeconds:0.0}s");
    }

    private static void TogglePacketLog(Player player, ExampleState state)
    {
        state.SetPacketLogEnabled(!state.PacketLogEnabled);
        PlayerApi.SendMessage(player, $"Packet log={state.PacketLogEnabled}. Dernier packet: {state.LastPacket}");
    }
}

public enum ToolboxDemoEffect
{
    Speed,
    JumpBoost,
    NightVision,
}

public sealed class ToolboxKitCommand : PlayerToolboxCommand
{
    internal static ExampleState? State { get; set; }

    [Cmd.Tag("count")]
    public int Count;

    [Cmd.Tag("name")]
    public Cmd.Optional<string> Name;

    protected override void Execute(CommandContext ctx, Player player)
    {
        var count = System.Math.Clamp(Count, 1, 64);
        var label = Name.LoadOr("Toolbox kit");
        var item = ItemFactory.CreateBuilder(new Item.GoldenApple(), count)
            .SetCustomName(label)
            .SetTag("toolbox.command", "tbxkit")
            .Build();

        InventoryApi.GiveItem(player, item);
        ToastApi.SendToast(player, "Toolbox kit", $"{count} golden apples");
        State?.RecordPacket($"Commande tbxkit executee par {PlayerApi.GetName(player)}");
    }
}

public sealed class ToolboxEffectCommand : PlayerToolboxCommand
{
    internal static ExampleState? State { get; set; }

    [Cmd.Tag("effect")]
    public ToolboxDemoEffect Effect;

    [Cmd.Tag("seconds")]
    public Cmd.Optional<int> Seconds;

    protected override void Execute(CommandContext ctx, Player player)
    {
        var seconds = System.Math.Clamp(Seconds.LoadOr(10), 1, 120);
        var type = Effect switch
        {
            ToolboxDemoEffect.JumpBoost => Dragonfly.Effect.JumpBoost,
            ToolboxDemoEffect.NightVision => Dragonfly.Effect.NightVision,
            _ => Dragonfly.Effect.Speed,
        };

        EffectApi.AddEffect(player, EffectApi.CreateEffectTicks(type, 1, seconds * 20));
        PlayerApi.SendMessage(player, $"Effect {Effect} applique pendant {seconds}s.");
        State?.RecordPacket($"Commande tbxeffect {Effect} {seconds}s");
    }
}

public sealed class ToolboxEchoCommand : ToolboxCommand
{
    [Cmd.Tag("message")]
    public Cmd.Varargs Message;

    protected override void Execute(CommandContext ctx)
    {
        ctx.SendMessage($"Echo: {Message}");
    }
}
