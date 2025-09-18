using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Configuration;

[PacketInfo(0x03, PacketDirection.C2S, ProtocolState.Configuration)]
public class C2SFinishConfigurationAck : IPacket
{
    public void Write(ProtocolStream stream) {}
    public void Read(ProtocolStream stream) {}

    public int Id => 0x03;
}