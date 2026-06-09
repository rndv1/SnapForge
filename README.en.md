# SnapForge

**Language:** [Русский](README.md) | English

Turn plain screenshots into beautiful GitHub-ready cards.

SnapForge is a ready-to-run local Web app and CLI built with C#/.NET for turning ordinary screenshots into polished PNG cards for GitHub READMEs, portfolios, changelogs, social posts, and project presentations.

## Problem

Raw screenshots are useful, but they often look unfinished in documentation: inconsistent dimensions, no context, rough cropping, and no visual frame. Developers should not need a design tool just to make a clean project card.

## Solution

SnapForge wraps a source screenshot in a minimal themed layout and exports a ready-to-use PNG card with:

- clean background;
- title and subtitle;
- framed screenshot with rounded corners;
- soft shadow and subtle border;
- small `Generated with SnapForge` attribution.

## Quick Start

Use SnapForge without building from source:

```text
https://github.com/rndv1/SnapForge/releases
```

Download the app archive for your platform, extract it, and run `snapforge`. On Windows, opening `snapforge.exe` starts the local Web GUI and opens the browser.

```powershell
.\snapforge.exe
```

The CLI is still available separately in `snapforge-cli-*` archives and in the `.nupkg` package:

```bash
snapforge-cli card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --title "SnapForge" \
  --subtitle "GitHub-ready screenshots" \
  --preset github \
  --theme dark
```

From source:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --title "SnapForge" \
  --subtitle "GitHub-ready screenshots" \
  --preset github \
  --theme dark
```

## CLI

```bash
snapforge-cli card <input> --output <output> --title <title> --subtitle <subtitle> --preset <preset> --theme <theme> [--background <hex>] [--padding <pixels>] [--config <path>]
snapforge-cli batch <config> [--stop-on-error]
```

When SnapForge is installed as a .NET tool from `.nupkg`, the command is `snapforge`.

## Presets

| Preset | Size | Best for |
| --- | ---: | --- |
| `github` | `1280x720` | README banners and project previews |
| `open-graph` | `1200x630` | Link previews and social cards |
| `social` | `1080x1080` | Square social posts |
| `portfolio` | `1600x900` | Portfolio case studies |
| `slide` | `1920x1080` | 16:9 slides |
| `slide-4-3` | `1600x1200` | 4:3 slides |

## Themes

| Theme | Style |
| --- | --- |
| `light` | Neutral light background and dark readable text |
| `dark` | GitHub-inspired dark background and light text |

## Features

- PNG card generation from local screenshots.
- Input validation and output directory creation.
- Protection against overwriting the source image.
- Presets, themes, custom background color, and padding controls.
- JSON config files and batch mode.
- Ready-to-run local Web app launched by `snapforge.exe`.
- Russian and English Web UI language switch.
- Screenshot upload through file picker, drag-and-drop onto the settings panel, or clipboard paste.
- GitHub Actions CI and release workflow.
- Prebuilt Web App archives, separate `snapforge-cli-*` archives, and a .NET tool package.
- `SHA256SUMS.txt` for release asset verification.

## Before And After

| Input screenshot | Generated card |
| --- | --- |
| <img src="examples/input/sample.png" alt="Raw SnapForge sample screenshot" width="360"> | <img src="examples/output/sample-github-dark.png" alt="Generated SnapForge GitHub dark card" width="420"> |

More examples are available in [examples/README.en.md](examples/README.en.md).

## Documentation

- [Installation](docs/en/INSTALLATION.md)
- [User Guide](docs/en/USER_GUIDE.md)
- [Developer Guide](docs/en/DEVELOPER_GUIDE.md)
- [Reviewer Checklist](docs/en/REVIEWER_CHECKLIST.md)
- [Contributing](CONTRIBUTING.en.md)
- [Code of Conduct](CODE_OF_CONDUCT.en.md)
- [Security Policy](SECURITY.en.md)
- [Changelog](CHANGELOG.en.md)

## Project Structure

```text
SnapForge/
├── src/
│   ├── SnapForge.Core/
│   ├── SnapForge.Cli/
│   └── SnapForge.Web/
├── tests/
├── examples/
├── docs/
├── .github/workflows/
├── README.md
├── README.en.md
└── SnapForge.sln
```

## Why SnapForge?

SnapForge is intentionally small and focused. It gives developers a repeatable way to create polished project visuals without opening a design app, choosing templates, or manually resizing screenshots every time.

## Roadmap

- [x] CLI card generation
- [x] Light and dark themes
- [x] GitHub, social, portfolio, Open Graph, and slide presets
- [x] JSON config files and batch mode
- [x] Local Web GUI
- [x] Web App executable
- [x] Web UI language switch
- [x] CI and release assets
- [x] Release checksums
- [ ] NuGet publishing
- [ ] More layout templates

## Made With C# And .NET

SnapForge is built with .NET 8, C#, Spectre.Console, Spectre.Console.Cli, SixLabors.ImageSharp, SixLabors.ImageSharp.Drawing, ASP.NET Core Razor Pages, and xUnit.

## License

SnapForge is licensed under the MIT License. See [LICENSE](LICENSE).
