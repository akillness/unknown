---
updated: 2026-09-18
cycle: 20260918-content-update-m26
status: current
supersedes: null
owner: game-production-director
---

# M26 · 구현 결과, QA 루프, 다음 개선 계획

## 결론

**딥리서치 권고 10건 중 4건(D2·D3·D4·D10)을 구현·검증했고, 3시간 간격 QA 루프를 설치했다.** 이 문서는 사용자 지시 "3시간 간격으로 QA 루프 설정하고, 개선 방향 업데이트해서 업데이트하고 빌드까지"에 대한 개선 방향의 정본이다. 개선 후보는 **이번 세션의 자체 QA 관찰과 조사 보고서**에서만 나온다. 사람 플레이 n=0이므로 재미·이해 개선을 주장하지 않는다.

실행 정본: [verification.json](../systems/tech-verification/m26/verification.json) · [조사 보고서](../planning/aside-similar-games-research-20260918.md) · [QA 루프 원장](../qa/loop/ledger.md) · [회고](../retrospectives/m26-similar-games-develop-20260918.md).

## 권고별 실제 달성 범위

| 권고 | 결과 | 관찰한 인수 증거 / 남은 경계 |
|---|---|---|
| D2 현재 조사 질문 | 구현·검증 | 사건 흐름 카드 아래 한 줄 질문 + 고정/필요 매체 아이콘. 비트별(t0-b1/b2/b3/완료, c1-b1/b2) 문구는 기록명·시각·정답 부재를 테스트가 검사. 판독 비교 레이아웃에서는 인라인 밴드로 같은 스트립 |
| D3 구조 상태 + 반례 | 구현·검증 | 미검토/한 매체/매체 2종/반례 고정 4단계, 범례 글리프, `MarkCounterexample` 토글(가산적 사실, 요구조건·완료 술어 무영향, Undo/Redo). "참이라는 판정이 아니다" 상시 병기 |
| D4 매체 실루엣 | 구현·검증 | 증거함·가설판·영수증 인용 목록에 매체 아이콘(게이트 off면 테두리 칩). 72 px 셀 캡션은 매체명만 — 기록명은 본문 줄 |
| D10 T0 영수증 | 구현·검증 | t0-b3 저장 성공 뒤 작업대 `t0-receipt`. 확정/보류/다음 질문(이름 없음). 재열람 멱등, 저장 바이트 불변 |
| D1 현장 마이크로 과제 | 보류 | F2 안내 대체는 사람 관찰 없이는 근거 부족. 첫 참가자 관찰 뒤 재검토 |
| D5 새 기록 알림 · D6 대조 레일 · D8 질문–기록 관계 · D9 150% 이중 헤더 | 보류 | 조사 보고서 §5·§6 그대로. 사람 데이터 전 착수 안 함 |
| 3개 잠금 확인·벌점·타이머·자동 모순선·자동 근거·자유서술 채점 | 비채택 | 캐논 불변식 위반 |

## QA 루프 (3시간 간격) — 설치 완료

