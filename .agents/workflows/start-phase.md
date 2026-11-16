---
description: "Conductor: pick the next phase, create its branch, write task briefs."
---

1. Read `AGENTS.md`, `GembaAudit-Roadmap.md`, `docs/PROGRESS.md` and `docs/DECISIONS.md`.
2. Pick the first phase not marked complete in `docs/PROGRESS.md`. State which phase it is. Never skip a phase.
3. Run `git status`. If the working tree is not clean, stop and report.
4. Create or switch to branch `phase/<N>-<slug>`.
5. Following the conductor skill, write one brief per task into `docs/TASK_BRIEF.md`, each with role, tier and starter prompt.
6. Mark the phase "in progress" in `docs/PROGRESS.md`.
7. Stop. Tell the user which conversations to open, with which skill and tier.
