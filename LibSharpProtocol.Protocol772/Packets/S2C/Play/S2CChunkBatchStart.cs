using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x0C, PacketDirection.S2C, ProtocolState.Play)]
public class S2CChunkBatchStart : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream) {}

    public int Id => 0x0C;
}