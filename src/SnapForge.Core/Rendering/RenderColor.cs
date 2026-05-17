using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SnapForge.Core.Rendering;

internal static class RenderColor
{
    public static Color ToColor(string hex)
    {
        var pixel = ToPixel(hex);
        return Color.FromRgba(pixel.R, pixel.G, pixel.B, pixel.A);
    }

    public static Rgba32 ToPixel(string hex)
    {
        var value = hex.Trim().TrimStart('#');
        if (value.Length is not 6 and not 8)
        {
            throw new FormatException($"Invalid color value '{hex}'. Expected #RRGGBB or #RRGGBBAA.");
        }

        var red = Convert.ToByte(value[..2], 16);
        var green = Convert.ToByte(value[2..4], 16);
        var blue = Convert.ToByte(value[4..6], 16);
        var alpha = value.Length == 8 ? Convert.ToByte(value[6..8], 16) : byte.MaxValue;

        return new Rgba32(red, green, blue, alpha);
    }
}
