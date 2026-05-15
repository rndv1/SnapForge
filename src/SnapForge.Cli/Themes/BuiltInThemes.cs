namespace SnapForge.Cli.Themes;

public static class BuiltInThemes
{
    public static readonly Theme Light = new(
        Name: "light",
        BackgroundColor: "#F7F8FA",
        BackgroundAccentColor: "#EDEFF2",
        SurfaceColor: "#FFFFFF",
        TextColor: "#111827",
        MutedTextColor: "#4B5563",
        BorderColor: "#D0D7DE",
        ShadowColor: "#00000026",
        AttributionColor: "#6B7280");

    public static readonly Theme Dark = new(
        Name: "dark",
        BackgroundColor: "#0D1117",
        BackgroundAccentColor: "#161B22",
        SurfaceColor: "#161B22",
        TextColor: "#F0F6FC",
        MutedTextColor: "#8B949E",
        BorderColor: "#30363D",
        ShadowColor: "#00000066",
        AttributionColor: "#8B949E");

    public static IReadOnlyCollection<Theme> All { get; } =
    [
        Light,
        Dark
    ];
}
