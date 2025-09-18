using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Core;

public abstract class Protocol
{
    public IPacket? CreatePacketInstance(int id, ProtocolState state, PacketDirection direction)
    {
        if(!_ctors.TryGetValue(direction, out var dir)) return null;
        if (!dir.TryGetValue(state, out var ids)) return null;
        if (!ids.TryGetValue(id, out var ctor)) return null;
        
        return ctor() as IPacket;
    }

    public void RegisterAssemblyPackets(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var info = type.GetCustomAttribute<PacketInfoAttribute>();
            if(info == null) continue;

            RegisterPacket(type, info);
        }
    }
    public bool RegisterPacket(Type type, PacketInfoAttribute? packetInfo = null)
    {
        packetInfo ??= type.GetCustomAttribute<PacketInfoAttribute>();
        if(packetInfo == null) return false;
        
        var ctor = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();

        if (!_ctors.TryGetValue(packetInfo.Direction, out var states))
        {
            states = new();
            _ctors[packetInfo.Direction] = states;
        }
        if (!states.TryGetValue(packetInfo.State, out var ids))
        {
            ids = new();
            states[packetInfo.State] = ids;
        }

        ids[packetInfo.Id] = ctor;
        return true;
    }

    protected Protocol()
    {
        RegisterAssemblyPackets(typeof(Protocol).Assembly);
    }
    
    public abstract string Name { get; }
    public abstract int Version { get; }

    private readonly Dictionary<PacketDirection, Dictionary<ProtocolState, Dictionary<int, Func<object>>>> _ctors =
        new();
}