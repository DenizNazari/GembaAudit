# Review - Phase <N>

Reviewer: <date>   Branch: <phase/N-slug>

```
VERDICT: APPROVED | CHANGES_REQUESTED
BUILD: pass|fail   TESTS: <passed>/<total>   WARNINGS: <count>
```

## Acceptance criteria
| Criterion | PASS / FAIL / NOT VERIFIED | Evidence |
|---|---|---|

## Findings
- [BLOCKER|MAJOR|MINOR] `<path>:<line>` - <problem> - <fix owner role>

BLOCKER = security hole, broken dependency rule, failing build/tests, data loss risk, work reported complete but not implemented. Any BLOCKER or unresolved MAJOR means CHANGES_REQUESTED. At most 5 MINOR items. No praise padding.

## Commands run
```text
<exact commands and output used as evidence>
```
