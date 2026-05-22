# Установка

**Язык:** Русский | [English](en/INSTALLATION.md)

SnapForge можно проверить без сборки из исходников: скачайте готовый архив из GitHub Releases.

## Готовые CLI-архивы

Последний релиз:

```text
https://github.com/rndv1/SnapForge/releases
```

Release workflow публикует:

| Файл | Платформа |
| --- | --- |
| `snapforge-win-x64.zip` | Windows x64 |
| `snapforge-linux-x64.tar.gz` | Linux x64 |
| `snapforge-osx-x64.tar.gz` | macOS Intel |
| `snapforge-osx-arm64.tar.gz` | macOS Apple Silicon |
| `SnapForge.<version>.nupkg` | пакет .NET tool |
| `SHA256SUMS.txt` | SHA-256 checksums для release assets |

После распаковки:

```bash
snapforge --help
```

Windows:

```powershell
.\snapforge.exe --help
```

## Проверка checksums

Скачайте `SHA256SUMS.txt` из того же релиза, что и архив.

Linux/macOS:

```bash
sha256sum -c SHA256SUMS.txt
```

Windows PowerShell:

```powershell
Get-FileHash .\snapforge-win-x64.zip -Algorithm SHA256
```

Сравните полученный хэш со строкой для нужного файла в `SHA256SUMS.txt`.

## Установка из release package

Скачайте `.nupkg` из GitHub Release, положите его в локальную папку и установите как .NET tool:

```bash
dotnet tool install --global SnapForge --add-source ./packages
snapforge --help
```

## Сборка из исходников

Для разработки по-прежнему поддерживается локальная сборка:

```bash
dotnet restore
dotnet build
dotnet test
```
