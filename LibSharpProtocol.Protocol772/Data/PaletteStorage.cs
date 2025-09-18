using System;

namespace LibSharpProtocol.Protocol772.Data;

public abstract class PaletteStorage
{
    public abstract int GetIdIndex(int blockIdx);
    public abstract long[] Data { get; }
}

public class EmptyPaletteStorage : PaletteStorage
{
    public override int GetIdIndex(int blockIdx) => 0;
    public override long[] Data => [];
}

public class PackedPaletteStorage : PaletteStorage
{
    public override int GetIdIndex(int blockIdx)
    {
        int startEntry   = blockIdx / EntriesPerLong;
        int offsetInLong = blockIdx % EntriesPerLong;
        int startBit     = offsetInLong * Bits;

        return (int)((Data[startEntry] >> startBit) & ((1L << Bits) - 1));
    }

    public PackedPaletteStorage(int bits, int size)
    {
        Bits = bits;
        Size = size;
        EntriesPerLong = 64 / bits;
        Data = new long[(size + EntriesPerLong - 1) / EntriesPerLong];
    }
    
    public int Bits { get; }
    public int Size { get; }
    public int EntriesPerLong { get; }
    public override long[] Data { get; }
}