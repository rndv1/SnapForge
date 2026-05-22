# Reviewer Checklist

This file maps SnapForge to the required repository deliverables.

## Source Code

Product source code is in:

- `src/SnapForge.Core`
- `src/SnapForge.Cli`
- `src/SnapForge.Web`

Tests are in:

- `tests/SnapForge.Tests`

## User Documentation

User-facing documentation is available in:

- `README.md`
- `docs/USER_GUIDE.md`
- `docs/INSTALLATION.md`
- `examples/README.md`

## Developer Documentation

Developer documentation is available in:

- `CONTRIBUTING.md`
- `docs/DEVELOPER_GUIDE.md`

## Installer Or Site Link

Reviewers can verify SnapForge without building from source by downloading release assets from:

```text
https://github.com/rndv1/SnapForge/releases
```

The release workflow creates:

- Windows CLI archive
- Linux CLI archive
- macOS CLI archives
- .NET tool package

## Quick Verification Command

After extracting a release archive:

```bash
snapforge --help
snapforge card ./examples/input/sample.png --output ./sample-card.png --title "SnapForge" --subtitle "Release verification" --preset github --theme dark
```
