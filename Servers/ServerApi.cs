using Dragonfly;

namespace Toolbox.Servers;

public static class ServerApi
{
    public static World GetOverworld(Server server)
    {
        return server.World();
    }

    public static World GetNether(Server server)
    {
        return server.Nether();
    }

    public static World GetEnd(Server server)
    {
        return server.End();
    }

    public static int GetOnlineCount(Server server)
    {
        return server.PlayerCount();
    }

    public static int GetMaxPlayers(Server server)
    {
        return server.MaxPlayerCount();
    }

    public static IEnumerable<Player> GetOnlinePlayers(Server server, World.Tx? tx = null)
    {
        return server.Players(tx);
    }

    public static (World.EntityHandle? Player, bool Ok) GetPlayer(Server server, Guid uuid)
    {
        return server.Player(uuid);
    }

    public static (World.EntityHandle? Player, bool Ok) GetPlayerByName(Server server, string name)
    {
        return server.PlayerByName(name);
    }

    public static (World.EntityHandle? Player, bool Ok) GetPlayerByXuid(Server server, string xuid)
    {
        return server.PlayerByXUID(xuid);
    }
}
