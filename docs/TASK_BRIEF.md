# TASK BRIEF

Owner: Conductor. Briefs are written here by `/start-phase` and fix rounds. Roles read only their own task.

## Template
```
## <TASK-ID> (phase N)
ROLE: <skill name>          TIER: <Hydrogen (low)|Argon (medium)|Argon (high)>
TASK: <one sentence>
INPUTS: <files and roadmap sections to read>
ALLOWED PATHS: <paths this role may edit>
ACCEPTANCE: <checklist copied from the roadmap phase>
CONSTRAINTS: <only constraints specific to this task>
STARTER PROMPT: Use the <skill> skill. Read AGENTS.md and task <TASK-ID> in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.
```

Task IDs: `P<phase>-<number>`, e.g. `P0-1`. Fix rounds: `P0-1-fix1`.

## Active briefs

### Execution Order (Phase 0)
1. **Parallel Wave 1:** `P0-1` (Backend Engineer — `Argon (high)`) and `P0-2` (DevOps Engineer — `Hydrogen (low)`) run in parallel (disjoint paths).
2. **Sequential Wave 2:** `P0-3` (QA Engineer — `Argon (medium)`) runs after `P0-1` is committed.
3. **Sequential Wave 3:** `P0-4` (Backend Engineer — `Argon (high)`) runs after `P0-3` is committed.
4. **End-of-Phase Step 1 (`P0-5` — Initial QA):** First QA check (after `P0-2` and `P0-4` are committed), write `docs/handoffs/P0-5-qa-engineer.md`, and commit.
5. **End-of-Phase Step 2 (`P0-6` — Docs):** `README.md` and `README.tr.md` updates, write `docs/handoffs/P0-6-docs-portfolio-writer.md`, and commit.
6. **End-of-Phase Step 3 (`P0-5` — Second QA):** Second QA check after `P0-6` is committed; verify the `README.md` / `README.tr.md` CI badge changes, update existing `docs/handoffs/P0-5-qa-engineer.md`, and commit.
7. **End-of-Phase Step 4 (`P0-7` — Reviewer):** Reviewer audit runs after the second `P0-5` check is completed and committed.
8. **Phase Gate:** Conductor runs `/close-phase` and waits for user approval.

---

## P0-1 (phase 0)
ROLE: backend-engineer          TIER: Argon (high)
TASK: Create the solution file, `.editorconfig`, `Directory.Build.props`, and scaffold the five `src/` projects (`Domain`, `Application`, `Infrastructure`, `Contracts`, `Api`) with Clean Architecture project references.
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (§0, §2, §3, FAZ 0 items 1–6 and 8), `docs/DECISIONS.md`
ALLOWED PATHS: `GembaAudit.sln`, `GembaAudit.slnx`, `Directory.Build.props`, `.editorconfig`, `src/GembaAudit.Domain/**` (Phase 0 `.csproj` scaffold only), `src/GembaAudit.Application/**` (Phase 0 `.csproj` scaffold only), `src/GembaAudit.Infrastructure/**`, `src/GembaAudit.Contracts/**`, `src/GembaAudit.Api/**`, `docs/handoffs/P0-1-backend-engineer.md`
ACCEPTANCE:
- [ ] `GembaAudit.slnx` (or `GembaAudit.sln`), `.editorconfig`, and `Directory.Build.props` (`Nullable=enable`, `TreatWarningsAsErrors=true`, `LangVersion=latest`, `ImplicitUsings=enable`) exist at the repository root.
- [ ] `Directory.Build.props` contains `<PropertyGroup>` properties only — zero `ProjectReference` or `PackageReference` items.
- [ ] Five projects exist under `src/` (`GembaAudit.Domain`, `GembaAudit.Application`, `GembaAudit.Infrastructure`, `GembaAudit.Contracts` as `classlib`; `GembaAudit.Api` as `webapi`) and are added to the solution; no project folders exist at the repository root.
- [ ] Layer dependency rule holds: `Domain` has zero `ProjectReference` items; `Application` references `Domain` (and `Contracts` if needed); `Infrastructure` references `Application` and `Domain`; `Api` references `Application`, `Infrastructure`, and `Contracts`.
- [ ] Default template stubs (`Class1.cs`, `WeatherForecast.cs`, `WeatherForecastController.cs` or weatherforecast minimal-API sample) are removed.
- [ ] `dotnet build` passes with 0 warnings and 0 errors, and `dotnet list package --vulnerable --include-transitive` reports 0 vulnerable packages.
CONSTRAINTS: Do NOT create or edit anything under `tests/**` (QA Engineer owns `tests/**`). Do NOT recreate or overwrite existing `.gitignore`, `README.md`, `README.tr.md`, `docs/PROGRESS.md`, or `docs/DECISIONS.md`. Never use `NoWarn`, `NuGetAuditSuppress`, or `WarningsNotAsErrors`.
STARTER PROMPT: Use the backend-engineer skill. Read AGENTS.md and task P0-1 in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.

