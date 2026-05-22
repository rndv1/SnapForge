# SnapForge User Guide

**Language:** [Русский](../USER_GUIDE.md) | English

SnapForge turns screenshots into polished PNG cards for READMEs, changelogs, social posts, portfolios, and slide decks. The reviewer-friendly path is the ready-to-run Web App launched by `snapforge.exe`.

## Install

Download a prebuilt Web App archive from GitHub Releases:

```text
https://github.com/rndv1/SnapForge/releases
```

Choose the Web App archive for your platform, extract it, and run:

```bash
./snapforge
```

On Windows, run `snapforge.exe`. The app opens the local Web UI in your browser.

## Web GUI

In the Web GUI, upload a screenshot, choose a preset, theme, background color, padding, and download the generated PNG.

The interface defaults to Russian. Use the `RU` / `EN` switch in the top-right corner to change languages.

## Generate One Card

For terminal usage, use the `snapforge-cli-*` archives or the installed .NET tool.

```bash
snapforge-cli card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --title "SnapForge" \
  --subtitle "GitHub-ready screenshots" \
  --preset github \
  --theme dark
```

Windows PowerShell:

```powershell
snapforge-cli card .\examples\input\sample.png `
  --output .\examples\output\sample-card.png `
  --title "SnapForge" `
  --subtitle "GitHub-ready screenshots" `
  --preset github `
  --theme dark
```

## Options

| Option | Description |
| --- | --- |
| `<input>` | Source screenshot path. |
| `--output` | Output PNG path. |
| `--title` | Card title. |
| `--subtitle` | Card subtitle. |
| `--preset` | `github`, `open-graph`, `social`, `portfolio`, `slide`, or `slide-4-3`. |
| `--theme` | `light` or `dark`. |
| `--background` | Optional custom background color, such as `#0F172A`. |
| `--padding` | Optional card padding from `32` through `240` pixels. |
| `--config` | Optional JSON config file. CLI options override config values. |

## JSON Config

```json
{
  "title": "SnapForge",
  "subtitle": "GitHub-ready screenshots",
  "preset": "github",
  "theme": "dark",
  "background": "#0F172A",
  "padding": 112
}
```

Run:

```bash
snapforge-cli card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --config ./examples/snapforge.config.json
```

## Batch Mode

```bash
snapforge-cli batch ./examples/snapforge.batch.json
```

Use `--stop-on-error` when a CI job should stop after the first failed card.

## Run The Web GUI From Source

```bash
dotnet run --project src/SnapForge.Web
```

Open the URL printed by ASP.NET Core if the browser does not open automatically.
