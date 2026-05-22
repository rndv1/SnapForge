# SnapForge Developer Guide

**Language:** [Русский](../DEVELOPER_GUIDE.md) | English

This guide explains how the repository is organized and how to work on SnapForge safely.

## Requirements

- .NET 8 SDK
- Git

## Repository Layout

```text
src/
├── SnapForge.Core/   Shared rendering, config, presets, themes, and models.
├── SnapForge.Cli/    Spectre.Console CLI commands and tool packaging.
└── SnapForge.Web/    Local Razor Pages Web App published as snapforge.exe.

tests/
└── SnapForge.Tests/  xUnit coverage for registries, config loading, and rendering contracts.
```

## Local Checks

Run these before opening a pull request:

```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build --verbosity normal
dotnet format --verify-no-changes --no-restore
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --configuration Release --no-build --output artifacts/packages
```

## Architecture Notes

SnapForge intentionally avoids heavyweight architecture. The main boundaries are:

- `SnapForge.Core` owns rendering, models, presets, themes, config parsing, and validation helpers.
- `SnapForge.Cli` owns command parsing, console output, command orchestration, and tool packaging.
- `SnapForge.Web` owns browser workflows, UI localization, browser launch behavior, and reuses `SnapForge.Core` for rendering.

Keep new features close to these boundaries.

## Rendering Changes

When changing visual output:

1. Add or update focused tests when behavior changes.
2. Generate at least one local card from `examples/input/sample.png`.
3. Review the PNG before opening a PR.
4. Update example images only when they are intentionally part of documentation.

## Release Flow

Releases are produced by `.github/workflows/release.yml`.

```bash
git checkout main
git pull --ff-only origin main
git tag -a v0.1.0 -m "Release v0.1.0"
git push origin v0.1.0
```

The release workflow publishes Web App archives `snapforge-*`, separate CLI archives `snapforge-cli-*`, a `.nupkg` package, and `SHA256SUMS.txt`.
