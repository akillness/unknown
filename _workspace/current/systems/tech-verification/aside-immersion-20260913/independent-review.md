---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

> [OBSERVED] 검토 당시 증거. AIM-03 메타 누락 및 AIM-04 금지어 목록 차이는 부모 후속 수정으로 해소했다. AIM-01/02는 OMP 소유 테스트/과거 문서에 남은 비차단 드리프트로 인수 파일에 전달한다. Unity 컴파일/테스트가 통과했다는 의미가 아니다.

# Independent read-only QA — Aside immersion pass (aside-immersion-20260913)

reviewer: independent QA subagent (read-only; no Unity launch, no edits, no commits)
as-of: 2026-09-13 ~16:20 KST, working tree of `/Users/jangyoung/orca/unknown`
caveat: OMP is actively editing this shared worktree (~50 files modified vs HEAD, `.meta.md` files
appeared during this review at 16:20). Line/hash citations below were re-read immediately before writing.

## Verdict

**Scoped ACK with 4 findings (0×S1, 0×S2-blocking, 2×S3, 2×S4).**
Nothing found that requires reverting the change. Two items must be broadcast to OMP, and two
reporting-wording constraints must be honored in the parent's summary.

---

## 1. Scope containment — PASS

Aside touched exactly 3 tracked files + 1 new untracked file:

| path | change | verified |
|---|---|---|
| `Presentation/M5DirectionProfile.cs` | 4 string literals (8 lines: 4−/4+) | `git diff --stat` = 8 changed lines, nothing else |
| `Resources/M5Direction.asset` | 4 fields (9 lines: 5−/4+; `secondCaption` was folded across 2 lines before) | only the 4 copy fields differ |
| `Resources/T0Strings.json` | 1 of 166 entries | see §2 |
| `Tests/EditMode/AsideImmersionContractTests.cs` | new, 47 lines | untracked |

**Independent JSON diff** (parsed both sides, not string-matched):
`keyCount 166→166`, `added: []`, `removed: []`, key order byte-identical,
exactly one changed entry: `caseObjective.ko` `"결손 4시간의 양 끝을 두 기록으로 고정"` →
`"현재 기록을 대조하고 다음 근거를 확인"`. No `en` key added (ko-only shape preserved, pre-existing).

**Receipt integrity — PASS.** Recomputed SHA-256 on disk now matches `implementation-receipt.json`
`afterSha` for all three files, and matches `baseline.json` for all three `immutableDuringOurWrite`
files (`T0GameSession.cs` `92e79cea…`, `T0Interface.cs` `da2e2d6e…`, `campaign.json` `f00deef5…`).
So Aside did not touch OMP-owned shared files, and OMP has not moved them since the baseline either.

**No runtime art promotion — PASS.** `runtimeApproved: 1`, `openingImage` guid `3bd21405…` and
`sectionSurface` guid `83da0b74…` are byte-unchanged. Both new PNGs live under repo-root
`assets/generated/2d/...` and are **not** present anywhere under `unity/` (`find` returned nothing),
so they are not imported, have no `.meta`, and cannot be referenced by any Unity asset.
Both provenance records carry `runtimeEligible:false`, `commercialReleaseEligible:false`,
`promoted_by:null`, `license:"UNVERIFIED"`.

---

## 2. Spoiler boundary — PASS

- Old fallback leaked the t0-b3 task (`결손 4시간`) into the pre-t0-b3 card; the new fallback is generic.
- New opening copy contains no forbidden term (`결손 / 4시간 / 네 시간 / 정전 / 한도연 / 대조의 밤 / 서린 / H-1 / H+3 / 판 #0`), no digits, no count-solution.
- **Canon grounding checked, not assumed.** `폐국 전날 밤` matches
  `_workspace/current/planning/game-draft-v1.md:86` ("폐국 전날 밤 21:00 ~ 05:00 단일 야간") and
  `synopsis`. `목록과 서랍을 대조해` is the authored t0-b1 action in
  `_workspace/current/planning/campaign.json:36`. **No new canon, chapter, character or game-time is introduced.**
- Side note (favourable): the new copy lands on the *correct* side of the still-open defect
  **C6-F18** (`qa/defect-register.md:286` — README says "폐국을 3주 앞둔", canon is D-1).

