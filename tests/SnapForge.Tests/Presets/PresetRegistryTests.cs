using SnapForge.Cli.Presets;

namespace SnapForge.Tests.Presets;

public sealed class PresetRegistryTests
{
    private readonly PresetRegistry registry = new(BuiltInPresets.All);

    [Theory]
    [InlineData("github", 1280, 720)]
    [InlineData("social", 1080, 1080)]
    [InlineData("portfolio", 1600, 900)]
    public void TryGet_ReturnsExpectedBuiltInPresetSize(string name, int expectedWidth, int expectedHeight)
    {
        var found = registry.TryGet(name, out var preset);

        Assert.True(found);
        Assert.NotNull(preset);
        Assert.Equal(expectedWidth, preset.Width);
        Assert.Equal(expectedHeight, preset.Height);
    }

    [Fact]
    public void TryGet_ReturnsFalseForUnknownPreset()
    {
        var found = registry.TryGet("unknown", out var preset);

        Assert.False(found);
        Assert.Null(preset);
    }
}
