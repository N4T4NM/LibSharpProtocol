using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x0B, PacketDirection.C2S, ProtocolState.Play)]
public class C2SClientStatus : IPacket
{
    public void Write(ProtocolStream stream) => stream.WriteVarInt((int)Action);

    public void Read(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x0B;
    public ClientStatusAction Action { get; set; }
}

public enum ClientStatusAction : int
{
    Respawn,
    RequestStatus
}