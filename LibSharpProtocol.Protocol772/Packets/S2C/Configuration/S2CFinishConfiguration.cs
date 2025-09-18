using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Configuration;

[PacketInfo(0x03, PacketDirection.S2C, ProtocolState.Configuration)]
public class S2CFinishConfiguration : IPacket
{
    public void Write(ProtocolStream stream) {}
    public void Read(ProtocolStream stream) {}

    public int Id => 0x03;
}