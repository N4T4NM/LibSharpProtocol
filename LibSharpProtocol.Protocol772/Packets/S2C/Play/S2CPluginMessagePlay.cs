using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x18, PacketDirection.S2C, ProtocolState.Play)]
public class S2CPluginMessagePlay : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        Channel = stream.ReadString();
        Data = stream.ReadRemaining();
    }

    public int Id => 0x18;
    public string Channel { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
}