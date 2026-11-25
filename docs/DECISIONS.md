# DECISIONS

Owner: Conductor. One row per decision. Roles report new decisions in their handoff file.

| ID | Date | Decision | Reason | Decided by |
|---|---|---|---|---|
| D-001 | 2026-11-16 | Development runs in Google Antigravity; roles are skills in `.agents/skills`, workflows in `.agents/workflows`, handoffs are files in `docs/` | Antigravity has no per-agent model files; file handoffs survive across conversations | User |
| D-002 | 2026-11-16 | Roles use effort tiers Hydrogen (low), Argon (medium), Argon (high); the model is picked in the UI | Keeps role files model-agnostic | User |
| D-003 | 2026-11-16 | Runtime .NET 10 (LTS); `.slnx` solution format accepted | `dotnet new sln` produces `.slnx` on .NET 10 | Conductor |
| D-004 | 2026-11-16 | MediatR pinned to 12.x | Later versions require a commercial license | Roadmap |
| D-005 | 2026-11-16 | NuGet security advisories are fixed by upgrading or pinning, never suppressed | Suppression hides real vulnerabilities | User |
| D-006 | 2026-11-16 | All projects live under `src/` and `tests/`; `Directory.Build.props` holds properties only | Earlier attempt created root-level projects and a props file with project references | User |
| D-007 | 2026-11-25 | Projects target `net9.0` with `GembaAudit.sln` | Local SDK is `.NET 9.0.319` (.NET 10 SDK not installed); per Roadmap §2 fallback to .NET 9 | Backend Engineer (P0-1) |
| D-008 | 2026-11-25 | Migrate all 8 projects from `net9.0` to `net10.0` | `.NET 10.0.404` SDK installed locally; aligns all projects with Roadmap §2 preferred runtime (.NET 10 LTS) | User / Conductor |
