# Handoff P0-6 - Docs & Portfolio Writer

Phase: 0   Date: 2026-11-25   Tier used: Hydrogen (low)

## Changed files
- `README.md`
- `README.tr.md`
- `docs/handoffs/P0-6-docs-portfolio-writer.md`

## Decisions (new only, or "none")
- none

## Acceptance criteria
| Criterion | PASS / FAIL / NOT VERIFIED | Evidence (quoted command output or file reference) |
|---|---|---|
| GitHub Actions CI badge (`ci.yml`) is added to the badge row in both `README.md` and `README.tr.md` using the `YOUR_USERNAME` placeholder — do NOT invent a real GitHub username or repository owner. | PASS | `git diff -- README.md README.tr.md` shows `+[![CI](https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml)` added at line 10 of both `README.md` and `README.tr.md`. Live remote badge rendering on GitHub is NOT VERIFIED until pushed to a real repository. |
| Update the Phase 0 status marker to `🚧` in both `README.md` and `README.tr.md`. | PASS | `git diff -- README.md README.tr.md` shows line 200 updated from `| 0 | Solution skeleton, CI | 🗓️ |` to `| 0 | Solution skeleton, CI | 🚧 |` in `README.md`, and from `| 0 | Çözüm iskeleti, CI | 🗓️ |` to `| 0 | Çözüm iskeleti, CI | 🚧 |` in `README.tr.md`. |
| Existing content, structure, wording, formatting, and placeholders (`YOUR_USERNAME`, author name) in `README.md` and `README.tr.md` are preserved in sync. | PASS | `git diff -- README.md README.tr.md` shows only `+1/-0` (badge) and `+1/-1` (Phase 0 marker) per file; `Select-String -Path README.md,README.tr.md -Pattern "ci\.yml\|YOUR_USERNAME\|YOUR NAME\|ADINIZ SOYADINIZ\|🚧"` confirms `YOUR_USERNAME`, `YOUR NAME`, `YOUR_PROFILE`, `ADINIZ SOYADINIZ`, and `PROFIL_ADINIZ` remain intact. |

## Build and test output
```text
Docs-only changes (`README.md`, `README.tr.md`); no `.cs` or `.csproj` files were modified, so `dotnet build` and `dotnet test` were not re-run in this task (see `docs/handoffs/P0-5-qa-engineer.md` and `docs/PROGRESS.md` for Phase 0 build/test status).

> git status --short
 M README.md
 M README.tr.md

> git diff -- README.md README.tr.md
diff --git a/README.md b/README.md
index ada7900..6693073 100644
--- a/README.md
+++ b/README.md
@@ -7,6 +7,7 @@
 **An offline-first, cross-platform 5S & occupational-safety audit platform, built with .NET 10, Clean Architecture and an AI layer that turns a site photo into an actionable finding.**
 
 ![Status](https://img.shields.io/badge/status-planning-lightgrey)
+[![CI](https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml)
 ![.NET](https://img.shields.io/badge/.NET-10-512BD4)
 ![Blazor](https://img.shields.io/badge/Blazor-Web%20%2B%20MAUI%20Hybrid-5C2D91)
 ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-pgvector-336791)
@@ -196,7 +197,7 @@ GembaAudit/
 
 | Phase | Focus | Status |
 |---|---|---|
-| 0 | Solution skeleton, CI | 🗓️ |
+| 0 | Solution skeleton, CI | 🚧 |
 | 1 | Domain model, state machines, scoring | 🗓️ |
 | 2 | Application layer (CQRS, validation) | 🗓️ |
 | 3 | Persistence (EF Core, Dapper, migrations, seed) | 🗓️ |
diff --git a/README.tr.md b/README.tr.md
index b2cd27c..fdce3a1 100644
--- a/README.tr.md
+++ b/README.tr.md
@@ -7,6 +7,7 @@
 **.NET 10, Clean Architecture ve saha fotoğrafını uygulanabilir bir uygunsuzluk kaydına çeviren bir yapay zekâ katmanıyla geliştirilen, çevrimdışı çalışabilen, çok platformlu 5S ve İş Sağlığı Güvenliği (İSG) denetim platformu.**
 
 ![Durum](https://img.shields.io/badge/durum-planlama-lightgrey)
+[![CI](https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml)
 ![.NET](https://img.shields.io/badge/.NET-10-512BD4)
 ![Blazor](https://img.shields.io/badge/Blazor-Web%20%2B%20MAUI%20Hybrid-5C2D91)
 ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-pgvector-336791)
@@ -196,7 +197,7 @@ GembaAudit/
 
 | Faz | Odak | Durum |
 |---|---|---|
-| 0 | Çözüm iskeleti, CI | 🗓️ |
+| 0 | Çözüm iskeleti, CI | 🚧 |
 | 1 | Domain modeli, durum makineleri, puanlama | 🗓️ |
 | 2 | Application katmanı (CQRS, doğrulama) | 🗓️ |
 | 3 | Kalıcılık (EF Core, Dapper, migration, seed) | 🗓️ |
```

## Blockers
none

## Not done / not verified
- `dotnet build` and `dotnet test` were not re-run in this docs-only task.
- Live GitHub Actions CI badge rendering (`https://github.com/YOUR_USERNAME/GembaAudit/actions/workflows/ci.yml/badge.svg`) is NOT VERIFIED until pushed to a real GitHub repository with a configured username.

## Next
- Conductor commits `README.md`, `README.tr.md`, and `docs/handoffs/P0-6-docs-portfolio-writer.md`.
- QA Engineer runs `P0-5` Stage 2 (post-`P0-6` README badge verification) and updates `docs/handoffs/P0-5-qa-engineer.md`.
- Reviewer audits Phase 0 (`P0-7`).
