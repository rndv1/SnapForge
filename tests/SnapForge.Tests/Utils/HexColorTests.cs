using SnapForge.Core.Utils;

namespace SnapForge.Tests.Utils;

public sealed class HexColorTests
{
    [Theory]
    [InlineData("#0f172a", "#0F172A")]
    [InlineData("0F172A", "#0F172A")]
    [InlineData("  #ffffff  ", "#FFFFFF")]
    public void TryNormalize_ReturnsNormalizedHexColor(string value, string expected)
    {
        var normalized = HexColor.TryNormalize(value, out var color);

        Assert.True(normalized);
        Assert.Equal(expected, color);
    }

    [Theory]
    [InlineData("")]
    [InlineData("blue")]
    [InlineData("#12345")]
    [InlineData("#1234567")]
    [InlineData("#12GG56")]
    public void TryNormalize_ReturnsFalseForInvalidHexColor(string value)
    {
        var normalized = HexColor.TryNormalize(value, out var color);

        Assert.False(normalized);
        Assert.Null(color);
    }
}
