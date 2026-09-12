---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M9 네이티브 실동작 플레이테스트 — 검증 (RFC-CX-015)

[OBSERVED] 판정 근거는 `production/decision-log.md` §**RFC-CX-015 — 네이티브 실동작 플레이테스트·모니터링과 게임플레이 영상**(`:504-517`)이며, 선행 구현·QA 근거는 §RFC-CX-011 계열 + `qa/completeness-m9-review.md`(R1/R2/R3)다. **RFC id 주의**: 본 작업은 최초 `RFC-CX-013`으로 기록됐으나 같은 시각 병행 세션 2개가 동일 id를 선점해 `RFC-CX-015`로 재번호됐다(`decision-log.md:506`, 014는 예비). 이 문서의 모든 RFC 인용은 **RFC-CX-015**다.

## 0. 무엇을 어떻게 조작했는가

[OBSERVED] 배치 테스트가 아니라 **빌드된 플레이어를 실제 창에서 사람이 조작하듯 몰았다**.

| 항목 | 값 | 근거 |
|---|---|---|
| Unity | `6000.5.6f1` | `build-mac.log:4583` 에디터 경로 |
| 빌드 대상 | macOS 스탠드얼론, `Builds/T0-mac/Unknown.app` | `Editor/T0ProjectBuilder.cs:85` `BuildMacAt` |
| 빌드 옵션 | **`BuildOptions.Development`** — 개발 빌드다. 릴리스/프로덕션 빌드가 **아니다** | `T0ProjectBuilder.cs:85`; 전 샷 우하단 `Development Build` 워터마크 |
| 실행 | `open -n <app> --args --t0-save-dir <격리 디렉터리> --m8-review-notes-diagnostic -screen-width 1280 -screen-height 800` | `decision-log.md:508`; 인자 소비 지점 `T0GameSession.cs:45`(`--t0-save-dir`), `ReviewNotesSession.cs:226`(`--m8-review-notes-diagnostic`) |
| 격리 저장 디렉터리 | 플레이테스트 `/tmp/unknown-m9-playtest-1789141632` · 녹화 세션 `/tmp/unknown-m9-record-1789168879` | `save-dir.txt` · `record-save-dir.txt` |
| 입력 | **cliclick 포인터 전용**(좌표 클릭 + ScrollRect 드래그). 합성 키 이벤트는 Unity Input System에 닿지 않았다 → **키보드/컨트롤러 동등성은 PlayMode 테스트에 위임** | `decision-log.md:508` |
| 창 판독 | orca computer로 창 상태·슬롯 확인 후 좌표 산출 | `decision-log.md:508` |
| 모니터링 | Player.log 예외 **0건**, save.json 실시간 대조 | `decision-log.md:508` |
| 증거 | 플레이테스트 스크린샷 **184 파일** `shots/` (접두 색인 `00`–`94`; decision-log의 "94장"은 색인 수이고 대부분 단계가 조작 전/후 2장이다) · 녹화 세션 `rec-shots/` **91 파일**(증가 중, 아래 주석) | 디스크 실측(디렉터리 엔트리 카운트) |

[OBSERVED] **증거 수 주의**: `shots/`는 184 파일로 안정적이나 `rec-shots/`는 병행 녹화 세션이 지금도 기록 중이다 — 본 검증 시작 시 64, 2026-09-12T00:12:06+00:00 기준 **91**(계열 `r*` 63 · `s*` 26 · `t*` 2). 이 문서의 `rec-shots` 수치는 **시점 스냅샷**이며 완료된 산출물 개수가 아니다.

[OBSERVED] `--m8-review-notes-diagnostic`가 **필수였던 이유**: 검토 노트 연출 게이트는 `ReviewDirectionEnabled => ReviewProfile.runtimeApproved || args.Contains("--m8-review-notes-diagnostic")`(`ReviewNotesSession.cs:226`)이고, `Resources/M8ReviewNotes.asset`은 지금도 `runtimeApproved: 0`이다. 즉 카드 텍스처·전환 연출은 **진단 플래그로 강제 점등한 상태**에서 관측했다. 승격은 일어나지 않았다(§6).

