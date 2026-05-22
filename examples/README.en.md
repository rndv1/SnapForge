# SnapForge Examples

**Language:** [Русский](README.md) | English

This directory contains local input screenshots and generated output cards.

```text
examples/
├── snapforge.config.json
├── snapforge.batch.json
├── input/
│   └── sample.png
└── output/
    ├── sample-github-dark.png
    ├── sample-social-light.png
    ├── sample-portfolio-dark.png
    ├── sample-open-graph-light.png
    └── sample-slide-dark.png
```

`examples/output` is ignored by Git except for `.gitkeep` and documented sample cards. Other generated cards stay local by default.

## Prepare An Input Screenshot

Place a screenshot in `examples/input`. PNG screenshots are recommended.

## GitHub README Card

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-github-dark.png \
  --title "SnapForge" \
  --subtitle "Turn plain screenshots into GitHub-ready cards" \
  --preset github \
  --theme dark
```

Output size: `1280x720`.

## More Presets

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-social-light.png \
  --title "SnapForge" \
  --subtitle "Beautiful screenshot cards from the command line" \
  --preset social \
  --theme light
```

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-portfolio-dark.png \
  --title "SnapForge CLI" \
  --subtitle "C# / .NET 8 / ImageSharp / Spectre.Console" \
  --preset portfolio \
  --theme dark
```

Use `open-graph`, `slide`, and `slide-4-3` for link previews and presentations.

## Config-Driven Card

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-config-card.png \
  --config ./examples/snapforge.config.json
```

## Batch Cards

```bash
dotnet run --project src/SnapForge.Cli -- batch ./examples/snapforge.batch.json
```

Batch mode keeps rendering after failed cards by default and exits with a non-zero status when any card fails. Use `--stop-on-error` when CI should stop at the first failed render.

## Gallery

| Input | Output |
| --- | --- |
| <img src="input/sample.png" alt="Raw sample screenshot" width="360"> | <img src="output/sample-github-dark.png" alt="Generated GitHub dark card" width="420"> |
