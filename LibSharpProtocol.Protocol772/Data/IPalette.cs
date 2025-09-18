using System;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Protocol772.Data.World;

namespace LibSharpProtocol.Protocol772.Data;


public interface IPalette : IProtocolSerializer
{
    public int GetState(int index);
    
    public interface Factory
    {
        public IPalette Create();
    }
}

public class SingleValuePalette : IPalette
{
    public void Write(ProtocolStream stream)
    {
        throw new NotImplementedException();
    }
    public void Read(ProtocolStream stream) => Entry = stream.ReadVarInt();
    public int GetState(int index) => Entry;
    
    public int Entry { get; set; }
    
    public static IPalette.Factory SINGLE { get; } = new SingleValuePaletteFactory();
    class SingleValuePaletteFactory : IPalette.Factory
    {
        public IPalette Create() => new SingleValuePalette();
    }
}
public class IndirectPalette : IPalette
{
    public void Write(ProtocolStream stream)
    {
        throw new NotImplementedException();
    }
    public void Read(ProtocolStream stream)
    {
        IDs = new int[stream.ReadVarInt()];
        for(int i = 0; i < IDs.Length; i++) IDs[i] = stream.ReadVarInt();
    }

    public int GetState(int index)
    {
        if(index >= IDs.Length) throw new ArgumentOutOfRangeException(nameof(index));
        return IDs[index];
    }

    public int[] IDs { get; set; } = [];
    
    public static IPalette.Factory INDIRECT { get; } = new IndirectPaletteFactory();
    class IndirectPaletteFactory : IPalette.Factory
    {
        public IPalette Create() => new IndirectPalette();
    }
}
public class DirectPalette : IPalette
{
    public void Write(ProtocolStream stream)
    {
        throw new NotImplementedException();
    }
    public void Read(ProtocolStream stream) {}
    public int GetState(int index) => index;
    
    public static IPalette.Factory DIRECT { get; } = new DirectPaletteFactory();
    class DirectPaletteFactory : IPalette.Factory
    {
        public IPalette Create() => new DirectPalette();
    }
}

public abstract class PaletteProvider()
{
    public abstract DataProvider Provide(int bits);
    public abstract int ContainerSize { get; }
    
    public static readonly PaletteProvider BLOCK_STATE = new BlockPaletteProvider();
    public static readonly PaletteProvider BIOME = new BiomePaletteProvider();
}