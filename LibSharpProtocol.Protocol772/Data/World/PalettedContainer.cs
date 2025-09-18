using System;
using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Protocol772.Data.World;

public class PalettedContainer : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        int bits = stream.ReadU8();
        this.Data = CreateData(bits);
        this.Data.Palette.Read(stream);

        for (int i = 0; i < this.Data.Storage.Data.Length; i++)
            this.Data.Storage.Data[i] = stream.ReadI64();
    }

    public PaletteData CreateData(int bits)
    {
        var dataProvider = PaletteProvider.Provide(bits);
        return dataProvider.CreateData(PaletteProvider.ContainerSize);
    }

    public PalettedContainer(PaletteProvider paletteProvider)
    {
        PaletteProvider = paletteProvider;
        Data = CreateData(0);
    }
    
    public PaletteProvider PaletteProvider { get; }
    public PaletteData Data { get; set; }
}
