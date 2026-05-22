using System.Text.Json.Serialization;

namespace SnapForge.Core.Configuration;

public sealed record CardConfig
{
    public static CardConfig Empty { get; } = new();

    public string? Input { get; init; }

    public string? Output { get; init; }

    public string? Title { get; init; }

    public string? Subtitle { get; init; }

    public string? Preset { get; init; }

    public string? Theme { get; init; }

    public string? Background { get; init; }

    public int? Padding { get; init; }

    [JsonIgnore]
    public bool HasValues =>
        !string.IsNullOrWhiteSpace(Input)
        || !string.IsNullOrWhiteSpace(Output)
        || !string.IsNullOrWhiteSpace(Title)
        || !string.IsNullOrWhiteSpace(Subtitle)
        || !string.IsNullOrWhiteSpace(Preset)
        || !string.IsNullOrWhiteSpace(Theme)
        || !string.IsNullOrWhiteSpace(Background)
        || Padding is not null;
}
