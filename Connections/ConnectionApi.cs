using Dragonfly;

namespace Toolbox.Connections;

public static class ConnectionApi
{
    public static string GetAddressNetwork(Net.Addr address)
    {
        return address.Network();
    }

    public static string GetAddressText(Net.Addr address)
    {
        return address.String();
    }

    public static string GetIdentityXuid(Login.IdentityData identity)
    {
        return identity.XUID;
    }

    public static string GetDisplayName(Login.IdentityData identity)
    {
        return identity.DisplayName;
    }

    public static string GetIdentityId(Login.IdentityData identity)
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

    public static string GetDeviceOsName(Login.ClientData client)
    {
        return client.DeviceOS.ToString();
    }

    public static bool IsGameVersion(Login.ClientData client, string version)
    {
        return string.Equals(client.GameVersion, version, StringComparison.OrdinalIgnoreCase);
    }

    public static ConnectionDeviceFamily GetDeviceFamily(Login.ClientData client)
    {
        return client.DeviceOS switch
        {
            Protocol.DeviceOS.Android or Protocol.DeviceOS.IOS or Protocol.DeviceOS.FireOS => ConnectionDeviceFamily.Mobile,
            Protocol.DeviceOS.Win10 or Protocol.DeviceOS.Win32 or Protocol.DeviceOS.OSX or Protocol.DeviceOS.Linux => ConnectionDeviceFamily.Desktop,
            Protocol.DeviceOS.XBOX or Protocol.DeviceOS.Orbis or Protocol.DeviceOS.NX => ConnectionDeviceFamily.Console,
            _ => ConnectionDeviceFamily.Unknown,
        };
    }

    public static bool IsMobile(Login.ClientData client)
    {
        return GetDeviceFamily(client) == ConnectionDeviceFamily.Mobile;
    }

    public static bool IsDesktop(Login.ClientData client)
    {
        return GetDeviceFamily(client) == ConnectionDeviceFamily.Desktop;
    }

    public static bool IsConsole(Login.ClientData client)
    {
        return GetDeviceFamily(client) == ConnectionDeviceFamily.Console;
    }

    public static bool HasIdentityXuid(Login.IdentityData identity)
    {
        return !string.IsNullOrWhiteSpace(identity.XUID);
    }
}
