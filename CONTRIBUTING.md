# Contributing To SnapForge

Thanks for considering a contribution to SnapForge.

SnapForge is intentionally small: a focused .NET CLI with clear rendering behavior, a compact set of presets, and a minimal public surface. Contributions should keep that shape intact.

## Local Setup

Requirements:

- .NET 8 SDK
- Git

Run the standard checks before opening a pull request:

```bash
dotnet restore
dotnet build
dotnet test
```

For the same command shape used by CI:

```bash
dotnet build --no-restore --configuration Release
dotnet test --no-build --configuration Release --verbosity normal
```

## Branches

Use short feature branches:

```bash
git checkout -b feature/my-change
```

Prefer focused branches that can be reviewed in one sitting.

## Pull Requests

Good SnapForge pull requests usually include:

- a short summary of what changed;
- screenshots or generated output when rendering changes visually affect cards;
- the commands used for verification;
- tests when the change affects registry behavior, options, validation, or rendering contracts.

Keep commits readable. A small series of clear commits is better than one large mixed commit.

## Code Style

- Keep responsibilities separated across commands, models, registries, rendering, themes, presets, and utils.
- Prefer simple code over framework-heavy architecture.
- Do not add external images, logos, or protected assets.
- Keep user-facing errors understandable and actionable.
- Preserve the CLI-first experience: command output should be useful without being noisy.

## Rendering Changes

When changing rendering behavior, generate at least one sample card locally:

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --title "SnapForge" \
  --subtitle "GitHub-ready screenshots" \
  --preset github \
  --theme dark
```

Review the output image before opening the PR.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
