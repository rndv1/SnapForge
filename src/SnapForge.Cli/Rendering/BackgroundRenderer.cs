using SnapForge.Cli.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SnapForge.Cli.Rendering;

public sealed class BackgroundRenderer
{
    public void Draw(Image<Rgba32> canvas, CardOptions options, CardLayout layout)
    {
        canvas.Mutate(context => context.BackgroundColor(RenderColor.ToColor(options.Theme.BackgroundColor)));
    }
}
