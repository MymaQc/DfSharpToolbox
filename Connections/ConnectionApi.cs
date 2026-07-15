using Dragonfly;

namespace Toolbox.Connections;

public static class ConnectionApi
{
    public static string GetNetwork(Net.Addr address)
    {
        return address.Network();
    }

    public static string GetAddress(Net.Addr address)
    {
        return address.String();
    }

    public static string GetXuid(Login.IdentityData identity)
    {
        return identity.XUID;
    }

    public static string GetDisplayName(Login.IdentityData identity)
    {
        return identity.DisplayName;
    }

    public static string GetIdentity(Login.IdentityData identity)
    {
        return identity.Identity;
    }

    public static string GetPlayFabId(Login.IdentityData identity)
    {
        return identity.PlayFabID;
    }

    public static string GetGameVersion(Login.ClientData client)
    {
        return client.GameVersion;
    }

    public static Protocol.DeviceOS GetDeviceOS(Login.ClientData client)
    {
        return client.DeviceOS;
    }

    public static string GetDeviceName(Login.ClientData client)
    {
        return client.DeviceOS.ToString();
    }

    public static bool IsGameVersion(Login.ClientData client, string version)
    {
        return string.Equals(client.GameVersion, version, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsMobile(Login.ClientData client)
    {
        return client.DeviceOS is Protocol.DeviceOS.Android or Protocol.DeviceOS.IOS or Protocol.DeviceOS.FireOS;
    }

    public static bool IsDesktop(Login.ClientData client)
    {
        return client.DeviceOS is Protocol.DeviceOS.Win10 or Protocol.DeviceOS.Win32 or Protocol.DeviceOS.OSX or Protocol.DeviceOS.Linux;
    }

    public static bool IsConsole(Login.ClientData client)
    {
        return client.DeviceOS is Protocol.DeviceOS.XBOX or Protocol.DeviceOS.Orbis or Protocol.DeviceOS.NX;
    }

    public static bool HasXuid(Login.IdentityData identity)
    {
        return !string.IsNullOrWhiteSpace(identity.XUID);
    }
}
