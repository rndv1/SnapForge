using SnapForge.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SnapForge.Core.Rendering;

public sealed class ScreenshotRenderer
{
    private const int BorderWidth = 2;
    private const int ShadowPadding = 42;

    public async Task DrawAsync(
        Image<Rgba32> canvas,
        CardOptions options,
        CardLayout layout,
        CancellationToken cancellationToken)
    {
        using var screenshot = await Image.LoadAsync<Rgba32>(options.InputPath, cancellationToken);

        var maxScreenshotWidth = Math.Max(1, layout.ScreenshotWidth - (BorderWidth * 2));
        var maxScreenshotHeight = Math.Max(1, layout.ScreenshotHeight - (BorderWidth * 2));
        var scale = Math.Min(
            maxScreenshotWidth / (float)screenshot.Width,
            maxScreenshotHeight / (float)screenshot.Height);

        var targetWidth = Math.Max(1, (int)Math.Round(screenshot.Width * scale));
        var targetHeight = Math.Max(1, (int)Math.Round(screenshot.Height * scale));

        screenshot.Mutate(context => context.Resize(targetWidth, targetHeight));
        ApplyRoundedCorners(screenshot, GetCornerRadius(targetWidth, targetHeight));

        var frameWidth = targetWidth + (BorderWidth * 2);
        var frameHeight = targetHeight + (BorderWidth * 2);
        var frameRadius = GetCornerRadius(frameWidth, frameHeight);
        using var framedScreenshot = new Image<Rgba32>(frameWidth, frameHeight);

        DrawingPrimitives.FillRoundedRectangle(
            framedScreenshot,
            0,
            0,
            frameWidth,
            frameHeight,
            frameRadius,
            RenderColor.ToPixel(options.Theme.BorderColor));

        framedScreenshot.Mutate(context => context.DrawImage(screenshot, new Point(BorderWidth, BorderWidth), 1f));

        using var shadow = new Image<Rgba32>(
            frameWidth + (ShadowPadding * 2),
            frameHeight + (ShadowPadding * 2));

        DrawingPrimitives.FillRoundedRectangle(
            shadow,
            ShadowPadding,
            ShadowPadding,
            frameWidth,
            frameHeight,
            frameRadius,
            RenderColor.ToPixel(options.Theme.ShadowColor));

        shadow.Mutate(context => context.GaussianBlur(22f));

        var frameX = layout.ScreenshotX + ((layout.ScreenshotWidth - frameWidth) / 2);
        var frameY = layout.ScreenshotY + ((layout.ScreenshotHeight - frameHeight) / 2);
        var shadowX = frameX - ShadowPadding;
        var shadowY = frameY - ShadowPadding + Math.Max(8, options.Preset.Height / 90);

        canvas.Mutate(context =>
        {
            context.DrawImage(shadow, new Point(shadowX, shadowY), 1f);
            context.DrawImage(framedScreenshot, new Point(frameX, frameY), 1f);
        });
    }

    private static int GetCornerRadius(int width, int height)
    {
        return Math.Clamp(Math.Min(width, height) / 24, 16, 30);
    }

    private static void ApplyRoundedCorners(Image<Rgba32> image, int radius)
    {
        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (var x = 0; x < row.Length; x++)
                {
                    if (!DrawingPrimitives.ContainsRoundedRectanglePoint(
                            x,
                            y,
                            0,
                            0,
                            accessor.Width,
                            accessor.Height,
                            radius))
                    {
                        row[x].A = 0;
                    }
                }
            }
        });
    }
}
