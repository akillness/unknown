---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M9 완성도 hop — 구현·검증 (RFC-CX-011)

[OBSERVED] 사용자 M9 지시(코어루프·튜토리얼·리소스 반영 연출·밸런스·hop 심화)를 3레인 병렬 감사 → RFC-CX-011 판정 → 2개 병렬 구현 서브에이전트(SYS-CORE/SYS-M8) + 디렉터 리소스 레인으로 이행했다. 기준 커밋 `6a1a761`(M8) 위이며, 선행 미커밋 reduced-motion fix를 보존한 채 얹었다.

## 실제 변경 (색인 — 정본은 코드·impl 보고 2건)

- **S-A** 힌트 3단계 사용 기록이 비트별로 저장·복원된다(`progress.hintLevelUsed` 필드명 불변, 재열람 단계 보존, `warnsBeforeReveal` 데이터 구동 경고). interaction-rules §4 "사용 기록은 저장되지만" 계약 이행.
- **S-B** 기본 확정 방식 `two-step` 정합(interaction-rules §1-1). 기존 세이브 설정 불변.
- **S-C** T0 확정 프리뷰가 `Simulation.Preview` 기반 변경 예정·되돌림 고지 문장을 표시(GDD §3.3 원칙1).
- **S-D** 사건 흐름 목표가 비트별 objective(비공개 가드 포함, 판정 B)로 바뀌고, circuit/reader 패널에 `toolTeaching` 데이터 구동 안내(1단 힌트+잔여 술어 집계) 병기. 오프닝 모토·버튼 L() 이관.
- **S-E** M8 TARGET 3건 구현: 명시적 `검토 질문 보기`(누를 때만 갱신·구조 변경 시 '이전 질문' 라벨), record 출처 `원문 열기`→닫으면 검토 노트 복귀+초점 복원, 독립 매체 부족/충족 질문 분기.
- **S-F** 검토 노트 연출 계층: `M8ReviewVfx.json` 저작 타이밍(180/140/120ms, reduced-motion 0ms)의 CanvasGroup 전환, `M8ReviewNotesProfile`(runtimeApproved 게이트) 카드 텍스처 슬롯, 직전 오버레이 복원.
- **S-G** `SavePolicy.snapshotInterval` 노브 활성화, 문서 열람 중 유휴 힌트 제안 정지.
- **S-H** GTI 카드 텍스처 `m8-review-card-r01`(1536x1024) 생성·provenance·`Tools/M8/Import review card` 해시 대조 임포트(`runtimeApproved=false`). 승격은 별도 검수 후.
- 배선: `T0ProjectBuilder.WireBeats`(전체 Prepare 없이 beats 테이블만 연결 — 후속 마일스톤 빌더가 소유한 씬 보존).

## 정확한 검증 범위

| 검사 | 결과 | 영수증 |
|---|---|---|
| EditMode (배선 전) | 53/53 | [XML](editmode.xml) · [log](editmode.log) |
| PlayMode (배선 전) | 68/68 + boot ignored(전용 인자 필요) | [XML](playmode.xml) |
| 직렬화 부트 (격리 인자) | 1/1 | [XML](boot.xml) |
| beats 배선 + 카드 임포트 | WIRE_OK · IMPORT_OK, [import-audit](import-audit.json) sha 일치 | [wire](wire-beats.log) · [import](import-card.log) |
| EditMode (배선 후 최종) | **53/53** | [XML](editmode-final.xml) |
| PlayMode (배선 후 최종) | **68/68** + boot ignored | [XML](playmode-final.xml) |
| 직렬화 부트 최종 | **1/1** | [XML](boot-final.xml) |
| macOS 개발 빌드 | `T0_MAC_BUILD Succeeded bytes=354233020` | [log](build-mac.log) |
| 네이티브 실행 (1차) | 생존/종료 마커가 stdout에 없어 영수증 무효 (QA D-M9-09) → 아래 FIX 1 트랜스크립트로 대체 | [log](player-smoke.log) |

[OBSERVED] 이전에 실패하던 M5 `ReducedMotionStartsFreshGameImmediatelyWithoutWrites`는 선행 미커밋 수정 위에서 본 실행 전부 통과다(playmode-final 68/68에 포함). 명령·해시는 [verification.json](verification.json).

## QA FIX cycle 1 (독립 QA S2 1건 · S3 10건 → 수리 6·판정 3·이월 2)

