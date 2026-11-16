---
description: "Conductor: verify a finished task and commit it on the phase branch."
---

1. Read the task's handoff in `docs/handoffs/`.
2. Run `dotnet build` and `dotnet test` yourself. Both must pass with 0 warnings. If not, stop and report; do not commit.
3. Run `git status`. Every changed file must be inside the implementing role's allowed paths. Otherwise report an ownership violation and do not stage those files.
4. Stage only the role's paths: `git add <paths>`. Never `git add -A` or `git add .`.
5. Scan the staged diff for secrets: `git diff --cached | Select-String -Pattern 'api[_-]?key|secret|password|token|Host=.*;|Server=.*;' -CaseSensitive:$false`. Any real secret means stop.
6. Commit with a Conventional Commit message with scope. Never force-push.
7. Add a log line to `docs/PROGRESS.md` and any new decisions to `docs/DECISIONS.md`.
