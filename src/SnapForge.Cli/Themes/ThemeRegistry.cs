namespace SnapForge.Cli.Themes;

public sealed class ThemeRegistry
{
    private readonly IReadOnlyDictionary<string, Theme> themes;

    public ThemeRegistry(IEnumerable<Theme> themes)
    {
        this.themes = themes.ToDictionary(
            theme => theme.Name,
            theme => theme,
            StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGet(string? name, out Theme? theme)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            theme = null;
            return false;
        }

        return themes.TryGetValue(name.Trim(), out theme);
    }

    public string FormatSupportedNames()
    {
        return string.Join(", ", themes.Keys.Order(StringComparer.OrdinalIgnoreCase));
    }
}