**Contrast claim — correctly restricted.** I recomputed both figures from the stated hexes and
sRGB relative luminance against `maxLuminance` 0.055517:
`#E7E3D8` → **7.762:1** (artifact says 7.7603) and `#A4AAA5` → **4.205:1** (artifact says 4.2039).
Both reproduce. `source-image-metrics.meta.md` correctly states the muted swatch is **below the
4.5:1 XAG-102 threshold** and forbids muted body text without an independently tested solid backing,
and the JSON carries `runtimeVerified:false` plus an explicit "NO actual UI rendering / resizing /
opacity / color-managed display certification" method note. No over-claim of accessibility.

---

## 3. Spec / default alignment — PASS

All four strings are identical in three places: the decision doc's "Applied copy contract" table,
the C# field initializers, and the serialized `.asset`. The new test enforces both sides of the
`.asset` ↔ fresh-`CreateInstance` counter, which is the specific drift this pass was meant to close.
`runtimeApproved` is asserted only on the persisted asset (correct — the C# default is `false` by
design), so the test does not falsely claim default parity there.

Cut durations preserved: `firstShotSeconds 3.125`, `secondShotSeconds 2.875`, plus
`observeTitle / trialTitle / recordTitle / recordPreparation` untouched.
No other file hardcodes the old copy (`M5DirectionProjectBuilder.cs` reads/creates the profile but
does not author these strings), so there is **no regeneration/revert path**.

---

## 4. Native verification — evidence gap, honestly documented

`editmode.exit` = **198**; `editmode.log` ends with
`[Licensing::Module] Error: 'com.unity.editor.headless' was not found.` /
`No valid Unity Editor license found. Please activate your license.`
`test-runner.log` is **0 bytes**; **no XML results exist anywhere**.

This is a *launch* failure, not a test verdict — Unity never reached compilation. Consequences the
parent must respect in its report:

- **No native pass may be asserted**, for the new test or for any existing suite.
- **`AsideImmersionContractTests.cs` has never been compiled or executed.** Its compile-correctness
  is reasoned-about only (see §5), not proven.
- No claim of "no PlayMode regression" is available from this packet.

