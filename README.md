# SnapForge

Turn plain screenshots into beautiful GitHub-ready cards.

SnapForge is a C#/.NET CLI project for generating clean, professional image cards from ordinary screenshots. It is designed for developers who want quick visuals for GitHub READMEs, portfolios, changelogs, social posts, and project presentations.

> Status: early project setup. The CLI and rendering pipeline will be implemented across focused feature branches.

## Planned Usage

```bash
snapforge card ./examples/input/api-screen.png \
  --output ./examples/output/api-card.png \
  --title "GrowthOS API" \
  --subtitle "ASP.NET Core / PostgreSQL / Docker" \
  --preset github \
  --theme dark
```

## MVP Scope

- Generate PNG cards from screenshots.
- Support `github`, `social`, and `portfolio` presets.
- Support `light` and `dark` themes.
- Render title, subtitle, screenshot frame, soft shadow, border, and attribution.
- Provide clear console output and understandable errors.

## Project Structure

```text
SnapForge/
├── src/
│   └── SnapForge.Cli/
├── tests/
│   └── SnapForge.Tests/
├── examples/
│   ├── input/
│   └── output/
├── README.md
├── LICENSE
└── SnapForge.sln
```

## Development

```bash
dotnet restore
dotnet build
dotnet test
```

## License

SnapForge is licensed under the MIT License.
