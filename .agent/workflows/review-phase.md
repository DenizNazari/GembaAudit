---
description: "Reviewer: audit the current phase against its acceptance criteria and write the review file."
---

1. Read the phase's acceptance criteria in `GembaAudit-Roadmap.md` and the latest `docs/PROGRESS.md`.
2. List what changed: `git diff --stat main`.
3. Run `dotnet build` and `dotnet test` yourself. Quote the exact results.
4. Review against the checklist in the reviewer skill. Prove every finding with a file path and line number.
5. Create `docs/reviews/phase-<N>.md` from `docs/reviews/_TEMPLATE.md`. Stubs, empty classes, the default weather-forecast template and placeholder tests count as NOT implemented.
6. State the verdict. Do not edit code and do not commit.
