using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LibSharpProtocol.Protocol772.Data.Blocks;

public class Block
{
    public static Block Register(string name, JsonBlock json)
    {
        List<BlockState> states = new();
        Block block = new(name, json.Definition.Type, states);
        _blocks[block.Name] = block;
        
        foreach (var jsonState in json.States)
        {
            var state = new BlockState(jsonState.Id, block, jsonState.Properties);
            BlockState.Register(state);
        }

        return block;
    }

    internal Block(string name, string type, IList<BlockState> states)
    {
        Name = name;
        Type = type;
        States = new(states);
    }
    
    public string Name { get; }
    public string Type { get; }
    public ReadOnlyCollection<BlockState> States { get; }

    private static readonly Dictionary<string, Block> _blocks = new();
    public static ReadOnlyDictionary<string, Block> Blocks { get; } = new(_blocks);
}