| 항목 | 값 |
|---|---|
| 실행 | `scripts/qa-loop.sh` → EditMode → PlayMode → 격리 boot → macOS QA 빌드(`T0ProjectBuilder.BuildMacQa`, `Builds/qa-loop/Unknown.app`) → 전수 SHA digest |
| 기록 | `scripts/qa-loop-report.py` → `qa/loop/ledger.md`(행 추가) · `ledger.jsonl` · `latest.json` · `latest-triage.md`(실패 테스트 → 프레임 → 소유 레인). 실행 폴더 `unity/Unknown/Builds/qa-loop/<UTC>/`(미추적, 최근 8개 보존) |
| 스케줄 | launchd `io.github.akillness.unknown.qa-loop`, `StartInterval 10800`, `RunAtLoad false`. 설치/상태/해제: `scripts/qa-loop-install.sh [--now\|--status\|--uninstall]` |
| 건너뜀 | 이전 사이클 실행 중(lock) · 다른 Unity 프로세스가 프로젝트를 연 상태 · Unity 부재 · `--dry-run` — 모두 원장에 SKIPPED 행으로 남는다 |
| 경계 | Git add/commit/push 없음 · Assets 편집 없음 · 게이트 승격 없음 · 모델 호출 없음. 릴리스 산출물(`Builds/T0-mac`)을 덮어쓰지 않는다. `QA_LOOP_POST_CMD`는 확장점이며 기본 미설정 |
| 해석 | 같은 HEAD·같은 dirty 수에서 digest만 바뀌면 빌드 비결정성 신호이지 회귀가 아니다. RED 행은 다음 세션이 `latest-triage.md`부터 읽고, 한 세션을 넘겨 살아남으면 `qa/defect-register.md`에 등록 |

수정(fix)은 루프가 하지 않는다. 루프는 **측정과 분류**까지이며, 수정·커밋·배포는 세션(사람 또는 에이전트)이 원장을 읽고 수행한다. 무인 에이전트 수정을 루프에 붙이는 것은 비용·무인 쓰기 위험이 있어 별도 승인 항목으로 둔다.

## 개선 큐 (우선순위 · 근거 · 상태)

| # | 항목 | 근거(관찰) | 상태 |
|---|---|---|---|
| U0 | 3시간 QA 루프 + 원장/분류 | 사용자 지시. 이번 세션 수동 영수증 6회 실행이 모두 사람 손을 탔다 | **적용(이 회차)** |
| U1 | 영수증 키보드 경로 `R` + 안내/조작 문구 | 네이티브 캡처에서 영수증 플레이트 도달에 Tab 탐색 7회. 가설판(H)·안내(F2)와 달리 영수증만 전용 키가 없었다. 인수 완료 전에는 열리지 않고 비공개 상태 문구만 남긴다 | **적용(이 회차)** |
| U2 | 반례 표시 토글의 게임패드 경로 | 현재 가설판 인용별 플레이트는 초점 이동으로 도달 가능(D-pad). 전용 버튼은 불필요 — 관찰 대기 | 보류 |
| U3 | D5 새 기록 알림(1줄 상태문) | 조사 §5. 어느 기록이 새로 열렸는지 사람 관찰 없이 문구 설계 불가 | 보류 |
| U4 | D9 150% 이중 문서 헤더 | 조사 §6. M21 판독 배경 계약과 충돌 여부 먼저 검토 | 보류 |
| U5 | 빌드 비결정성 조사 | 캡션 문자열만 바뀐 재빌드가 +27 B. QA 루프 원장이 같은 HEAD에서 digest 변화를 누적 관찰하면 착수 | 관찰 |
| U6 | Windows 빌드 | Unity Hub에 `WindowsStandaloneSupport` 모듈 부재. 설치는 사용자 결정 | 차단 |

## 이번 회차에 적용한 것 (U0 · U1)

- U0: 위 표. 첫 사이클 결과는 `qa/loop/ledger.md` 첫 행.
- U1: `WatchBindings.json`에 `Overlay` 액션 `<Keyboard>/r` (`receipt`), `WatchInput` 역할 분기, `OpenOverlay`의 영수증 게이트(t0-b3 미완료면 `receiptNotYet` 상태문만), `controls`·`guideControlsDetail` 문구, 테스트 `RKeyOpensReceiptOnlyAfterCompletion`.

## 다음 세션이 할 일

1. `qa/loop/ledger.md` 최신 행을 읽는다. RED면 `latest-triage.md`의 프레임에서 시작한다.
2. GREEN이 이어지면 U2~U4 중 사람 관찰이 생긴 항목만 착수한다. 관찰 없이 착수하지 않는다.
3. 같은 HEAD에서 digest가 3회 이상 바뀌면 U5를 연다.
