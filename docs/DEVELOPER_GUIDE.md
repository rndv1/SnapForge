# SnapForge Developer Guide

This guide explains how the repository is organized and how to work on SnapForge safely.

## Requirements

- .NET 8 SDK
- Git

## Repository Layout

```text
src/
├── SnapForge.Core/   Shared rendering, config, presets, themes, and models.
├── SnapForge.Cli/    Spectre.Console CLI commands and tool packaging.
└── SnapForge.Web/    Local Razor Pages Web GUI.

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

- `SnapForge.Core` owns reusable behavior: rendering, models, presets, themes, config parsing, validation helpers.
- `SnapForge.Cli` owns command parsing, console output, command-level orchestration, and tool packaging.
- `SnapForge.Web` owns browser workflows and reuses `SnapForge.Core` for rendering.

Keep new features close to these boundaries. Avoid moving rendering logic into CLI or Web-specific code.

## Rendering Changes

When changing visual output:

1. Add or update focused tests when behavior changes.
2. Generate at least one local card from `examples/input/sample.png`.
3. Review the PNG before opening a PR.
4. Update example images only when they are intentionally part of documentation.

## Release Flow

Releases are produced by `.github/workflows/release.yml`.

To publish a release:

```bash
git checkout main
git pull --ff-only origin main
git tag v0.1.0
git push origin v0.1.0
```

The release workflow publishes platform-specific CLI archives and a `.nupkg` package to the GitHub Release.
