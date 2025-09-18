using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x2E, PacketDirection.S2C, ProtocolState.Play)]
public class S2CUpdateEntityPosition : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        EntityId = stream.ReadVarInt();
        DeltaX = stream.ReadI16();
        DeltaY = stream.ReadI16();
        DeltaZ = stream.ReadI16();
        OnGround = stream.ReadBool();
    }

    public int Id => 0x2E;
    public int EntityId { get; set; }
    public short DeltaX { get; set; }
    public short DeltaY { get; set; }
    public short DeltaZ { get; set; }
    public bool OnGround { get; set; }
}