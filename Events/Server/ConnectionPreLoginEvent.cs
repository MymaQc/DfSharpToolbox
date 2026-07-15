using Dragonfly;

namespace Toolbox.Events.Server;

public sealed class ConnectionPreLoginEvent(Net.Addr address, Login.IdentityData identity, Login.ClientData client) : CancellableEvent
{
    public Net.Addr Address { get; } = address;

    public Login.IdentityData Identity { get; } = identity;

    public Login.ClientData Client { get; } = client;

    public string KickMessage { get; set; } = string.Empty;
}
