using System.ComponentModel;
using SnapForge.Cli.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SnapForge.Cli.Commands;

public sealed class CardCommand : Command<CardCommand.Settings>
{
    private static readonly HashSet<string> SupportedPresets = new(StringComparer.OrdinalIgnoreCase)
    {
        "github",
        "social",
        "portfolio"
    };

    private static readonly HashSet<string> SupportedThemes = new(StringComparer.OrdinalIgnoreCase)
    {
        "light",
        "dark"
    };

    private static readonly Dictionary<string, (int Width, int Height)> PresetSizes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["github"] = (1280, 720),
        ["social"] = (1080, 1080),
        ["portfolio"] = (1600, 900)
    };

    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<input>")]
        [Description("Path to the source screenshot.")]
        public string? Input { get; set; }

        [CommandOption("-o|--output <output>")]
        [Description("Path where the generated PNG card will be written.")]
        public string? Output { get; set; }

        [CommandOption("--title <title>")]
        [Description("Title rendered on the card.")]
        public string? Title { get; set; }

        [CommandOption("--subtitle <subtitle>")]
        [Description("Subtitle rendered below the title.")]
        public string? Subtitle { get; set; }

        [CommandOption("--preset <preset>")]
        [Description("Card preset: github, social, or portfolio.")]
        public string? Preset { get; set; }

        [CommandOption("--theme <theme>")]
        [Description("Card theme: light or dark.")]
        public string? Theme { get; set; }
    }

    protected override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Input))
        {
            return ValidationResult.Error("Input path is required.");
        }

        if (!PathHelper.TryResolveFullPath(settings.Input, out var inputPath, out var inputPathError))
        {
            return ValidationResult.Error($"Input path is invalid: {inputPathError}");
        }

        if (!File.Exists(inputPath))
        {
            return ValidationResult.Error($"Input file does not exist: {inputPath}");
        }

        if (string.IsNullOrWhiteSpace(settings.Output))
        {
            return ValidationResult.Error("Output path is required. Use --output <path>.");
        }

        if (!PathHelper.TryResolveFullPath(settings.Output, out var outputPath, out var outputPathError))
        {
            return ValidationResult.Error($"Output path is invalid: {outputPathError}");
        }

        if (Directory.Exists(outputPath))
        {
            return ValidationResult.Error("Output path must include a PNG file name, not just a directory.");
        }

        if (!string.Equals(Path.GetExtension(outputPath), ".png", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Error("Output file must use the .png extension.");
        }

        if (PathHelper.PathsEqual(inputPath, outputPath))
        {
            return ValidationResult.Error("Output path must be different from the input file. SnapForge never overwrites the source screenshot.");
        }

        if (string.IsNullOrWhiteSpace(settings.Title))
        {
            return ValidationResult.Error("Title is required. Use --title <title>.");
        }

        if (string.IsNullOrWhiteSpace(settings.Subtitle))
        {
            return ValidationResult.Error("Subtitle is required. Use --subtitle <subtitle>.");
        }

        if (string.IsNullOrWhiteSpace(settings.Preset))
        {
            return ValidationResult.Error("Preset is required. Use --preset github, --preset social, or --preset portfolio.");
        }

        if (!SupportedPresets.Contains(settings.Preset))
        {
            return ValidationResult.Error($"Unknown preset '{settings.Preset}'. Supported presets: github, social, portfolio.");
        }

        if (string.IsNullOrWhiteSpace(settings.Theme))
        {
            return ValidationResult.Error("Theme is required. Use --theme light or --theme dark.");
        }

        if (!SupportedThemes.Contains(settings.Theme))
        {
            return ValidationResult.Error($"Unknown theme '{settings.Theme}'. Supported themes: light, dark.");
        }

        return ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var inputPath = Path.GetFullPath(settings.Input!);
        var outputPath = Path.GetFullPath(settings.Output!);
        var outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrWhiteSpace(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        var preset = settings.Preset!.Trim().ToLowerInvariant();
        var theme = settings.Theme!.Trim().ToLowerInvariant();
        var size = PresetSizes[preset];

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[grey]Field[/]"))
            .AddColumn(new TableColumn("[grey]Value[/]"));

        table.AddRow("Input path", Markup.Escape(inputPath));
        table.AddRow("Output path", Markup.Escape(outputPath));
        table.AddRow("Selected preset", preset);
        table.AddRow("Selected theme", theme);
        table.AddRow("Final image size", $"{size.Width}x{size.Height}");
        table.AddRow("Status", "[yellow]Renderer pending[/]");

        AnsiConsole.Write(new Rule("[bold]SnapForge card[/]").RuleStyle("grey"));
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[grey]The ImageSharp rendering pipeline will be added in PR #4.[/]");

        return 0;
    }
}
