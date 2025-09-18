using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x5E, PacketDirection.S2C, ProtocolState.Play)]
public class S2CSetEntityVelocity : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        EntityId = stream.ReadVarInt();
        VelocityX = stream.ReadI16();
        VelocityY = stream.ReadI16();
        VelocityZ = stream.ReadI16();
    }

    public int Id => 0x5E;
    public int EntityId { get; set; }
    public short VelocityX { get; set; }
    public short VelocityY { get; set; }
    public short VelocityZ { get; set; }
}
 