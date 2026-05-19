using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SnapForge.Core.Models;
using SnapForge.Core.Presets;
using SnapForge.Core.Rendering;
using SnapForge.Core.Themes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnapForge.Web.Pages;

public sealed class IndexModel : PageModel
{
    private const int MaxHistoryItems = 5;

    private const long MaxUploadBytes = 15 * 1024 * 1024;

    private const string HistorySessionKey = "SnapForge.Web.History";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private static readonly HashSet<string> SupportedInputExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png",
        ".jpg",
        ".jpeg",
        ".webp",
        ".gif",
        ".bmp"
    };

    private static readonly PresetRegistry Presets = new(BuiltInPresets.All);

    private static readonly ThemeRegistry Themes = new(BuiltInThemes.All);

    private readonly CardRenderer renderer = new();

    [BindProperty]
    public CardFormInput Input { get; set; } = CardFormInput.Default();

    public GeneratedCard? Result { get; private set; }

    public IReadOnlyList<GeneratedCard> History { get; private set; } = [];

    public string? ErrorMessage { get; private set; }

    public string? SuccessMessage { get; private set; }

    public IReadOnlyList<SelectListItem> PresetOptions =>
        BuiltInPresets.All
            .Select(preset => new SelectListItem(
                text: $"{preset.Name} ({preset.Width}x{preset.Height})",
                value: preset.Name))
            .ToArray();

    public IReadOnlyList<SelectListItem> ThemeOptions =>
        BuiltInThemes.All
            .Select(theme => new SelectListItem(
                text: theme.Name,
                value: theme.Name))
            .ToArray();

    public string SelectedPresetSize
    {
        get
        {
            if (Presets.TryGet(Input.Preset, out var preset) && preset is not null)
            {
                return $"{preset.Width}x{preset.Height}";
            }

            return "1280x720";
        }
    }

    public string PreviewTitle => Result is null ? "Sample output" : "Generated output";

    public string PreviewImageSource => ResultDataUrl ?? Url.Content("~/images/sample-card.png");

    public string PreviewImageAlt => Result is null
        ? "Sample SnapForge card preview"
        : "Generated SnapForge card preview";

    public string? ResultDataUrl => Result is null
        ? null
        : Result.DataUrl;

    public string DisplayPreset => Result?.PresetName ?? Input.Preset;

    public string DisplayTheme => Result?.ThemeName ?? Input.Theme;

    public string DisplaySize => Result is null
        ? SelectedPresetSize
        : $"{Result.Width}x{Result.Height}";

    public string DisplayFileSize => Result is null
        ? "sample"
        : FormatBytes(Result.FileSizeBytes);

    public void OnGet()
    {
        Input = CardFormInput.Default();
        History = ReadHistory();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        Input.Normalize();
        History = ReadHistory();

        var validationError = ValidateInput(out var preset, out var theme);
        if (validationError is not null)
        {
            ErrorMessage = validationError;
            return Page();
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "snapforge-web", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        var inputExtension = Path.GetExtension(Input.Screenshot!.FileName);
        var inputPath = Path.Combine(tempDirectory, $"input{inputExtension}");
        var outputPath = Path.Combine(tempDirectory, "card.png");

        try
        {
            await using (var upload = System.IO.File.Create(inputPath))
            {
                await Input.Screenshot.CopyToAsync(upload, cancellationToken);
            }

            var options = new CardOptions(
                InputPath: inputPath,
                OutputPath: outputPath,
                Title: Input.Title,
                Subtitle: Input.Subtitle,
                Preset: preset!,
                Theme: theme!);

            var renderResult = await renderer.RenderAsync(options, cancellationToken);
            var pngBytes = await System.IO.File.ReadAllBytesAsync(outputPath, cancellationToken);

            Result = new GeneratedCard(
                Base64Png: Convert.ToBase64String(pngBytes),
                Title: Input.Title,
                Width: renderResult.Width,
                Height: renderResult.Height,
                FileSizeBytes: renderResult.FileSizeBytes,
                PresetName: preset!.Name,
                ThemeName: theme!.Name,
                CreatedAtUtc: DateTimeOffset.UtcNow);

            History = StoreHistory(Result);
            SuccessMessage = "PNG generated.";
        }
        catch (UnknownImageFormatException)
        {
            ErrorMessage = "Input image format is not supported.";
        }
        catch (InvalidImageContentException exception)
        {
            ErrorMessage = $"Input image could not be decoded: {exception.Message}";
        }
        catch (UnauthorizedAccessException exception)
        {
            ErrorMessage = $"SnapForge cannot access a temporary file: {exception.Message}";
        }
        catch (IOException exception)
        {
            ErrorMessage = $"SnapForge could not read or write a temporary file: {exception.Message}";
        }
        finally
        {
            TryDeleteDirectory(tempDirectory);
        }

        return Page();
    }

    private IReadOnlyList<GeneratedCard> ReadHistory()
    {
        var json = HttpContext.Session.GetString(HistorySessionKey);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<IReadOnlyList<GeneratedCard>>(json, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private IReadOnlyList<GeneratedCard> StoreHistory(GeneratedCard card)
    {
        var history = ReadHistory();
        var updatedHistory = history
            .Prepend(card)
            .Take(MaxHistoryItems)
            .ToArray();

        HttpContext.Session.SetString(
            HistorySessionKey,
            JsonSerializer.Serialize(updatedHistory, JsonOptions));

        return updatedHistory;
    }

    private string? ValidateInput(out Preset? preset, out Theme? theme)
    {
        preset = null;
        theme = null;

        if (Input.Screenshot is null || Input.Screenshot.Length == 0)
        {
            return "Choose a screenshot file.";
        }

        if (Input.Screenshot.Length > MaxUploadBytes)
        {
            return $"Screenshot must be {FormatBytes(MaxUploadBytes)} or smaller.";
        }

        var extension = Path.GetExtension(Input.Screenshot.FileName);
        if (!SupportedInputExtensions.Contains(extension))
        {
            return "Screenshot must be PNG, JPEG, WebP, GIF, or BMP.";
        }

        if (string.IsNullOrWhiteSpace(Input.Title))
        {
            return "Title is required.";
        }

        if (string.IsNullOrWhiteSpace(Input.Subtitle))
        {
            return "Subtitle is required.";
        }

        if (!Presets.TryGet(Input.Preset, out preset))
        {
            return $"Unknown preset. Supported presets: {Presets.FormatSupportedNames()}.";
        }

        if (!Themes.TryGet(Input.Theme, out theme))
        {
            return $"Unknown theme. Supported themes: {Themes.FormatSupportedNames()}.";
        }

        return null;
    }

    private static string FormatBytes(long bytes)
    {
        return bytes < 1024
            ? $"{bytes:N0} bytes"
            : $"{bytes / 1024d:N1} KB";
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}

public sealed class CardFormInput
{
    public IFormFile? Screenshot { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string Preset { get; set; } = BuiltInPresets.GitHub.Name;

    public string Theme { get; set; } = BuiltInThemes.Dark.Name;

    public static CardFormInput Default()
    {
        return new CardFormInput
        {
            Title = "SnapForge",
            Subtitle = "GitHub-ready screenshots",
            Preset = BuiltInPresets.GitHub.Name,
            Theme = BuiltInThemes.Dark.Name
        };
    }

    public void Normalize()
    {
        Title = Title.Trim();
        Subtitle = Subtitle.Trim();
        Preset = Preset.Trim();
        Theme = Theme.Trim();
    }
}

public sealed record GeneratedCard(
    string Base64Png,
    string Title,
    int Width,
    int Height,
    long FileSizeBytes,
    string PresetName,
    string ThemeName,
    DateTimeOffset CreatedAtUtc)
{
    [JsonIgnore]
    public string DataUrl => $"data:image/png;base64,{Base64Png}";

    [JsonIgnore]
    public string DownloadFileName => $"snapforge-card-{CreatedAtUtc:yyyyMMdd-HHmmss}.png";

    [JsonIgnore]
    public string SizeLabel => $"{Width}x{Height}";

    [JsonIgnore]
    public string CreatedAtLabel => CreatedAtUtc.ToLocalTime().ToString("HH:mm");
}
