using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x2B, PacketDirection.C2S, ProtocolState.Play)]
public class C2SPlayerLoaded : IPacket
{
    public void Write(ProtocolStream stream) {}
    public void Read(ProtocolStream stream) {}
    public int Id => 0x2B;
}