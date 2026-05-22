# Changelog

**Язык:** Русский | [English](CHANGELOG.en.md)

## Unreleased

### Added

- Генерация `SHA256SUMS.txt` для release assets.
- Русская документация как основной вариант.
- Английские варианты документации с переключателями языка.
- Готовые Web App архивы, где `snapforge.exe` запускает локальный Web GUI и открывает браузер.
- Переключатель языка `RU` / `EN` в Web GUI.

### Changed

- Создание GitHub Release теперь выполняется через GitHub CLI вместо стороннего release action.
- CLI-архивы переименованы в `snapforge-cli-*`, чтобы не путать терминальный CLI и готовое Web-приложение.

## 0.1.0

Первый релиз SnapForge.

### Added

- CLI-команда для генерации screenshot cards.
- PNG rendering через ImageSharp.
- Темы `light` и `dark`.
- Встроенные пресеты для GitHub, Open Graph, social posts, portfolio cards и presentation slides.
- Пользовательские цвета фона.
- Настройка padding.
- JSON config files.
- Batch mode.
- Локальный Web GUI.
- GitHub Actions CI.
- Release workflow для готовых CLI-архивов и .NET tool package.
