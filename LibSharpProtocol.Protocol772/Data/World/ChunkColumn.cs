using System;
using System.Linq;
using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Protocol772.Data.World;

public class ChunkColumn(int sectionCount) : IProtocolSerializer
{
    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }
    public void Read(ProtocolStream stream)
    {
        for (int i = 0; i < SectionCount; i++)
        {
            var chunkSec = new ChunkSection();
            var biomeSec = new BiomeSection();

            chunkSec.Read(stream);
            biomeSec.Read(stream);
            
            Sections[i] = chunkSec;
            Biomes[i] = biomeSec;
        }
    }

    public int GetState(int x, int y, int z)
    {
        y = Math.Max(y, WorldBase);
        
        int secIdx = (y - WorldBase) / ChunkSection.CHUNK_SECTION_HEIGHT;
        int localY = (y - WorldBase) % ChunkSection.CHUNK_SECTION_HEIGHT;
        
        return Sections[secIdx].GetState(x, localY, z);
    }
    public int SectionCount { get; } = sectionCount;
    public int WorldBase { get; } = -64;
    public ChunkSection[] Sections { get; } = new ChunkSection[sectionCount];
    public BiomeSection[] Biomes { get; } = new BiomeSection[sectionCount];
}