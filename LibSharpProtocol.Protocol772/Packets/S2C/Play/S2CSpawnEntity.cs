using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x01, PacketDirection.S2C, ProtocolState.Play)]
public class S2CSpawnEntity : IPacket
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        EntityId = stream.ReadVarInt();
        EntityUUID = stream.ReadUUID();
        Type = (EntityType)(int)stream.ReadVarInt();
        Position = stream.ReadVec3d();
        Pitch = stream.ReadU8();
        Yaw = stream.ReadU8();
        HeadYaw = stream.ReadU8();
        Data = stream.ReadVarInt();
        VelocityX = stream.ReadI16();
        VelocityY = stream.ReadI16();
        VelocityZ = stream.ReadI16();
    }

    public int Id => 0x01;
    public int EntityId { get; set; }
    public UUID EntityUUID { get; set; }
    public EntityType Type { get; set; }
    public Vec3d Position { get; set; }
    public byte Pitch { get; set; }
    public byte Yaw { get; set; }
    public byte HeadYaw { get; set; }
    public int Data { get; set; }
    public short VelocityX { get; set; }
    public short VelocityY { get; set; }
    public short VelocityZ { get; set; }
}

public enum EntityType : int
{
    Cow = 28,
    Player = 149
}