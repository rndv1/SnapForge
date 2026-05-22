# Руководство пользователя SnapForge

**Язык:** Русский | [English](en/USER_GUIDE.md)

SnapForge превращает скриншоты в аккуратные PNG-карточки для README, changelog, социальных сетей, портфолио и презентаций. Основной способ для проверяющего — готовый Web App, который запускается через `snapforge.exe`.

## Установка

Самый простой способ проверки — скачать готовый Web App архив из GitHub Releases:

```text
https://github.com/rndv1/SnapForge/releases
```

Выберите Web App архив для своей платформы, распакуйте и запустите:

```bash
./snapforge
```

На Windows исполняемый файл называется `snapforge.exe`. При запуске приложение откроет браузер с локальным интерфейсом.

## Web GUI

В Web GUI можно загрузить скриншот, выбрать пресет, тему, цвет фона, padding и скачать готовый PNG.

Скриншот можно выбрать через кнопку, перетащить на панель настроек или вставить из буфера обмена через `Ctrl+V`.

Интерфейс по умолчанию русский. В правом верхнем углу есть переключатель `RU` / `EN`.

## Генерация одной карточки

Для терминального режима используйте CLI-архивы `snapforge-cli-*` или установленный .NET tool.

```bash
snapforge-cli card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --title "SnapForge" \
  --subtitle "GitHub-ready screenshots" \
  --preset github \
  --theme dark
```

Windows PowerShell:

```powershell
snapforge-cli card .\examples\input\sample.png `
  --output .\examples\output\sample-card.png `
  --title "SnapForge" `
  --subtitle "GitHub-ready screenshots" `
  --preset github `
  --theme dark
```

## Опции

| Опция | Описание |
| --- | --- |
| `<input>` | путь к исходному скриншоту |
| `--output` | путь к выходному PNG |
| `--title` | заголовок карточки |
| `--subtitle` | подзаголовок карточки |
| `--preset` | `github`, `open-graph`, `social`, `portfolio`, `slide` или `slide-4-3` |
| `--theme` | `light` или `dark` |
| `--background` | необязательный цвет фона, например `#0F172A` |
| `--padding` | необязательный внешний отступ от `32` до `240` пикселей |
| `--config` | необязательный JSON-конфиг; CLI-значения переопределяют конфиг |

## JSON config

```json
{
  "title": "SnapForge",
  "subtitle": "GitHub-ready screenshots",
  "preset": "github",
  "theme": "dark",
  "background": "#0F172A",
  "padding": 112
}
```

Запуск с конфигом:

```bash
snapforge-cli card ./examples/input/sample.png \
  --output ./examples/output/sample-card.png \
  --config ./examples/snapforge.config.json
```

Относительные пути `input` и `output` внутри конфига считаются относительно файла конфига.

## Batch mode

Batch mode генерирует несколько карточек из одного JSON-файла:

```bash
snapforge-cli batch ./examples/snapforge.batch.json
```

По умолчанию SnapForge продолжает обработку после ошибки отдельной карточки и завершает процесс с кодом `1`, если была хотя бы одна ошибка. Используйте `--stop-on-error`, если CI должен остановиться на первой ошибке.

## Запуск Web GUI из исходников

```bash
dotnet run --project src/SnapForge.Web
```

Откройте URL, который выведет ASP.NET Core, если браузер не открылся автоматически.
