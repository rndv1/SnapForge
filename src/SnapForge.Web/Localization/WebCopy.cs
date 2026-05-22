namespace SnapForge.Web.Localization;

public sealed class WebCopy
{
    public const string RussianCode = "ru";

    public const string EnglishCode = "en";

    private static readonly WebCopy Russian = new()
    {
        Code = RussianCode,
        HtmlLang = "ru",
        PageTitle = "Генератор карточек",
        ProductLabel = "SnapForge Web",
        MainHeading = "Создать карточку",
        GitHubLink = "GitHub",
        CardSettings = "Настройки карточки",
        ScreenshotLabel = "Скриншот",
        DropScreenshot = "Перетащите скриншот",
        NoFileSelected = "Файл не выбран",
        Browse = "Выбрать",
        TitleLabel = "Заголовок",
        SubtitleLabel = "Подзаголовок",
        PresetLabel = "Пресет",
        ThemeLabel = "Тема",
        CustomBackground = "Свой фон",
        ColorLabel = "Цвет",
        CustomPadding = "Свой отступ",
        PixelsLabel = "Пиксели",
        GenerateButton = "Сгенерировать PNG",
        PreviewLabel = "Предпросмотр",
        PreviewTitleSample = "Пример результата",
        PreviewTitleGenerated = "Готовая карточка",
        PreviewAltSample = "Пример карточки SnapForge",
        PreviewAltGenerated = "Сгенерированная карточка SnapForge",
        DownloadPng = "Скачать PNG",
        MetadataPreset = "Пресет",
        MetadataTheme = "Тема",
        MetadataBackground = "Фон",
        MetadataPadding = "Отступ",
        MetadataSize = "Размер",
        MetadataFile = "Файл",
        HistoryTitle = "История сессии",
        DefaultTitle = "SnapForge",
        DefaultSubtitle = "Скриншоты для GitHub и портфолио",
        ThemeDefault = "по теме",
        AutoPadding = "авто",
        SampleFile = "пример",
        SuccessMessage = "PNG сгенерирован.",
        ChooseScreenshotError = "Выберите файл скриншота.",
        ScreenshotTooLargeTemplate = "Скриншот должен быть не больше {0}.",
        UnsupportedExtensionError = "Скриншот должен быть PNG, JPEG, WebP, GIF или BMP.",
        TitleRequiredError = "Укажите заголовок.",
        SubtitleRequiredError = "Укажите подзаголовок.",
        UnknownPresetTemplate = "Неизвестный пресет. Поддерживаемые пресеты: {0}.",
        UnknownThemeTemplate = "Неизвестная тема. Поддерживаемые темы: {0}.",
        InvalidBackgroundTemplate = "Цвет фона должен быть в формате {0}.",
        InvalidPaddingTemplate = "Отступ должен быть в поддерживаемом диапазоне: {0}.",
        UnknownImageFormatError = "Формат изображения не поддерживается.",
        InvalidImageTemplate = "Не удалось прочитать изображение: {0}",
        TemporaryAccessTemplate = "SnapForge не может получить доступ к временному файлу: {0}",
        TemporaryIoTemplate = "SnapForge не смог прочитать или записать временный файл: {0}",
        LightThemeLabel = "light (светлая)",
        DarkThemeLabel = "dark (тёмная)",
        BytesUnit = "байт",
        KilobytesUnit = "КБ",
        ErrorTitle = "Что-то пошло не так",
        ErrorDescription = "Генератор не смог выполнить запрос.",
        BackToGenerator = "Вернуться к генератору"
    };

