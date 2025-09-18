using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Login;

[PacketInfo(0x02, PacketDirection.S2C, ProtocolState.Login)]
public class S2CLoginSuccess : IPacket
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        UUID = stream.ReadUUID();
        Username = stream.ReadString();
        Properties = stream.ReadArray(PlayerProperty.New);
    }

    public int Id => 0x02;
    public UUID UUID { get; set; }
    public string Username { get; set; } = string.Empty;
    public PlayerProperty[] Properties { get; set; } = [];
}

public class PlayerProperty : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        Name = stream.ReadString();
        Value = stream.ReadString();
        Signature = stream.ReadOptional(stream.ReadString);
    }

    public static PlayerProperty New(ProtocolStream s)
    {
        PlayerProperty prop = new();
        prop.Read(s);
        
        return prop;
    }

    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Signature { get; set; }
}