namespace SnapForge.Core.Presets;

public sealed class PresetRegistry
{
    private readonly IReadOnlyDictionary<string, Preset> presets;

    public PresetRegistry(IEnumerable<Preset> presets)
    {
        this.presets = presets.ToDictionary(
            preset => preset.Name,
            preset => preset,
            StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGet(string? name, out Preset? preset)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            preset = null;
            return false;
        }

        return presets.TryGetValue(name.Trim(), out preset);
    }

    public string FormatSupportedNames()
    {
        return string.Join(", ", presets.Keys.Order(StringComparer.OrdinalIgnoreCase));
    }
}
