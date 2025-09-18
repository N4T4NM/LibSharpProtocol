using System;
using System.Collections.Generic;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Protocol772.Data.Blocks;
using LibSharpProtocol.Protocol772.Data.World;

namespace LibSharpProtocol.Protocol772.Data.World;

public class Chunk(int chunkX, int chunkZ, ChunkColumn column, string dimension)
{
    public BlockState? GetState(int x, int y, int z)
    {
        Vec3d vec = new(x, y, z);
        if (_states.TryGetValue(vec, out var state)) return state;
        
        int id = Column.GetState(x, y, z);
        var s = BlockState.States.GetValueOrDefault(id);
        _states[vec] = s;

        return s;
    }
    public void SetState(int x, int y, int z, BlockState state)
    {
        if (x >= ChunkSection.CHUNK_WIDTH || z >= ChunkSection.CHUNK_WIDTH)
            throw new ArgumentOutOfRangeException();
        
        Vec3d vec = new(x, y, z);
        _states[vec] = state;
    }
    
    public static Chunk Load(int chunkX, int chunkZ, ChunkData data, string dimension) => new(chunkX, chunkZ, data.Unpack(dimension), dimension);

    public int X { get; } = chunkX;
    public int Z { get; } = chunkZ;
    private ChunkColumn Column { get; } = column;
    public string Dimension { get; } = dimension;

    private readonly Dictionary<Vec3d, BlockState?> _states = new();
}