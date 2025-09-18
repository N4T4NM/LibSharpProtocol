namespace LibSharpProtocol.Protocol772.Data;

public class PaletteData(DataProvider config, PaletteStorage storage, IPalette palette)
{
    public int GetState(int blockIdx)
    {
        int idx = this.Storage.GetIdIndex(blockIdx);
        return Palette.GetState(idx);
    }
    
    public DataProvider Config { get; } = config;
    public PaletteStorage Storage { get; } = storage;
    public IPalette Palette { get; } = palette;
}