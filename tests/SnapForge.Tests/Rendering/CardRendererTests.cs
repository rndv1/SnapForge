using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SnapForge.Core.Models;
using SnapForge.Core.Presets;
using SnapForge.Core.Rendering;
using SnapForge.Core.Themes;

namespace SnapForge.Tests.Rendering;

public sealed class CardRendererTests
{
    private readonly PresetRegistry presets = new(BuiltInPresets.All);

    [Theory]
    [InlineData("github", 1280, 720)]
    [InlineData("social", 1080, 1080)]
    [InlineData("portfolio", 1600, 900)]
    [InlineData("open-graph", 1200, 630)]
    public async Task RenderAsync_WritesPngWithPresetDimensions(
        string presetName,
        int expectedWidth,
        int expectedHeight)
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "snapforge-tests", Guid.NewGuid().ToString("N"));

        try
        {
            var inputPath = Path.Combine(tempDirectory, "input.png");
            var outputPath = Path.Combine(tempDirectory, "output", "card.png");
            Directory.CreateDirectory(tempDirectory);
            await CreateInputScreenshotAsync(inputPath);

            var found = presets.TryGet(presetName, out var preset);
            Assert.True(found);

            var options = new CardOptions(
                InputPath: inputPath,
                OutputPath: outputPath,
                Title: "SnapForge",
                Subtitle: "GitHub-ready screenshots",
                Preset: preset!,
                Theme: BuiltInThemes.Dark);

            var result = await new CardRenderer().RenderAsync(options);

            Assert.True(File.Exists(outputPath));
            Assert.Equal(expectedWidth, result.Width);
            Assert.Equal(expectedHeight, result.Height);
            Assert.True(result.FileSizeBytes > 0);

            using var outputImage = await Image.LoadAsync<Rgba32>(outputPath);
            Assert.Equal(expectedWidth, outputImage.Width);
            Assert.Equal(expectedHeight, outputImage.Height);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }
    }

    private static async Task CreateInputScreenshotAsync(string path)
    {
        using var image = new Image<Rgba32>(900, 520);

        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (var x = 0; x < row.Length; x++)
                {
                    row[x] = x % 120 < 60
                        ? new Rgba32(245, 247, 250)
                        : new Rgba32(229, 233, 240);
                }
            }
        });

        await image.SaveAsPngAsync(path);
    }
}