## 1. 진행 경로 (단계 × 관측 결과 × 증거 샷)

[OBSERVED] 각 행의 샷 파일은 디스크에 존재하며 접두 색인이 재현 순서다. 결과 열은 `decision-log.md:509`가 기록한 실동작 관측이고, 굵게 표시한 4행은 내가 PNG를 직접 열어 화면 문자열까지 재확인한 행이다.

| # | 단계 | 관측 결과 | 증거 샷 |
|---|---|---|---|
| 1 | M5 오프닝 | 2샷 재생, S-D 모토·버튼 `L()` 이관 반영 | `00-title` `01-start` `02-opening` `03-opening-shot2` |
| 2 | 셸 진입 | 노드 6개 목록 + 툴바 6칸 | `04-shell` |
| 3 | 인수 각서·3줄 판독 | 각서 3줄 열람, `ViewLine` 커맨드 적재 | `05-brief` `06-brief-3lines` `06a` `06b` |
| 4 | 증거함·검토 노트 개시 | 노트 열림→정착(페이드 경유) | `07-evidence` `08-notes-open` `08-notes-settled` |
| 5 | **검토 질문(독립 매체 부족 분기)** | **`검토 질문 보기` 버튼 렌더 확인. 버튼을 눌렀으나 화면 변화 0 — 질문 패널이 버튼 *위*로 나가 화면 밖 → D-M9-15 발현** | **`09-question-0links` `10-question-visible` `10b-scrolled`** |
| 6 | 출처 연결 → 원문 열기 → 닫기 | 초점 복귀까지 실증(셸 경로는 `원문 열기` 허용) | `11-link` `11-linked` `12-original` `13-close-doc` `13-returned` |
| 7 | Esc 닫힘 경로 | D-M9-02 수리 동작(`CloseReviewNotes`) 실증 | `14-esc` `14-after-esc` `15-esc2` `16-close` `16-after-close` |
| 8 | 힌트 1→2단·경고 게이트·재열람 | 1단→2단 상승, 3단 앞 경고 게이트, 닫고 재열람해도 단계 보존 | `15-hints-f1` `17-hints` `18-hint1` `18-hint-l1` `19-hint2` `19-hint-l2` `20-close` `21-reopen` `21-hints-reopened` `22-hint3` `22-hint-warning` `23-close` |
| 9 | 이관 목록 ✓ 6/6 → 결정 | 6항 전부 체크 후 결정 수용 | `24-shell` `25-transfer` `26a` `26b` `26c` `26-transfer-done` `26-decision` `27-close` |
| 10 | 판 #0 · t0-b1 완료 | 비트 목표가 t0-b1→다음 비트로 전환 | `28-plate` `28-b1-done` |
| 11 | 회로 지도 진입 | 안내 헤더(`toolTeaching`) 병기 | `29-nav-circuit` `29-circuitmap-node` `30-open-circuit` `30-circuit-tool` `31-trace` |
| 12 | 정렬 (−1, +1) · 앵커 | 오버레이 개시→좌(−1)→상(+1)→앵커 고정 | `32-begin-overlay` `32-overlaying` `33-left` `33-after-left` `34-up` `34-after-up` `35-anchor` `35-anchored` |
| 13 | 구획 3 · 근거 3 | 잔여 조건 **2→1→0 실시간 감소** | `36-marked` `36a` `36b` `36c` `37-attach` `37-circuit-state` `38-area3` `38-after-area3` `39-ev2` `39-after-ev2` `40-ev1` `40-ev1-overlay` |
| 14 | t0-b2 체크 → t0-b3 | 판 부착 후 b3 진입 | `41-b2-check` `41-attach-plate` `42-close` |
| 15 | 판독기 진입 | 안내 헤더 병기 | `43-nav-reader` `43-reader-node` `44-open-reader` `44-reader` |
| 16 | 표준판 적재·판독 | 파형 화면 판독 | `45-load-plate` `45-plate-loaded` `46-read` `46-after-read` |
| 17 | 눈금 선택기 | 페이지당 30행 중 ~8행 가시, 휠 무반응 → **드래그 스크롤로 도달**(결함 아님, §4 관찰) | `47-picker` `47-pick-start` `48-close-picker` `48-reader-again` `49-drag-scroll` `50-next1` `50-next2` `50-page2` `51-page2-scrolled` |
| 18 | 시간창 H−1:00 → H+3:00 | 페이지 2→4 드래그 도달 후 시작·끝 확정 | `52-pick-h-1` `52-start-set` `53-reader-scrolled` `54-next1`–`54-next4` `54-page4` `54-pick-end` `55-page4-scrolled` `55-prev` `56-page4-more` `57-page4-deep` `58-pick-h3` `58-window-set` |
| 19 | **인용 1 — 프리뷰 → 확정** | **`적용하면 바뀌는 항목 / 당직실 표준판 인용 고정 · H-1:00 → H+3:00 / 확정 후에도 되돌림으로 취소할 수 있습니다. / 이 변경을 적용합니다. 저장이 성공한 뒤에만 확정됩니다. 되돌림은 계속 사용할 수 있습니다.` 렌더 + `다음`→확정 2단** | **`59-cite-visible` `60-cite` `60-preview` `61-confirm-step` `61-next` `62-confirm` `62-committed`** |
| 20 | 자료 교체 → 조위대장 판독 | 매체 다른 두 번째 기록 적재 | `63-swap` `63-record-picker` `64-picker` `64-swap` `65-load-ledger` `65-ledger` `66-read-ledger` `66-ledger-scrolled` |
| 21 | 인용 2 → `T0 완료` | 프리뷰→확정 후 T0 완료 표시 | `67-cite2` `67-preview2` `68-confirm2` `68-next2` `68-t0-complete` |
| 22 | 추가 인용 정착 | 프리뷰→확정 반복, 정착 확인 | `69-find-cite` `70-cite2` `70-preview2` `71-settled` `72-cite2` `72-preview2` `73-confirm-step` `73-next` `74-confirm` `74-after-confirm` |
| 23 | 되돌림 / 다시 | `headSeq 31→30→31`, 자동 사본 보존 | `75-undo` `75-after-undo` `76-redo` `76-after-redo` `77-redo` `77-after-redo` `77-close` |
| 24 | **OS 재시작 후 재개** | **T0 완료 상태인데 t0-b1 환영문("각서와 목록을 열람하고…") 표시 → D-M9-16 발현** | **`78-relaunch-title` `79-continue` `79-resumed`** |
| 25 | 힌트 상태 재검증 | `t0-b1:2` 유지 · `t0-b3` 0단(OS 재시작을 건너 살아남음) | `80-hints-b3` `80-hints-b3-state` `81-close` |
| 26 | C1 진입 | 스테이지 전환(`21:00 · c1-b1`) | `82-c1-entry` `82-continue-c1` |
| 27 | **C1 관찰 2/2** | **`C1 · 순찰로의 두 분기 / 관찰 0/2 / 다음 행동: 두 기록 확인` 안내 렌더. 상세에 `출처: watchlog-bureau · log` 노출 → D-M9-17 발현** | **`83-observed` `83-obs1` `83-obs2` `84-open1` `84-detail1` `85-record1` `85-back1` `86-open2` `86-record2` `86-back2` `86-both-observed`** |
| 28 | 분기·조건 확인 | 접힘/조명 조건 충족 후 확인 | `87-fold-light` `87-patrol-ready` `87-ack` `88-ack` `88-acked` |
| 29 | two-step 순찰 확정·수문 권한 | 프리뷰→`다음`→확정 2단 후 순찰 완료·권한 부여 | `89-patrol-preview` `89-confirm-patrol` `90-confirm-step` `90-next` `91-confirm` `91-patrol-complete` |
| 30 | **C1 검토 노트** | **출처 5건 전부 `매체: 기록판 / 장부 / 문서 기록`로 렌더, `원문 열기` 액션 0건 = D-M9-01 게이트 실증. 카드 텍스처 배경·순찰 패널(r01) 3D 동시 렌더** | **`92-evidence-c1` `92-notes-c1` `92-notes-in-c1` `93-notes-c1` `93-open-notes` `94-notes-c1-bottom`** |

