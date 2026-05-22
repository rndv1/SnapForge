# Примеры SnapForge

**Язык:** Русский | [English](README.en.md)

Эта папка содержит локальные входные скриншоты и сгенерированные карточки.

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

`examples/output` игнорируется Git, кроме `.gitkeep` и документированных sample cards. Остальные generated cards остаются локальными по умолчанию.

## Подготовить входной скриншот

Положите скриншот в `examples/input`. Рекомендуемый формат — PNG.

## Карточка для GitHub README

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-github-dark.png \
  --title "SnapForge" \
  --subtitle "Turn plain screenshots into GitHub-ready cards" \
  --preset github \
  --theme dark
```

Размер результата: `1280x720`.

Windows PowerShell:

```powershell
dotnet run --project src/SnapForge.Cli -- card .\examples\input\sample.png `
  --output .\examples\output\sample-github-dark.png `
  --title "SnapForge" `
  --subtitle "Turn plain screenshots into GitHub-ready cards" `
  --preset github `
  --theme dark
```

## Другие пресеты

Social card:

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-social-light.png \
  --title "SnapForge" \
  --subtitle "Beautiful screenshot cards from the command line" \
  --preset social \
  --theme light
```

Portfolio card:

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-portfolio-dark.png \
  --title "SnapForge CLI" \
  --subtitle "C# / .NET 8 / ImageSharp / Spectre.Console" \
  --preset portfolio \
  --theme dark
```

Для link previews используйте `open-graph`, для презентаций — `slide` или `slide-4-3`.

## Пользовательский фон

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-custom-background.png \
  --title "SnapForge" \
  --subtitle "Custom project-ready backgrounds" \
  --preset github \
  --theme dark \
  --background "#0F172A"
```

## Пользовательский padding

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-custom-padding.png \
  --title "SnapForge" \
  --subtitle "Adjustable card spacing" \
  --preset github \
  --theme dark \
  --padding 140
```

## Config-driven card

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-config-card.png \
  --config ./examples/snapforge.config.json
```

## Batch cards

```bash
dotnet run --project src/SnapForge.Cli -- batch ./examples/snapforge.batch.json
```

Batch mode продолжает обработку после ошибки отдельной карточки и возвращает ненулевой код, если хотя бы одна карточка не сгенерировалась. Используйте `--stop-on-error`, если CI должен остановиться на первой ошибке.

## Галерея

| Input | Output |
| --- | --- |
| <img src="input/sample.png" alt="Raw sample screenshot" width="360"> | <img src="output/sample-github-dark.png" alt="Generated GitHub dark card" width="420"> |
