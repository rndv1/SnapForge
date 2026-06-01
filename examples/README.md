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

`examples/output` игнорируется Git, кроме `.gitkeep` и документированных примеров карточек. Остальные сгенерированные карточки остаются локальными по умолчанию.

## Подготовить входной скриншот

Положите скриншот в `examples/input`. Рекомендуемый формат — PNG.

## Карточка для GitHub README

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-github-dark.png \
  --title "SnapForge" \
  --subtitle "Скриншоты, готовые для GitHub" \
  --preset github \
  --theme dark
```

Размер результата: `1280x720`.

Windows PowerShell:

```powershell
dotnet run --project src/SnapForge.Cli -- card .\examples\input\sample.png `
  --output .\examples\output\sample-github-dark.png `
  --title "SnapForge" `
  --subtitle "Скриншоты, готовые для GitHub" `
  --preset github `
  --theme dark
```

## Другие пресеты

Карточка для соцсетей:

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-social-light.png \
  --title "SnapForge" \
  --subtitle "Красивые карточки из командной строки" \
  --preset social \
  --theme light
```

Карточка для портфолио:

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-portfolio-dark.png \
  --title "SnapForge CLI" \
  --subtitle "C# / .NET 8 / ImageSharp / Spectre.Console" \
  --preset portfolio \
  --theme dark
```

Для превью ссылок используйте `open-graph`, для презентаций — `slide` или `slide-4-3`.

## Пользовательский фон

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-custom-background.png \
  --title "SnapForge" \
  --subtitle "Настраиваемые фоны для проектов" \
  --preset github \
  --theme dark \
  --background "#0F172A"
```

## Пользовательский отступ

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-custom-padding.png \
  --title "SnapForge" \
  --subtitle "Настраиваемые отступы карточки" \
  --preset github \
  --theme dark \
  --padding 140
```

## Карточка из конфига

```bash
dotnet run --project src/SnapForge.Cli -- card ./examples/input/sample.png \
  --output ./examples/output/sample-config-card.png \
  --config ./examples/snapforge.config.json
```

## Пакетная генерация

```bash
dotnet run --project src/SnapForge.Cli -- batch ./examples/snapforge.batch.json
```

Пакетный режим продолжает обработку после ошибки отдельной карточки и возвращает ненулевой код, если хотя бы одна карточка не сгенерировалась. Используйте `--stop-on-error`, если CI должен остановиться на первой ошибке.

## Галерея

| Входное изображение | Результат |
| --- | --- |
| <img src="input/sample.png" alt="Исходный пример скриншота" width="360"> | <img src="output/sample-github-dark.png" alt="Сгенерированная тёмная GitHub-карточка" width="420"> |
