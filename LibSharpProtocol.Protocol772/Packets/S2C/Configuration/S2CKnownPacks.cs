using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Configuration;

[PacketInfo(0x0E, PacketDirection.S2C, ProtocolState.Configuration)]
public class S2CKnownPacks : IPacket
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        KnownPacks = stream.ReadArray(KnownPack.New);
    }

    public int Id => 0x0E;
    public KnownPack[] KnownPacks { get; set; } = [];
}

public class KnownPack : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteString(Namespace);
        stream.WriteString(ID);
        stream.WriteString(Version);
    }

    public void Read(ProtocolStream stream)
    {
        Namespace = stream.ReadString();
        ID = stream.ReadString();
        Version = stream.ReadString();
    }
    
    public static KnownPack New(ProtocolStream s)
    {
        KnownPack pack = new();
        pack.Read(s);
        
        return pack;
    }
    
    public string Namespace { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}