using System;
using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Core.Packets;

public interface IPacket : IProtocolSerializer
{
    public int Id { get; }
}


[AttributeUsage(AttributeTargets.Class)]
public class PacketInfoAttribute(int id, PacketDirection direction, ProtocolState state) : Attribute
{
    public int Id { get; } = id;
    public PacketDirection Direction { get; } = direction;
    public ProtocolState State { get; } = state;
}
public enum PacketDirection
{
    Unknown = -1,
    C2S,
    S2C
}