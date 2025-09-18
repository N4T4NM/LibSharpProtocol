using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x0A, PacketDirection.C2S, ProtocolState.Play)]
public class C2SChunkBatchReceived : IPacket
{
    public void Write(ProtocolStream stream) => stream.WriteF32(ChunksPerTick);
    public void Read(ProtocolStream stream) => ChunksPerTick = stream.ReadF32();

    public int Id => 0x0A;
    public float ChunksPerTick { get; set; }
}