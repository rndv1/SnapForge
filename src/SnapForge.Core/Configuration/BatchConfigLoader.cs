using System.Text.Json;

namespace SnapForge.Core.Configuration;

public static class BatchConfigLoader
{
    private static readonly JsonSerializerOptions JsonOptions = ConfigurationJsonOptions.Create();

    public static BatchConfig Load(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<BatchConfig>(stream, JsonOptions) ?? new BatchConfig();
    }
}
