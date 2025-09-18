using System;

namespace LibSharpProtocol.Core.Data;

public struct Vec3d(double x, double y, double z) : IEquatable<Vec3d>
{
    public bool Equals(Vec3d other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }
    public override bool Equals(object? obj)
    {
        return obj is Vec3d other && Equals(other);
    }
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Z.GetHashCode();
            return hashCode;
        }
    }
    public override string ToString() => $"{X}, {Y}, {Z}";
    
    public static bool operator ==(Vec3d left, Vec3d right) => left.Equals(right);
    public static bool operator !=(Vec3d left, Vec3d right) => !left.Equals(right);

    public static readonly Vec3d Zero = new(0, 0, 0);
    
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    public double Z { get; set; } = z;
}