using Dragonfly;
using Toolbox;
using Toolbox.Connections;
using Toolbox.Effects;
using Toolbox.Events.Packets;
using Toolbox.Events.Player.Blocks;
using Toolbox.Events.Player.Chat;
using Toolbox.Events.Player.Lifecycle;
using Toolbox.Events.Player.Movement;
using Toolbox.Events.Player.State;
using Toolbox.Events.Server;
using Toolbox.Items;
using Toolbox.Packets;
using Toolbox.Players;
using Toolbox.Presentation;
using Toolbox.Sounds;

namespace ToolboxExample;

internal static class ExampleListeners
{
    public static void Register(ToolboxPlugin plugin, ExampleState state)
    {
        var events = plugin.GetEvents();

        events.On<ConnectionPreLoginEvent>(ev =>
        {
            var address = ConnectionApi.GetAddressText(ev.Address);
            var version = ConnectionApi.GetGameVersion(ev.Client);
            var device = ConnectionApi.GetDeviceFamily(ev.Client);
            var name = ConnectionApi.GetDisplayName(ev.Identity);
            var xuid = ConnectionApi.GetIdentityXuid(ev.Identity);
            state.RecordConnection($"{name} xuid={xuid} version={version} device={device} address={address}");
        });

        events.On<PlayerJoinEvent>(ev =>
        {
            state.RecordJoin();
            PlayerApi.SendMessage(ev.Player, $"Bienvenue {PlayerApi.GetName(ev.Player)}. Tape /tbxhelp.");
            ToastApi.SendToast(ev.Player, "ToolboxExample", $"Join #{state.JoinCount}");
            SoundApi.PlaySound(ev.Player, ToolboxSound.LevelUp);
            EffectApi.AddEffect(ev.Player, EffectApi.Create(Effect.Speed, 1, 100, hideParticles: true));
        });

        events.On<PlayerQuitEvent>(ev =>
        {
            Console.WriteLine($"{PlayerApi.GetName(ev.Player)} quit ToolboxExample");
        });

        events.On<PlayerChatEvent>(ev =>
        {
            ev.Message = ev.Message.Trim();
            if (ev.Message.Contains("badword", StringComparison.OrdinalIgnoreCase))
            {
                ev.Message = ev.Message.Replace("badword", "***", StringComparison.OrdinalIgnoreCase);
            }
        });

        events.On<PlayerMoveEvent>(ev =>
        {
            if (!Toolbox.Math.PositionApi.IsFinite(ev.NewPosition) || ev.NewPosition.Y < -64)
            {
                ev.Cancel();
            }
        });

        events.On<PlayerFoodLossEvent>(ev =>
        {
            ev.To = ev.From;
        });

        events.On<PlayerBlockBreakEvent>(ev =>
        {
            ev.Experience += 1;
            var bonus = ItemFactory.CreateBuilder(new Item.Apple())
                .SetCustomName("Toolbox bonus")
                .SetTag("toolbox.drop", true)
                .Build();
            ev.Drops = [.. ev.Drops, bonus];
        });

        events.On<ClientPacketEvent>(ev => RecordPacket("client", ev, state));
        events.On<ServerPacketEvent>(ev => RecordPacket("server", ev, state));
    }

    private static void RecordPacket(string direction, PacketEvent ev, ExampleState state)
    {
        if (!state.PacketLogEnabled)
        {
            return;
        }

        var fields = ev.GetPacketFields()
            .Take(4)
            .Select(field => $"{field.Name}={field.DisplayValue}");
        var known = PacketApi.IsKnownPacket(ev.GetPacket());
        var line = $"{direction} #{ev.GetPacketId()} {ev.GetPacketName()} known={known} xuid={ev.GetContextXuid()} [{string.Join(", ", fields)}]";
        state.RecordPacket(line);
        Console.WriteLine(line);
    }
}
