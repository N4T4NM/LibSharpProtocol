using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.C2S.Play;

[PacketInfo(0x1D, PacketDirection.C2S, ProtocolState.Play)]
public class C2SSetPlayerPosition : IPacket
{
    public void Write(ProtocolStream stream)
    {
        stream.WriteVec3d(Position);
        stream.WriteU8((byte)Flags);
    }
    public void Read(ProtocolStream stream) => throw new NotImplementedException();

    public int Id => 0x1D;
    public Vec3d Position { get; set; }
    public PlayerPositionFlags Flags { get; set; }
}

[Flags]
public enum PlayerPositionFlags : byte
{
    None = 0x00,
    OnGround = 0x01,
    PushingAgainstWall = 0x02
}