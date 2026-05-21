using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SnapForge.Core.Models;
using SnapForge.Core.Utils;

namespace SnapForge.Core.Rendering;

public sealed class BackgroundRenderer
{
    public void Draw(Image<Rgba32> canvas, CardOptions options, CardLayout layout)
    {
        var backgroundColor = options.Theme.BackgroundColor;
        var backgroundAccentColor = options.Theme.BackgroundAccentColor;

        if (options.BackgroundColor is not null && HexColor.TryNormalize(options.BackgroundColor, out var normalizedBackgroundColor))
        {
            backgroundColor = normalizedBackgroundColor;
            backgroundAccentColor = normalizedBackgroundColor;
        }

        var top = RenderColor.ToPixel(backgroundColor);
        var bottom = RenderColor.ToPixel(backgroundAccentColor);

        canvas.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var amount = accessor.Height == 1 ? 0 : y / (float)(accessor.Height - 1);
                var color = Lerp(top, bottom, amount);
                var row = accessor.GetRowSpan(y);

                for (var x = 0; x < row.Length; x++)
                {
                    row[x] = color;
                }
            }
        });
    }

    private static Rgba32 Lerp(Rgba32 from, Rgba32 to, float amount)
    {
        return new Rgba32(
            (byte)Math.Round(from.R + ((to.R - from.R) * amount)),
            (byte)Math.Round(from.G + ((to.G - from.G) * amount)),
            (byte)Math.Round(from.B + ((to.B - from.B) * amount)),
            byte.MaxValue);
    }
}
