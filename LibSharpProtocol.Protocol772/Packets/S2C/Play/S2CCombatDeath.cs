using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x3D, PacketDirection.S2C, ProtocolState.Play)]
public class S2CCombatDeath : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();
    public void Read(ProtocolStream stream) => PlayerId = stream.ReadVarInt();

    public int Id => 0x3D;
    public int PlayerId { get; set; }
    // TODO: Message
}