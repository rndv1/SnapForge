using System.Text.Json;
using SnapForge.Core.Configuration;

namespace SnapForge.Tests.Configuration;

public sealed class CardConfigLoaderTests
{
    [Fact]
    public void Load_ReturnsConfigValues()
    {
        var path = WriteConfig(
            """
            {
              "input": "./input/sample.png",
              "output": "./output/sample-card.png",
              "title": "SnapForge",
              "subtitle": "GitHub-ready screenshots",
              "preset": "github",
              "theme": "dark",
              "background": "#0F172A",
              "padding": 112
            }
            """);

        var config = CardConfigLoader.Load(path);

        Assert.Equal("./input/sample.png", config.Input);
        Assert.Equal("./output/sample-card.png", config.Output);
        Assert.Equal("SnapForge", config.Title);
        Assert.Equal("GitHub-ready screenshots", config.Subtitle);
        Assert.Equal("github", config.Preset);
        Assert.Equal("dark", config.Theme);
        Assert.Equal("#0F172A", config.Background);
        Assert.Equal(112, config.Padding);
        Assert.True(config.HasValues);
    }

    [Fact]
    public void Load_ReturnsEmptyConfigForEmptyJsonObject()
    {
        var path = WriteConfig("{}");

        var config = CardConfigLoader.Load(path);

        Assert.False(config.HasValues);
    }

    [Fact]
    public void Load_AllowsCommentsAndTrailingCommas()
    {
        var path = WriteConfig(
            """
            {
              // Useful while iterating on project branding.
              "title": "SnapForge",
            }
            """);

        var config = CardConfigLoader.Load(path);

        Assert.Equal("SnapForge", config.Title);
    }

    [Fact]
    public void Load_ThrowsJsonExceptionForInvalidJson()
    {
        var path = WriteConfig("{ invalid json");

        Assert.Throws<JsonException>(() => CardConfigLoader.Load(path));
    }

    private static string WriteConfig(string json)
    {
        var directory = Path.Combine(Path.GetTempPath(), "snapforge-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, "snapforge.config.json");
        File.WriteAllText(path, json);
        return path;
    }
}
