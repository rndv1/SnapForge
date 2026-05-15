using System.ComponentModel;
using SnapForge.Cli.Models;
using SnapForge.Cli.Presets;
using SnapForge.Cli.Rendering;
using SnapForge.Cli.Themes;
using SnapForge.Cli.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SnapForge.Cli.Commands;

public sealed class CardCommand : Command<CardCommand.Settings>
{
    private static readonly PresetRegistry Presets = new(BuiltInPresets.All);

    private static readonly ThemeRegistry Themes = new(BuiltInThemes.All);

    private static readonly CardRenderer Renderer = new();

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

        if (!ImageFormatResolver.IsPngPath(outputPath))
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

        if (!Presets.TryGet(settings.Preset, out _))
        {
            return ValidationResult.Error($"Unknown preset '{settings.Preset}'. Supported presets: {Presets.FormatSupportedNames()}.");
        }

        if (string.IsNullOrWhiteSpace(settings.Theme))
        {
            return ValidationResult.Error("Theme is required. Use --theme light or --theme dark.");
        }

        if (!Themes.TryGet(settings.Theme, out _))
        {
            return ValidationResult.Error($"Unknown theme '{settings.Theme}'. Supported themes: {Themes.FormatSupportedNames()}.");
        }

        return ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var inputPath = Path.GetFullPath(settings.Input!);
        var outputPath = Path.GetFullPath(settings.Output!);

        Presets.TryGet(settings.Preset, out var preset);
        Themes.TryGet(settings.Theme, out var theme);

        var options = new CardOptions(
            InputPath: inputPath,
            OutputPath: outputPath,
            Title: settings.Title!.Trim(),
            Subtitle: settings.Subtitle!.Trim(),
            Preset: preset!,
            Theme: theme!);

        AnsiConsole.MarkupLine("[grey]Rendering card...[/]");
        var result = Renderer.RenderAsync(options, cancellationToken).GetAwaiter().GetResult();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[grey]Field[/]"))
            .AddColumn(new TableColumn("[grey]Value[/]"));

        table.AddRow("Input path", Markup.Escape(inputPath));
        table.AddRow("Output path", Markup.Escape(outputPath));
        table.AddRow("Selected preset", preset!.Name);
        table.AddRow("Selected theme", theme!.Name);
        table.AddRow("Final image size", $"{result.Width}x{result.Height}");
        table.AddRow("Status", "[green]Generated[/]");

        AnsiConsole.Write(new Rule("[bold]SnapForge card[/]").RuleStyle("grey"));
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"[grey]PNG written: {Markup.Escape(result.OutputPath)} ({result.FileSizeBytes:N0} bytes)[/]");

        return 0;
    }
}
