# Руководство разработчика SnapForge

**Язык:** Русский | [English](en/DEVELOPER_GUIDE.md)

Этот документ описывает устройство репозитория и базовый рабочий процесс разработки.

## Требования

- .NET 8 SDK
- Git

## Структура репозитория

```text
src/
├── SnapForge.Core/   Общий рендеринг, конфиги, пресеты, темы и модели.
├── SnapForge.Cli/    CLI-команды Spectre.Console и упаковка tool-пакета.
└── SnapForge.Web/    Локальный Web App на Razor Pages, публикуемый как snapforge.exe.

tests/
└── SnapForge.Tests/  xUnit-тесты реестров, конфигов и контрактов рендера.
```

## Локальные проверки

Перед pull request запускайте:

```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build --verbosity normal
dotnet format --verify-no-changes --no-restore
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --configuration Release --no-build --output artifacts/packages
```

## Архитектура

SnapForge намеренно не использует тяжёлую архитектуру. Основные границы такие:

- `SnapForge.Core` содержит рендеринг, модели, пресеты, темы, парсинг конфигов и helper-валидацию.
- `SnapForge.Cli` отвечает за команды, консольный вывод, orchestration и упаковку .NET tool.
- `SnapForge.Web` отвечает за браузерный сценарий, локализацию интерфейса, запуск браузера и переиспользует `SnapForge.Core`.

Новые возможности лучше добавлять рядом с этими границами, не перенося рендеринг в CLI или Web-слой.

## Изменения рендеринга

Когда меняется визуальный вывод:

1. Добавьте или обновите точечные тесты.
2. Сгенерируйте хотя бы одну карточку из `examples/input/sample.png`.
3. Посмотрите PNG перед PR.
4. Обновляйте example images только если это намеренная часть документации.

## Release flow

Релизы собирает `.github/workflows/release.yml`.

```bash
git checkout main
git pull --ff-only origin main
git tag -a v0.1.0 -m "Release v0.1.0"
git push origin v0.1.0
```

Workflow публикует Web App архивы `snapforge-*`, отдельные CLI-архивы `snapforge-cli-*`, `.nupkg` и `SHA256SUMS.txt`.
