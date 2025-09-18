using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x2B, PacketDirection.S2C, ProtocolState.Play)]
public class S2CLoginPlay : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        EntityId = stream.ReadI32();
        IsHardcore = stream.ReadBool();
        DimensionNames = stream.ReadArray(ReadIdentifier);
        MaxPlayers = stream.ReadVarInt();
        ViewDistance = stream.ReadVarInt();
        SimulationDistance = stream.ReadVarInt();
        ReducedDebugInfo = stream.ReadBool();
        EnableRespawnScreen = stream.ReadBool();
        DoLimitedCrafting = stream.ReadBool();
        DimensionType = stream.ReadVarInt();
        DimensionName = ReadIdentifier(stream);
        HashedSeed = stream.ReadI64();
        GameMode = stream.ReadU8();
        PreviousGameMode = stream.ReadI8();
        IsDebug = stream.ReadBool();
        IsFlat = stream.ReadBool();
        HasDeathLocation = stream.ReadBool();

        if (HasDeathLocation)
        {
            DeathDimensionName = ReadIdentifier(stream);
            DeathLocation = stream.ReadPackedVec3d();
        }
        PortalCooldown = stream.ReadVarInt();
        SeaLevel = stream.ReadVarInt();
        EnforcesSecureChat = stream.ReadBool();
    }

    string ReadIdentifier(ProtocolStream stream) => stream.ReadString();

    public int Id => 0x2B;
    
    public int EntityId { get; set; }
    public bool IsHardcore { get; set; }
    public string[] DimensionNames { get; set; } = Array.Empty<string>();
    public int MaxPlayers { get; set; }
    public int ViewDistance { get; set; }
    public int SimulationDistance { get; set; }
    public bool ReducedDebugInfo { get; set; }
    public bool EnableRespawnScreen { get; set; }
    public bool DoLimitedCrafting { get; set; }
    public int DimensionType { get; set; }
    public string DimensionName { get; set; } = string.Empty;
    public long HashedSeed { get; set; }
    public byte GameMode { get; set; }
    public sbyte PreviousGameMode { get; set; }
    public bool IsDebug { get; set; }
    public bool IsFlat { get; set; }
    public bool HasDeathLocation { get; set; }
    public string? DeathDimensionName { get; set; }
    public Vec3d? DeathLocation { get; set; }
    public int PortalCooldown { get; set; }
    public int SeaLevel { get; set; }
    public bool EnforcesSecureChat { get; set; }
}