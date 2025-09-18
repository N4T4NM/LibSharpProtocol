using System;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Protocol772.Data.Blocks;

namespace LibSharpProtocol.Protocol772.Data.World;

public class ChunkSection : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        SolidBlockCount = stream.ReadI16();
        Blocks.Read(stream);
    }
    
    public int GetState(int x, int y, int z) => GetState(GetBlockIndex(x, y, z));
    public int GetState(int blockIdx) => this.Blocks.Data.GetState(blockIdx);
    public int GetBlockIndex(int x, int y, int z) => (y << 8) | (z << 4) | x;
    
    public short SolidBlockCount { get; set; }
    public PalettedContainer Blocks { get; } = new(PaletteProvider.BLOCK_STATE);

    public const int CHUNK_SECTION_HEIGHT = 16;
    public const int CHUNK_WIDTH = 16;
    public const int BLOCK_SECTION_VOLUME = CHUNK_SECTION_HEIGHT * CHUNK_WIDTH * CHUNK_WIDTH;
}

public sealed class BlockPaletteProvider : PaletteProvider
{
    public override DataProvider Provide(int bits)
    {
        return bits switch
        {
            0 => new DataProvider(SINGLE, bits),
            < 4 => new DataProvider(INDIRECT, 4),
            <= 8 => new DataProvider(INDIRECT, bits),
            _ => new DataProvider(DIRECT, 15)
        };
    }

    public override int ContainerSize => 4096;

    public static IPalette.Factory SINGLE => SingleValuePalette.SINGLE;
    public static IPalette.Factory INDIRECT => IndirectPalette.INDIRECT;
    public static IPalette.Factory DIRECT => DirectPalette.DIRECT;
}