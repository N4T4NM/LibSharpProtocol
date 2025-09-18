using System;
using System.IO;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Protocol772.Data.World;

namespace LibSharpProtocol.Protocol772.Data;

public class ChunkData : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }
    public void Read(ProtocolStream stream)
    {
        Heightmaps = stream.ReadArray(ReadHeightMap);
        
        int chunksDataLength = stream.ReadVarInt();
        PackedData = stream.Read(chunksDataLength);
    }

    public ChunkColumn Unpack(string dimension)
    {
        if(_unpackedChunk != null && _unpackedDimension == dimension) return _unpackedChunk;
        _unpackedDimension = dimension;
        
        // TODO: Use information from registry instead of const values
        int dimensionHeight;
        switch (dimension)
        {
            case "minecraft:overworld": dimensionHeight = 384; break;
            default: throw new NotImplementedException(dimension);
        }

        int sections = dimensionHeight / ChunkSection.CHUNK_SECTION_HEIGHT;
        using var s = new ProtocolStream(PackedData);
        
        //File.WriteAllBytes("/home/natan/chunk.dat", PackedData);
        
        _unpackedChunk = new(sections);
        _unpackedChunk.Read(s);
        
        return _unpackedChunk;
    }

    Heightmap ReadHeightMap(ProtocolStream stream)
    {
        var hm = new Heightmap();
        hm.Read(stream);

        return hm;
    }
    
    public Heightmap[] Heightmaps { get; set; } = [];
    public byte[] PackedData { get; set; } = [];

    private ChunkColumn? _unpackedChunk;
    private string? _unpackedDimension;
}

public enum HeightmapType : int
{
    WORLD_SURFACE = 1,
    MOTION_BLOCKING = 4,
    MOTION_BLOCKING_NO_LEAVES = 5
}