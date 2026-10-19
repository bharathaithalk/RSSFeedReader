<!--
Sync Impact Report
Version change: unresolved template → 1.0.0 (initial project-specific constitution)
Modified principles: unresolved template placeholders → five project-specific principles
Added sections: Project Constraints; Development Workflow and Quality Gates
Removed sections: none
Follow-up TODOs: Confirm the original ratification date.
-->

# RSS Feed Reader Constitution

## Core Principles

### I. Security and Trust Boundaries
All application inputs and feed content MUST be treated as untrusted. The MVP MUST NOT
fetch remote feeds or store secrets in source control. Input checks MUST match the
documented MVP scope, which accepts feed URLs without URL validation. When remote
fetching is approved for a later phase, requests MUST be limited to HTTP or HTTPS and
must prevent access to loopback, private, link-local, and cloud metadata destinations,
including after redirects. Errors shown to users MUST NOT disclose secrets or
implementation details.

### II. Maintainable Separation of Responsibilities
The ASP.NET Core Web API MUST own subscription data and API behavior; the Blazor
WebAssembly frontend MUST own user interaction and presentation. API contracts MUST be
explicit and kept consistent across both layers. New code MUST follow existing project
patterns and avoid abstractions that do not support a documented requirement.

### III. Verified Behavior and Code Quality
Changes MUST include or update automated tests for the behavior they add or alter.
Subscription changes MUST verify that adding a subscription updates the list returned
to the UI. Changes to API/frontend integration MUST verify the affected contract and
communication path. Before a change is considered complete, the affected projects
MUST build successfully and relevant tests MUST pass.

### IV. MVP Scope and Incremental Delivery
The MVP is limited to adding feed subscriptions by URL and displaying the subscription
list. It MUST use in-memory storage and MUST NOT fetch or parse feeds, persist data,
remove subscriptions, or add background polling. Features outside this scope require
an explicitly approved specification before implementation. Later phases MUST add
capabilities incrementally without silently changing MVP behavior.

### V. Clear and Safe User Feedback
The UI MUST reflect successful subscription changes immediately. Operations that can
fail MUST return an appropriate API outcome and provide a clear, safe user-facing
message; diagnostic detail belongs in appropriate application logs, not in messages
exposed to users. The MVP has no network feed operations, so feed-fetch error handling
is required only when that capability is approved and implemented.

## Project Constraints

The supported application architecture is an ASP.NET Core Web API backend with a
Blazor WebAssembly frontend, written in C#. The application MUST remain cross-platform
for local development on Windows, macOS, and Linux. The backend and frontend MUST
retain their defined responsibilities. A technology or architecture change MUST be
justified by a documented project requirement and reflected in the relevant project
documentation.

## Development Workflow and Quality Gates

For changes that affect application behavior, developers MUST run the relevant
automated tests and build the affected backend and/or frontend projects. Changes to
API/frontend communication MUST also be checked against the configured backend URL
and CORS policy, and smoke-tested through the affected user flow when practical.
Reviews MUST confirm that the change stays within the approved feature scope,
preserves the API contract, and complies with the security and testing principles.
Any necessary exception MUST be documented with its rationale and scope.

## Governance

This constitution governs project implementation and review. Amendments MUST be
documented in this file, reviewed for impact on existing work, and reflected in the
constitution version and last-amended date. The version follows semantic versioning:
MAJOR for incompatible governance changes, MINOR for new or materially expanded
principles or sections, and PATCH for clarifications and other non-semantic changes.

Each implementation and review MUST check applicable requirements in this
constitution. Any deviation MUST be explicit, justified, and approved as part of the
change review. When requirements conflict, the narrower approved feature scope and
security constraints take precedence; unresolved conflicts MUST be settled before
implementation.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): Original adoption date unknown | **Last Amended**: 2026-10-19
