# Участие в разработке SnapForge

**Язык:** Русский | [English](CONTRIBUTING.en.md)

Спасибо за интерес к SnapForge.

SnapForge намеренно остаётся небольшим проектом: сфокусированный .NET CLI, понятный рендеринг, компактный набор пресетов и минимальная публичная поверхность. Вклад в проект должен сохранять эту форму.

## Локальная подготовка

Требования:

- .NET 8 SDK
- Git

Стандартные проверки перед pull request:

```bash
dotnet restore
dotnet build
dotnet test
```

Команды в форме, близкой к CI:

```bash
dotnet build --no-restore --configuration Release
dotnet test --no-build --configuration Release --verbosity normal
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --no-build --configuration Release --output artifacts/packages
```

## Локальная упаковка как .NET tool

```bash
dotnet pack src/SnapForge.Cli/SnapForge.Cli.csproj --configuration Release --output artifacts/packages
dotnet tool install --global SnapForge --add-source ./artifacts/packages
snapforge --help
```

Если tool уже установлен:

```bash
dotnet tool uninstall --global SnapForge
```

## Ветки

Используйте короткие feature branches:

```bash
git checkout -b feature/my-change
```

Ветка должна быть достаточно узкой, чтобы её можно было спокойно посмотреть за один review pass.

## Pull requests

Хороший PR для SnapForge обычно содержит:

- короткое описание изменений;
- команды проверки;
- screenshots или generated output, если меняется визуальный рендеринг;
- тесты, если меняется поведение registry, options, validation или rendering contracts.

Коммиты лучше держать читаемыми и логичными. Несколько маленьких коммитов обычно понятнее одного смешанного.

## Code style

- Общий рендеринг, модели, пресеты, темы и утилиты держите в `SnapForge.Core`.
- `SnapForge.Cli` должен отвечать за команды, валидацию, консольный вывод и упаковку.
- `SnapForge.Web` должен отвечать за browser workflow и переиспользовать `SnapForge.Core`.
- Предпочитайте простой код тяжёлой архитектуре.
- Не добавляйте внешние изображения, логотипы или защищённые ассеты.
- Пользовательские ошибки должны быть понятными и actionable.
- CLI-first experience важен: вывод команды должен быть полезным, но не шумным.

## Изменения Web GUI

При изменении web-интерфейса запустите приложение и проверьте upload, preview и download:

```bash
dotnet run --project src/SnapForge.Web
```

Первый экран должен оставаться генератором, а не marketing landing page.

## Лицензия

Отправляя вклад в проект, вы соглашаетесь, что он будет распространяться по лицензии MIT.
