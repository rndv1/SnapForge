# Чеклист проверяющего

**Язык:** Русский | [English](en/REVIEWER_CHECKLIST.md)

Этот файл показывает, где в SnapForge находятся обязательные артефакты проекта.

## Исходный код продукта

Код продукта:

- `src/SnapForge.Core`
- `src/SnapForge.Cli`
- `src/SnapForge.Web`

Тесты:

- `tests/SnapForge.Tests`

## Документация пользователя

- `README.md`
- `README.en.md`
- `docs/USER_GUIDE.md`
- `docs/INSTALLATION.md`
- `examples/README.md`

## Документация разработчика

- `CONTRIBUTING.md`
- `docs/DEVELOPER_GUIDE.md`

## Установочник или ссылка на сайт

SnapForge можно проверить без сборки из исходников через GitHub Releases:

```text
https://github.com/rndv1/SnapForge/releases
```

Release workflow создаёт:

- Windows CLI archive;
- Linux CLI archive;
- macOS CLI archives;
- .NET tool package;
- `SHA256SUMS.txt` для проверки целостности файлов.

## Быстрая проверка

После распаковки release archive:

```bash
snapforge --help
snapforge card ./examples/input/sample.png --output ./sample-card.png --title "SnapForge" --subtitle "Release verification" --preset github --theme dark
```
