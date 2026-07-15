namespace Toolbox.Packets;

public readonly record struct PacketFieldSnapshot(
    string Name,
    string Type,
    object? Value,
    string DisplayValue,
    bool Writable,
    string? Json,
    string? Error);
