using System.Text.Json;

namespace SnapForge.Core.Configuration;

internal static class ConfigurationJsonOptions
{
    public static JsonSerializerOptions Create()
    {
        return new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
    }
}
