---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M17 explicit intro replay is dead under reduced motion

> **What this document is**: one adversarial review pass over the playable T0/C1 runtime plus
> exactly one smallest player-visible correction, verified by automated Unity tests and a player
> build. It is **not** a human playtest, **not** a native capture, **not** a performance
> measurement, and **not** a G4–G7 promotion.

In-place update inside the existing cycle (RFC-Q2: `cycle` unchanged → `supersedes: null`, no
archive obligation).

## 0. Conclusion

[OBSERVED] One P2 defect confirmed and corrected: for a player who has enabled **모션 축소**
(reduced motion), the settings action **도입 안내 다시 보기** (replay the intro guidance) was a
silent no-op. The onboarding guidance — including the core-loop verbs 관찰 · 시험 · 기록 — was
unreachable for exactly the players who use the accessibility setting.

| Step | Result | Receipt |
|---|---|---|
| Focused RED | 1 case, 0 passed, **1 failed** on the defect assertion | `red.xml`, `red.log` |
| Focused GREEN | 1 case, **1 passed**, 0 failed | `green.xml`, `green.log` |
| EditMode | 54 total, 54 passed, 0 failed | `editmode.xml`, `editmode.log` |
| PlayMode | 85 total, 84 passed, 0 failed, 1 skipped | `playmode.xml`, `playmode.log` |
| macOS player build | `T0_MAC_BUILD Succeeded bytes=403838145` | `build-mac.log:6012` |
| M14/M15/M16 preservation | 67/67 baselined files byte-identical, HEAD unchanged | §6 |

## 1. Defect found (P2 — onboarding/disclosure + settings accessibility)

### 1.1 The authored contract

Two owned artifacts define the behaviour, and they agree:

- `presentation/intro-gameplay-m5.json` → `acceptance`/presentation block:
  - `"reducedMotion": "one static frame, all three verb labels immediately visible, explicit continue, no timed wait"`
  - `"replayExplicitOnly": true`
- `systems/system-specs/intro-gameplay-m5.md` §"C1 operation orientation" (line 29):
  - "Settings offers explicit replay while no save is pending. Replay preserves underlying
    document/tool and **returns to the prior settings overlay**."
- `presentation/intro-gameplay-m5.md` (line 83–84): "다시 보기만 명시적 입력으로 허용한다 …
  저감모션은 이미지 하나와 세 라벨 전체를 바로 보여주고 **시간을 기다리지 않는 명시적 시작**을 사용한다."

So the reduced-motion presentation is a *defined, authored screen* — one static frame, all three
verb labels, explicit continue — and replay is a *defined, explicit player request*.

### 1.2 The code path

[OBSERVED] `App/T0OpeningSession.cs` before this increment:

```csharp
void BeginOpening(){
    OpeningActive=true;openingElapsed=0;openingCaption=0;
    if(ReducedMotion){FinishOpening();return;}   // ← unconditional
    overlay=null;Render();
}
void ReplayOpening(){if(!DirectionEnabled||SavePending)return;openingReplay=true;openingReturnOverlay=overlay;BeginOpening();}
void FinishOpening(){
    OpeningActive=false;
    if(openingReplay){openingReplay=false;overlay=openingReturnOverlay;openingReturnOverlay=null;Render();return;}
    openingFinished=true;StartGame();
}
```

`BeginOpening` auto-finished on reduced motion **regardless of why it was entered**. On the replay
route that collapses to: `openingReplay=true` → `BeginOpening()` → `FinishOpening()` → restore
`overlay="settings"` → `Render()`. Net observable effect: **nothing**. The overlay the player was
already looking at is redrawn.

The static reduced-motion frame that `OpeningScreen` is written to produce —

```csharp
screen.Body=ReducedMotion?directionProfile.firstCaption+"\n\n"+directionProfile.secondTitle+"\n"+directionProfile.secondCaption : …
screen.Actions.Add(A("intro-skip",openingReplay?L("openingReturn"):ReducedMotion?L("openingBeginWork"):L("openingSkip"),FinishOpening));
```

— was therefore unreachable by the one control authored to reach it.

### 1.3 Player-visible symptom and reachability

[OBSERVED] Reachable in the **shipped** build with no diagnostic flag:
`Resources/M5Direction.asset` carries `runtimeApproved: 1`, so
`DirectionEnabled => directionProfile.runtimeApproved || --m5-direction-diagnostic` is true from
approval alone. The action is added in `T0GameSession.Render` →
`OverlayScreen` (`T0GameSession.cs:319`) whenever `DirectionEnabled && !OpeningActive`, enabled
while `!SavePending`.

Repro, entirely inside default UI:
1. Title screen → **접근성 · 설정**
2. Activate **모션 축소** (label reads `모션 축소: True`)
3. Activate **도입 안내 다시 보기**

