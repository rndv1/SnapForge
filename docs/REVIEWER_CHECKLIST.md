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
- `CODE_OF_CONDUCT.md`
- `SECURITY.md`
- `docs/DEVELOPER_GUIDE.md`

## Установочник или ссылка на сайт

SnapForge можно проверить без сборки из исходников через GitHub Releases:

```text
https://github.com/rndv1/SnapForge/releases
```

Процесс релиза создаёт:

- архив веб-приложения для Windows с `snapforge.exe`;
- архив веб-приложения для Linux;
- архивы веб-приложения для macOS;
- отдельные CLI-архивы `snapforge-cli-*`;
- пакет .NET tool;
- `SHA256SUMS.txt` для проверки целостности файлов.

## Быстрая проверка

После распаковки Windows-архива веб-приложения:

```powershell
.\snapforge.exe
```

Откроется локальный веб-интерфейс. Загрузите PNG/JPG-скриншот, выберите настройки и скачайте результат. Интерфейс поддерживает переключение `RU` / `EN`.
