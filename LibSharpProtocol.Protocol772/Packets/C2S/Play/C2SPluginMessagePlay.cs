using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x15, PacketDirection.C2S, ProtocolState.Play)]
public class C2SPluginMessagePlay : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteString(Channel);
        stream.Write(Data);
    }
    public void Read(ProtocolStream stream) => throw new NotImplementedException();

    public int Id => 0x15;
    public string Channel { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
}