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

public sealed class BatchCommand : Command<BatchCommand.Settings>
{
    private static readonly PresetRegistry Presets = new(BuiltInPresets.All);

    private static readonly ThemeRegistry Themes = new(BuiltInThemes.All);

    private static readonly CardRenderer Renderer = new();

    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<config>")]
        [Description("Path to a SnapForge batch JSON config file.")]
        public string? Config { get; set; }

        [CommandOption("--stop-on-error")]
        [Description("Stop rendering after the first failed card.")]
        public bool StopOnError { get; set; }
    }

    protected override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Config))
        {
            return ValidationResult.Error("Batch config path is required.");
        }

        if (!PathHelper.TryResolveFullPath(settings.Config, out var configPath, out var pathError))
        {
            return ValidationResult.Error($"Batch config path is invalid: {pathError}");
        }

        if (!File.Exists(configPath))
        {
            return ValidationResult.Error($"Batch config file does not exist: {configPath}");
        }

        return ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(settings.Config))
        {
            WriteError("Batch config path is required.");
            return 1;
        }

        if (!PathHelper.TryResolveFullPath(settings.Config, out var configPath, out var pathError))
        {
            WriteError($"Batch config path is invalid: {pathError}");
            return 1;
        }

        var batch = LoadBatch(configPath);
        if (batch is null)
        {
            return 1;
        }

        if (batch.Cards.Length == 0)
        {
            WriteError("Batch config must contain at least one card in the cards array.");
            return 1;
        }

        AnsiConsole.MarkupLine($"[grey]Rendering {batch.Cards.Length} cards from batch config...[/]");

        var rows = new List<BatchRow>();
        for (var index = 0; index < batch.Cards.Length; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cardConfig = Merge(batch.Defaults, batch.Cards[index]);
            if (!TryCreateOptions(cardConfig, configPath, out var options, out var error))
            {
                rows.Add(BatchRow.Failed(index + 1, cardConfig.Output, error));
                if (settings.StopOnError)
                {
                    break;
                }

                continue;
            }

            try
            {
                var result = Renderer.RenderAsync(options, cancellationToken).GetAwaiter().GetResult();
                rows.Add(BatchRow.Generated(index + 1, options, result));
            }
            catch (SixLabors.ImageSharp.UnknownImageFormatException)
            {
                rows.Add(BatchRow.Failed(index + 1, options.OutputPath, "Input image format is not supported."));
            }
            catch (SixLabors.ImageSharp.InvalidImageContentException exception)
            {
                rows.Add(BatchRow.Failed(index + 1, options.OutputPath, $"Input image could not be decoded: {exception.Message}"));
            }
            catch (UnauthorizedAccessException exception)
            {
                rows.Add(BatchRow.Failed(index + 1, options.OutputPath, $"SnapForge cannot access a selected path: {exception.Message}"));
            }
            catch (IOException exception)
            {
                rows.Add(BatchRow.Failed(index + 1, options.OutputPath, $"SnapForge could not read or write a file: {exception.Message}"));
            }

            if (settings.StopOnError && rows[^1].Status == BatchStatus.Failed)
            {
                break;
            }
        }

        WriteReport(configPath, rows);
        return rows.Any(row => row.Status == BatchStatus.Failed) ? 1 : 0;
    }

    private static BatchConfig? LoadBatch(string configPath)
    {
        try
        {
            return BatchConfigLoader.Load(configPath);
        }
        catch (JsonException exception)
        {
            WriteError($"Batch config file is not valid JSON: {exception.Message}");
            return null;
        }
        catch (NotSupportedException exception)
        {
            WriteError($"Batch config contains unsupported JSON values: {exception.Message}");
            return null;
        }
        catch (UnauthorizedAccessException exception)
        {
            WriteError($"SnapForge cannot access the batch config file: {exception.Message}");
            return null;
        }
        catch (IOException exception)
        {
            WriteError($"SnapForge could not read the batch config file: {exception.Message}");
            return null;
        }
    }

    private static bool TryCreateOptions(CardConfig config, string configPath, out CardOptions options, out string error)
    {
        options = EmptyOptions();
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(config.Input))
        {
            error = "Input path is required.";
            return false;
        }

        var input = ResolvePathValue(config.Input, configPath);
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

        if (string.IsNullOrWhiteSpace(config.Output))
        {
            error = "Output path is required.";
            return false;
        }

        var output = ResolvePathValue(config.Output, configPath);
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
            error = "Output path must be different from the input file.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(config.Title))
        {
            error = "Title is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(config.Subtitle))
        {
            error = "Subtitle is required.";
            return false;
        }

        if (!Presets.TryGet(config.Preset, out var preset))
        {
            error = $"Unknown preset '{config.Preset}'. Supported presets: {Presets.FormatSupportedNames()}.";
            return false;
        }

        if (!Themes.TryGet(config.Theme, out var theme))
        {
            error = $"Unknown theme '{config.Theme}'. Supported themes: {Themes.FormatSupportedNames()}.";
            return false;
        }

        string? backgroundColor = null;
        if (!string.IsNullOrWhiteSpace(config.Background)
            && !HexColor.TryNormalize(config.Background, out backgroundColor))
        {
            error = $"Background color must use {HexColor.ExpectedFormat}, for example #0D1117.";
            return false;
        }

        if (config.Padding is not null && !CardPadding.IsValid(config.Padding.Value))
        {
            error = $"Padding must be in the supported range: {CardPadding.FormatRange()}.";
            return false;
        }

        options = new CardOptions(
            InputPath: inputPath,
            OutputPath: outputPath,
            Title: config.Title.Trim(),
            Subtitle: config.Subtitle.Trim(),
            Preset: preset!,
            Theme: theme!,
            BackgroundColor: backgroundColor,
            Padding: config.Padding);

        return true;
    }

    private static CardConfig Merge(CardConfig defaults, CardConfig card)
    {
        return new CardConfig
        {
            Input = Choose(card.Input, defaults.Input),
            Output = Choose(card.Output, defaults.Output),
            Title = Choose(card.Title, defaults.Title),
            Subtitle = Choose(card.Subtitle, defaults.Subtitle),
            Preset = Choose(card.Preset, defaults.Preset),
            Theme = Choose(card.Theme, defaults.Theme),
            Background = Choose(card.Background, defaults.Background),
            Padding = card.Padding ?? defaults.Padding
        };
    }

    private static string? Choose(string? value, string? fallback)
    {
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    private static string ResolvePathValue(string value, string configPath)
    {
        if (Path.IsPathRooted(value))
        {
            return value;
        }

        var configDirectory = Path.GetDirectoryName(configPath);
        return string.IsNullOrWhiteSpace(configDirectory)
            ? value
            : Path.Combine(configDirectory, value);
    }

    private static void WriteReport(string configPath, IReadOnlyCollection<BatchRow> rows)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[grey]#[/]"))
            .AddColumn(new TableColumn("[grey]Output[/]"))
            .AddColumn(new TableColumn("[grey]Preset[/]"))
            .AddColumn(new TableColumn("[grey]Theme[/]"))
            .AddColumn(new TableColumn("[grey]Size[/]"))
            .AddColumn(new TableColumn("[grey]Status[/]"));

        foreach (var row in rows)
        {
            table.AddRow(
                row.Index.ToString(),
                Markup.Escape(row.Output),
                Markup.Escape(row.Preset),
                Markup.Escape(row.Theme),
                Markup.Escape(row.Size),
                row.Status == BatchStatus.Generated
                    ? "[green]Generated[/]"
                    : $"[red]Failed[/]\n[grey]{Markup.Escape(row.Message)}[/]");
        }

        var generated = rows.Count(row => row.Status == BatchStatus.Generated);
        var failed = rows.Count(row => row.Status == BatchStatus.Failed);

        AnsiConsole.Write(new Rule("[bold]SnapForge batch[/]").RuleStyle("grey"));
        AnsiConsole.MarkupLine($"[grey]Config: {Markup.Escape(configPath)}[/]");
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine(failed == 0
            ? $"[green]Generated {generated} card(s).[/]"
            : $"[yellow]Generated {generated} card(s), failed {failed} card(s).[/]");
    }

    private static void WriteError(string message)
    {
        AnsiConsole.MarkupLine("[red]SnapForge could not run the batch.[/]");
        AnsiConsole.MarkupLine($"[grey]{Markup.Escape(message)}[/]");
    }

    private static CardOptions EmptyOptions()
    {
        return new CardOptions(
            InputPath: string.Empty,
            OutputPath: string.Empty,
            Title: string.Empty,
            Subtitle: string.Empty,
            Preset: BuiltInPresets.GitHub,
            Theme: BuiltInThemes.Dark);
    }

    private sealed record BatchRow(
        int Index,
        string Output,
        string Preset,
        string Theme,
        string Size,
        BatchStatus Status,
        string Message)
    {
        public static BatchRow Generated(int index, CardOptions options, RenderResult result)
        {
            return new BatchRow(
                index,
                options.OutputPath,
                options.Preset.Name,
                options.Theme.Name,
                $"{result.Width}x{result.Height}",
                BatchStatus.Generated,
                string.Empty);
        }

        public static BatchRow Failed(int index, string? output, string message)
        {
            return new BatchRow(
                index,
                string.IsNullOrWhiteSpace(output) ? "(not set)" : output,
                "-",
                "-",
                "-",
                BatchStatus.Failed,
                message);
        }
    }

    private enum BatchStatus
    {
        Generated,
        Failed
    }
}
