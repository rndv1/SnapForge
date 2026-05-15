using SnapForge.Cli.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SnapForge.Cli.Rendering;

public sealed class ScreenshotRenderer
{
    public Task DrawAsync(
        Image<Rgba32> canvas,
        CardOptions options,
        CardLayout layout,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
