using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x41, PacketDirection.S2C, ProtocolState.Play)]
public class S2CSyncPlayerPosition : IPacket
{
    public void Write(ProtocolStream stream) => throw new NotImplementedException();

    public void Read(ProtocolStream stream)
    {
        TeleportId = stream.ReadVarInt();
        Position = stream.ReadVec3d();
        Velocity = stream.ReadVec3d();
        Yaw = stream.ReadF32();
        Pitch = stream.ReadF32();
        Flags = (TeleportFlags)stream.ReadI32();
    }

    public int Id => 0x41;
    public int TeleportId { get; set; }
    public Vec3d Position { get; set; }
    public Vec3d Velocity { get; set; }
    public float Yaw { get; set; }
    public float Pitch { get; set; }
    public TeleportFlags Flags { get; set; }
}

[Flags]
public enum TeleportFlags : int
{
    RelativeX = 0x1,
    RelativeY = 0x2,
    RelativeZ = 0x4,
    RelativeYaw = 0x8,
    RelativePitch = 0x10,
    RelativeVelocityX = 0x20,
    RelativeVelocityY = 0x40,
    RelativeVelocityZ = 0x80,
    RotateVelocityAccordingToRotation = 0x100
}