# Installation

SnapForge can be verified without building from source by downloading a prebuilt release archive.

## Prebuilt CLI Archives

Download the latest release from:

```text
https://github.com/rndv1/SnapForge/releases
```

Available release assets are created by the Release workflow:

| Asset | Platform |
| --- | --- |
| `snapforge-win-x64.zip` | Windows x64 |
| `snapforge-linux-x64.tar.gz` | Linux x64 |
| `snapforge-osx-x64.tar.gz` | macOS Intel |
| `snapforge-osx-arm64.tar.gz` | macOS Apple Silicon |
| `SnapForge.<version>.nupkg` | .NET tool package |

After extracting the platform archive, run:

```bash
snapforge --help
```

Windows:

```powershell
.\snapforge.exe --help
```

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