---

## P0-2 (phase 0)
ROLE: devops-engineer          TIER: Hydrogen (low)
TASK: Create `.github/workflows/ci.yml` to run restore, build, and test with a PostgreSQL 16 service container on push and pull request.
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (§0, §2, §3, FAZ 0 item 7), `docs/DECISIONS.md`
ALLOWED PATHS: `.github/workflows/ci.yml`, `docs/handoffs/P0-2-devops-engineer.md`
ACCEPTANCE:
- [ ] `.github/workflows/ci.yml` triggers on `push` (`main`, `phase/**`) and `pull_request` (`main`).
- [ ] Workflow provisions a pinned PostgreSQL 16 service container with health checks, sets up .NET 10 SDK, and runs `dotnet restore`, `dotnet build --no-restore --configuration Release -warnaserror`, and `dotnet test --no-build --configuration Release`.
- [ ] All GitHub Action versions and container image tags are pinned (no `latest`), and no secrets are hardcoded.
CONSTRAINTS: Edit only `.github/workflows/ci.yml` and your handoff file. Do NOT touch `src/**`, `tests/**`, Dockerfiles (Phase 6), or `README.md`. Since `P0-4` completes the full solution later, note in your handoff that live GitHub Actions execution is NOT VERIFIED locally until the solution and push are complete.
STARTER PROMPT: Use the devops-engineer skill. Read AGENTS.md and task P0-2 in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.

---

## P0-3 (phase 0)
ROLE: qa-engineer          TIER: Argon (medium)
TASK: Create the three xUnit test projects under `tests/` (`GembaAudit.Domain.Tests`, `GembaAudit.Application.Tests`, `GembaAudit.Api.IntegrationTests`) and add their project references to `src/` projects.
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (§0, §2, §3, FAZ 0 items 3, 4, 8), `docs/DECISIONS.md`, `docs/handoffs/P0-1-backend-engineer.md`
ALLOWED PATHS: `tests/GembaAudit.Domain.Tests/**`, `tests/GembaAudit.Application.Tests/**`, `tests/GembaAudit.Api.IntegrationTests/**`, `docs/handoffs/P0-3-qa-engineer.md`
ACCEPTANCE:
- [ ] Three xUnit projects exist under `tests/`: `tests/GembaAudit.Domain.Tests`, `tests/GembaAudit.Application.Tests`, `tests/GembaAudit.Api.IntegrationTests`.
- [ ] Project references configured: `Domain.Tests` → `Domain`; `Application.Tests` → `Application` and `Domain`; `Api.IntegrationTests` → `Api`, `Application`, `Infrastructure`, `Contracts`.
- [ ] Each test project builds with 0 warnings (`dotnet build` on each `.csproj`), `dotnet test` passes on each test project (with a valid smoke/sanity assertion or empty test compilation check), and `dotnet list package --vulnerable --include-transitive` reports 0 vulnerable packages across all three test projects.
CONSTRAINTS: Run only after `P0-1` is committed. Do NOT edit `GembaAudit.slnx` / `GembaAudit.sln`, `Directory.Build.props`, or any file under `src/**` (Backend Engineer will add the test projects to the solution in `P0-4`). Never use `NoWarn` or `NuGetAuditSuppress`.
STARTER PROMPT: Use the qa-engineer skill. Read AGENTS.md and task P0-3 in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.

