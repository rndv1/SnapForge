using System.Diagnostics.CodeAnalysis;

namespace SnapForge.Core.Utils;

public static class HexColor
{
    public const string ExpectedFormat = "#RRGGBB";

    public static bool TryNormalize(string? value, [NotNullWhen(true)] out string? normalized)
    {
        normalized = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        var hex = trimmed.StartsWith('#')
            ? trimmed[1..]
            : trimmed;

        if (hex.Length != 6)
        {
            return false;
        }

        foreach (var character in hex)
        {
            if (!Uri.IsHexDigit(character))
            {
                return false;
            }
        }

        normalized = $"#{hex.ToUpperInvariant()}";
        return true;
    }
}
