namespace SnapForge.Cli.Models;

public sealed record RenderResult(
    string OutputPath,
    int Width,
    int Height,
    long FileSizeBytes);
