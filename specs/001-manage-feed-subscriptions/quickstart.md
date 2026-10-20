# Quickstart: MVP Feed Subscriptions

This guide validates the feature after its implementation projects have been created.
Commands use the proposed project paths in `plan.md`.

## Prerequisites

- .NET 10 SDK installed.
- A supported desktop browser.
- The repository's API and Blazor UI projects created as described in `plan.md`.

## Build and automated tests

From the repository root:

```powershell
dotnet restore RssFeedReader.sln
dotnet test RssFeedReader.sln
dotnet build RssFeedReader.sln --no-restore
```

Expected: restore, tests, and build complete successfully. The API tests cover
empty/whitespace rejection, insertion order, exact duplicate suppression, and
clearing the collection with a new process. HTTP integration tests cover the
contract in `contracts/subscriptions.openapi.yaml`.

## Run the application

In a first terminal, start the API:

```powershell
dotnet run --project backend/RssFeedReader.Api
```

In a second terminal, start the browser UI:

```powershell
dotnet run --project frontend/RssFeedReader.UI
```

Open the browser address printed by the UI process. The configured API base address
must point at the running API, and the API's local CORS policy must permit the UI's
development origin.

## Verify through the UI

1. Confirm the subscription list is initially empty.
2. Submit `https://example.test/feed.xml`; confirm it appears immediately as text.
3. Submit `https://example.test/second.xml`; confirm both entries appear in insertion
   order.
4. Submit `https://example.test/feed.xml` again; confirm it remains listed only once.
5. Submit an empty or whitespace-only value; confirm no empty row appears.
6. Confirm no feed items are shown and no request is made to either submitted feed.
7. Stop and restart the API; confirm the list is empty again.

## Verify the API directly

Set `$apiBase` to the address printed by the API process, then run:

```powershell
$apiBase = 'http://localhost:5000'
Invoke-RestMethod -Uri "$apiBase/api/subscriptions" -Method Get
Invoke-RestMethod -Uri "$apiBase/api/subscriptions" -Method Post `
  -ContentType 'application/json' `
  -Body '{"url":"https://example.test/feed.xml"}'
Invoke-RestMethod -Uri "$apiBase/api/subscriptions" -Method Post `
  -ContentType 'application/json' `
  -Body '{"url":"https://example.test/feed.xml"}'
Invoke-RestMethod -Uri "$apiBase/api/subscriptions" -Method Get
```

Expected: GET returns an empty JSON array before adding and a one-entry array
afterward; the first POST returns the created subscription and the repeated POST
returns the existing subscription without adding another row. A whitespace-only
URL returns HTTP 400. Replace port `5000` with the actual configured API address if
needed.
