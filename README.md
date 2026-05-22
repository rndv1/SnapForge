# SnapForge

**Язык:** Русский | [English](README.en.md)

Turn plain screenshots into beautiful GitHub-ready cards.

SnapForge — это CLI-утилита и небольшой локальный Web GUI на C#/.NET для генерации аккуратных PNG-карточек из обычных скриншотов. Проект помогает быстро готовить изображения для GitHub README, портфолио, changelog, социальных сетей и презентаций.

## Проблема

Скриншоты полезны, но в документации они часто выглядят сыро: разные размеры, нет контекста, нет рамки, тени и визуального порядка. Открывать дизайнерский инструмент ради одной README-картинки — лишний шаг.

## Решение

SnapForge берёт исходный скриншот, помещает его в минималистичный тематический макет и экспортирует готовую PNG-карточку:

- чистый фон;
- заголовок и подзаголовок;
- скриншот внутри рамки;
- скруглённые углы;
- мягкая тень;
- тонкая граница;
- подпись `Generated with SnapForge`.

## Быстрый старт

SnapForge можно проверить без сборки из исходников:

```text
https://github.com/rndv1/SnapForge/releases
```

Скачайте архив для своей платформы, распакуйте и запустите:

```bash
snapforge --help
```

Windows:

```powershell
.\snapforge.exe --help
```

Сгенерировать карточку:

```bash
snapforge card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --title "SnapForge" \
  --subtitle "GitHub-ready screenshots" \
  --preset github \
  --theme dark
```

Запуск из исходников:

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
snapforge card <input> --output <output> --title <title> --subtitle <subtitle> --preset <preset> --theme <theme> [--background <hex>] [--padding <pixels>] [--config <path>]
snapforge batch <config> [--stop-on-error]
```

`--background` принимает цвет в формате `#RRGGBB` или `RRGGBB`.

`--padding` задаёт внешний отступ карточки от `32` до `240` пикселей.

`--config` позволяет хранить повторяемые настройки проекта в JSON.

## Пресеты

| Пресет | Размер | Для чего |
| --- | ---: | --- |
| `github` | `1280x720` | README, changelog, превью проекта |
| `open-graph` | `1200x630` | Open Graph и link preview |
| `social` | `1080x1080` | квадратные social-карточки |
| `portfolio` | `1600x900` | портфолио и case study |
| `slide` | `1920x1080` | слайды 16:9 |
| `slide-4-3` | `1600x1200` | слайды 4:3 |

## Темы

| Тема | Стиль |
| --- | --- |
| `light` | светлый нейтральный фон и тёмный читаемый текст |
| `dark` | тёмный GitHub/dev-tool стиль и светлый текст |

## Возможности

- Генерация PNG-карточек из локальных скриншотов.
- Проверка входного файла и создание выходной директории.
- Защита от перезаписи исходного изображения.
- Пресеты, темы, пользовательский фон и настройка отступов.
- JSON-конфиги для повторяемого брендинга проекта.
- Batch mode для генерации нескольких карточек за один запуск.
- Локальный Web GUI на ASP.NET Core Razor Pages.
- GitHub Actions CI и release workflow.
- Готовые release-архивы для Windows, Linux и macOS.
- `.nupkg` для установки как .NET tool.
- `SHA256SUMS.txt` для проверки целостности release assets.

## До и после

| Исходный скриншот | Готовая карточка |
| --- | --- |
| <img src="examples/input/sample.png" alt="Исходный пример SnapForge" width="360"> | <img src="examples/output/sample-github-dark.png" alt="Сгенерированная GitHub-карточка SnapForge" width="420"> |

Больше примеров: [examples/README.md](examples/README.md).

## Документация

- [Установка](docs/INSTALLATION.md)
- [Руководство пользователя](docs/USER_GUIDE.md)
- [Руководство разработчика](docs/DEVELOPER_GUIDE.md)
- [Чеклист проверяющего](docs/REVIEWER_CHECKLIST.md)
- [Участие в разработке](CONTRIBUTING.md)
- [Changelog](CHANGELOG.md)

Английские версии доступны через переключатель языка в начале каждого документа.

## Структура проекта

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

## Разработка

```bash
dotnet restore
dotnet build
dotnet test
```

Запуск CLI:

```bash
dotnet run --project src/SnapForge.Cli -- --help
dotnet run --project src/SnapForge.Cli -- card --help
```

Запуск Web GUI:

```bash
dotnet run --project src/SnapForge.Web
```

## Why SnapForge?

SnapForge намеренно маленький и сфокусированный. Он нужен разработчикам, которым нужен быстрый и воспроизводимый способ делать аккуратные визуалы проекта без ручной работы в редакторе дизайна.

## Roadmap

- [x] CLI-команда `card`
- [x] темы `light` и `dark`
- [x] пресеты `github`, `social`, `portfolio`, `open-graph`, `slide`, `slide-4-3`
- [x] PNG export
- [x] JSON config files
- [x] batch mode
- [x] локальный Web GUI
- [x] CI и release assets
- [x] checksums для release assets
- [ ] публикация в NuGet
- [ ] дополнительные layout templates

## Made With C# And .NET

SnapForge построен на .NET 8, C#, Spectre.Console, Spectre.Console.Cli, SixLabors.ImageSharp, SixLabors.ImageSharp.Drawing, ASP.NET Core Razor Pages и xUnit.

## Лицензия

SnapForge распространяется по лицензии MIT. См. [LICENSE](LICENSE).
