using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Core.Packets.Login;

[PacketInfo(0x00, PacketDirection.C2S, ProtocolState.Login)]
public class C2SLoginStart : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteString(Name);
        stream.WriteUUID(PlayerUUID);
    }

    public void Read(ProtocolStream stream)
    {
        Name = stream.ReadString();
        PlayerUUID = stream.ReadUUID();
    }

    public int Id => 0x00;
    public string Name { get; set; } = string.Empty;
    public UUID PlayerUUID { get; set; }
}