Expected: the one static guidance frame with an explicit continue.
Actual: the settings overlay redraws; no guidance ever appears.

Severity rationale (P2, not P1): no data loss, no progress blocker, no save corruption — so not
P1. Above P3 because (a) it is an **enabled, focusable control that produces no response**, which
is an interaction-clarity defect on its own; (b) the content it gates is **onboarding disclosure**
— the 관찰 · 시험 · 기록 core-loop verbs; and (c) the loss falls **only** on players using an
accessibility affordance, while a default-motion player can always replay. Reduced-motion players
also skip the intro at fresh start (verified behaviour, §2), so replay was their *only* route to
this guidance — and it was dead.

## 2. Checked and closed in the same review (NOT defects)

Recorded so aesthetic speculation is not promoted to defect status:

- **Reduced motion skipping the intro at fresh start is correct, not a bug.** It is the verified
  contract, asserted by `M5DirectionPlayModeTests.ReducedMotionStartsFreshGameImmediatelyWithoutWrites`
  and recorded in `tech-verification/intro-gameplay-m5.md` ("reduced motion immediate continue").
  This increment deliberately leaves it untouched and that test still passes.
- **M16's native post-entry capture gap is not a game-code defect.** Per the M16 report it is
  macOS TCC Accessibility denial (`accessibility: not-granted`); synthetic input was never
  delivered. No bypass was attempted and no capture is claimed here.
- **`select-slot-{n}` not setting `started`** returns the player to the title screen with the
  selected journal restored; that matches the recovery flow and loses nothing. No change.
- **`AtomicSaveStore.Load` has two unreachable guards** (`version>3` and `version<0` re-tested
  after the combined precondition already returned). Dead code only — no reachable player-visible
  behaviour differs. Not corrected in this increment; correctness-neutral hygiene.
- **`Tool` "next" dereferences `Gamepad.current`** (`WatchInput.cs`). Re-examined; as in M15 no
  reachable keyboard-only null path was demonstrated, because the binding is `<Gamepad>/rightShoulder`
  and `Rebind` pins the rebind to `<Gamepad>`. Left unchanged — **no test, no claim**.

## 3. Change (exact paths)

| Path | Change |
|---|---|
| `unity/Unknown/Assets/_Project/App/T0OpeningSession.cs` | +5 / −1. One condition: `if(ReducedMotion)` → `if(ReducedMotion&&!openingReplay)`, plus a 4-line rationale comment. sha256 `3f062548fdbf689a93878c2b3d5237990abcbb9b9bfd6f61a83545a271e4efda` |
| `unity/Unknown/Assets/_Project/Tests/PlayMode/T0IntroReplayReducedMotionTests.cs` | New focused PlayMode test (1 case). sha256 `ec2d9923a5a7e8f1ec099c881cc5689b80e535cd32319d5b357940d1838c9e3a` |
| `unity/Unknown/Assets/_Project/Tests/PlayMode/T0IntroReplayReducedMotionTests.cs.meta` | New, generated by Unity on import |
| `_workspace/current/systems/tech-verification/m17-unity-polish/` | This report and the run receipts |

Exact functional diff:

```diff
-            if(ReducedMotion){FinishOpening();return;}
+            if(ReducedMotion&&!openingReplay){FinishOpening();return;}
```

Why this is the smallest correct fix: `openingReplay` is already the existing, exact discriminator
between the two entry routes — `BeginOpeningOrStart` sets it `false`, `ReplayOpening` sets it
`true`. No new state, no new string, no data change. The reduced-motion screen, its
`openingReturn` label, the `UpdateOpening` clock suppression and the `FinishOpening` return-to-
overlay path all already existed and are now simply reachable.

[OBSERVED] Invariants held: no save-data field renamed or removed (CLAUDE.md §9 migration
invariant does not apply — this path performs no write at all); no balance/economy number touched;
no simulation state written by presentation code; no asset promoted or generated; no runtime
approval altered.

## 4. TDD evidence

Runner: `Unity 6000.5.6f1 -batchmode -nographics -runTests`, filter
`Tide.Tests.T0IntroReplayReducedMotionTests`.

**RED was meaningful, not a setup error.** The failure is the defect assertion at
`T0IntroReplayReducedMotionTests.cs:48`:

```
explicit replay under reduced motion must present the opening guidance
  Expected: True
  But was:  False
```

[OBSERVED] Every assertion *before* it passed, which is what proves the harness reached the real
control rather than failing to arrive:
- `game.Surface == "settings"` — the accessibility overlay was reached
- `Text()` contains `모션 축소: True` — reduced motion was genuinely enabled, not assumed
- `ActionIds` contains `intro-replay` — the control was actually offered to the player
- `Click("intro-replay")` succeeded — `Interface.Focus` found it **enabled and focusable**, and
  `BeginActivation`/`EndActivation` fired its handler

