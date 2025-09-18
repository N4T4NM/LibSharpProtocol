using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Core.Packets.Handshaking;

[PacketInfo(0x00, PacketDirection.C2S, ProtocolState.Handshaking)]
public class C2SHandshake : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteVarInt(ProtocolVersion);
        stream.WriteString(ServerAddress);
        stream.WriteU16(ServerPort);
        stream.WriteVarInt((int)Intent);
    }

    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id { get; } = 0x00;
    public int ProtocolVersion { get; set; }
    public string ServerAddress { get; set; } = string.Empty;
    public ushort ServerPort { get; set; }
    public ProtocolState Intent { get; set; }
}