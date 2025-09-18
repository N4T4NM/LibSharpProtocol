using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x26, PacketDirection.S2C, ProtocolState.Play)]
public class S2CKeepAlivePlay : IPacket
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream) => KeepAliveID = stream.ReadI64();

    public int Id => 0x26;
    public long KeepAliveID { get; set; }
}