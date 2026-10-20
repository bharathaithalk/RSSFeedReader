# Data Model: MVP Feed Subscriptions

## Feed Subscription

Represents one URL entered by the local user and held for the running backend
process.

| Field | Type | Rules |
|---|---|---|
| `url` | String | Required; reject null, empty, or whitespace-only input. Preserve the supplied string without URL validation or normalization. |

### Identity and uniqueness

- The exact `url` string is the identity of a subscription during the process lifetime.
- Equality is ordinal and case-sensitive. Do not trim, canonicalize, or otherwise
  normalize a non-empty submitted value.
- An exact repeat does not add a second entry. Different strings remain distinct,
  even when they might resolve to the same resource.
- Preserve insertion order for list responses and display.

### Lifecycle

1. A non-empty URL is submitted.
2. If the exact value is not present, it is added to the in-memory collection.
3. If the exact value is already present, the collection remains unchanged.
4. All entries are discarded when the backend process stops or restarts.

No relationship to feed items is modeled; feed fetching and item storage are outside
the MVP.
