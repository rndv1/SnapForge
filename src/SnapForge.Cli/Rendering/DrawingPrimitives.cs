using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SnapForge.Cli.Rendering;

internal static class DrawingPrimitives
{
    public static void FillRoundedRectangle(
        Image<Rgba32> image,
        int x,
        int y,
        int width,
        int height,
        int radius,
        Rgba32 color)
    {
        image.ProcessPixelRows(accessor =>
        {
            var startY = Math.Max(0, y);
            var endY = Math.Min(accessor.Height, y + height);
            var startX = Math.Max(0, x);
            var endX = Math.Min(accessor.Width, x + width);

            for (var py = startY; py < endY; py++)
            {
                var row = accessor.GetRowSpan(py);

                for (var px = startX; px < endX; px++)
                {
                    if (ContainsRoundedRectanglePoint(px, py, x, y, width, height, radius))
                    {
                        row[px] = Blend(row[px], color);
                    }
                }
            }
        });
    }

    public static bool ContainsRoundedRectanglePoint(
        int pointX,
        int pointY,
        int x,
        int y,
        int width,
        int height,
        int radius)
    {
        var left = x + radius;
        var right = x + width - radius - 1;
        var top = y + radius;
        var bottom = y + height - radius - 1;

        if ((pointX >= left && pointX <= right) || (pointY >= top && pointY <= bottom))
        {
            return true;
        }

        var centerX = pointX < left ? left : right;
        var centerY = pointY < top ? top : bottom;
        var deltaX = pointX - centerX;
        var deltaY = pointY - centerY;

        return (deltaX * deltaX) + (deltaY * deltaY) <= radius * radius;
    }

    private static Rgba32 Blend(Rgba32 destination, Rgba32 source)
    {
        if (source.A == byte.MaxValue)
        {
            return source;
        }

        if (source.A == 0)
        {
            return destination;
        }

        var sourceAlpha = source.A / 255f;
        var destinationAlpha = destination.A / 255f;
        var alpha = sourceAlpha + (destinationAlpha * (1f - sourceAlpha));

        if (alpha <= 0f)
        {
            return default;
        }

        return new Rgba32(
            (byte)Math.Round(((source.R * sourceAlpha) + (destination.R * destinationAlpha * (1f - sourceAlpha))) / alpha),
            (byte)Math.Round(((source.G * sourceAlpha) + (destination.G * destinationAlpha * (1f - sourceAlpha))) / alpha),
            (byte)Math.Round(((source.B * sourceAlpha) + (destination.B * destinationAlpha * (1f - sourceAlpha))) / alpha),
            (byte)Math.Round(alpha * 255f));
    }
}