---

## P0-4 (phase 0)
ROLE: backend-engineer          TIER: Argon (high)
TASK: Add the three test projects under `tests/` to the solution file (`GembaAudit.slnx` / `GembaAudit.sln`) and verify solution-wide build, test, and package vulnerability checks.
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (FAZ 0 items 3, 8), `docs/handoffs/P0-3-qa-engineer.md`
ALLOWED PATHS: `GembaAudit.sln`, `GembaAudit.slnx`, `docs/handoffs/P0-4-backend-engineer.md`
ACCEPTANCE:
- [ ] `dotnet sln list` shows all 8 projects (5 under `src/`, 3 under `tests/`).
- [ ] Solution-wide `dotnet build` and `dotnet test` pass with 0 warnings and 0 errors.
- [ ] Solution-wide `dotnet list package --vulnerable --include-transitive` reports 0 vulnerable packages.
CONSTRAINTS: Run only after `P0-3` is committed. Edit ONLY `GembaAudit.slnx` / `GembaAudit.sln` and your handoff file. Do NOT modify any file under `tests/**`.
STARTER PROMPT: Use the backend-engineer skill. Read AGENTS.md and task P0-4 in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.

---

## P0-5 (phase 0)
ROLE: qa-engineer          TIER: Argon (medium)
TASK: Execute the two-stage Phase 0 QA verification (Stage 1 before `P0-6`: solution, build/test, `.editorconfig`, CI workflow; Stage 2 after `P0-6`: `README.md`/`README.tr.md` badge verification and updating `docs/handoffs/P0-5-qa-engineer.md`).
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (FAZ 0 Tasks & Acceptance Criteria), `README.md`, `README.tr.md`, `.editorconfig`, `.github/workflows/ci.yml`, `docs/handoffs/P0-1-backend-engineer.md`, `docs/handoffs/P0-2-devops-engineer.md`, `docs/handoffs/P0-3-qa-engineer.md`, `docs/handoffs/P0-4-backend-engineer.md`, `docs/handoffs/P0-6-docs-portfolio-writer.md` (in Stage 2)
ALLOWED PATHS: `tests/**`, `docs/handoffs/P0-5-qa-engineer.md`
ACCEPTANCE:
- [ ] **Stage 1 (pre-`P0-6`):** Solution-wide `dotnet build` and `dotnet test` succeed with 0 warnings and 0 errors.
- [ ] **Stage 1 (pre-`P0-6`):** `src/GembaAudit.Domain/GembaAudit.Domain.csproj` contains zero `ProjectReference` items (verified via `Select-String`).
- [ ] **Stage 1 (pre-`P0-6`):** All 8 projects live under `src/` and `tests/`; no project folders exist at the repository root.
- [ ] **Stage 1 (pre-`P0-6`):** `Directory.Build.props` contains properties only (`Select-String` confirms zero `ProjectReference` / `PackageReference`) and `.editorconfig` exists at the repository root with valid C#/.NET formatting rules.
- [ ] **Stage 1 (pre-`P0-6`):** `dotnet list package --vulnerable --include-transitive` reports 0 vulnerable packages across all 8 projects.
- [ ] **Stage 1 (pre-`P0-6`):** `.github/workflows/ci.yml` exists and is verified for: `push` (`main`, `phase/**`) and `pull_request` (`main`) triggers, pinned GitHub Actions versions (no `@latest`), pinned PostgreSQL 16 service container with healthcheck configuration, and `dotnet restore` / `dotnet build` / `dotnet test` steps (live GitHub remote execution is marked `NOT VERIFIED` until pushed).
- [ ] **Stage 2 (post-`P0-6`):** Verify `README.md` and `README.tr.md` after `P0-6` is committed (CI workflow badge added with `YOUR_USERNAME` placeholder URL, no invented repo/username, existing content preserved in sync) and update `docs/handoffs/P0-5-qa-engineer.md`.
CONSTRAINTS: Run in two stages: (1) Initial QA check after `P0-2` and `P0-4` are committed → write `docs/handoffs/P0-5-qa-engineer.md` → Conductor commits; (2) Second QA check after `P0-6` is committed → verify `README` badge changes and update `docs/handoffs/P0-5-qa-engineer.md` → Conductor commits before `P0-7`. Do NOT edit `src/**`, `.github/**`, `README*`, or solution files. Quote exact command outputs in your handoff.
STARTER PROMPT: Use the qa-engineer skill. Read AGENTS.md and task P0-5 in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.

