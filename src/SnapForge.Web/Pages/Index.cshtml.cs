using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SnapForge.Core.Models;
using SnapForge.Core.Presets;
using SnapForge.Core.Rendering;
using SnapForge.Core.Themes;
using SnapForge.Core.Utils;
using SnapForge.Web.Localization;

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

    [BindProperty(SupportsGet = true)]
    public string? Lang { get; set; }

    public GeneratedCard? Result { get; private set; }

    public IReadOnlyList<GeneratedCard> History { get; private set; } = [];

    public string? ErrorMessage { get; private set; }

    public string? SuccessMessage { get; private set; }

    public WebCopy Text { get; private set; } = WebCopy.For(WebCopy.RussianCode);

    public string CurrentLanguage { get; private set; } = WebCopy.RussianCode;

    public IReadOnlyList<SelectListItem> PresetOptions =>
        BuiltInPresets.All
            .Select(preset => new SelectListItem(
                text: $"{preset.Name} ({preset.Width}x{preset.Height})",
                value: preset.Name))
            .ToArray();

    public IReadOnlyList<SelectListItem> ThemeOptions =>
        BuiltInThemes.All
            .Select(theme => new SelectListItem(
                text: Text.ThemeOption(theme.Name),
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

    public string PreviewTitle => Result is null ? Text.PreviewTitleSample : Text.PreviewTitleGenerated;

    public string PreviewImageSource => ResultDataUrl ?? Url.Content("~/images/sample-card.png");

    public string PreviewImageAlt => Result is null
        ? Text.PreviewAltSample
        : Text.PreviewAltGenerated;

    public string? ResultDataUrl => Result is null
        ? null
        : Result.DataUrl;

    public string DisplayPreset => Result?.PresetName ?? Input.Preset;

    public string DisplayTheme => Result?.ThemeName ?? Input.Theme;

    public string DisplayBackground => Result is null
        ? (Input.UseCustomBackground ? Input.BackgroundColor : Text.ThemeDefault)
        : FormatBackground(Result);

    public string DisplayPadding => Result is null
        ? (Input.UseCustomPadding ? $"{Input.Padding}px" : Text.AutoPadding)
        : FormatPadding(Result);

    public string DisplaySize => Result is null
        ? SelectedPresetSize
        : $"{Result.Width}x{Result.Height}";

    public string DisplayFileSize => Result is null
        ? Text.SampleFile
        : FormatBytes(Result.FileSizeBytes);

    public void OnGet()
    {
        SetLanguage(Lang);
        Input = CardFormInput.Default(CurrentLanguage);
        History = ReadHistory();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        Input.Normalize();
        SetLanguage(Input.Language);
        Input.Language = CurrentLanguage;
        History = ReadHistory();

        var validationError = ValidateInput(out var preset, out var theme, out var backgroundColor, out var padding);
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
                Theme: theme!,
                BackgroundColor: backgroundColor,
                Padding: padding);

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
                CreatedAtUtc: DateTimeOffset.UtcNow,
                BackgroundColor: backgroundColor,
                Padding: padding);

            History = StoreHistory(Result);
            SuccessMessage = Text.SuccessMessage;
        }
        catch (UnknownImageFormatException)
        {
            ErrorMessage = Text.UnknownImageFormatError;
        }
        catch (InvalidImageContentException exception)
        {
            ErrorMessage = string.Format(Text.InvalidImageTemplate, exception.Message);
        }
        catch (UnauthorizedAccessException exception)
        {
            ErrorMessage = string.Format(Text.TemporaryAccessTemplate, exception.Message);
        }
        catch (IOException exception)
        {
            ErrorMessage = string.Format(Text.TemporaryIoTemplate, exception.Message);
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

    private string? ValidateInput(out Preset? preset, out Theme? theme, out string? backgroundColor, out int? padding)
    {
        preset = null;
        theme = null;
        backgroundColor = null;
        padding = null;

        if (Input.Screenshot is null || Input.Screenshot.Length == 0)
        {
            return Text.ChooseScreenshotError;
        }

        if (Input.Screenshot.Length > MaxUploadBytes)
        {
            return string.Format(Text.ScreenshotTooLargeTemplate, FormatBytes(MaxUploadBytes));
        }

        var extension = Path.GetExtension(Input.Screenshot.FileName);
        if (!SupportedInputExtensions.Contains(extension))
        {
            return Text.UnsupportedExtensionError;
        }

        if (string.IsNullOrWhiteSpace(Input.Title))
        {
            return Text.TitleRequiredError;
        }

        if (string.IsNullOrWhiteSpace(Input.Subtitle))
        {
            return Text.SubtitleRequiredError;
        }

        if (!Presets.TryGet(Input.Preset, out preset))
        {
            return string.Format(Text.UnknownPresetTemplate, Presets.FormatSupportedNames());
        }

        if (!Themes.TryGet(Input.Theme, out theme))
        {
            return string.Format(Text.UnknownThemeTemplate, Themes.FormatSupportedNames());
        }

        if (Input.UseCustomBackground && !HexColor.TryNormalize(Input.BackgroundColor, out backgroundColor))
        {
            return string.Format(Text.InvalidBackgroundTemplate, HexColor.ExpectedFormat);
        }

        if (Input.UseCustomPadding)
        {
            if (!CardPadding.IsValid(Input.Padding))
            {
                return string.Format(Text.InvalidPaddingTemplate, CardPadding.FormatRange());
            }

            padding = Input.Padding;
        }

        return null;
    }

    private void SetLanguage(string? language)
    {
        CurrentLanguage = WebCopy.NormalizeLanguage(language);
        Text = WebCopy.For(CurrentLanguage);
    }

    public string FormatBackground(GeneratedCard card)
    {
        return card.BackgroundColor ?? Text.ThemeDefault;
    }

    public string FormatPadding(GeneratedCard card)
    {
        return card.Padding is null ? Text.AutoPadding : $"{card.Padding}px";
    }

    private string FormatBytes(long bytes)
    {
        return bytes < 1024
            ? $"{bytes:N0} {Text.BytesUnit}"
            : $"{bytes / 1024d:N1} {Text.KilobytesUnit}";
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

    public string Language { get; set; } = WebCopy.RussianCode;

    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string Preset { get; set; } = BuiltInPresets.GitHub.Name;

    public string Theme { get; set; } = BuiltInThemes.Dark.Name;

    public bool UseCustomBackground { get; set; }

    public string BackgroundColor { get; set; } = BuiltInThemes.Dark.BackgroundColor;

    public bool UseCustomPadding { get; set; }

    public int Padding { get; set; } = CardPadding.Default;

    public static CardFormInput Default(string language = WebCopy.RussianCode)
    {
        var text = WebCopy.For(language);

        return new CardFormInput
        {
            Language = text.Code,
            Title = text.DefaultTitle,
            Subtitle = text.DefaultSubtitle,
            Preset = BuiltInPresets.GitHub.Name,
            Theme = BuiltInThemes.Dark.Name,
            BackgroundColor = BuiltInThemes.Dark.BackgroundColor,
            Padding = CardPadding.Default
        };
    }

    public void Normalize()
    {
        Language = WebCopy.NormalizeLanguage(Language);
        Title = Title.Trim();
        Subtitle = Subtitle.Trim();
        Preset = Preset.Trim();
        Theme = Theme.Trim();
        BackgroundColor = BackgroundColor.Trim();
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
    DateTimeOffset CreatedAtUtc,
    string? BackgroundColor = null,
    int? Padding = null)
{
    [JsonIgnore]
    public string DataUrl => $"data:image/png;base64,{Base64Png}";

    [JsonIgnore]
    public string DownloadFileName => $"snapforge-card-{CreatedAtUtc:yyyyMMdd-HHmmss}.png";

    [JsonIgnore]
    public string SizeLabel => $"{Width}x{Height}";

    [JsonIgnore]
    public string CreatedAtLabel => CreatedAtUtc.ToLocalTime().ToString("HH:mm");

    [JsonIgnore]
    public string BackgroundLabel => BackgroundColor ?? "theme default";

    [JsonIgnore]
    public string PaddingLabel => Padding is null ? "auto" : $"{Padding}px";
}
