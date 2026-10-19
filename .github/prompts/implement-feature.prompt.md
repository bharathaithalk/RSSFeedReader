---
description: Implement a feature in the RSS Feed Reader using its approved scope and project conventions.
agent: agent
argument-hint: Describe the feature and its acceptance criteria
---

Implement the following feature in this repository:

${input:feature}

Before editing, read `.specify/memory/constitution.md`, the relevant stakeholder documents, and the existing code that the change will affect. Verify that the request is within the approved MVP scope or is covered by an approved feature specification. If its scope or expected behavior is unclear, ask for clarification before implementing it. Do not silently add deferred capabilities such as feed fetching, persistence, subscription removal, or background polling.

Follow the repository architecture and established patterns: the ASP.NET Core Web API owns data and API behavior; the Blazor WebAssembly frontend owns interaction and presentation. Make a focused change, update or add tests for changed behavior, and preserve the API contract.

Run the relevant tests and build for each affected project. In your final response, summarize the implementation, list validation commands and results, and identify any outstanding decisions or blockers.
