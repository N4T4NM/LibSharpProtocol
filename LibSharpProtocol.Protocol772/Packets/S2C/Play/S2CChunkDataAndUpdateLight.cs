using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;
using LibSharpProtocol.Protocol772.Data;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x27, PacketDirection.S2C, ProtocolState.Play)]
public class S2CChunkDataAndUpdateLight : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        ChunkX = stream.ReadI32();
        ChunkZ = stream.ReadI32();
        Data.Read(stream);
    }

    public int Id => 0x27;
    public int ChunkX { get; set; }
    public int ChunkZ { get; set; }
    public ChunkData Data { get; set; } = new();
    // TODO: Light Data (Can be ignored for now)
}
 