using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x22, PacketDirection.S2C, ProtocolState.Play)]
public class S2CGameEvent : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        Event = (GameEvent)stream.ReadU8();
        Value = stream.ReadF32();
    }

    public int Id => 0x22;
    public GameEvent Event { get; set; }
    public float Value { get; set; }
}

public enum GameEvent : byte
{
    NoRespawnAvailable = 0,
    BeginRaining,
    EndRaining,
    ChangeGameMode,
    WinGame,
    DemoEvent,
    ArrowHitPlayer,
    RainLevelChange,
    ThunderLevelChange,
    PlayPufferfishStingSound,
    PlayElderGuardianMobAppearance,
    EnableRespawnScreen,
    LimitedCrafting,
    StartWaitingForLevelChunks
}