## 2. M9 슬라이스 실동작 대조

| 슬라이스 | 계약 | 실동작 판정 | 증거 |
|---|---|---|---|
| **S-A** 힌트 3단 기록 저장·복원 | `progress.hintLevelUsed` 비트별 보존, 재열람 단계 유지, `warnsBeforeReveal` 경고 게이트 | **PASS** [OBSERVED] — 1→2단 상승·재열람 보존·3단 앞 경고 관측, **OS 재시작 후에도 `t0-b1:2` 유지**. 최종 save.json 재파싱: `progress.hintLevelUsed = {"t0-b1": 2}` (내가 디스크에서 직접 확인) | `17`–`23` 계열, `80-hints-b3-state`, `save.json` sha `ce1771…` |
| **S-B** 기본 확정 = `two-step` | 프리뷰 → `다음` → 확정 2단 | **PASS** [OBSERVED] — T0 인용 확정과 C1 순찰 확정 **양쪽**에서 2단 관측. `60-preview`에 `다음` 버튼 직접 확인 | `60-preview` `61-next` `62-confirm` · `89-patrol-preview` `90-next` `91-confirm` |
| **S-C** 프리뷰 diff + 되돌림 고지 | `Simulation.Preview` 기반 변경 예정 문장 + 되돌림 가능 고지 | **PASS** [OBSERVED] — 화면 문자열 직접 판독: `적용하면 바뀌는 항목` / `당직실 표준판 인용 고정 · H-1:00 → H+3:00` / `확정 후에도 되돌림으로 취소할 수 있습니다.` / `이 변경을 적용합니다. 저장이 성공한 뒤에만 확정됩니다. 되돌림은 계속 사용할 수 있습니다.` | `60-preview` (직접 열람) |
| **S-D** 비트별 목표 + 도구 안내 헤더 | 목표가 비트별 objective로 전환, 패널에 `toolTeaching` 안내 + 잔여 술어 집계 | **PASS** [OBSERVED] — t0-b1 완료 시 목표 전환, 회로 지도 잔여 조건 **2→1→0 실시간**, 판독기 헤더. 직접 판독: `사건 흐름 · 필요한 인용 0/2` + `다음: 근거를 살펴보고 인용을 고정하세요.`(T0), `관찰 0 / 2` + `다음 행동: 두 기록 확인`(C1) | `28-b1-done` `30-circuit-tool` `36`–`40` 계열 `44-reader` · `60-preview` `84-detail1` (직접 열람) |
| **S-E** M8 TARGET 3건 | 명시적 `검토 질문 보기`, record 출처 `원문 열기`→닫으면 노트 복귀+초점 복원, 독립 매체 부족/충족 질문 분기, C1 게이트 | **부분 PASS** [OBSERVED] — 버튼 렌더·원문 열기→닫기→초점 복귀·C1 `원문 열기` 0건은 **PASS**. **질문 표시 자체는 D-M9-15로 FAIL** 후 수리 | `09-question-0links` `10-question-visible` (버튼 존재·변화 0, 직접 열람) · `11`–`13` 계열 · `94-notes-c1-bottom` (직접 열람, `원문 열기` 0건) |
| **S-F** 검토 노트 연출 계층 | `M8ReviewVfx.json` 타이밍 CanvasGroup 전환, 카드 텍스처 슬롯, 직전 오버레이 복원 | **PASS(조건부)** [OBSERVED] — 카드 텍스처 배경이 네이티브 창에 렌더되고 플레이스홀더 문자열이 가독. 단 **`--m8-review-notes-diagnostic`로 강제 점등한 상태**이며 `runtimeApproved: 0`이다(§6) | `08-notes-settled` `09-question-0links` `94-notes-c1-bottom` (직접 열람 — 베이지 카드지 배경 + 본문 가독 확인) |
| **S-G** `SavePolicy.snapshotInterval` · 문서 열람 중 유휴 힌트 정지 | 노브 활성, 문서 중 힌트 제안 없음 | **부분 [INFERENCE]** — 자동 사본 보존은 되돌림/다시 구간에서 관측(`75`–`77`), 문서 장시간 열람 중 힌트 제안 부재는 **관측 근거 부족**(체류 시간 미측정). 런타임 PASS로 쓰지 않는다 | `75-undo` `76-redo` `77-close`; 유휴 정지는 **미측정** |
| **S-H** GTI 카드 텍스처 후보 | `m8-review-card-r01`(1536x1024) 해시 대조 임포트, `runtimeApproved=false`, 승격은 별도 검수 | **후보 유지** [OBSERVED] — 네이티브 창 가독성은 이번에 처음 확인됐으나 **승격 절차는 실행되지 않았다**. `Resources/M8ReviewNotes.asset` = `runtimeApproved: 0`, `cardPaper` guid `69bf0559…` 바인딩 | `94-notes-c1-bottom` · `M8ReviewNotes.asset`(디스크 직접 확인) |