    private static readonly WebCopy English = new()
    {
        Code = EnglishCode,
        HtmlLang = "en",
        PageTitle = "Card Generator",
        ProductLabel = "SnapForge Web",
        MainHeading = "Generate a card",
        GitHubLink = "GitHub",
        CardSettings = "Card settings",
        ScreenshotLabel = "Screenshot",
        DropScreenshot = "Drop screenshot",
        NoFileSelected = "No file selected",
        Browse = "Browse",
        TitleLabel = "Title",
        SubtitleLabel = "Subtitle",
        PresetLabel = "Preset",
        ThemeLabel = "Theme",
        CustomBackground = "Custom background",
        ColorLabel = "Color",
        CustomPadding = "Custom padding",
        PixelsLabel = "Pixels",
        GenerateButton = "Generate PNG",
        PreviewLabel = "Preview",
        PreviewTitleSample = "Sample output",
        PreviewTitleGenerated = "Generated output",
        PreviewAltSample = "Sample SnapForge card preview",
        PreviewAltGenerated = "Generated SnapForge card preview",
        DownloadPng = "Download PNG",
        MetadataPreset = "Preset",
        MetadataTheme = "Theme",
        MetadataBackground = "Background",
        MetadataPadding = "Padding",
        MetadataSize = "Size",
        MetadataFile = "File",
        HistoryTitle = "Session history",
        DefaultTitle = "SnapForge",
        DefaultSubtitle = "GitHub-ready screenshots",
        ThemeDefault = "theme default",
        AutoPadding = "auto",
        SampleFile = "sample",
        SuccessMessage = "PNG generated.",
        ChooseScreenshotError = "Choose a screenshot file.",
        ScreenshotTooLargeTemplate = "Screenshot must be {0} or smaller.",
        UnsupportedExtensionError = "Screenshot must be PNG, JPEG, WebP, GIF, or BMP.",
        TitleRequiredError = "Title is required.",
        SubtitleRequiredError = "Subtitle is required.",
        UnknownPresetTemplate = "Unknown preset. Supported presets: {0}.",
        UnknownThemeTemplate = "Unknown theme. Supported themes: {0}.",
        InvalidBackgroundTemplate = "Background color must use {0}.",
        InvalidPaddingTemplate = "Padding must be in the supported range: {0}.",
        UnknownImageFormatError = "Input image format is not supported.",
        InvalidImageTemplate = "Input image could not be decoded: {0}",
        TemporaryAccessTemplate = "SnapForge cannot access a temporary file: {0}",
        TemporaryIoTemplate = "SnapForge could not read or write a temporary file: {0}",
        LightThemeLabel = "light",
        DarkThemeLabel = "dark",
        BytesUnit = "bytes",
        KilobytesUnit = "KB",
        ErrorTitle = "Something went wrong",
        ErrorDescription = "The generator could not complete the request.",
        BackToGenerator = "Back to generator"
    };

    public required string Code { get; init; }

    public required string HtmlLang { get; init; }

    public required string PageTitle { get; init; }

    public required string ProductLabel { get; init; }

    public required string MainHeading { get; init; }

    public required string GitHubLink { get; init; }

    public required string CardSettings { get; init; }

    public required string ScreenshotLabel { get; init; }

    public required string DropScreenshot { get; init; }

    public required string NoFileSelected { get; init; }

    public required string Browse { get; init; }

    public required string TitleLabel { get; init; }

    public required string SubtitleLabel { get; init; }

    public required string PresetLabel { get; init; }

    public required string ThemeLabel { get; init; }

    public required string CustomBackground { get; init; }

    public required string ColorLabel { get; init; }

    public required string CustomPadding { get; init; }

    public required string PixelsLabel { get; init; }

    public required string GenerateButton { get; init; }

    public required string PreviewLabel { get; init; }

    public required string PreviewTitleSample { get; init; }

    public required string PreviewTitleGenerated { get; init; }

    public required string PreviewAltSample { get; init; }

    public required string PreviewAltGenerated { get; init; }

    public required string DownloadPng { get; init; }

    public required string MetadataPreset { get; init; }

    public required string MetadataTheme { get; init; }

    public required string MetadataBackground { get; init; }

    public required string MetadataPadding { get; init; }

    public required string MetadataSize { get; init; }

    public required string MetadataFile { get; init; }

    public required string HistoryTitle { get; init; }

    public required string DefaultTitle { get; init; }

    public required string DefaultSubtitle { get; init; }

    public required string ThemeDefault { get; init; }

    public required string AutoPadding { get; init; }

    public required string SampleFile { get; init; }

    public required string SuccessMessage { get; init; }

    public required string ChooseScreenshotError { get; init; }

    public required string ScreenshotTooLargeTemplate { get; init; }

    public required string UnsupportedExtensionError { get; init; }

    public required string TitleRequiredError { get; init; }

    public required string SubtitleRequiredError { get; init; }

    public required string UnknownPresetTemplate { get; init; }

    public required string UnknownThemeTemplate { get; init; }

    public required string InvalidBackgroundTemplate { get; init; }

    public required string InvalidPaddingTemplate { get; init; }

    public required string UnknownImageFormatError { get; init; }

    public required string InvalidImageTemplate { get; init; }

    public required string TemporaryAccessTemplate { get; init; }

    public required string TemporaryIoTemplate { get; init; }

    public required string LightThemeLabel { get; init; }

    public required string DarkThemeLabel { get; init; }

    public required string BytesUnit { get; init; }

    public required string KilobytesUnit { get; init; }

    public required string ErrorTitle { get; init; }

    public required string ErrorDescription { get; init; }

    public required string BackToGenerator { get; init; }

    public static WebCopy For(string? language)
    {
        return string.Equals(language, EnglishCode, StringComparison.OrdinalIgnoreCase)
            ? English
            : Russian;
    }

    public static string NormalizeLanguage(string? language)
    {
        return string.Equals(language, EnglishCode, StringComparison.OrdinalIgnoreCase)
            ? EnglishCode
            : RussianCode;
    }

    public string ThemeOption(string name)
    {
        return name switch
        {
            "light" => LightThemeLabel,
            "dark" => DarkThemeLabel,
            _ => name
        };
    }
}
