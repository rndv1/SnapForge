namespace SnapForge.Cli.Rendering;

public sealed record CardLayout(
    int Margin,
    int HeaderY,
    int SubtitleY,
    int ScreenshotX,
    int ScreenshotY,
    int ScreenshotWidth,
    int ScreenshotHeight,
    int AttributionY);