`native-verification.meta.md` already states exactly this ("failure receipts, not a test verdict…
Do not reuse other OMP runs as verification of these source hashes"), and `green.json` /
`red.json` / `verify-immersion.meta.md` all scope themselves to "static source/data contract and a
labeled JavaScript guard model only; NOT Unity compilation". **The written evidence is honest** —
this section is a constraint on the parent's prose, not a defect in the artifacts.

---

## 5. New test — compile reasoning and honesty

Compile prerequisites check out against `Tide.Tests.Sim.asmdef`:
`includePlatforms:["Editor"]` (so `UnityEditor.SerializedObject` is legal),
`precompiledReferences:["Newtonsoft.Json.dll"]` (so `Newtonsoft.Json.Linq` resolves),
`references` include `Tide.App` (so `T0GameSession` resolves),
`defineConstraints:["TIDE_TEST_FRAMEWORK"]` matching the file's `#if` guard.
`Tide.Presentation` is **not** referenced — but the test deliberately loads via
`Resources.Load<ScriptableObject>` + `asset.GetType()` and reads fields through `SerializedObject`,
so it never names `M5DirectionProfile` and needs no such reference. That is correct, not accidental.
`Assert.AreEqual(3.125f, …)` without a delta is safe: both values are exactly representable in binary32.

`T0GameSession.CaseObjective(JObject, string, IEnumerable<string>, string)` exists at
`App/T0GameSession.cs:231` and is `public static` with the 4-arg signature the test uses. Guard body:
`if(string.IsNullOrEmpty(objective) || recordNames.Any(objective.Contains)) return fallback;`
— so the test's "missing rows" and "objective containing a record display name" cases both
legitimately reach the fallback, and the third test's non-naming objective legitimately wins.

**Honesty: acceptable, with two wording nits (S4, below).** The test does not claim runtime,
readability or immersion, and its constant `SafeObjective` is checked against the actual resource.

---

## Findings

| id | sev | lane | finding | repro / evidence | recommended action |
|---|---|---|---|---|---|
| AIM-01 | **S3** | OMP (broadcast) | `Tests/PlayMode/T0CaseThreadTests.cs:208` `default: return "결손 4시간의 양 끝을 두 기록으로 고정";` still hardcodes the **old** fallback string that Aside changed. **Currently unreachable** — the switch covers t0-b1/b2/b3 and the selector `??"t0-b3"` guarantees one of the three, and all three beats' authored objectives pass the guard (verified: no record display name in any of `인수 각서/이관 목록/당직실 표준판/근무 규정 필사본/기록국 조위대장`). So **no live regression**. It is a latent landmine: the moment any beat objective is emptied or names a record, runtime shows the new fallback and this OMP-owned test fails with a misleading message. | read `T0CaseThreadTests.cs:203-210`; evaluated `Data/Tables/beats.json` × `records.json` | Broadcast to OMP. Aside must not edit this shared file. |
| AIM-02 | **S3** | OMP (broadcast) | Doc drift around the same value. `_workspace/current/planning/objective-copy-m13.md:30` still asserts the player sees `결손 4시간의 양 끝을 두 기록으로 고정` at t0-b1, and `:97` prescribes leaving `default:` as "the fallback text". Both are now stale — partly pre-existing (OMP already gave t0-b1 a guard-passing objective) and partly from this pass (the fallback value changed). Also the stale comment at `T0CaseThreadTests.cs:200-202` ("t0-b1's authored objective names records, so the disclosure guard must fall back") no longer describes the data. | grep of the old literal across `unity/` + `_workspace/` | Broadcast to OMP as one item together with AIM-01. Not Aside's to fix. |
| AIM-03 | S4 | Aside | `AsideImmersionContractTests.cs` has **no sibling `.meta`**, while every other test file — including OMP's brand-new untracked PlayMode tests — has one. Unity will mint one with a fresh GUID on next import, surfacing a surprise untracked file inside the project OMP is actively running. | `ls Tests/EditMode/*.meta` | Either add the `.meta` or explicitly note the expected untracked file in the handoff. |
| AIM-04 | S4 | Aside | Two small honesty/consistency nits in the new test. (a) In `MissingAndRedactedEarlyObjectivesUseSafeResourceFallback` the `StringAssert.DoesNotContain` spoiler loop runs on `actual` **after** `actual` was already asserted equal to the hardcoded `SafeObjective`, so it constrains the constant rather than demonstrating any guard behaviour — it must not be described as proof that the system blocks spoilers. (b) The C# spoiler list has 8 terms but `verify-immersion.mjs` `forbidden` has 10 — the C# list omits `판 #0` and `네 시간`, so the native contract is weaker than the JS model. Also "Redacted" in the test name is loose: the guard fires on record-name containment, not redaction. | `AsideImmersionContractTests.cs:31-39` vs `verify-immersion.mjs:15` | Align the two term lists; describe the change as a *safer fallback string*, not a spoiler guard. |

## Reporting constraints for the parent

1. Do **not** state or imply that native/EditMode/PlayMode tests pass. The only native receipt is a
   licence failure (exit 198, no XML). Say "native verification blocked; new test not yet compiled".
2. Do **not** restate `implementation-receipt.json`'s `sharedFilesUnchangedDuringThisWrite` as
   "shared files are unmodified". Those three files are byte-identical to the Aside baseline, but
   ~50 other files in the tree are modified by OMP. The receipt's during-window wording is correct;
   a vs-HEAD paraphrase would be false.
3. Contrast: report **7.76:1 for `#E7E3D8` paper only**, with the explicit companion that
   `#A4AAA5` muted is **4.20:1 and fails the 4.5:1 threshold**. Both are source-pixel measurements
   on the original PNG, not in-engine, composited or certified.
4. The two generated images remain candidates: the opening concept is `revision-required`
   (parent rejected: composition / closed books / reading field not achieved), the nav texture is
   `visual-candidate-reviewed` only. Neither is runtime or commercially cleared.
5. `decision-and-implementation.md` frontmatter is `status: draft` while all of its
   tech-verification sidecars are `status: current`. Consistent with "pending approval", but the
   parent should not present the decision doc as a current approved spec.
