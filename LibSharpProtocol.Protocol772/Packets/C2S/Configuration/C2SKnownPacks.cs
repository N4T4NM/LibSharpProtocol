using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;
using LibSharpProtocol.Protocol772.Packets.S2C.Configuration;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Configuration;

[PacketInfo(0x07, PacketDirection.C2S, ProtocolState.Configuration)]
public class C2SKnownPacks : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteArray(KnownPacks, IProtocolSerializer.WriteElement);
    }

    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x07;
    public KnownPack[] KnownPacks { get; set; } = [];
}