using System.ComponentModel;
using System.Text.Json;
using SnapForge.Core.Configuration;
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
        [CommandArgument(0, "[input]")]
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
        [Description("Card preset: github, social, portfolio, open-graph, slide, or slide-4-3.")]
        public string? Preset { get; set; }

        [CommandOption("--theme <theme>")]
        [Description("Card theme: light or dark.")]
        public string? Theme { get; set; }

        [CommandOption("--background <hex>")]
        [Description("Optional card background color as #RRGGBB.")]
        public string? BackgroundColor { get; set; }

        [CommandOption("--padding <pixels>")]
        [Description("Optional card padding in pixels.")]
        public int? Padding { get; set; }

        [CommandOption("--config <path>")]
        [Description("Optional JSON config file with reusable card settings.")]
        public string? Config { get; set; }
    }

    protected override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (!TryResolveSettings(settings, out _, out var error))
        {
            return ValidationResult.Error(error);
        }

        return ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!TryResolveSettings(settings, out var resolvedSettings, out var error))
        {
            WriteError(error);
            return 1;
        }

        var options = CreateOptions(resolvedSettings);

        try
        {
            AnsiConsole.MarkupLine("[grey]Rendering card...[/]");
            var result = Renderer.RenderAsync(options, cancellationToken).GetAwaiter().GetResult();
            WriteReport(options, result, resolvedSettings.ConfigPath);

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

    private static bool TryResolveSettings(Settings settings, out ResolvedCardSettings resolvedSettings, out string error)
    {
        resolvedSettings = ResolvedCardSettings.Empty;
        error = string.Empty;

        if (!TryLoadConfig(settings.Config, out var config, out var configPath, out error))
        {
            return false;
        }

        var input = ResolvePathValue(settings.Input, config.Input, configPath);
        if (string.IsNullOrWhiteSpace(input))
        {
            error = "Input path is required. Pass <input> or set input in --config.";
            return false;
        }

        if (!PathHelper.TryResolveFullPath(input, out var inputPath, out var inputPathError))
        {
            error = $"Input path is invalid: {inputPathError}";
            return false;
        }

        if (!File.Exists(inputPath))
        {
            error = $"Input file does not exist: {inputPath}";
            return false;
        }

        var output = ResolvePathValue(settings.Output, config.Output, configPath);
        if (string.IsNullOrWhiteSpace(output))
        {
            error = "Output path is required. Use --output <path> or set output in --config.";
            return false;
        }

        if (!PathHelper.TryResolveFullPath(output, out var outputPath, out var outputPathError))
        {
            error = $"Output path is invalid: {outputPathError}";
            return false;
        }

        if (Directory.Exists(outputPath))
        {
            error = "Output path must include a PNG file name, not just a directory.";
            return false;
        }

        if (!ImageFormatResolver.IsPngPath(outputPath))
        {
            error = "Output file must use the .png extension.";
            return false;
        }

        if (PathHelper.PathsEqual(inputPath, outputPath))
        {
            error = "Output path must be different from the input file. SnapForge never overwrites the source screenshot.";
            return false;
        }

        var title = ResolveValue(settings.Title, config.Title);
        if (string.IsNullOrWhiteSpace(title))
        {
            error = "Title is required. Use --title <title> or set title in --config.";
            return false;
        }

        var subtitle = ResolveValue(settings.Subtitle, config.Subtitle);
        if (string.IsNullOrWhiteSpace(subtitle))
        {
            error = "Subtitle is required. Use --subtitle <subtitle> or set subtitle in --config.";
            return false;
        }

        var presetName = ResolveValue(settings.Preset, config.Preset);
        if (string.IsNullOrWhiteSpace(presetName))
        {
            error = $"Preset is required. Supported presets: {Presets.FormatSupportedNames()}.";
            return false;
        }

        if (!Presets.TryGet(presetName, out var preset))
        {
            error = $"Unknown preset '{presetName}'. Supported presets: {Presets.FormatSupportedNames()}.";
            return false;
        }

        var themeName = ResolveValue(settings.Theme, config.Theme);
        if (string.IsNullOrWhiteSpace(themeName))
        {
            error = "Theme is required. Use --theme light or --theme dark, or set theme in --config.";
            return false;
        }

        if (!Themes.TryGet(themeName, out var theme))
        {
            error = $"Unknown theme '{themeName}'. Supported themes: {Themes.FormatSupportedNames()}.";
            return false;
        }

        var backgroundColorValue = ResolveValue(settings.BackgroundColor, config.Background);
        string? backgroundColor = null;
        if (!string.IsNullOrWhiteSpace(backgroundColorValue)
            && !HexColor.TryNormalize(backgroundColorValue, out backgroundColor))
        {
            error = $"Background color must use {HexColor.ExpectedFormat}, for example #0D1117.";
            return false;
        }

        var padding = settings.Padding ?? config.Padding;
        if (padding is not null && !CardPadding.IsValid(padding.Value))
        {
            error = $"Padding must be in the supported range: {CardPadding.FormatRange()}.";
            return false;
        }

        resolvedSettings = new ResolvedCardSettings(
            InputPath: inputPath,
            OutputPath: outputPath,
            Title: title.Trim(),
            Subtitle: subtitle.Trim(),
            Preset: preset!,
            Theme: theme!,
            BackgroundColor: backgroundColor,
            Padding: padding,
            ConfigPath: configPath);

        return true;
    }

    private static CardOptions CreateOptions(ResolvedCardSettings settings)
    {
        return new CardOptions(
            InputPath: settings.InputPath,
            OutputPath: settings.OutputPath,
            Title: settings.Title,
            Subtitle: settings.Subtitle,
            Preset: settings.Preset,
            Theme: settings.Theme,
            BackgroundColor: settings.BackgroundColor,
            Padding: settings.Padding);
    }

    private static bool TryLoadConfig(
        string? path,
        out CardConfig config,
        out string? configPath,
        out string error)
    {
        config = CardConfig.Empty;
        configPath = null;
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(path))
        {
            return true;
        }

        if (!PathHelper.TryResolveFullPath(path, out var resolvedPath, out var pathError))
        {
            error = $"Config path is invalid: {pathError}";
            return false;
        }

        if (!File.Exists(resolvedPath))
        {
            error = $"Config file does not exist: {resolvedPath}";
            return false;
        }

        try
        {
            config = CardConfigLoader.Load(resolvedPath);
            configPath = resolvedPath;
            return true;
        }
        catch (JsonException exception)
        {
            error = $"Config file is not valid JSON: {exception.Message}";
            return false;
        }
        catch (NotSupportedException exception)
        {
            error = $"Config file contains unsupported JSON values: {exception.Message}";
            return false;
        }
        catch (UnauthorizedAccessException exception)
        {
            error = $"SnapForge cannot access the config file: {exception.Message}";
            return false;
        }
        catch (IOException exception)
        {
            error = $"SnapForge could not read the config file: {exception.Message}";
            return false;
        }
    }

    private static string? ResolveValue(string? commandValue, string? configValue)
    {
        return string.IsNullOrWhiteSpace(commandValue)
            ? configValue
            : commandValue;
    }

    private static string? ResolvePathValue(string? commandValue, string? configValue, string? configPath)
    {
        if (!string.IsNullOrWhiteSpace(commandValue))
        {
            return commandValue;
        }

        if (string.IsNullOrWhiteSpace(configValue))
        {
            return null;
        }

        if (Path.IsPathRooted(configValue) || string.IsNullOrWhiteSpace(configPath))
        {
            return configValue;
        }

        var configDirectory = Path.GetDirectoryName(configPath);
        return string.IsNullOrWhiteSpace(configDirectory)
            ? configValue
            : Path.Combine(configDirectory, configValue);
    }

    private static void WriteReport(CardOptions options, RenderResult result, string? configPath)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[grey]Field[/]"))
            .AddColumn(new TableColumn("[grey]Value[/]"));

        table.AddRow("Input path", Markup.Escape(options.InputPath));
        table.AddRow("Output path", Markup.Escape(options.OutputPath));
        if (configPath is not null)
        {
            table.AddRow("Config path", Markup.Escape(configPath));
        }

        table.AddRow("Selected preset", options.Preset.Name);
        table.AddRow("Selected theme", options.Theme.Name);
        if (options.BackgroundColor is not null)
        {
            table.AddRow("Background color", options.BackgroundColor);
        }

        if (options.Padding is not null)
        {
            table.AddRow("Card padding", $"{options.Padding}px");
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

    private sealed record ResolvedCardSettings(
        string InputPath,
        string OutputPath,
        string Title,
        string Subtitle,
        Preset Preset,
        Theme Theme,
        string? BackgroundColor,
        int? Padding,
        string? ConfigPath)
    {
        public static ResolvedCardSettings Empty { get; } = new(
            InputPath: string.Empty,
            OutputPath: string.Empty,
            Title: string.Empty,
            Subtitle: string.Empty,
            Preset: BuiltInPresets.GitHub,
            Theme: BuiltInThemes.Dark,
            BackgroundColor: null,
            Padding: null,
            ConfigPath: null);
    }
}