## 3. 실동작에서만 드러난 결함

[OBSERVED] 직전 QA 3회차(R1/R2/R3)는 READ-ONLY 정적 열람이었고 D-M9-01~14를 다뤘다. 아래 3건은 **창에서 눌러야만 보이는** 결함이라 정적 검토가 잡지 못했다.

### D-M9-15 · **S2** · UX(`T0Interface` + `ReviewNotesView`) — 질문 패널이 버튼 위에 렌더되어 초점 스크롤이 화면 밖으로 밀어낸다

- 증상 [OBSERVED]: `검토 질문 보기`를 눌러도 **화면이 변하지 않는다**. 질문 패널이 앵커 버튼 *위*에 붙고, 초점 스크롤(40%)이 그 버튼을 아래로 내려 패널이 뷰포트 밖으로 나간다. 답은 생성돼 있으나 보이지 않는다.
- 회복 불가 [OBSERVED]: 포인터 휠 스크롤이 검토 노트 리스트에 닿지 않아 되돌릴 방법이 없다.
- 증거: `09-question-0links.png`(버튼 존재) → `10-question-visible.png`(누른 뒤, 렌더 영역 동일) → `10b-scrolled.png`(스크롤 복구 시도). 두 PNG를 직접 열어 문자열 단위로 대조했다.
- 수리 [OBSERVED — 소스 확인]: 질문을 앵커 액션 `review-note-question` **아래**에 렌더. `T0ReviewNotesInterface.cs:10`에 `QuestionAnchorId` 필드 추가, `ReviewNotesSession.cs:292`에서 `QuestionAnchorId = "review-note-question"` 설정, `T0Interface.cs:137`이 액션 루프 안에서 `action.Id == model.ReviewNotes.QuestionAnchorId`일 때 `RenderReviewQuestion` 호출. 앵커가 비면 `T0ReviewNotesInterface.cs:65`가 종전 위치로 폴백.
- 소유 레인: SYS-M8(ReviewNotesSession) + SYS-CORE(T0Interface).

