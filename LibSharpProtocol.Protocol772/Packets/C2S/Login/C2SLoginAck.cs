using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Login;

[PacketInfo(0x03, PacketDirection.C2S, ProtocolState.Login)]
public class C2SLoginAck : IPacket
{
    public void Write(ProtocolStream stream) {}
    public void Read(ProtocolStream stream) {}

    public int Id => 0x03;
}