[OBSERVED] 독립 QA `qa/completeness-m9-review.md` 판정 뒤 디렉터가 직접 수리했다(판정 근거: decision-log "RFC-CX-011 QA FIX cycle 1"). 수리: D-M9-01(S2, C1 단계 `원문 열기` 게이트 + 회귀 테스트 `C1StagesNeverOfferSourceOriginalOpen`), D-M9-02(Esc→`CloseReviewNotes`), D-M9-04(`T0Simulation.IsSatisfied` 단일 출처), D-M9-05("비트"→"단계"), D-M9-08(테스트 리터럴 기대값), D-M9-10(pending 중 힌트 단계 flush). 판정: D-M9-03(`select_ms` 제거, 문구 180/140/120), D-M9-06(티칭 헤더 = 힌트 사다리와 별개, 기록 안 함), D-M9-07(영향 구역·근거 2종 이월). 이월: D-M9-11(planner 콘텐츠).

| 검사 | 결과 | 영수증 |
|---|---|---|
| EditMode (FIX 1) | **53/53** | [XML](editmode-fix1.xml) · [log](editmode-fix1.log) |
| PlayMode (FIX 1) | **69/69** (+1 C1 회귀) + boot ignored | [XML](playmode-fix1.xml) · [log](playmode-fix1.log) |
| 직렬화 부트 (FIX 1, 격리 인자) | **1/1** | [XML](boot-fix1.xml) |
| macOS 개발 빌드 (FIX 1) | `T0_MAC_BUILD Succeeded bytes=354230869` | [log](build-mac-fix1.log) |
| 네이티브 실행 (FIX 1) | 선행 프로세스 0 확인 → `open` 실행 → 새 pid 12초 생존 → Player.log `T0_BOOT entry-start/ui-root-loaded/hub-loaded/session-initialized` → kill 후 종료 확인 → 사후 프로세스 0 | [transcript](player-smoke-transcript.log) · [Player.log](player.log) |

[OBSERVED] 1차 실행 트랜스크립트 검토 중 첫 스모크의 플레이어가 31분간 살아 있었음을 발견했다(첫 `kill`이 다른 pid를 잡음). 전부 종료 후 사전/사후 프로세스 수 0을 기록하는 재실행으로 대체했다.

## QA FIX cycle 2 (R2: 수리 6 CLOSED · 신규 S2 D-M9-13 → 마지막 허용 회차)

[OBSERVED] QA R2가 FIX 1 수리 6건을 독립 확인해 닫고, 인접 경로 S2 D-M9-13(도구 패널 열린 상태의 `원문 열기` — 복귀 실패 + 숨은 도구 입력 전달; R1 누락)을 새로 열었다. 최소 수리: `ReviewSourceOriginalAvailable`에 `tool==null` 추가(셸에서는 유지). 회귀 `OpenToolPanelsNeverOfferSourceOriginalOpenWhileShellStillDoes` + C1 회귀에 `evidence` 복원 단정(G-2). D-M9-12: manifest 이월 목록·changelog 정합. 판정 근거: decision-log "RFC-CX-011 QA FIX cycle 2".

| 검사 | 결과 | 영수증 |
|---|---|---|
| EditMode (FIX 2) | **53/53** | [XML](editmode-fix2.xml) |
| PlayMode (FIX 2) | **70/70** (+1 도구 열림 회귀) + boot ignored | [XML](playmode-fix2.xml) · [log](playmode-fix2.log) |
| 직렬화 부트 (FIX 2) | **1/1** | [XML](boot-fix2.xml) |
| macOS 개발 빌드 (FIX 2) | `T0_MAC_BUILD Succeeded bytes=354230868` | [log](build-mac-fix2.log) |

[OBSERVED] FIX 회차 2/2 소진(quality-gates FIX≤2). R3에서 S2가 다시 열리면 개방 결함 등록 + hop 완료 보류.

## 경계 (이 검증이 말하지 않는 것)

- [OBSERVED] 화면 캡처 시도는 데스크톱 전체 캡처만 얻어 게임 창 판독 증거로 무효였고 즉시 폐기했다(다른 세션 창 포함). **네이티브 창 수준 가독성 검수는 미완**이며, 따라서 카드 텍스처는 `runtimeApproved=false` 후보로 남는다. 승격은 `Tools/M8/Approve review card` + decision-log 감사로만.
- [CARRIED] 실기기 한국어 IME/컨트롤러 검수, 사람 플레이테스트(재미·몰입·25분 예산 실측), 성능 캡처는 이 자동화 검사로 입증하지 않는다.
- [CARRIED] `hintOfferCooldownSeconds` 노브 분리(G4d)·EN 힌트 textKey(G10)·alignment 신규 동사·ReadOriginal 마모 undo 판정은 RFC-CX-011 명시 이월 항목.
- [OBSERVED] 병행 세션이 같은 시각 campaign.json·synopsis·planning(M10 TRACE-RPG)을 편집 중이다. 본 M9는 그 파일들을 쓰지 않았고, Tables/*는 바이트 불변이다.
