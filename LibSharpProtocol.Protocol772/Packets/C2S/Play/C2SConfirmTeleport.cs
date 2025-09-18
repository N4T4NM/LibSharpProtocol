using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x00, PacketDirection.C2S, ProtocolState.Play)]
public class C2SConfirmTeleport : IPacket
{
    public void Write(ProtocolStream stream) => stream.WriteVarInt(TeleportId);
    public void Read(ProtocolStream stream) => throw new NotImplementedException();

    public int Id => 0x00;
    public int TeleportId { get; set; }
}