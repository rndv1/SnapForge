# Changelog

**Language:** [Русский](CHANGELOG.md) | English

## Unreleased

### Added

- Code of conduct and security policy for GitHub Community Standards.
- Polished Russian documentation and translated the remaining English headings.
- Release asset checksum generation with `SHA256SUMS.txt`.
- Russian documentation as the default documentation language.
- English documentation variants with language switch links.
- Ready-to-run Web App archives where `snapforge.exe` starts the local Web GUI and opens the browser.
- `RU` / `EN` language switch in the Web GUI.

### Changed

- Release creation now uses GitHub CLI instead of a third-party release action.
- CLI archives are renamed to `snapforge-cli-*` to separate terminal usage from the ready-to-run Web App.

## 0.1.0

Initial SnapForge release.

### Added

- CLI command for generating screenshot cards.
- PNG rendering with ImageSharp.
- Light and dark themes.
- Built-in presets for GitHub, Open Graph, social posts, portfolio cards, and presentation slides.
- Custom background colors.
- Optional card padding controls.
- JSON config files.
- Batch mode.
- Local Web GUI.
- GitHub Actions CI.
- Release workflow for prebuilt CLI archives and .NET tool packages.