So the control existed, was enabled, was successfully activated, and produced no guidance.

The test asserts player-visible outcomes, not just a flag: the `Opening black` frame is present in
the hierarchy, all three verb labels 관찰/시험/기록 are in rendered `Text`, an explicit continue
action exists, `OpeningElapsed` does not advance across 0.2 s of real time (the "no timed wait"
clause), explicit continue returns `Surface` to `settings`, and the journal hash/head are unchanged
with no `save.json` written.

## 5. Full regression

| Suite | This run | M15 baseline | Delta |
|---|---|---|---|
| EditMode | 54 total, 54 passed, 0 failed | 54 / 54 | **0** — case sets identical |
| PlayMode | 85 total, 84 passed, 0 failed, 1 skipped | 84 total, 83 passed, 0 failed, 1 skipped | +1 case = this increment's test |

[OBSERVED] Case-by-case set diff against M15's XML: the only added PlayMode case is
`Tide.Tests.T0IntroReplayReducedMotionTests.ExplicitIntroReplayUnderReducedMotionPresentsStaticGuidanceAndReturnsWithoutWrites`.
**No case present in M15 is missing here**, and the EditMode set is byte-identical at 54/54
(including the third-party `AddressableAssets.DocExampleCode.TestStub.RequiredTest` that M15
attributed to package-resolution state on this machine).

[OBSERVED] The single skipped PlayMode case,
`T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`, is skipped in the M14
and M15 baselines too. Pre-existing, unchanged, not introduced here.

[OBSERVED] **All 15 `M5DirectionPlayModeTests` cases pass**, including the four that specifically
pin the behaviour this change could have broken:
`ReducedMotionStartsFreshGameImmediatelyWithoutWrites` (fresh reduced-motion start still immediate),
`SettingsPauseAndReducedMotionShowAllLabelsAt150Percent` (mid-opening toggle still renders the
static frame and fits at 150%), `ReplayFromToolKeepsKeyboardNavigationAndReturnContext` and
`LoadedSaveBypassesIntroAndExplicitReplayPreservesBytesAndContext` (default-motion replay context
and save bytes preserved).

## 6. Scope compliance — M14/M15/M16 preservation

[OBSERVED] Before the first write this session, all 39 `git status --porcelain` entries were
expanded to **67 files** and sha256-baselined. Re-verified after the test runs **and** again after
the build:

| Check | Result |
|---|---|
| HEAD | `52ad5510522eca6d5cfd8d743d85f28cb254635f` **unchanged** |
| Deleted baselined files | **0** |
| Content-changed baselined files | **0** (67/67 byte-identical) |
| M14/M15/M16-owned files among them | 33 tracked, **0 violations** |
| `graphify-out/*` (4 files) | **unchanged** — not regenerated, not hand-edited |

[OBSERVED] M15's `App/T0GameSession.cs` edit and its `T0SettingsSlotPersistenceTests.cs`(+`.meta`),
M14's `campaign.json` / `systems/data/t0/*` / `Data/Tables/*` / `Data/Authoring/*` / `M9CoreTests.cs`
/ `T0CaseThreadTests.cs`, and all three `m14-objective/`, `m15-settings-slot/`,
`m16-runtime-capture/` evidence directories are present and byte-identical. My correction is
disjoint from their surfaces: M14 changed authored objective data and generated tables, M15 changed
one settings **path** in `T0GameSession`, and this increment changed one **condition** in
`T0OpeningSession`. No shared file.

[OBSERVED] No `reset`, `stash`, `revert`, `clean`, `checkout`, `commit` or `push` was run. No asset
was generated or purchased, no runtime approval flag was altered, no graph file was modified, and
no pre-existing evidence was edited. `BuildMac` does not call `Prepare()`
(`T0ProjectBuilder.cs:71`), so the build did not regenerate scenes or assets; `Builds/`, `Library/`,
`Temp/` are gitignored.

## 7. Build receipt

[OBSERVED] Gate measured **before** building, per the standing rule:

| Item | Value | Verdict |
|---|---|---|
| Free disk before build | **11.34 GiB** (11,886,700 KiB on `/System/Volumes/Data`) | ≥ 10 GiB → build allowed |
| Unity Editor processes | **none** (`Unity.app` / `Unity Hub` / `UnityShaderCompiler`) | allowed |
| `Temp/UnityLockfile` | **absent** | allowed |
| Free disk after build | 11.32 GiB | — |

| Item | Value |
|---|---|
| Method | `Tide.EditorTools.T0ProjectBuilder.BuildMac` |
| Options | `BuildOptions.Development` — development build, not release |
| Result | `T0_MAC_BUILD Succeeded bytes=403838145` (`build-mac.log:6012`) |
| Measured on disk | **403,838,145 bytes across 315 files — exact match to the reported total** |
| Main binary | `Contents/MacOS/Unknown T0`, 67,916 bytes |
| Main binary sha256 | `01248d9c6dbd7517c3a687d781cee5a3a8ed830bb9471c5eba8036b4eae55fb9` |

