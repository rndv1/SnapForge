using SnapForge.Cli.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SnapForge.Cli.Rendering;

public sealed class BackgroundRenderer
{
    public void Draw(Image<Rgba32> canvas, CardOptions options, CardLayout layout)
    {
        var top = RenderColor.ToPixel(options.Theme.BackgroundColor);
        var bottom = RenderColor.ToPixel(options.Theme.BackgroundAccentColor);

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
