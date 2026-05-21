using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SnapForge.Core.Models;
using SnapForge.Core.Utils;

namespace SnapForge.Core.Rendering;

public sealed class CardRenderer
{
    private readonly BackgroundRenderer backgroundRenderer = new();
    private readonly TextRenderer textRenderer = new();
    private readonly ScreenshotRenderer screenshotRenderer = new();

    public async Task<RenderResult> RenderAsync(CardOptions options, CancellationToken cancellationToken = default)
    {
        var width = options.Preset.Width;
        var height = options.Preset.Height;
        var layout = CreateLayout(width, height, options.Padding);

        using var canvas = new Image<Rgba32>(width, height);

        backgroundRenderer.Draw(canvas, options, layout);
        textRenderer.Draw(canvas, options, layout);
        await screenshotRenderer.DrawAsync(canvas, options, layout, cancellationToken);

        var outputDirectory = Path.GetDirectoryName(options.OutputPath);
        if (!string.IsNullOrWhiteSpace(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        await canvas.SaveAsPngAsync(options.OutputPath, cancellationToken);

        var outputFile = new FileInfo(options.OutputPath);
        return new RenderResult(options.OutputPath, width, height, outputFile.Length);
    }

    private static CardLayout CreateLayout(int width, int height, int? padding)
    {
        var defaultMargin = Math.Max(48, (int)Math.Round(width * 0.075));
        var margin = Math.Min(CardPadding.Normalize(padding, defaultMargin), Math.Max(32, width / 4));
        var headerY = Math.Max(44, (int)Math.Round(height * 0.075));
        var subtitleY = headerY + Math.Clamp((int)Math.Round(height * 0.075), 46, 68);
        var screenshotY = Math.Clamp((int)Math.Round(height * 0.265), 170, 300);
        var attributionY = height - Math.Max(32, (int)Math.Round(height * 0.052));
        var screenshotBottom = attributionY - Math.Max(38, (int)Math.Round(height * 0.06));

        return new CardLayout(
            Margin: margin,
            HeaderY: headerY,
            SubtitleY: subtitleY,
            ScreenshotX: margin,
            ScreenshotY: screenshotY,
            ScreenshotWidth: width - (margin * 2),
            ScreenshotHeight: Math.Max(180, screenshotBottom - screenshotY),
            AttributionY: attributionY);
    }
}
