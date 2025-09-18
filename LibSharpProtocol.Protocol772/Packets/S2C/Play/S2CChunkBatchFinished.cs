using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x0B, PacketDirection.S2C, ProtocolState.Play)]
public class S2CChunkBatchFinished : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream) => BatchSize = stream.ReadVarInt();

    public int Id => 0x0B;
    public int BatchSize { get; set; }
}