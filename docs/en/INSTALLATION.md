# Installation

**Language:** [Русский](../INSTALLATION.md) | English

SnapForge can be verified without building from source by downloading a prebuilt release archive.

## Prebuilt CLI Archives

Download the latest release:

```text
https://github.com/rndv1/SnapForge/releases
```

Available release assets:

| Asset | Platform |
| --- | --- |
| `snapforge-win-x64.zip` | Windows x64 |
| `snapforge-linux-x64.tar.gz` | Linux x64 |
| `snapforge-osx-x64.tar.gz` | macOS Intel |
| `snapforge-osx-arm64.tar.gz` | macOS Apple Silicon |
| `SnapForge.<version>.nupkg` | .NET tool package |
| `SHA256SUMS.txt` | SHA-256 checksums for release assets |

After extracting the platform archive:

```bash
snapforge --help
```

Windows:

```powershell
.\snapforge.exe --help
```

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
