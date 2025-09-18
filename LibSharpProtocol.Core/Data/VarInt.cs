using System;

namespace LibSharpProtocol.Core.Data;

public struct VarInt(int value) : IEquatable<VarInt>,  IEquatable<int>
{
    public static VarInt Read(VarIntByteReader reader)
    {
        int value = 0x00;
        int pos = 0x00;
        byte[] data = new byte[0x05];
        int wb = 0x00;
        while (true)
        {
            var cb = reader();
            data[wb++] = cb;
            
            value |= (cb & SEG_BITS) << pos;
            if((cb & CON_BIT) == 0x00) break;

            pos += 0x07;
            if (pos >= 32) throw new IndexOutOfRangeException();
        }

        byte[] result = new byte[wb];
        Array.Copy(data, result, wb);
        
        return new(value, result);
    }
    
    public byte[] ToArray()
    {
        if (_buffer == null) InitBuffer();
        return _buffer!;
    }
    public override string ToString() => _value.ToString();

    void InitBuffer()
    {
        byte[] data = new byte[0x05];
        int value = _value;
        int bw = 0x00;
        
        while (true)
        {
            if ((value & ~SEG_BITS) == 0x00)
            {
                data[bw++] = (byte)value;
                break;
            }

            data[bw++] = (byte)((value & SEG_BITS) | CON_BIT);
            value >>= 0x07;
        }

        _buffer = new byte[bw];
        Array.Copy(data, _buffer, bw);
    }

    VarInt(int value, byte[] buffer) : this(value)
    {
        _buffer = buffer;
    }
    
    public int SizeOf
    {
        get
        {
            if (_buffer == null) InitBuffer();
            return _buffer!.Length;
        }
    }
    
    public bool Equals(VarInt other) => other._value == _value;
    public bool Equals(int other) => other == _value;

    public override bool Equals(object? obj)
    {
        switch (obj)
        {
            case VarInt v: return Equals(v);
            case int v: return Equals(v);
            default: return obj?.Equals(_value) == true;
        }
    }
    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(VarInt l, VarInt r) => l._value == r._value;
    public static bool operator ==(VarInt l, int r) => l._value == r;
    public static bool operator ==(int l, VarInt r) => l == r._value;
    
    public static bool operator !=(VarInt l, VarInt r) => l._value != r._value;
    public static bool operator !=(VarInt l, int r) => l._value != r;
    public static bool operator !=(int l, VarInt r) => l != r._value;
    
    public static implicit operator VarInt(int value) => new(value);
    public static implicit operator int(VarInt value) => value._value;
    
    private readonly int _value = value;
    private byte[]? _buffer;

    private const int SEG_BITS = 0x7F;
    private const int CON_BIT = 0x80;

    public class AsyncBuilder
    {
        public bool Push(byte b)
        {
            _data[_wb++] = b;
            _value |= (b & SEG_BITS) << _pos;
            if ((b & CON_BIT) == 0x00) return true;

            _pos += 0x07;
            if(_pos >= 32) throw new IndexOutOfRangeException();

            return false;
        }

        public VarInt GetResult()
        {
            byte[] result = new byte[_wb];
            Array.Copy(_data, result, _wb);

            return new(_value, result);
        }

        private int _value;
        private int _pos;
        private int _wb;
        private byte[] _data = new byte[0x05];
    }
}

public delegate byte VarIntByteReader();