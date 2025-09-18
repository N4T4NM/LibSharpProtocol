using System;
using System.Net;
using System.Threading.Tasks;
using LibSharpProtocol.Core.Async;
using LibSharpProtocol.Core.Auth;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Core;

public class ProtocolClient(Protocol protocol, IAuthProvider auth) : IDisposable
{
    public void ReadPacket(PacketReceivedHandler handler) => Socket.ReadPacket(handler);
    public void WritePacket(IPacket packet, PacketSentHandler? handler = null) => Socket.WritePacket(packet, handler);

    public Task<GenericPacket> ReadPacketAsync() => Socket.ReadPacketAsync();
    public Task WritePacketAsync(IPacket packet) => Socket.WritePacketAsync(packet);

    public async Task ConnectAsync(EndPoint ep)
    {
        await Socket.ConnectAsync(ep);
        await AuthProvider.LogInAsync(this, ep);
    }
    
    public void Close() => Socket.Close();
    public void Dispose() => Socket.Dispose();
    
    public Protocol Protocol { get; } = protocol;
    public ProtocolSocket Socket { get; } = new(protocol);
    public ProtocolState State { get => Socket.State; set => Socket.State = value; }
    public int CompressionThreshold { get => Socket.CompressionThreshold; set => Socket.CompressionThreshold = value; }
    public IAuthProvider AuthProvider { get; } = auth;
}