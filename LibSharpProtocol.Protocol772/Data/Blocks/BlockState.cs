using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace LibSharpProtocol.Protocol772.Data.Blocks;

public class BlockState
{
    public static void LoadFromJson(string path)
    {
        var data = JsonBlockReader.Read(path);
        foreach (var block in data)
            Block.Register(block.Key, block.Value);
    }
    internal static void Register(BlockState state)
    {
        if(_states.TryAdd(state.Id, state)) _blockIds.Add(state.Id);
    }

    public static int EnsureIDsLoaded()
    {
        if (_blockIds.Count == 0) throw new DataException("You should call BlockState.LoadFromJson before reading IDs");
        return IDs.Count;
    }

    public override string ToString() => $"({Id}) {Block.Name}";

    internal BlockState(int id, Block block, IDictionary<string, string> properties)
    {
        Id = id;
        Block = block;
        Properties = new ReadOnlyDictionary<string, string>(properties);
    }
    
    public int Id { get; }
    public Block Block { get; }
    public ReadOnlyDictionary<string, string> Properties { get; }
    
    private static readonly List<int> _blockIds = new();
    private static readonly Dictionary<int, BlockState> _states = new();
    
    public static ReadOnlyCollection<int> IDs { get; } = new(_blockIds);
    public static ReadOnlyDictionary<int, BlockState> States { get; } = new(_states);
}