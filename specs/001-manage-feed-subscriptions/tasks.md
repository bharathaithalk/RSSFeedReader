# Tasks: MVP Feed Subscriptions

**Input**: Design documents from `/specs/001-manage-feed-subscriptions/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`,
`contracts/subscriptions.openapi.yaml`, `quickstart.md`

**Tests**: Automated tests are included because the project constitution requires
tests for behavior changes and the plan calls for API unit and integration tests.

**Organization**: Tasks are grouped by the P1 user story in the feature spec.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the initial .NET solution and its API, UI, and test projects.

- [x] T001 Create `RssFeedReader.sln`, the .NET 10 projects `backend/RssFeedReader.Api/RssFeedReader.Api.csproj`, `frontend/RssFeedReader.UI/RssFeedReader.UI.csproj`, and `tests/RssFeedReader.Api.Tests/RssFeedReader.Api.Tests.csproj`; add all three projects to the solution, add the test-project reference to the API, expose the API entry point to `WebApplicationFactory<Program>` in `backend/RssFeedReader.Api/Program.cs`, and add xUnit, Microsoft.NET.Test.Sdk, and Microsoft.AspNetCore.Mvc.Testing test dependencies.

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Configure local API/UI communication before implementing subscription behavior.

- [x] T002 Configure the development CORS origin and frontend API base address in `backend/RssFeedReader.Api/Program.cs`, `backend/RssFeedReader.Api/appsettings.Development.json`, and `frontend/RssFeedReader.UI/wwwroot/appsettings.json`; keep the origin and API address configurable for local development.

**Checkpoint**: Solution and local API/UI configuration are ready for the subscription story.

## Phase 3: User Story 1 - Add and View Feed Subscriptions (Priority: P1) 🎯 MVP

**Goal**: Let the single local user add URLs and view the unique subscriptions for the current backend run.

**Independent Test**: Start the API and UI, add two different nonblank URLs, and verify both appear in insertion order within one second; submit an exact repeat and verify it appears once; submit whitespace and verify no entry is added; restart the API and verify the list is empty.

### Tests for User Story 1

- [x] T003 [P] [US1] Write unit tests in `tests/RssFeedReader.Api.Tests/Unit/InMemorySubscriptionStoreTests.cs` covering null, empty, and whitespace-only rejection; preservation of nonblank input without normalization; exact ordinal duplicate suppression; case-sensitive distinct values; insertion order; and a new store starting empty. Run them once the model/store types exist and confirm they fail before store behavior is implemented.
- [x] T004 [P] [US1] Write API integration tests in `tests/RssFeedReader.Api.Tests/Integration/SubscriptionsApiTests.cs` covering an empty `GET /api/subscriptions`, a successful `POST` returning 201, an exact duplicate returning 200 without adding a second row, and missing/null/empty/whitespace input returning 400. Run them before endpoint implementation and confirm the expected route/behavior failures.

### Implementation for User Story 1

- [x] T005 [P] [US1] Define the `FeedSubscription` response model with its required `url` field in `backend/RssFeedReader.Api/Subscriptions/FeedSubscription.cs`.
- [x] T006 [US1] Implement the process-local ordered subscription collection in `backend/RssFeedReader.Api/Subscriptions/InMemorySubscriptionStore.cs`; reject null, empty, and whitespace-only values, preserve other input exactly, compare URLs using ordinal case-sensitive equality, ignore exact repeats, and clear state when a new store/process starts.
- [x] T007 [US1] Register the subscription store and implement `GET /api/subscriptions` and `POST /api/subscriptions` in `backend/RssFeedReader.Api/Program.cs` according to `specs/001-manage-feed-subscriptions/contracts/subscriptions.openapi.yaml`; return the ordered list, 201 for a new URL, 200 for an exact repeat, and a safe 400 response for invalid input.
- [x] T008 [P] [US1] Implement the subscriptions API client in `frontend/RssFeedReader.UI/Services/SubscriptionsApiClient.cs` to read the configured API base address, fetch the list, and submit a URL using the documented endpoint and response contract.
- [x] T009 [US1] Implement the subscription entry form and list in `frontend/RssFeedReader.UI/Pages/Subscriptions.razor`; load and refresh the list through the API client, prevent empty submissions, show safe feedback for API errors, display URLs as text rather than active links, remove the template demo pages `frontend/RssFeedReader.UI/Pages/Home.razor`, `frontend/RssFeedReader.UI/Pages/Counter.razor`, and `frontend/RssFeedReader.UI/Pages/Weather.razor`, remove their links from `frontend/RssFeedReader.UI/Layout/NavMenu.razor`, and make this page the single root route.

**Checkpoint**: User Story 1 must be usable and independently verifiable before any deferred feature is considered.

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify integration and preserve runnable validation instructions.

- [x] T010 [P] Update `specs/001-manage-feed-subscriptions/quickstart.md` if project scaffold output changes the documented project paths, launch commands, or configured local API address.
- [x] T011 Run the automated tests and solution build described in `specs/001-manage-feed-subscriptions/quickstart.md`, then complete its browser and API smoke scenarios; confirm there are no requests to submitted feed URLs and report any failing command or scenario.

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; creates the solution and projects.
- **Foundational (Phase 2)**: Depends on T001 and blocks the user story.
- **User Story 1 (Phase 3)**: Depends on T002. Author tests T003 and T004 before
  implementing their behavior. T005 and T008 can proceed in parallel once the tests
  have been authored; T006 follows T005, T007 follows T006, and T009 follows T008.
- **Polish (Phase 4)**: Depends on the user story implementation being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after the foundational phase; it is the only story
  in this MVP and has no dependency on another user story.

### Within User Story 1

- T003 and T004 are independent test files and may be authored in parallel.
- After T003 and T004 are authored, T005 can proceed in parallel with T008.
- T006 depends on T005 and is validated by T003.
- T007 depends on T006 and is validated by T004.
- T008 uses the API contract and can proceed in parallel with T005-T007.
- T009 depends on T008.
- T010 may proceed independently after project creation; T011 depends on the
  completed implementation and runnable projects.

## Parallel Execution Examples

### User Story 1

```text
After T002:
Task: T003 Add store unit tests in tests/RssFeedReader.Api.Tests/Unit/InMemorySubscriptionStoreTests.cs
Task: T004 Add API integration tests in tests/RssFeedReader.Api.Tests/Integration/SubscriptionsApiTests.cs

After T003 and T004 are authored:
Task: T005 Define FeedSubscription in backend/RssFeedReader.Api/Subscriptions/FeedSubscription.cs
Task: T008 Implement the client in frontend/RssFeedReader.UI/Services/SubscriptionsApiClient.cs

After T005:
Task: T006 Implement the in-memory store in backend/RssFeedReader.Api/Subscriptions/InMemorySubscriptionStore.cs

After T006:
Task: T007 Implement API routes in backend/RssFeedReader.Api/Program.cs
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 and Phase 2 to establish the solution and local communication.
2. Complete T003 and T004, then implement the model, store, API, and UI for US1.
3. Complete T011 and stop when the independent story test passes.
4. Do not implement feed fetching, feed-item display, persistence, removal, or
   background polling as part of this MVP.
