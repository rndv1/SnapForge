using SnapForge.Core.Utils;

namespace SnapForge.Tests.Utils;

public sealed class CardPaddingTests
{
    [Theory]
    [InlineData(32)]
    [InlineData(96)]
    [InlineData(240)]
    public void IsValid_ReturnsTrueForSupportedPadding(int value)
    {
        Assert.True(CardPadding.IsValid(value));
    }

    [Theory]
    [InlineData(31)]
    [InlineData(241)]
    public void IsValid_ReturnsFalseForUnsupportedPadding(int value)
    {
        Assert.False(CardPadding.IsValid(value));
    }

    [Fact]
    public void Normalize_ReturnsFallbackWhenPaddingIsNotProvided()
    {
        var padding = CardPadding.Normalize(null, fallback: 112);

        Assert.Equal(112, padding);
    }

    [Theory]
    [InlineData(-10, 32)]
    [InlineData(300, 240)]
    public void Normalize_ClampsPaddingToSupportedRange(int value, int expected)
    {
        var padding = CardPadding.Normalize(value, fallback: 96);

        Assert.Equal(expected, padding);
    }
}
