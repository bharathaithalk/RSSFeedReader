# RSS Feed Reader

A local MVP for adding feed URLs and viewing the unique subscriptions held by the
running API process. The app does not fetch feeds or retain subscriptions after
the API stops.

## Prerequisites

- .NET 10 SDK
- A desktop browser

Check the installed SDK:

```powershell
dotnet --version
```

## Run the application

Run the API and UI in separate PowerShell terminals from the repository root.

Terminal 1 — start the API:

```powershell
dotnet run --project .\backend\RssFeedReader.Api
```

The API listens at `http://localhost:5173`.

Terminal 2 — start the Blazor UI:

```powershell
dotnet run --project .\frontend\RssFeedReader.UI
```

Open [http://localhost:5287](http://localhost:5287) in your browser. Add feed
URLs using the form; they are displayed as text and are not requested by the app.
Subscriptions are stored in memory and are cleared when the API process stops.

To stop either project, press `Ctrl+C` in its terminal.

## Build and test

From the repository root:

```powershell
dotnet restore .\RssFeedReader.sln
dotnet test .\RssFeedReader.sln
dotnet build .\RssFeedReader.sln --no-restore
```

## API endpoints

- `GET http://localhost:5173/api/subscriptions` — list subscriptions.
- `POST http://localhost:5173/api/subscriptions` — add a subscription with a JSON
  body such as `{"url":"https://example.test/feed.xml"}`.

An exact repeat returns the existing subscription. Missing, empty, or
whitespace-only URLs return HTTP 400.

## Local configuration

- API URL: `frontend/RssFeedReader.UI/wwwroot/appsettings.json`, key
  `ApiBaseAddress`.
- Permitted development UI origins:
  `backend/RssFeedReader.Api/appsettings.Development.json`, key
  `Cors:AllowedOrigins`.
- Project ports: `backend/RssFeedReader.Api/Properties/launchSettings.json` and
  `frontend/RssFeedReader.UI/Properties/launchSettings.json`.

If you change a project port, update the corresponding API address or CORS origin
so the UI can connect.
