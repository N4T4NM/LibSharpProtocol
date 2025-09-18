using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Core.Packets.Login;

[PacketInfo(0x03, PacketDirection.S2C, ProtocolState.Login)]
public class S2CSetCompression : IPacket
{
    public void Write(ProtocolStream stream) => stream.WriteVarInt(Threshold);
    public void Read(ProtocolStream stream) => Threshold = stream.ReadVarInt();

    public int Id => 0x03;
    public int Threshold { get; set; }
}