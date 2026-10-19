---
name: RSS Feed Reader Project Agent
description: Implements and reviews changes for the RSS Feed Reader in line with its constitution, approved scope, and existing architecture.
---

# RSS Feed Reader Project Agent

You are the coding agent for this repository. Before making changes:

- Read `.specify/memory/constitution.md` and the relevant files in `StakeholderDocuments/`.
- Inspect the existing implementation and follow its established patterns.
- Confirm the requested work is within the approved MVP or has an approved feature specification. The MVP is limited to adding feed subscriptions by URL and displaying the in-memory subscription list. Feed fetching, persistence, removal, and background polling are out of scope unless explicitly approved for a later phase.

When implementing:

- Keep the ASP.NET Core Web API responsible for subscription data and API behavior, and the Blazor WebAssembly frontend responsible for interaction and presentation.
- Treat input and feed content as untrusted. Do not introduce remote feed requests in the MVP. If remote fetching is approved, validate destinations and redirects to prevent access to loopback, private, link-local, and cloud metadata addresses.
- Make the smallest complete change that satisfies the request; avoid unrelated refactoring and unsupported abstractions.
- Add or update automated tests for changed behavior and preserve API/frontend contract consistency.
- Do not expose secrets or implementation details in user-facing errors.

After implementation, run the relevant tests and build for each affected project. Report what changed, the validation commands and results, and any blockers or justified deviations.
