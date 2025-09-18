using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LibSharpProtocol.Protocol772.Data.Blocks;

public static class JsonBlockReader
{
    public static Dictionary<string, JsonBlock> Read(string file)
    {
        using var stream = File.OpenRead(file);
        return JsonSerializer.Deserialize<Dictionary<string, JsonBlock>>(stream, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        })!;
    }
}

public class JsonBlock
{
    public JsonBlockDefinition Definition { get; set; } = new();
    public List<JsonBlockState> States { get; set; } = new();
}
public class JsonBlockState
{
    public int Id { get; set; }
    public Dictionary<string, string> Properties { get; set; } = new();
}

public class JsonBlockDefinition
{
    public string Type { get; set; } = string.Empty;
}