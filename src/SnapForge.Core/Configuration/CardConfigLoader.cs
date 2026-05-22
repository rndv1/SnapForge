using System.Text.Json;

namespace SnapForge.Core.Configuration;

public static class CardConfigLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public static CardConfig Load(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<CardConfig>(stream, JsonOptions) ?? CardConfig.Empty;
    }
}
