using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x08, PacketDirection.S2C, ProtocolState.Play)]
public class S2CBlockUpdate : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();
    public void Read(ProtocolStream stream)
    {
        Position = stream.ReadPackedVec3d();
        BlockId = stream.ReadVarInt();
    }

    public int Id => 0x08;
    public Vec3d Position { get; set; }
    public int BlockId { get; set; }
}
 