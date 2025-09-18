using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x46, PacketDirection.S2C, ProtocolState.Play)]
public class S2CRemoveEntities : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();
    public void Read(ProtocolStream stream) => EntityIds = stream.ReadArray(ReadId);

    VarInt ReadId(ProtocolStream stream) => stream.ReadVarInt();

    public int Id => 0x46;
    public VarInt[] EntityIds { get; set; } = [];
}