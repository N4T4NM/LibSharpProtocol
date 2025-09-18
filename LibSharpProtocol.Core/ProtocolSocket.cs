using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using LibSharpProtocol.Core.Async;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Core;

public class ProtocolSocket : IDisposable
{
    public void ReadPacket(PacketReceivedHandler handler) => ReadPacketAsyncHandler.Run(this, handler);
    public void WritePacket(IPacket packet, PacketSentHandler? handler) => WritePacketAsyncHandler.Run(this, packet, handler);

    public Task<GenericPacket> ReadPacketAsync() => ReadPacketAsyncHandler.AwaitAsync(this);
    public Task WritePacketAsync(IPacket packet) => WritePacketAsyncHandler.AwaitAsync(this, packet);
    
    public Task ConnectAsync(EndPoint ep) => BaseSocket.ConnectAsync(ep);
    
    public void Close() => BaseSocket.Close();
    public void Dispose()
    {
        if(DisposeSocket) BaseSocket.Dispose();
    }
    
    public ProtocolSocket(Protocol protocol) : this(protocol, new(SocketType.Stream, ProtocolType.Tcp), true) { }
    public ProtocolSocket(Protocol protocol, Socket baseSocket, bool disposeSocket)
    {
        if(baseSocket.ProtocolType != ProtocolType.Tcp) throw new ArgumentException("Expected a TCP socket", nameof(baseSocket));
        
        Protocol = protocol;
        BaseSocket = baseSocket;
        DisposeSocket = disposeSocket;
    }
    
    public Protocol Protocol { get; }
    public Socket BaseSocket { get; }
    public bool DisposeSocket { get; }
    public ProtocolState State { get; set; }
    public int CompressionThreshold { get; set; } = 0;
}
public enum ProtocolState
{
    Unknown = -1,
    Handshaking,
    Status,
    Login,
    Transfer,
    Configuration,
    Play
}