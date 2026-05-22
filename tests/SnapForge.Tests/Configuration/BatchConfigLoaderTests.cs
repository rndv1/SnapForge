using System.Text.Json;
using SnapForge.Core.Configuration;

namespace SnapForge.Tests.Configuration;

public sealed class BatchConfigLoaderTests
{
    [Fact]
    public void Load_ReturnsDefaultsAndCards()
    {
        var path = WriteConfig(
            """
            {
              "defaults": {
                "title": "SnapForge",
                "subtitle": "Batch-generated cards",
                "preset": "github",
                "theme": "dark",
                "background": "#0F172A",
                "padding": 112
              },
              "cards": [
                {
                  "input": "./input/sample.png",
                  "output": "./output/readme.png"
                },
                {
                  "input": "./input/sample.png",
                  "output": "./output/social.png",
                  "preset": "social",
                  "theme": "light"
                }
              ]
            }
            """);

        var config = BatchConfigLoader.Load(path);

        Assert.Equal("SnapForge", config.Defaults.Title);
        Assert.Equal("Batch-generated cards", config.Defaults.Subtitle);
        Assert.Equal("github", config.Defaults.Preset);
        Assert.Equal("dark", config.Defaults.Theme);
        Assert.Equal("#0F172A", config.Defaults.Background);
        Assert.Equal(112, config.Defaults.Padding);
        Assert.Equal(2, config.Cards.Length);
        Assert.Equal("./output/readme.png", config.Cards[0].Output);
        Assert.Equal("social", config.Cards[1].Preset);
        Assert.Equal("light", config.Cards[1].Theme);
    }

    [Fact]
    public void Load_ReturnsEmptyBatchForEmptyJsonObject()
    {
        var path = WriteConfig("{}");

        var config = BatchConfigLoader.Load(path);

        Assert.False(config.Defaults.HasValues);
        Assert.Empty(config.Cards);
    }

    [Fact]
    public void Load_AllowsCommentsAndTrailingCommas()
    {
        var path = WriteConfig(
            """
            {
              // Useful while iterating on many outputs.
              "cards": [
                {
                  "title": "SnapForge",
                },
              ],
            }
            """);

        var config = BatchConfigLoader.Load(path);

        Assert.Single(config.Cards);
        Assert.Equal("SnapForge", config.Cards[0].Title);
    }

    [Fact]
    public void Load_ThrowsJsonExceptionForInvalidJson()
    {
        var path = WriteConfig("{ invalid json");

        Assert.Throws<JsonException>(() => BatchConfigLoader.Load(path));
    }

    private static string WriteConfig(string json)
    {
        var directory = Path.Combine(Path.GetTempPath(), "snapforge-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, "snapforge.batch.json");
        File.WriteAllText(path, json);
        return path;
    }
}
