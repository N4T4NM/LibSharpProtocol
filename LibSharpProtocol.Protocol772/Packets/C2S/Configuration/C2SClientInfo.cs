using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Configuration;

[PacketInfo(0x00, PacketDirection.C2S, ProtocolState.Configuration)]
public class C2SClientInfo : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteString(Locale);
        stream.WriteU8(ViewDistance);
        stream.WriteVarInt((int)ChatMode);
        stream.WriteBool(ChatColors);
        stream.WriteU8((byte)DisplayedSkinParts);
        stream.WriteVarInt((int)MainHand);
        stream.WriteBool(EnableTextFiltering);
        stream.WriteBool(AllowServerListings);
        stream.WriteVarInt((int)ParticleStatus);
    }

    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x00;
    public string Locale { get; set; } = "en_us";
    public byte ViewDistance { get; set; } = 12;
    public ChatMode ChatMode { get; set; } = ChatMode.Enabled;
    public bool ChatColors { get; set; } = true;
    public SkinParts DisplayedSkinParts { get; set; } = SkinParts.All;
    public MainHand MainHand { get; set; } = MainHand.Right;
    public bool EnableTextFiltering { get; set; } = false;
    public bool AllowServerListings { get; set; } = true;
    public ParticleStatus ParticleStatus { get; set; } = ParticleStatus.All;
}

[Flags]
public enum SkinParts : byte
{
    None = 0x00,
    Cape = 0x01,
    Jacket = 0x02,
    LeftSleeve = 0x04,
    RightSleeve = 0x08,
    LeftPantsLeg = 0x10,
    RightPantsLeg = 0x20,
    Hat = 0x40,
    All = Cape | Jacket | LeftSleeve | RightSleeve |  LeftPantsLeg | RightPantsLeg | Hat
}

public enum ChatMode : int
{
    Enabled,
    CommandsOnly,
    Hidden
}

public enum MainHand : int
{
    Left,
    Right
}

public enum ParticleStatus : int
{
    All,
    Decreased,
    Minimal
}