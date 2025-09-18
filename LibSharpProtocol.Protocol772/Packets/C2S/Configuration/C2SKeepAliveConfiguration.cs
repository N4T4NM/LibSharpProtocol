using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Configuration;

[PacketInfo(0x04, PacketDirection.C2S, ProtocolState.Configuration)]
public class C2SKeepAliveConfiguration : IPacket
{
    public void Write(ProtocolStream stream) => stream.WriteI64(KeepAliveId);

    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x04;
    public long KeepAliveId { get; set; }
}