using Dragonfly;
using Toolbox.Commands;
using Toolbox.Entities;
using Toolbox.Players;

namespace ToolboxExample;

internal static class ExampleEntities
{
    internal static CustomEntityType<ExampleEntityMarker> Register()
    {
        return EntityApi.Define<ExampleEntityMarker>(
                "toolbox:ticking_armor_stand",
                "minecraft:armor_stand")
            .BoundingBox(properties =>
            {
                var height = properties.Get("height", 1.8);
                return Cube.Box(-0.3, 0, -0.3, 0.3, height, 0.3);
            })
            .DefaultProperty("seconds_alive", 0)
            .DefaultProperty("height", 1.8)
            .TickEvery(20, tick =>
            {
                var seconds = tick.Properties.Get("seconds_alive", 0) + 1;
                tick.Properties.Set("seconds_alive", seconds);
                tick.NameTag = $"Toolbox entity - {seconds}s";
            })
            .OnClose(entity =>
                Console.WriteLine($"Toolbox example entity closed or unloaded after {entity.Properties.Get("seconds_alive", 0)}s."))
            .Register();
    }

    internal static void RegisterCommand(CustomEntityType<ExampleEntityMarker> entityType)
    {
        CommandApi.RegisterPlayerCommand(
            "tbxentity",
            "Fait apparaitre une entite custom geree par Toolbox.",
            (context, player) => Spawn(context, player, entityType));
    }

    private static void Spawn(
        CommandContext context,
        Player player,
        CustomEntityType<ExampleEntityMarker> entityType)
    {
        var transaction = context.GetTransaction();
        if (transaction is null)
        {
            context.SendError("Cette commande a besoin d'une transaction world.");
            return;
        }

        var playerPosition = PlayerApi.GetPosition(player);
        var spawned = EntityApi.Spawn(entityType, transaction)
            .At(playerPosition with
            {
                X = playerPosition.X + 2,
            })
            .Rotated(PlayerApi.GetRotation(player))
            .NameTag("Toolbox entity - 0s")
            .Property("owner", PlayerApi.GetName(player))
            .Create();

        context.SendMessage($"Entite creee: {spawned.Handle.UUID()}. Son nom est actualise chaque seconde.");
    }

    internal sealed class ExampleEntityMarker;
}