### D-M9-16 · **S3** · SYS-CORE(`T0GameSession`) — 재개 상태문이 t0-b1 환영문에 고정

- 증상 [OBSERVED]: OS 재시작 후 저장을 재개하면 **T0 완료 상태에서도** t0-b1 환영문 `각서와 목록을 열람하고 상시 슬롯을 준비하세요.`가 뜬다. 진행 상황과 상태문이 불일치한다.
- 재현: `78-relaunch-title` → `79-continue` → `79-resumed`.
- 수리 [OBSERVED — 소스 확인]: `T0GameSession.cs:79` `ResumeStatus()` 신설 — 서명/순찰 활성 분기 우선, 그 다음 `t0-b3` 완료→`L("caseReview")`, `t0-b1` 완료→**신규 키** `L("resumeInProgress")`, 그 외 `L("welcome")`. `:80` `StartGame()`이 `status=ResumeStatus()`로 호출. 신규 키 실물 확인: `T0Strings.json` `resumeInProgress` = ko `저장된 당직 작업을 복원했습니다. 사건 흐름의 다음 행동을 이어가세요.` / en `Restored the saved watch. Continue with the next action in the case thread.`
- 소유 레인: SYS-CORE.

### D-M9-17 · **S3** · SYS-CORE + C1 — 내부 id가 플레이어 문자열로 4개소 노출

