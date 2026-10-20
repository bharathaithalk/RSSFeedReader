# Research: MVP Feed Subscriptions

## .NET target

- **Decision**: Target .NET 10 LTS for the API, browser UI, and tests.
- **Rationale**: The stakeholder documents select ASP.NET Core and Blazor but do not
  state a framework version. .NET 10 is the active LTS release as of this plan and
  the installed SDK is 10.0.203. Microsoft lists support through November 14, 2028.
- **Alternatives considered**: .NET 8 LTS and .NET 9 STS. Both are near end of support
  as of this plan; .NET 11 is still a release candidate. .NET 10 provides a supported
  stable target without adding a preview dependency.
- **Source**: [Microsoft .NET release lifecycle](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)

## Application architecture

- **Decision**: Keep the project-approved ASP.NET Core Web API and Blazor WebAssembly
  separation. Use a small API surface for listing and adding subscriptions, with the
  UI calling the API for both operations.
- **Rationale**: This honors the architecture and responsibilities in
  `StakeholderDocuments/TechStack.md` and leaves the UI independent of the storage
  mechanism. There is no existing application code to extend.
- **Alternatives considered**: A frontend-only local collection would be simpler but
  would conflict with the approved backend ownership of subscription data and the
  constitution's architecture constraint.

## In-memory uniqueness and lifecycle

- **Decision**: Keep one process-local collection, preserve insertion order, and
  compare submitted URL strings using exact ordinal equality. Do not normalize,
  validate, or fetch URLs.
- **Rationale**: This implements the approved in-memory MVP and the clarification
  that exact repeat URLs do not create duplicate entries. Keeping a stable list makes
  the UI predictable while remaining adequate for the stated minimum of five entries.
- **Alternatives considered**: URL canonicalization would change equality behavior
  and introduce validation complexity; persistent storage and cross-process
  synchronization are explicitly outside scope.

## API and testing

- **Decision**: Expose `GET /api/subscriptions` and `POST /api/subscriptions`; document
  the request and response behavior in the OpenAPI contract. Cover store behavior
  with unit tests and HTTP behavior with API integration tests.
- **Rationale**: These operations map directly to list and add flows. A narrow,
  documented contract supports testing the frontend/backend boundary without adding
  a feed service or extra dependency.
- **Alternatives considered**: Separate routes for individual subscriptions are
  unnecessary because removal and item-level lookup are out of scope.
