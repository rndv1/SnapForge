namespace SnapForge.Core.Utils;

public static class CardPadding
{
    public const int Default = 96;

    public const int Minimum = 32;

    public const int Maximum = 240;

    public static bool IsValid(int value)
    {
        return value is >= Minimum and <= Maximum;
    }

    public static int Normalize(int? value, int fallback)
    {
        if (value is null)
        {
            return fallback;
        }

        return Math.Clamp(value.Value, Minimum, Maximum);
    }

    public static string FormatRange()
    {
        return $"{Minimum}-{Maximum}px";
    }
}
