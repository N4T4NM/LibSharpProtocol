using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x21, PacketDirection.S2C, ProtocolState.Play)]
public class S2CUnloadChunk : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        ChunkZ = stream.ReadI32();
        ChunkX = stream.ReadI32();
    }

    public int Id => 0x21;
    public int ChunkZ { get; set; }
    public int ChunkX { get; set; }
}
 