# Contributing To SnapForge

**Language:** [Русский](CONTRIBUTING.md) | English

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

```bash
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --configuration Release --output artifacts/packages
dotnet tool install --global SnapForge --add-source ./artifacts/packages
snapforge --help
```

If you already have the tool installed:

```bash
dotnet tool uninstall --global SnapForge
```

## Branches And Pull Requests

Use short feature branches:

```bash
git checkout -b feature/my-change
```

Good SnapForge pull requests include a short summary, verification commands, screenshots or generated output when visual rendering changes, and focused tests when behavior changes.

## Code Style

- Keep reusable rendering, models, presets, themes, and utilities in `SnapForge.Core`.
- Keep `SnapForge.Cli` focused on commands, validation, console output, and packaging.
- Keep `SnapForge.Web` focused on browser workflows that reuse `SnapForge.Core`.
- Prefer simple code over framework-heavy architecture.
- Do not add external images, logos, or protected assets.
- Keep user-facing errors understandable and actionable.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
