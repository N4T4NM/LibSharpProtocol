using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x1B, PacketDirection.C2S, ProtocolState.Play)]
public class C2SKeepAlivePlayAck : IPacket
{
    public void Write(ProtocolStream stream) => stream.WriteI64(KeepAliveID);
    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x1B;
    public long KeepAliveID { get; set; }
}