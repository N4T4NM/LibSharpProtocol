using System;
using System.Text;

namespace LibSharpProtocol.Core.Data;

public struct UUID(byte[] data) : IEquatable<UUID>
{
    public bool Equals(UUID other) => _lo == other._lo && _hi == other._hi;
    public byte[] GetBuffer() => _data ?? new byte[16];
    public override string ToString()
    {
        if (_str != null) return _str;
        
        return _str = $"{_data[0]:x2}{_data[1]:x2}{_data[2]:x2}{_data[3]:x2}-" +
               $"{_data[4]:x2}{_data[5]:x2}-" +
               $"{_data[6]:x2}{_data[7]:x2}-" +
               $"{_data[8]:x2}{_data[9]:x2}-" +
               $"{_data[10]:x2}{_data[11]:x2}{_data[12]:x2}{_data[13]:x2}{_data[14]:x2}{_data[15]:x2}";
    }

    public override bool Equals(object? obj) => obj is UUID uuid && Equals(uuid);
    public override int GetHashCode() => HashCode.Combine(_lo, _hi);

    public static bool operator ==(UUID left, UUID right) => left.Equals(right);
    public static bool operator !=(UUID left, UUID right) => !left.Equals(right);
    
    public UUID() : this(new byte[16]) { }
    
    public static int SizeOf => 16;
    private readonly byte[] _data = data;
    private string? _str;
    
    private readonly long _lo = BitConverter.ToInt64(data, 0);
    private readonly long _hi = BitConverter.ToInt64(data, 8);
}