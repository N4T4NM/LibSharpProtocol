using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Configuration;

[PacketInfo(0x04, PacketDirection.S2C, ProtocolState.Configuration)]
public class S2CKeepAliveConfiguration : IPacket
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream) => KeepAliveId = stream.ReadI64();

    public int Id => 0x04;
    public long KeepAliveId { get; set; }
}