- 증상 [OBSERVED]: 매체명 대신 데이터 id가 그대로 표시된다. 4개소 = ① 판독기 자료 선택 `station-bureau-standard` ② 순찰 관찰 상세 `출처: watchlog-bureau` ③ 증거함 `· watchlog-bureau` ④ 구획 근거 `· log` / `· ledger`.
- 증거(직접 열람): `84-detail1.png` 본문에 `출처: watchlog-bureau · log` — record id와 `sourceType` 둘 다 노출됨을 화면에서 확인했다. 나머지 3개소는 `47-picker`·`39-ev2`·`37-attach` 계열.
- 수리 [OBSERVED — 소스 확인]: 4개소를 `ReviewMediaName(sourceType)`로 교체 — `C1GameSession.cs:62`(`"매체: "+ReviewMediaName(...)`), `T0GameSession.cs:307`(구획 근거), `:309`(판독기 자료 선택), `:311`(증거함·순찰 관찰). 매핑은 `ReviewNotesSession.cs:322-330`: `plate→기록판`, `log→문서 기록`, `ledger→장부`, 그 외 `기록 매체`.
- 수리 후 실증: `94-notes-c1-bottom.png`에서 출처 5건이 전부 `매체: 기록판 / 장부 / 문서 기록`로 렌더됨을 직접 확인.
- **이월** [CARRIED]: 자동 보존 단서 id(`t0-b2-c1` 등)는 표시명 데이터가 없어 교체하지 못했다 → planner(clue 표시명).
- 소유 레인: SYS-CORE + C1 세션.

### D-M9-11 실증(신규 아님, planner 이월 유지)

- [OBSERVED] `t0-b2.objective` 둘째 문장 `안내 표시가 각 단계에 붙는다.`(저작 지시문)가 사건 흐름에 그대로 노출됨을 실동작으로 확인했다. R1/R2/R3는 데이터 열람으로만 지적했고 화면 노출은 이번이 첫 실증이다. 소유 = planner(`campaign.json`, 병행 편집 중) → **STILL-OPEN**.

## 4. 결함이 아닌 관찰

- [OBSERVED] 눈금 선택기는 페이지당 30행 중 ~8행만 보이고 휠 스크롤이 닿지 않지만 **UGUI ScrollRect 드래그로 도달 가능**하다(`49-drag-scroll`). 관성 때문에 클릭 직전 레이아웃이 움직여 오클릭이 발생했는데, 사람 손은 정지를 기다리므로 **자동화 특유의 현상**으로 판정한다.
- [OBSERVED] 3D 뷰포트는 승인 프리팹만 실물이다 — 사물 서랍 r03(`09-question-0links` 좌상단), C1 순찰 패널 r01(`84-detail1`·`94-notes-c1-bottom` 좌상단). 회로 지도·판독기 노드는 **그레이박스**(M7 프리팹 재구성 이월).
- [OBSERVED] 부제가 raw 비트 id를 표시한다 — `21:00 · t0-b1`, `21:00 · t0-b3`, `21:00 · c1-b1`. R2가 D-M9-05 범위 밖(HEAD 동일)으로 이미 판정한 기존 노출이며 M9 신규가 아니다. 별건으로 남는다.

## 5. 수리 후 재검증 영수증

[OBSERVED] D-M9-15/16/17 수리 뒤 전량 재실행했다. `sha256`은 내가 `python hashlib`로 디스크 바이트에서 직접 계산했다.

