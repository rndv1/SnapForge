# Changelog

**Язык:** Русский | [English](CHANGELOG.en.md)

## Unreleased

### Added

- Генерация `SHA256SUMS.txt` для release assets.
- Русская документация как основной вариант.
- Английские варианты документации с переключателями языка.

### Changed

- Создание GitHub Release теперь выполняется через GitHub CLI вместо стороннего release action.

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
