# AGENTS.md — GembaAudit

Read this file first. Then read `GembaAudit-Roadmap.md` (source of truth for scope, architecture and phases), `docs/PROGRESS.md` (status) and `docs/DECISIONS.md`.

## Project
GembaAudit: 5S / occupational-safety field audit platform. .NET 10, Clean Architecture, PostgreSQL, Blazor Web + MAUI Blazor Hybrid, offline sync, AI layer. Phases 0-10 are defined in the roadmap and run strictly in order. No phase has been implemented yet; treat any existing stub, template or placeholder code as NOT implemented.

## Environment
- Windows with Google Antigravity. The terminal is PowerShell. Use PowerShell syntax or `dotnet`/`git` directly. Do not use Unix-only tools (`grep`, `head`, `cat`, `find`); use `Select-String`, `Get-Content`, `Get-ChildItem`. Never wrap commands in `cmd /c`.
- Roles are skills in `.agents/skills/<role>/SKILL.md`. Slash-command workflows are in `.agents/workflows/`: `/start-phase`, `/write-handoff`, `/commit-task`, `/review-phase`, `/close-phase`.
- Human guide: `docs/WORKFLOW-GUIDE.md`.

## Operating model
- One role per conversation. The Conductor conversation plans, verifies, commits and records. Other roles implement inside their own paths.
- Work passes between conversations through files: `docs/TASK_BRIEF.md` (in), `docs/handoffs/` (out), `docs/reviews/` (Reviewer), `docs/PROGRESS.md` and `docs/DECISIONS.md` (Conductor only).
- Run conversations in parallel only when their allowed paths are disjoint.
- The user decides at phase gates. Commits on the phase branch need no user approval.

## Tiers (effort levels)
The files state a tier, never a model. The user picks the model and reasoning level in the conversation's model selector.

| Tier | Level | Pick in the model selector | Roles |
|---|---|---|---|
| Hydrogen | low | fastest/cheapest option, lowest reasoning | DevOps Engineer, Docs & Portfolio Writer |
| Argon | medium | balanced option, medium reasoning | Frontend Engineer, QA Engineer |
| Argon | high | strongest option, highest reasoning | Conductor, Domain Architect, Backend Engineer, Mobile & Sync Engineer, AI Engineer, Reviewer |

Escalation (if a role fails the same task twice — build/test failure or a Reviewer BLOCKER):
- `Hydrogen (low)` fails twice → re-brief at `Argon (medium)`.
- `Argon (medium)` fails twice → re-brief at `Argon (high)`.
- `Argon (high)` fails twice → escalate to the user.

## Ownership
| Role | May edit |
|---|---|
| Conductor | `docs/PROGRESS.md`, `docs/DECISIONS.md`, `docs/TASK_BRIEF.md`, all git write operations |
| Domain Architect | `src/GembaAudit.Domain`, `src/GembaAudit.Application` (except Sync, Ai) |
| Backend Engineer | solution file (`GembaAudit.sln`, `GembaAudit.slnx`), `Directory.Build.props`, `.editorconfig`, `Infrastructure` (except Ai), `Api` (except Ai/Sync/Hubs), `Contracts` |
| Frontend Engineer | `UI.Shared`, `Web` |
| Mobile & Sync Engineer | `Mobile`, `UI.Shared/Platform`, `UI.Shared/Sync`, `Application/Sync`, `Api/Controllers/SyncController.cs`, `Api/Hubs` |
| DevOps Engineer | `.github`, Dockerfiles, compose, `.env.example`, `docs/DEPLOY.md` |
| QA Engineer | `tests/**` |
| AI Engineer | `Infrastructure/Ai`, `Application/Ai`, `AiController.cs`, `ml/` |
| Reviewer | `docs/reviews/**` only |
| Docs & Portfolio Writer | `README.md`, `README.tr.md`, `docs/ARCHITECTURE.md`, `docs/images`, `CHANGELOG.md`, `LICENSE` |

Every role may also write its own files in `docs/handoffs/`. Editing outside your ownership is a process violation the Reviewer reports.

## Global rules
1. Follow roadmap section 0. A phase closes only when its acceptance criteria pass and the user approves.
2. Dependency rule: `Domain <- Application <- Infrastructure <- Api`. Domain references nothing.
3. Build must be warning-free (`TreatWarningsAsErrors`); `dotnet build` and `dotnet test` must pass before any handoff.
4. Never suppress warnings or NuGet advisories (`NoWarn`, `#pragma warning disable`, `NuGetAuditSuppress`, `WarningsNotAsErrors`) without Reviewer approval. Security advisories (NU1901-NU1904) are fixed by upgrading or pinning a patched version.
5. `Directory.Build.props` holds properties only, never `ProjectReference` or `PackageReference`.
6. Layout: all projects live under `src/` or `tests/`; no project folders at the repository root.
7. Never commit secrets. Only `.env.example` is tracked. Keys come from environment variables.
8. Do not make unrelated changes (no drive-by package bumps or refactors). Do not guess on ambiguity: pick the simplest option and report it under Decisions in your handoff, or stop and ask if it affects architecture.
9. Git: only the Conductor commits (see its skill): per task, on `phase/<N>-<slug>`, path-scoped staging, Conventional Commits. Merge to `main` only after Reviewer APPROVED and user approval.
10. Be honest in reports: state what was not tested or not verified.
11. Code identifiers, commit messages and README are English.

## Evidence Rule
- Report a task or phase "complete" only if every acceptance criterion is verified by a command or file inspection whose output you quote. Otherwise write NOT VERIFIED.
- Never report counts (tests, coverage, files, warnings) you did not read from tool output in this session.
- Stubs, empty classes, the default weather-forecast template and placeholder tests count as NOT implemented. "Planned" is not done.
- Only the Reviewer writes review findings and verdicts. The Conductor never writes audit findings itself; if the Reviewer did not run, say so and stop.
- Phase reports list each acceptance criterion as PASS / FAIL / NOT VERIFIED with one evidence line.
- Correct `docs/PROGRESS.md` immediately after any review result.

## Communication Efficiency

- Keep internal communication concise.
- Avoid repeating task descriptions.
- Do not explain obvious implementation details.
- Never omit technical constraints for brevity.
- Preserve complete error messages when debugging.
- Report only changed files, decisions, blockers and test results.
- Do not repeat information already present in PROGRESS.md.
- Never sacrifice code quality or correctness for shorter output.

### Terse-style skills (e.g. caveman)
If a brevity/terse-style skill is installed, apply it to conversational prose only. Never apply it to source code, needed comments, commit messages, error messages, exact commands, README and architecture docs, handoff files, review findings, or any constraint. Correctness and completeness outrank brevity.
