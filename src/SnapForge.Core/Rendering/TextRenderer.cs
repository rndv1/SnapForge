using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SnapForge.Core.Models;

namespace SnapForge.Core.Rendering;

public sealed class TextRenderer
{
    private static readonly Lazy<FontFamily> FontFamily = new(ResolveFontFamily);

    public void Draw(Image<Rgba32> canvas, CardOptions options, CardLayout layout)
    {
        var titleSize = Math.Clamp(options.Preset.Width / 26f, 38f, 58f);
        var subtitleSize = Math.Clamp(options.Preset.Width / 54f, 19f, 28f);
        var attributionSize = Math.Clamp(options.Preset.Width / 78f, 15f, 20f);

        var titleFont = FontFamily.Value.CreateFont(titleSize, FontStyle.Bold);
        var subtitleFont = FontFamily.Value.CreateFont(subtitleSize, FontStyle.Regular);
        var attributionFont = FontFamily.Value.CreateFont(attributionSize, FontStyle.Regular);

        var maxTextWidth = options.Preset.Width - (layout.Margin * 2);
        var title = FitText(options.Title, maxTextWidth, titleSize);
        var subtitle = FitText(options.Subtitle, maxTextWidth, subtitleSize);

        canvas.Mutate(context =>
        {
            context.DrawText(
                title,
                titleFont,
                RenderColor.ToColor(options.Theme.TextColor),
                new PointF(layout.Margin, layout.HeaderY));

            context.DrawText(
                subtitle,
                subtitleFont,
                RenderColor.ToColor(options.Theme.MutedTextColor),
                new PointF(layout.Margin, layout.SubtitleY));

            context.DrawText(
                "Generated with SnapForge",
                attributionFont,
                RenderColor.ToColor(options.Theme.AttributionColor),
                new PointF(layout.Margin, layout.AttributionY));
        });
    }

    private static string FitText(string text, int maxWidth, float fontSize)
    {
        var maxCharacters = Math.Max(12, (int)Math.Floor(maxWidth / (fontSize * 0.54f)));
        if (text.Length <= maxCharacters)
        {
            return text;
        }

        return string.Concat(text.AsSpan(0, Math.Max(0, maxCharacters - 3)), "...");
    }

    private static FontFamily ResolveFontFamily()
    {
        var preferredFonts = new[]
        {
            "Segoe UI",
            "Inter",
            "Arial",
            "DejaVu Sans",
            "Liberation Sans",
            "Noto Sans"
        };

        foreach (var fontName in preferredFonts)
        {
            if (SystemFonts.TryGet(fontName, out var fontFamily))
            {
                return fontFamily;
            }
        }

        return SystemFonts.Collection.Families.First();
    }
}
