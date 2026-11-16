---
description: "Conductor: run the phase gate and ask the user to approve it."
---

1. Run `dotnet build` and `dotnet test` (from Phase 6 also `docker compose up --build` and check health). Quote the results.
2. Check every acceptance criterion of the phase in `GembaAudit-Roadmap.md`. Build a table: criterion, PASS/FAIL/NOT VERIFIED, one line of evidence.
3. Confirm `docs/reviews/phase-<N>.md` exists and says `VERDICT: APPROVED`. If not, stop; the phase is not complete.
4. Update `docs/PROGRESS.md` (status, verdict, evidence).
5. Stop and ask the user to approve the gate. Do not merge and do not start the next phase.
6. Only after the user approves: merge `phase/<N>-<slug>` into `main` with `git merge --no-ff`.
