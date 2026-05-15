using SnapForge.Cli.Presets;
using SnapForge.Cli.Themes;

namespace SnapForge.Cli.Models;

public sealed record CardOptions(
    string InputPath,
    string OutputPath,
    string Title,
    string Subtitle,
    Preset Preset,
    Theme Theme);
