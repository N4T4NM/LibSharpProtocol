using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x08, PacketDirection.C2S, ProtocolState.Play)]
public class C2SChatMessage : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteString(Message);
        stream.WriteI64(Timestamp);
        stream.WriteI64(Salt);
        stream.WriteOptional(Signature);
        stream.WriteVarInt(MessageCount);
        stream.Write(BitSet);
        stream.WriteU8(Checksum);
    }

    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x08;

    public string Message { get; set; } = string.Empty;
    public long Timestamp { get; set; } = (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
    public long Salt { get; set; }
    public byte[]? Signature { get; set; }
    public int MessageCount { get; set; }
    public byte[] BitSet { get; set; } = new byte[3];
    public byte Checksum { get; set; }
}