| 검사 | 결과 | 파일 | sha256 | 실행 시각(UTC) |
|---|---|---|---|---|
| EditMode | **53/53** Passed (failed 0 · skipped 0) | [editmode.xml](editmode.xml) · [log](editmode.log) | `c7b0d56803886445ebdcce45be6e0b66f37585e3932d61ceef477ef7e72f6b60` | 2026-09-11 23:16:53Z (2.380s) |
| PlayMode | **73 passed / 0 failed / 6 skipped** (total 79, `result=Skipped:Ignored`) | [playmode.xml](playmode.xml) · [log](playmode.log) | `7d662ff0c41eae5b04b0662f205b63d9481a39e5bd6ad7fd50a2581521b06af0` | 2026-09-11 23:17:14Z (32.089s) |
| 직렬화 부트 (격리 인자) | **1/1** Passed | [boot.xml](boot.xml) · [log](boot.log) | `ea0a0e967c9491cfeab76c83d83f2acc85a986170ab226d1a3fcc330e6b49672` | 2026-09-11 23:18:02Z (0.587s) |
| macOS 플레이어 빌드 (`BuildOptions.Development`) | `T0_MAC_BUILD Succeeded bytes=**354247240**` | [build-mac.log](build-mac.log):6277 | — | 2026-09-11 |

### PlayMode skipped 6건의 정체 (M9 소유 아님)

[OBSERVED] `playmode.xml`을 재파싱해 skip 사유 문자열까지 확인했다. **실패는 0건이고, 6건 모두 게이트 ignore다.**

| # | 테스트 | 사유(XML `reason`) | 소유 |
|---|---|---|---|
| 1 | `M7HubPlayModeTests.GateOnSwapsListedRootsAddsOneLampAndFlatAmbientOnce` | `M7Hub.asset not imported — run Tools/M7/Import hub shell candidates first` | **병행 M7 세션** |
| 2 | `M7ReaderPlayModeTests.GateOnBuildsStageWithTwoLightsAndTearsDownOnExit` | `M7ReaderStage.asset not imported — …` | **병행 M7 세션** |
| 3 | `M7ReaderPlayModeTests.GateOnReducedMotionKeepsCrankAtRest` | `M7ReaderStage.asset not imported — …` | **병행 M7 세션** |
| 4 | `M7UiSkinPlayModeTests.GateOnAtTextScale150RendersWithoutLayoutChange` | `M7UiSkin.asset not imported — …` | **병행 M7 세션** |
| 5 | `M7UiSkinPlayModeTests.GateOnInsertsNonRaycastBackingsAndKeepsButtonsClickable` | `M7UiSkin.asset not imported — …` | **병행 M7 세션** |
| 6 | `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen` | `Run with an explicit isolated --t0-save-dir for serialized boot verification.` | M9 — **별도 `boot.xml`에서 1/1 PASS로 커버됨** |

[OBSERVED] 1–5는 **별행 M7 세션(텍스처/리더 스테이지)의 신규 테스트**로, 해당 세션이 에셋 임포트를 끝내기 전이라 스스로 ignore한다(게이트 설계대로). **M9 소유가 아니며 M9 수리로 깨진 것이 아니다.** 6은 `T0BootSceneTests.cs:20`의 설계된 ignore이고 전용 인자 런(`boot.xml`)에서 통과한다. 따라서 **M9 소유 테스트의 유효 결과는 EditMode 53/53 + PlayMode 73/73 + boot 1/1, 실패 0**이다.

### 저장 상태 실측

[OBSERVED] 격리 디렉터리 최종 상태를 직접 파싱했다 — `/tmp/unknown-m9-playtest-1789141632/save.json`, sha256 `ce1771599f024c728c3a645fe9a651ecacf5e8f0eed86e7eadbc6a19caf1f012`, 10042 B, `schemaVersion 3`.

| 필드 | 값 | 의미 |
|---|---|---|
| `progress.hintLevelUsed` | `{"t0-b1": 2}` | S-A 힌트 기록이 OS 재시작·스테이지 전환을 넘어 보존됨 |
| `beatId` / `stageId` | `c1-b1` / `C1` | C1 순찰까지 완주한 최종 상태 |
| `commandLog.headSeq` | `38` | 최종값. decision-log가 기록한 `31→30→31`은 **되돌림/다시 구간의 중간값**이며, 그 뒤 C1 진행으로 38까지 늘었다 |
| `commandLog.byteCap` | `6291456` | 커맨드 로그 상한 정상 |
| 동거 파일 | `save.bak`, `checkpoint.pre-commit.json`, `checkpoint.pre-commit.json.bak` | 자동 사본·확정 전 체크포인트 보존(S-G 일부) |

