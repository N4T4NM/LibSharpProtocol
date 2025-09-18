namespace LibSharpProtocol.Protocol772.Data;

public class DataProvider(IPalette.Factory factory, int bits)
{
    public PaletteData CreateData(int size)
    {
        PaletteStorage storage = Bits == 0 ? new EmptyPaletteStorage() : new PackedPaletteStorage(Bits, size);
        IPalette palette = Factory.Create();
        
        return new PaletteData(this, storage, palette);
    }

    public IPalette.Factory Factory { get; } = factory;
    public int Bits { get; } = bits;
}