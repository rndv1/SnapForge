# Installation

**Language:** [Русский](../INSTALLATION.md) | English

SnapForge can be verified without building from source by downloading a prebuilt release archive.

## Ready-To-Run App

Download the latest release:

```text
https://github.com/rndv1/SnapForge/releases
```

Available release assets:

| Asset | Platform |
| --- | --- |
| `snapforge-win-x64.zip` | Web App for Windows x64 |
| `snapforge-linux-x64.tar.gz` | Web App for Linux x64 |
| `snapforge-osx-x64.tar.gz` | Web App for macOS Intel |
| `snapforge-osx-arm64.tar.gz` | Web App for macOS Apple Silicon |
| `snapforge-cli-win-x64.zip` | CLI for Windows x64 |
| `snapforge-cli-linux-x64.tar.gz` | CLI for Linux x64 |
| `snapforge-cli-osx-x64.tar.gz` | CLI for macOS Intel |
| `snapforge-cli-osx-arm64.tar.gz` | CLI for macOS Apple Silicon |
| `SnapForge.<version>.nupkg` | .NET tool package |
| `SHA256SUMS.txt` | SHA-256 checksums for release assets |

After extracting the Web App archive:

```bash
./snapforge
```

Windows:

```powershell
.\snapforge.exe
```

The app starts a local server, opens the browser, and keeps running while the process window is open.

## CLI

For terminal usage, download `snapforge-cli-*` or install the `.nupkg` as a .NET tool.

## Verify Checksums

Download `SHA256SUMS.txt` from the same release as the archive.

Linux/macOS:

```bash
sha256sum -c SHA256SUMS.txt
```

Windows PowerShell:

```powershell
Get-FileHash .\snapforge-win-x64.zip -Algorithm SHA256
```

Compare the hash with the matching line in `SHA256SUMS.txt`.

## Install From Release Package

Download the `.nupkg` from the GitHub Release, place it in a local folder, and install it as a .NET tool:

```bash
dotnet tool install --global SnapForge --add-source ./packages
snapforge --help
```

## Build From Source

Source builds are still supported for contributors:

```bash
dotnet restore
dotnet build
dotnet test
```
