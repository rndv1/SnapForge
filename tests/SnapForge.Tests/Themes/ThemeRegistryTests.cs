using SnapForge.Cli.Themes;

namespace SnapForge.Tests.Themes;

public sealed class ThemeRegistryTests
{
    private readonly ThemeRegistry registry = new(BuiltInThemes.All);

    [Theory]
    [InlineData("dark")]
    [InlineData("light")]
    public void TryGet_ReturnsBuiltInTheme(string name)
    {
        var found = registry.TryGet(name, out var theme);

        Assert.True(found);
        Assert.NotNull(theme);
        Assert.Equal(name, theme.Name);
    }

    [Fact]
    public void TryGet_ReturnsFalseForUnknownTheme()
    {
        var found = registry.TryGet("unknown", out var theme);

        Assert.False(found);
        Assert.Null(theme);
    }
}