[OBSERVED] `decision-log.md:508`은 OS 재시작 전후 `save.json` sha 불변과 Player.log 예외 0건을 기록한다. 내가 읽은 파일은 **세션 종료 후 최종 상태**이므로 재시작 시점의 sha와 동일 값일 필요는 없다 — 그 불변 관측은 세션 중 기록이고 내가 재현하지 않았다.

## 6. 이 검증이 말하지 않는 것

- [OBSERVED] **사람 플레이테스트 n=0.** 조작 주체는 디렉터가 cliclick으로 몬 포인터다. 재미·몰입·이해도·좌절 지점은 **미측정**이다.
- [OBSERVED] **25분 예산 실소요 미측정.** 자동화 조작은 좌표 산출·재시도·드래그 관성 대기를 포함해 사람 플레이와 시간 구조가 다르다. 8시간 완주 예산도 미측정이다.
- [OBSERVED] **키보드·컨트롤러 경로는 이 실동작이 입증하지 않는다.** 합성 키 이벤트가 Unity Input System에 닿지 않아 포인터만 썼다. Tab 초점·Enter 선택·방향키 조절·1~6 도구·Ctrl+Z는 화면 하단 안내로만 보였고 **PlayMode 테스트에 위임**된다. 실기기 컨트롤러 검수 **미완**.
- [CARRIED] **한국어 IME 실기기 검수 미완.** 검토 노트 메모 편집은 진단 플래그 상태에서 렌더만 확인했다.
- [OBSERVED] **3D는 일부 승인 프리팹만.** 서랍 r03·C1 순찰 패널 r01만 실물이고 회로 지도·판독기 노드는 그레이박스다(§4).
- [OBSERVED] **카드 텍스처 승격은 일어나지 않았다.** `Resources/M8ReviewNotes.asset` = `runtimeApproved: 0`. 이번 렌더는 `--m8-review-notes-diagnostic`로 강제한 것이므로 **기본 실행 경로의 연출은 여전히 꺼져 있다**. 승격은 `Tools/M8/Approve review card` + decision-log 감사로만.
- [OBSERVED] **빌드는 `BuildOptions.Development` 개발 빌드다**(전 샷에 `Development Build` 워터마크). 릴리스 구성·IL2CPP·스트리핑 프로파일은 검증 범위 밖이다.
- [OBSERVED] **성능·프레이밍 미측정.** 프레임 타임·GC·메모리·해상도 스케일링을 캡처하지 않았다. 창은 1280x800 고정 1개 구성만 봤다.
- [OBSERVED] **G4/G7 런타임 게이트는 PASS가 아니다.** 이 문서는 조작 가능성과 문자열 정합의 실증이며, 품질 게이트가 요구하는 사람 플레이·성능 측정 요건을 충족하지 않는다.
- [OBSERVED] **게임플레이 영상은 이 검증 시점에 산출물이 없다.** `docs/media/gameplay-m9/`는 **존재하지 않고**, `rec-shots/`는 녹화 세션 스틸이며 2026-09-12T00:12:06+00:00 기준 91 파일(세 계열 `r*`/`s*`/`t*`) — 내 검증 도중 64→91으로 늘어난 **증가 중인 디렉터리**다. 확정 프레임 수로 인용할 수 없다. `decision-log.md:517`의 `core-loop`·`gameplay` 2편은 **[TARGET]**이며 완료 주장이 아니다.
- [OBSERVED] **샷↔단계 결속의 한계.** §1 표의 결속은 파일명 색인 순서 + decision-log 기록에 근거한다. 내가 픽셀까지 직접 재확인한 것은 굵게 표시한 4행(`09`/`10`, `60-preview`, `84-detail1`, `94-notes-c1-bottom`)이고 나머지 행은 파일 존재와 명명 규약까지가 [OBSERVED], 화면 내용은 [INFERENCE]다.
- [OBSERVED] **병행 세션 경계.** 본 검증은 `planning/`·`synopsis/`·`worldview/`·`campaign.json`·`concept/`·`handoff/`를 읽거나 쓰지 않았다. PlayMode skipped 1–5는 그 세션들의 진행 중 상태를 반영할 뿐이다.
