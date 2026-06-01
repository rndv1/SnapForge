# Установка

**Язык:** Русский | [English](en/INSTALLATION.md)

SnapForge можно проверить без сборки из исходников: скачайте готовый архив из GitHub Releases.

## Готовое приложение

Последний релиз:

```text
https://github.com/rndv1/SnapForge/releases
```

Процесс релиза публикует:

| Файл | Платформа |
| --- | --- |
| `snapforge-win-x64.zip` | веб-приложение для Windows x64 |
| `snapforge-linux-x64.tar.gz` | веб-приложение для Linux x64 |
| `snapforge-osx-x64.tar.gz` | веб-приложение для macOS Intel |
| `snapforge-osx-arm64.tar.gz` | веб-приложение для macOS Apple Silicon |
| `snapforge-cli-win-x64.zip` | CLI для Windows x64 |
| `snapforge-cli-linux-x64.tar.gz` | CLI для Linux x64 |
| `snapforge-cli-osx-x64.tar.gz` | CLI для macOS Intel |
| `snapforge-cli-osx-arm64.tar.gz` | CLI для macOS Apple Silicon |
| `SnapForge.<version>.nupkg` | пакет .NET tool |
| `SHA256SUMS.txt` | контрольные суммы SHA-256 для релизных файлов |

После распаковки веб-приложения запустите:

```bash
./snapforge
```

Windows:

```powershell
.\snapforge.exe
```

Приложение поднимет локальный сервер, откроет браузер и будет работать, пока открыто окно процесса.

## CLI

Если нужен терминальный режим, скачайте `snapforge-cli-*` или установите `.nupkg` как .NET tool.

## Проверка контрольных сумм

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

## Установка из релизного пакета

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