---

## P0-6 (phase 0)
ROLE: docs-portfolio-writer          TIER: Hydrogen (low)
TASK: Add the CI badge (using the `YOUR_USERNAME` placeholder URL) to `README.md` and `README.tr.md` and update the Phase 0 status marker to `🚧` while preserving all existing content.
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (FAZ 0 Acceptance Criteria), `docs/PROGRESS.md`, `README.md`, `README.tr.md`, `docs/handoffs/P0-5-qa-engineer.md`
ALLOWED PATHS: `README.md`, `README.tr.md`, `docs/handoffs/P0-6-docs-portfolio-writer.md`
ACCEPTANCE:
- [ ] GitHub Actions CI badge (`ci.yml`) is added to the badge row in both `README.md` and `README.tr.md` using the `YOUR_USERNAME` placeholder (e.g. `https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml/badge.svg`) — do NOT invent a real GitHub username or repository owner.
- [ ] Existing content, structure, and placeholders (`YOUR_USERNAME`, author name) in `README.md` and `README.tr.md` are preserved in sync.
CONSTRAINTS: Run after `P0-5` Stage 1 (initial QA check) is committed and before `P0-5` Stage 2 (post-Docs QA check). Never invent a real GitHub username or repository URL; keep `YOUR_USERNAME` in the badge URL. Never recreate or overwrite `README.md` or `README.tr.md` from scratch.
STARTER PROMPT: Use the docs-portfolio-writer skill. Read AGENTS.md and task P0-6 in docs/TASK_BRIEF.md. Stay inside the allowed paths. When done run /write-handoff. Do not commit.

---

## P0-7 (phase 0)
ROLE: reviewer          TIER: Argon (high)
TASK: Audit the complete Phase 0 change set on `phase/0-skeleton` (spanning `P0-1` through `P0-6` and both `P0-5` QA stages) against all roadmap acceptance criteria and the Reviewer checklist, and write `docs/reviews/phase-0.md`.
INPUTS: `AGENTS.md`, `GembaAudit-Roadmap.md` (FAZ 0), `docs/PROGRESS.md`, `docs/handoffs/P0-1-backend-engineer.md`, `docs/handoffs/P0-2-devops-engineer.md`, `docs/handoffs/P0-3-qa-engineer.md`, `docs/handoffs/P0-4-backend-engineer.md`, `docs/handoffs/P0-5-qa-engineer.md`, `docs/handoffs/P0-6-docs-portfolio-writer.md`
ALLOWED PATHS: `docs/reviews/phase-0.md`
ACCEPTANCE:
- [ ] Review covers all Phase 0 commits on `phase/0-skeleton` (`P0-1` through `P0-6`, including the `P0-6` documentation commit and the second `P0-5` QA handoff commit), and `git diff --stat main` is quoted to show the total cumulative changes on `phase/0-skeleton` relative to `main`.
- [ ] `docs/reviews/phase-0.md` is created from `docs/reviews/_TEMPLATE.md` with exact quoted outputs of `git diff --stat main`, `dotnet build`, `dotnet test`, and `dotnet list package --vulnerable --include-transitive`.
- [ ] Every Phase 0 acceptance criterion is listed as `PASS`, `FAIL`, or `NOT VERIFIED` with one line of evidence, and an explicit `VERDICT: APPROVED | CHANGES_REQUESTED` is stated.
CONSTRAINTS: Read-only audit of the codebase and project configuration; write only docs/reviews/phase-0.md. Do not modify code, tests, CI, or any other documentation files. Do not commit.
STARTER PROMPT: Use the reviewer skill. Read AGENTS.md and task P0-7 in docs/TASK_BRIEF.md. Run /review-phase. Do not commit.
