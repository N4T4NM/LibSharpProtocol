using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Core.Async;

public class ReadPacketAsyncHandler : IDisposable
{
    public static void Run(ProtocolSocket socket, PacketReceivedHandler handler) => _ = new ReadPacketAsyncHandler(socket, handler);
    public static Task<GenericPacket> AwaitAsync(ProtocolSocket socket) => new Awaiter(socket).Await();
    
    void OnCompleted(object? sender, SocketAsyncEventArgs e)
    {
        if(e.SocketError != SocketError.Success) throw new SocketException((int)e.SocketError);
        if (e.BytesTransferred == 0) RunReceiverLoop();
        
        if (!_hasHeader)
        {
            if (_builder.Push(_headerBuffer[0x00]))
            {
                _hasHeader = true;
                _dataLength = _builder.GetResult();
                _outputBuffer = new byte[_dataLength];
                _args.SetBuffer(_outputBuffer, 0, _dataLength);
            }
            RunReceiverLoop();
            return;
        }
        
        _stream.Write(_outputBuffer, 0, e.BytesTransferred);
        if (_stream.Length < _dataLength)
        {
            e.SetBuffer(0, (int)(_dataLength - _stream.Length));
            RunReceiverLoop();
            return;
        }
        
        _stream.Position = 0;
        var pck = new GenericPacket();
        pck.Read(_stream, Socket.CompressionThreshold);
        pck.State = Socket.State;
        pck.Direction = PacketDirection.S2C;
        
        _handler.Invoke(pck);
        Dispose();
    }

    public void Dispose()
    {
        if(Disposed) return;
        _args.Dispose();
        _stream.Dispose();
        Disposed = true;
    }

    void RunReceiverLoop()
    {
        if (!Socket.BaseSocket.ReceiveAsync(_args)) AsyncDispatcher.Run(() => OnCompleted(this, _args));
    }

    public ReadPacketAsyncHandler(ProtocolSocket socket, PacketReceivedHandler handler)
    {
        Socket = socket;
        _handler = handler;
        _stream = new();
        
        _args.Completed += OnCompleted;
        _args.SetBuffer(_headerBuffer);

        RunReceiverLoop();
    }
    
    public ProtocolSocket Socket { get; }
    public bool Disposed { get; private set; }
    
    private byte[] _headerBuffer = new byte[0x01];
    private byte[] _outputBuffer = [];
    
    private readonly SocketAsyncEventArgs _args = new();
    private readonly PacketReceivedHandler _handler;

    private VarInt _dataLength;
    private bool _hasHeader;
    private readonly VarInt.AsyncBuilder _builder = new();
    private readonly ProtocolStream _stream;

    class Awaiter
    {
        public Task<GenericPacket> Await() => _signal.Await();
        
        void OnReceived(GenericPacket packet) => _signal.Signal(packet);
        public Awaiter(ProtocolSocket socket)
        {
            _ = new ReadPacketAsyncHandler(socket, OnReceived);
        }
        
        private readonly AsyncSignal<GenericPacket> _signal = new();
    }
}
public delegate void PacketReceivedHandler(GenericPacket packet);