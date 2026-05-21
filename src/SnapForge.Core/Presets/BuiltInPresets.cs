namespace SnapForge.Core.Presets;

public static class BuiltInPresets
{
    public static readonly Preset GitHub = new("github", 1280, 720);

    public static readonly Preset Social = new("social", 1080, 1080);

    public static readonly Preset Portfolio = new("portfolio", 1600, 900);

    public static readonly Preset OpenGraph = new("open-graph", 1200, 630);

    public static IReadOnlyCollection<Preset> All { get; } =
    [
        GitHub,
        Social,
        Portfolio,
        OpenGraph
    ];
}
