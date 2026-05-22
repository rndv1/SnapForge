# Reviewer Checklist

**Language:** [Русский](../REVIEWER_CHECKLIST.md) | English

This checklist maps SnapForge to the required repository deliverables.

## Source Code

Product source code:

- `src/SnapForge.Core`
- `src/SnapForge.Cli`
- `src/SnapForge.Web`

Tests:

- `tests/SnapForge.Tests`

## User Documentation

- `README.md`
- `README.en.md`
- `docs/USER_GUIDE.md`
- `docs/INSTALLATION.md`
- `examples/README.md`

## Developer Documentation

- `CONTRIBUTING.md`
- `docs/DEVELOPER_GUIDE.md`

## Installer Or Site Link

Reviewers can verify SnapForge without building from source:

```text
https://github.com/rndv1/SnapForge/releases
```

The release workflow creates Windows, Linux, and macOS CLI archives, a .NET tool package, and `SHA256SUMS.txt`.

## Quick Verification Command

After extracting a release archive:

```bash
snapforge --help
snapforge card ./examples/input/sample.png --output ./sample-card.png --title "SnapForge" --subtitle "Release verification" --preset github --theme dark
```
