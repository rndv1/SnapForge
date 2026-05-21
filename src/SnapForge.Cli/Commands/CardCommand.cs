using System.ComponentModel;
using SnapForge.Core.Models;
using SnapForge.Core.Presets;
using SnapForge.Core.Rendering;
using SnapForge.Core.Themes;
using SnapForge.Core.Utils;
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
        [Description("Card preset: github, social, portfolio, or open-graph.")]
        public string? Preset { get; set; }

        [CommandOption("--theme <theme>")]
        [Description("Card theme: light or dark.")]
        public string? Theme { get; set; }

        [CommandOption("--background <hex>")]
        [Description("Optional card background color as #RRGGBB.")]
        public string? BackgroundColor { get; set; }
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
            return ValidationResult.Error("Preset is required. Use --preset github, --preset social, --preset portfolio, or --preset open-graph.");
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

        if (!string.IsNullOrWhiteSpace(settings.BackgroundColor)
            && !HexColor.TryNormalize(settings.BackgroundColor, out _))
        {
            return ValidationResult.Error($"Background color must use {HexColor.ExpectedFormat}, for example #0D1117.");
        }

        return ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var options = CreateOptions(settings);

        try
        {
            AnsiConsole.MarkupLine("[grey]Rendering card...[/]");
            var result = Renderer.RenderAsync(options, cancellationToken).GetAwaiter().GetResult();
            WriteReport(options, result);

            return 0;
        }
        catch (SixLabors.ImageSharp.UnknownImageFormatException)
        {
            WriteError("Input image format is not supported. Try a PNG, JPEG, GIF, BMP, or WebP screenshot.");
            return 1;
        }
        catch (SixLabors.ImageSharp.InvalidImageContentException exception)
        {
            WriteError($"Input image could not be decoded: {exception.Message}");
            return 1;
        }
        catch (UnauthorizedAccessException exception)
        {
            WriteError($"SnapForge cannot access one of the selected paths: {exception.Message}");
            return 1;
        }
        catch (IOException exception)
        {
            WriteError($"SnapForge could not read or write a file: {exception.Message}");
            return 1;
        }
    }

    private static CardOptions CreateOptions(Settings settings)
    {
        Presets.TryGet(settings.Preset, out var preset);
        Themes.TryGet(settings.Theme, out var theme);
        HexColor.TryNormalize(settings.BackgroundColor, out var backgroundColor);

        return new CardOptions(
            InputPath: Path.GetFullPath(settings.Input!),
            OutputPath: Path.GetFullPath(settings.Output!),
            Title: settings.Title!.Trim(),
            Subtitle: settings.Subtitle!.Trim(),
            Preset: preset!,
            Theme: theme!,
            BackgroundColor: backgroundColor);
    }

    private static void WriteReport(CardOptions options, RenderResult result)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[grey]Field[/]"))
            .AddColumn(new TableColumn("[grey]Value[/]"));

        table.AddRow("Input path", Markup.Escape(options.InputPath));
        table.AddRow("Output path", Markup.Escape(options.OutputPath));
        table.AddRow("Selected preset", options.Preset.Name);
        table.AddRow("Selected theme", options.Theme.Name);
        if (options.BackgroundColor is not null)
        {
            table.AddRow("Background color", options.BackgroundColor);
        }

        table.AddRow("Final image size", $"{result.Width}x{result.Height}");
        table.AddRow("Status", "[green]Generated[/]");

        AnsiConsole.Write(new Rule("[bold]SnapForge card[/]").RuleStyle("grey"));
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"[grey]PNG written: {Markup.Escape(result.OutputPath)} ({FormatBytes(result.FileSizeBytes)})[/]");
    }

    private static void WriteError(string message)
    {
        AnsiConsole.MarkupLine("[red]SnapForge could not generate the card.[/]");
        AnsiConsole.MarkupLine($"[grey]{Markup.Escape(message)}[/]");
    }

    private static string FormatBytes(long bytes)
    {
        return bytes < 1024
            ? $"{bytes:N0} bytes"
            : $"{bytes / 1024d:N1} KB";
    }
}