[OBSERVED] The build proves the change compiles and links into a standalone player. It is **not** a
run, **not** a capture, and **not** a performance result. The player was **not launched** this
increment.

## 8. Solution / no-spoiler boundary

[OBSERVED] Independently re-verified rather than assumed. The text this correction makes reachable
is exactly the text a default-motion player already sees, so the disclosure surface does not grow.
Programmatic check of every caption field in `Resources/M5Direction.asset` plus
`openingMotto`/`openingMottoDetail` from `Resources/T0Strings.json`, against all five
`records.json` `displayNameKo` values (`인수 각서`, `이관 목록`, `당직실 표준판`, `근무 규정 필사본`,
`기록국 조위대장`):

| Prohibited content | Result |
|---|---|
| Record / source display names | **none present** |
| Exact values (any digit) | **none present** |
| Action sequences or solution paths | none — the text is generic verb framing (`관찰 · 시험 · 기록`, "원본을 펼쳐 내용을 살펴봅니다.") |

[OBSERVED] `CaseObjective`'s record-name disclosure guard, the `KeptClueLines` record-id grouping
and the hint `warnsBeforeReveal` spoiler gate were **not touched**; their EditMode/PlayMode
coverage is unchanged and still passing. The replay path submits no `PuzzleCommand`, reveals no
hint level and writes no save, so it cannot advance or leak puzzle state.

## 9. Limits — explicitly not claimed

1. **Automated Unity test verification plus a compile/link build only.** No human playtest
   (n=0), no native player run, no synthetic-input capture, no screenshots.
2. **No performance claim.** No frame time, memory, GPU or thermal measurement.
3. **No physical input claim.** Zero real keyboard/mouse/controller events; input equivalence
   remains the PlayMode tests' scope.
4. **G4–G7 remain NOT-MEASURED and are not promoted.** This is a defect correction, not a gate
   movement. It does not promote accessibility certification.
5. **Not an accessibility audit.** One reduced-motion affordance was repaired; the settings surface
   as a whole was not certified against any standard.
6. The test covers the **title-screen** replay route. The in-game routes (replay from a tool or a
   loaded save under reduced motion) share the same single corrected condition, so they are fixed
   by construction — but that is **[INFERENCE]**, not separately asserted.
7. The reduced-motion frame's layout was verified to exist and to carry all three verb labels; the
   pre-existing 150 % fit assertion lives in `SettingsPauseAndReducedMotionShowAllLabelsAt150Percent`
   and is unchanged. No new layout measurement was taken at other scales.
8. No commit or push was performed.

## 10. Deviations from CLAUDE.md, stated honestly

- **§5 mandatory post-edit `graphify update .` was NOT run.** The four `graphify-out/*` files are
  pre-existing *uncommitted* M15 evidence, and this session's scope forbids modifying graph files
  or altering pre-existing evidence. Refreshing the graph would overwrite that evidence. Deviation
  chosen deliberately; consequence recorded: **the code graph is stale by exactly this one-line
  change in `T0OpeningSession.BeginOpening`** and should be refreshed by whoever next owns a
  graph-writing step.
- **`mex log` / `mex check` were NOT run**, for the same out-of-scope reason (`.mex/` is outside
  the scoped M17 paths). `memory_sync` receipt: **`skipped`**. Verified rather than assumed — the
  contract resolver `scripts/mex-agent-bin.sh` → `resolve_mex_agent` returns skip reason
  "mex-agent not found or failed identity probe (PATH `mex` is TeX on this machine)". A `mex`
  0.7.1 exists at `~/.nvm/versions/node/v22.19.0/bin/mex` but **fails the identity probe**, so per
  CLAUDE.md §11 it was not executed.
- Therefore **G8 remains PARTIAL, not PASS**. The file contract alone was re-checked:
  `freshness-check.sh` exit 0, 0 findings across 613 markdown artifacts under `_workspace/current`.
  Per CLAUDE.md §11 this is **not** a full G8 PASS: the script reports its own scope as
  "frontmatter contract + supersedes topology only", it does **not** verify memory_sync receipts,
  and with no `--since` supplied **point-in-time staleness was not measured**.

## 11. Artifacts

```
m17-unity-polish/
├── verification.md   # this document
├── red.xml / red.log         # focused RED — 1 failed on the defect assertion
├── green.xml / green.log     # focused GREEN — 1 passed
├── editmode.xml / editmode.log   # 54/54
├── playmode.xml / playmode.log   # 85 total, 84 passed, 0 failed, 1 pre-existing skip
└── build-mac.log             # T0_MAC_BUILD Succeeded bytes=403838145
```
