using SnapForge.Core.Presets;
using SnapForge.Core.Themes;

namespace SnapForge.Core.Models;

public sealed record CardOptions(
    string InputPath,
    string OutputPath,
    string Title,
    string Subtitle,
    Preset Preset,
    Theme Theme,
    string? BackgroundColor = null,
    int? Padding = null);
