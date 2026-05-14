namespace SnapForge.Cli.Themes;

public sealed record Theme(
    string Name,
    string BackgroundColor,
    string BackgroundAccentColor,
    string SurfaceColor,
    string TextColor,
    string MutedTextColor,
    string BorderColor,
    string ShadowColor,
    string AttributionColor);
