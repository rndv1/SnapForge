namespace SnapForge.Core.Configuration;

public sealed record BatchConfig
{
    public CardConfig Defaults { get; init; } = CardConfig.Empty;

    public CardConfig[] Cards { get; init; } = [];
}
