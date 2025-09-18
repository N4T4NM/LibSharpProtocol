using System;
using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Protocol772.Data.World;

public class BiomeSection : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        this.Container.Read(stream);
    }

    public PalettedContainer Container { get; } = new(PaletteProvider.BIOME);
    public const int BIOME_SECTION_VOLUME = ChunkSection.BLOCK_SECTION_VOLUME / (4 * 4 * 4) | 0;
}

public sealed class BiomePaletteProvider : PaletteProvider
{
    public override DataProvider Provide(int bits)
    {
        return bits switch
        {
            0 => new DataProvider(SINGLE, bits),
            <= 3 => new DataProvider(INDIRECT, bits),
            _ => new DataProvider(DIRECT, 7)
        };
    }

    public override int ContainerSize => BiomeSection.BIOME_SECTION_VOLUME;
    
    public static IPalette.Factory SINGLE => SingleValuePalette.SINGLE;
    public static IPalette.Factory INDIRECT => IndirectPalette.INDIRECT;
    public static IPalette.Factory DIRECT => DirectPalette.DIRECT;
}