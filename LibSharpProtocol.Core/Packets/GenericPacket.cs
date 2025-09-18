using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Core.Packets;

public class GenericPacket : IPacket
{
    public void Write(ProtocolStream stream)
    {
        throw new NotImplementedException();
    }
    public void Read(ProtocolStream stream, int compressionThreshold)
    {
        _id = null;
        _body = null;
        _translated = null;

        _bodyOffset = null;
        
        _compressionThreshold = compressionThreshold;
        if (compressionThreshold > 0)
        {
            _data = stream.ReadRemaining();
            _decompressedData = null;
        }
        else
        {
            _data = null;
            _decompressedData = stream.ReadRemaining();
        }
    }
    void IProtocolSerializer.Read(ProtocolStream stream) => throw new NotSupportedException();
    
    public byte[] ToNetworkBuffer(int compressionThreshold)
    {
        byte[] data = LoadData(compressionThreshold);
        
        using var stream = new ProtocolStream();
        stream.WriteVarInt(data.Length);
        stream.Write(data);
        
        return stream.ToArray();
    }
    public IPacket TranslatePacket(Protocol protocol)
    {
        if(_translated != null) return _translated;
        _translated = protocol.CreatePacketInstance(this.Id, this.State, this.Direction);

        if (_translated == null) return this;

        using var s = new ProtocolStream(LoadBody());
        _translated.Read(s);
        
        return _translated;
    }
    public static GenericPacket From(IPacket packet)
    {
        if(packet is GenericPacket generic) return generic;
        
        using var stream = new ProtocolStream();
        packet.Write(stream);

        return new GenericPacket(packet.Id, stream.ToArray());
    }

    public override string ToString() => $"0x{Id:X2} | {State} ({Direction})";

    int LoadId()
    {
        if (_id != null) return _id.Value;
        using var stream = new ProtocolStream(LoadDecompressedData());
        _id = stream.ReadVarInt();
        _bodyOffset = stream.Position;

        return _id.Value;
    }
    
    byte[] LoadData(int compressionThreshold)
    {
        if (compressionThreshold > 0)
        {
            if (_compressionThreshold == compressionThreshold && _data != null) return _data;
            _compressionThreshold = compressionThreshold;
            
            byte[] src = LoadDecompressedData();
            using var s = new ProtocolStream();
            if (src.Length > compressionThreshold)
            {
                using var zlib = new ZLibStream(s, CompressionMode.Compress, leaveOpen: true);

                s.WriteVarInt(src.Length);
                zlib.Write(src, 0, src.Length);
                zlib.Flush();
                zlib.Close();
                
                return _data = s.ToArray();
            }
            
            s.WriteVarInt(0x00);
            s.Write(src);

            return _data = s.ToArray();
        } else if (_compressionThreshold != 0)
        {
            _compressionThreshold = 0;
            return _data = LoadDecompressedData();
        }
        
        if(_data != null) return _data;
        return _data = LoadDecompressedData();
    }
    byte[] LoadDecompressedData()
    {
        if (_decompressedData != null) return _decompressedData;
        if (_body == null)
        {
            if(_data == null) throw new InvalidDataException("Packet doesn't contain a body or data");
            
            using var compressed = new ProtocolStream(_data);
            var decompLength = compressed.ReadVarInt();

            if (decompLength == 0) return _decompressedData = compressed.ReadRemaining();
            
            using var zlib = new ZLibStream(compressed, CompressionMode.Decompress, leaveOpen: true);
            _decompressedData = new byte[decompLength];

            int offset = 0;
            while (offset < decompLength)
            {
                int br = zlib.Read(_decompressedData, offset, decompLength - offset);
                if (br == 0)
                {
                    Debug.WriteLine($"[WARNING] Could not read all bytes. Expected: {decompLength}, Got: {offset}");
                    break;
                }

                offset += br;
            }
            
            return _decompressedData;
        }
        
        if(_id == null) throw new InvalidDataException("Packet doesn't contain an ID");

        using var s = new ProtocolStream();
        s.WriteVarInt(_id.Value);
        s.Write(_body);

        return _decompressedData = s.ToArray();
    }

    byte[] LoadBody()
    {
        if(_body != null) return _body;
        
        if (_bodyOffset == null) LoadId();
        using var s = new ProtocolStream(_decompressedData!);
        s.Position = _bodyOffset!.Value;

        return _body = s.ReadRemaining();
    }

    
    public GenericPacket() {}
    internal GenericPacket(int id, byte[] body)
    {
        _id = id;
        _body = body;
    }

    public int Id => LoadId();
    public int DataLength => LoadData(_compressionThreshold).Length;
    public int DecompressedDataLength => LoadDecompressedData().Length;
    public int CompressionThreshold => _compressionThreshold;
    public byte[] Body => LoadBody();

    public PacketDirection Direction { get; set; } = PacketDirection.Unknown;
    public ProtocolState State { get; set; } = ProtocolState.Unknown;
    
    private int _compressionThreshold;
    
    private int? _id;
    private byte[]? _data;
    private byte[]? _decompressedData;
    private byte[]? _body;

    private long? _bodyOffset;

    private IPacket? _translated;
}