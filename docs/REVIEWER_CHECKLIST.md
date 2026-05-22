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

- Windows Web App archive с `snapforge.exe`;
- Linux Web App archive;
- macOS Web App archives;
- отдельные CLI archives `snapforge-cli-*`;
- .NET tool package;
- `SHA256SUMS.txt` для проверки целостности файлов.

## Быстрая проверка

После распаковки Windows Web App archive:

```powershell
.\snapforge.exe
```

Откроется локальный Web GUI. Загрузите PNG/JPG-скриншот, выберите настройки и скачайте результат. Интерфейс поддерживает переключение `RU` / `EN`.
