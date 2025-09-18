using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Core.Async;

public class WritePacketAsyncHandler : IDisposable
{
    public static void Run(ProtocolSocket socket, IPacket packet, PacketSentHandler? handler) => _ = new WritePacketAsyncHandler(socket, packet, handler);
    public static Task AwaitAsync(ProtocolSocket socket, IPacket packet) => new Awaiter(socket, packet).Await();
    
    void OnCompleted(object? sender, SocketAsyncEventArgs e)
    {
        if(e.SocketError != SocketError.Success) throw new SocketException((int)e.SocketError);

        _handler?.Invoke();
        Dispose();
    }

    public void Dispose()
    {
        if(Disposed) return;
        _args.Dispose();
        Disposed = true;
    }

    public WritePacketAsyncHandler(ProtocolSocket socket, IPacket packet, PacketSentHandler? handler)
    {
        Socket = socket;
        _handler = handler;

        _args.Completed += OnCompleted;

        var pck = GenericPacket.From(packet);
        _args.SetBuffer(pck.ToNetworkBuffer(Socket.CompressionThreshold));

        if (!Socket.BaseSocket.SendAsync(_args)) AsyncDispatcher.Run(() => OnCompleted(this, _args));
    }
    
    public ProtocolSocket Socket { get; }
    public bool Disposed { get; private set; }
    
    private readonly SocketAsyncEventArgs _args = new();
    private readonly PacketSentHandler? _handler;

    class Awaiter
    {
        public Task Await() => _signal.Await();
        
        void OnSent() => _signal.Signal(true);
        public Awaiter(ProtocolSocket socket, IPacket packet)
        {
            _ = new WritePacketAsyncHandler(socket, packet, OnSent);
        }
        
        private readonly AsyncSignal<bool> _signal = new();
    }
}
public delegate void PacketSentHandler();