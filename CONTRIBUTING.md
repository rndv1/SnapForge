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
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --no-build --configuration Release --output artifacts/packages
```

## Local Tool Packaging

To test SnapForge as an installed CLI tool:

```bash
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --configuration Release --output artifacts/packages
dotnet tool install --global SnapForge --add-source ./artifacts/packages
snapforge --help
```

If you already have the tool installed, uninstall it before reinstalling from a fresh package:

```bash
dotnet tool uninstall --global SnapForge
```

## CI Package Artifacts

The `Build` workflow packs the CLI tool on every pull request and push to `main`.
The generated `.nupkg` is uploaded as the `snapforge-cli-package` workflow artifact.

Use this artifact to verify tool installation from a branch without publishing anything to NuGet. Official package publishing can be added later as a separate release workflow.

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

- Keep reusable rendering, models, presets, themes, and utilities in `SnapForge.Core`.
- Keep `SnapForge.Cli` focused on command parsing, validation, console output, and tool packaging.
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
