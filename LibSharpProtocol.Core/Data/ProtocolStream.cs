using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace LibSharpProtocol.Core.Data;

public class ProtocolStream(Stream baseStream, bool leaveOpen) : Stream
{
    public void WriteVarInt(VarInt vi) => Write(vi.ToArray());
    public void WriteString(string s)
    {
        byte[] data = Encoding.UTF8.GetBytes(s);
        WriteVarInt(data.Length);
        Write(data);
    }
    public void WriteUUID(UUID uuid) => Write(uuid.GetBuffer());

    public void WriteVec3d(Vec3d vec)
    {
        WriteF64(vec.X);
        WriteF64(vec.Y);
        WriteF64(vec.Z);
    }
    public void WriteArray<T>(T[] array, ArrayWriter<T> writer)
    {
        WriteVarInt(array.Length);
        foreach (var e in array) writer(this, e);
    }

    public void WriteOptional(byte[]? buffer)
    {
        if (buffer == null)
        {
            WriteU8(0x00);
            return;
        }
        
        WriteU8(0x01);
        Write(buffer);
    }
    public void WriteOptional<T>(T? value, OptionalWriter<T> writer)
    {
        if (value == null)
        {
            WriteU8(0x00);
            return;
        }
        
        WriteU8(0x01);
        writer(value);
    }
    public void WriteBool(bool b) => WriteU8((byte)(b ? 1 : 0));
    public void WriteU8(byte u8) => WriteByte(u8);
    public void WriteU16(ushort u16) => WritePrimitive(BinaryPrimitives.WriteUInt16BigEndian, u16, 0x02);
    public void WriteI64(long i64) => WritePrimitive(BinaryPrimitives.WriteInt64BigEndian, i64, 0x08);
    public void WriteF32(float f32) => WritePrimitive(BinaryPrimitives.WriteSingleBigEndian, f32, 0x04);
    public void WriteF64(double f64) => WritePrimitive(BinaryPrimitives.WriteDoubleBigEndian, f64, 0x08);
    public void WritePrimitive<T>(PrimitiveWriter<T> writer, T value, int length)
    {
        byte[] buffer = new byte[length];
        writer(buffer, value);
        
        Write(buffer);
    }
    
    public byte[] Read(int count)
    {
        byte[] buffer = new byte[count];
        int offset = 0;
        while (offset < count)
        {
            int br = Read(buffer, offset, count);
            if (br == 0) throw new EndOfStreamException();
            
            offset += br;
            count -= br;
        }
        
        return buffer;
    }

    public VarInt ReadVarInt() => VarInt.Read(ReadU8);
    public string ReadString()
    {
        var len = ReadVarInt();
        byte[] data = Read(len);

        return Encoding.UTF8.GetString(data);
    }
    public UUID ReadUUID() => new(Read(16));
    public bool ReadBool() => ReadU8() == 1;
    public Vec3d ReadVec3d() => new(ReadF64(), ReadF64(), ReadF64());

    public Vec3d ReadPackedVec3d()
    {
        long i64 = ReadI64();
        
        int x = (int)(i64 >> 38);
        int y = (int)(i64 << 52 >> 52);
        int z = (int)(i64 << 26 >> 38);
        
        return new Vec3d(x, y, z);
    }
    public T? ReadOptional<T>(OptionalReader<T> reader)
    {
        bool opt = ReadBool();
        if (!opt) return default;

        return reader();
    }

    public T? ReadOptional<T>(OptionalStreamReader<T> reader)
    {
        bool opt = ReadBool();
        if (!opt) return default;
        
        return reader(this);
    }
    public T[] ReadArray<T>(ArrayReader<T> reader)
    {
        int len = ReadVarInt();
        T[] result = new T[len];
        for (int i = 0; i < len; i++) result[i] = reader(this);
        
        return result;
    }
    public byte ReadU8() => (byte)ReadByte();
    public sbyte ReadI8() => (sbyte)Read(0x01)[0x00];
    public short ReadI16() => ReadPrimitive(BinaryPrimitives.ReadInt16BigEndian, 0x02);
    public int ReadI32() => ReadPrimitive(BinaryPrimitives.ReadInt32BigEndian, 0x04);
    public long ReadI64() => ReadPrimitive(BinaryPrimitives.ReadInt64BigEndian, 0x08);
    public ulong ReadU64() => ReadPrimitive(BinaryPrimitives.ReadUInt64BigEndian, 0x08);
    public float ReadF32() =>  ReadPrimitive(BinaryPrimitives.ReadSingleBigEndian, 0x04);
    public double ReadF64() => ReadPrimitive(BinaryPrimitives.ReadDoubleBigEndian, 0x08);
    
    public T ReadPrimitive<T>(PrimitiveReader<T> reader, int length)
    {
        byte[] data = Read(length);
        return reader(data);
    }

    public byte[] ToArray()
    {
        if(BaseStream is MemoryStream ms) return ms.ToArray();

        Position = 0;
        return Read((int)Length);
    }
    public byte[] ReadRemaining() => Read((int)(Length - Position));
    
    #region Overrides
    public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);
    public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);
    
    public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);
    public override void SetLength(long value) => BaseStream.SetLength(value);
    
    public override void Flush() => BaseStream.Flush();
    public override void Close()
    {
        base.Close();
        if(!LeaveOpen) BaseStream.Close();
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if(!LeaveOpen) BaseStream.Dispose();
    }
    #endregion
    
    public ProtocolStream() : this(new MemoryStream(), false) { }
    public ProtocolStream(byte[] buffer) : this(new MemoryStream(buffer), false) { }

    public override bool CanRead => BaseStream.CanRead;
    public override bool CanSeek => BaseStream.CanSeek;
    public override bool CanWrite => BaseStream.CanWrite;
    public override long Length => BaseStream.Length;
    public override long Position { get => BaseStream.Position; set => BaseStream.Position = value; }
    
    public Stream BaseStream { get; } = baseStream;
    public bool LeaveOpen { get; } = leaveOpen;

    public delegate void PrimitiveWriter<in T>(Span<byte> dest, T value);
    public delegate T PrimitiveReader<out T>(ReadOnlySpan<byte> src);
    
    public delegate T ArrayReader<out T>(ProtocolStream s);
    public delegate void ArrayWriter<in T>(ProtocolStream s, T value);
    public delegate T OptionalReader<out T>();
    public delegate void OptionalWriter<in T>(T value);
    public delegate T OptionalStreamReader<out T>(ProtocolStream s);
}