using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Protocol772.Data;


public class Heightmap : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public void Read(ProtocolStream stream)
    {
        Type = (HeightmapType)(int)stream.ReadVarInt();
        Data = stream.ReadArray(ReadDataEntry);
    }
    long ReadDataEntry(ProtocolStream s) => s.ReadI64();
    
    public HeightmapType Type { get; set; }
    public long[] Data { get; set; } = [];
}