---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-systems-designer
describes: _workspace/current/systems/prototype/
---

# prototype/ 메타 — 실행 영수증과 정직성 경계

## 소유·출처

- 소유 레인: `game-systems-designer`.
- 규칙 출처: `_workspace/current/systems/interaction-rules.md` (§0 전역 불변식, §2.3 조위정합,
  §2.5 부식예산, §2.6 이중서명, §3 출처 독립성, §5 확정→결과→체크포인트, §6 저장·복구, §7 엔딩).
- 세계 출처: `_workspace/current/worldview/worldview-bible.md` (§2 기록의 물리 — 매체 3종·분해능 4분·
  정합 후 ±4분·오차띠 겹침 unknown, §3 불가침 6법).
- 시나리오 출처: `_workspace/current/planning/campaign.json` 의 **T0**(`t0-b1`~`t0-b3`)와
  **C3**(`c3-b1`~`c3-b4`)에서 **규칙 부분만** 추출. 서사·대사·구역·연출은 이식하지 않았다.
- 형제 문서: `systems/game-ui-contract.json`, `systems/unity-implementation.md`.

## 해시 (2026-09-10 재측정)

명령: `cd _workspace/current/systems/prototype && shasum -a 256 model.mjs test-model.mjs build-prototype.mjs README.md && wc -c && wc -l`

| 파일 | sha256 [OBSERVED 2026-09-10] | 크기 | 09-09 대비 |
|---|---|---|---|
| `model.mjs` | `53a9d3a4244189bd3ebad9299d097b8897ea77f9393e20ea20c6bc91faca0d37` | 35,149 bytes / 762 lines | **변경됨** (이전 `10c13ac9…` / 35,128 / 762 — C3-F36 라벨 1줄 교체, 아래 절) |
| `test-model.mjs` | `c8a133cf97770e801f006aed4333c3eff95b24a16d781fd6dbae9d4589321b45` | 26,116 bytes / 544 lines | 동일 |
| `build-prototype.mjs` | `cfa2437ebeec9a2b008a3761c05bb972bb455b111f1581f5dd9a52e8786792d6` | 28,420 bytes / 527 lines | **변경됨** (이전 `33bb4e1e…` / 28,358 / 525) |
| `README.md` | `89a7ff4bc55d5fabf388565855efe4f25cd7b960dfb3a0269c0eb2839ae8d4c3` | 6,824 bytes | 동일 |
| `artifacts/interaction-prototype.html` | — | — | **부재** |

**두 건의 드리프트를 관측한 그대로 적는다 [OBSERVED 2026-09-10]:**

1. `build-prototype.mjs`가 2줄·62바이트 늘었고 해시가 바뀌었다. 본 레인이 이번 회차에 이 파일을 편집하지 않았으므로 다른 세션의 변경으로 본다. **변경 내용은 확인하지 않았고 되돌리지도 않았다**(CLAUDE.md §8).
2. `artifacts/interaction-prototype.html`이 현재 트리에 **없다**(`find _workspace -name interaction-prototype.html` → 0건). 아카이브에도 없다. 따라서 아래 "HTML 산출물 자체 검사"·"브라우저 기능 확인" 절의 수치는 **2026-09-09 실행 시점의 [CARRIED] 관측**이며, 현재 파일로 재현되지 않는다.

**재현 절차**: 아래 검증 실행을 다시 돌리려면 `node build-prototype.mjs`를 먼저 실행해 산출물을 재생성해야 한다. 재생성하면 `build-prototype.mjs`가 바뀌었으므로 HTML 해시도 달라질 수 있다. 생성기는 `model.mjs` **원문 바이트를 그대로** HTML 에 넣고 포함 여부를 검사하므로(`원문 바이트 일치`) 두 해시는 서로 종속이다.

**본 회차(09-10 R4) 재실행 여부**: `node test-model.mjs`는 **재실행했다**(아래 C3-F36 절 · 37 통과 / 0 실패 / exit 0 [OBSERVED 2026-09-10]). `node build-prototype.mjs`는 재실행하지 않았으므로 HTML 산출물 관련 수치는 여전히 `[CARRIED]`다.

## C3-F36 · `EVENT_PAIRS` 라벨 정본화 [OBSERVED 2026-09-10, R4]

| 항목 | 내용 |
|---|---|
| 대상 | `model.mjs` L58 |
| 이전 | `{ id: 'pair-20', label: '봉인 호출 ↔ 밸브 대기', gapMin: 20 }` |
| 이후 | `{ id: 'pair-20', label: '밸브 개폐 각인 ↔ 봉인 완료 접점 각인', gapMin: 20 }` |
| 근거 | `production/decision-log.md` 「잔여 S2/S3 배정」 C3-F36 · RFC-P3-013 + C3-F25(밸브 개폐 H-1:24 → 봉인 완료 접점 H-1:04, 간격 20분) · `worldview/timeline.md` §8 앵커 표 |
| 불변 | `id`(`pair-20`) · `gapMin`(20) · 배열 순서 · 테스트 코드 0줄 변경. 테스트는 `EVENT_PAIRS.length`(2)와 `byId['pair-20'] === 'ordered'`만 보고 라벨을 보지 않는다(`test-model.mjs` L61·L226) |
| 검증 명령 | `cd _workspace/current/systems/prototype && node test-model.mjs` |
| 결과 | **37 통과 / 0 실패 · exit 0** [OBSERVED 2026-09-10]. 편집 전 실행과 출력 전문 비교 시 **차이는 전수 탐색 소요 ms 1행뿐**(24947 ms → 21717 ms, 실행시간 변동) — 상태 2,097,216 · 전이 73,402,560 · 불변식 위반 0 · 차단 사유 21종 전부 동일 |
| 남는 것 | 이 라벨은 UI 문자열이 아니라 프로토타입 내부 표시명이다. Unity 런타임 문자열은 로컬라이즈 키로 가며 이 파일을 참조하지 않는다 |

**폐기 문구 보존 주의 [OBSERVED 2026-09-10]**: 위 표의 「이전」 행은 폐기 라벨의 **기록 보존**이며 본문 사용이 아니다(`worldview/consistency-audit.md` 「사용 금지 문구」 절과 같은 원칙 — 삭제하지 않고 재유입 방지용으로 남긴다). 재측정:

```
grep -rl "봉인 호출 ↔ 밸브 대기" _workspace/current/systems | wc -l   # → 1 (본 파일, 기록)
grep -rn "봉인 호출 ↔ 밸브 대기" _workspace/current/systems/*.md _workspace/current/systems/system-specs _workspace/current/systems/prototype/*.mjs  # → 0행
grep -rl "봉인 호출 ↔ 밸브 대기" _workspace/current                    # → 4파일: 본 파일 + qa/{c3-review,defect-register,gate-measurements}.md (전부 기록)
```

**코드·스펙 본문 잔존 0행**이며, 회귀 신호로 쓸 축은 "`.mjs` + `system-specs/` 0행" 하나다(총 파일 수는 기록 문서가 늘면 증가한다 — `qa/c3-review.md` §9.4의 경고와 같은 성질).

## 검증 실행 `[CARRIED — 2026-09-09 실행 · test-model.mjs 만 2026-09-10 R4 재실행]`

```
node --version            # v26.5.0
node test-model.mjs       # exit 0
node build-prototype.mjs  # exit 0
```

실행 환경: macOS, 네트워크 접근 0회, 패키지 설치 0회, 유료 도구 0개, `node:` 표준 라이브러리만 사용
(`node:assert/strict`, `node:fs/promises`, `node:crypto`, `node:path`, `node:url`).

### 테스트 결과 `[CARRIED]`

| 항목 | 값 |
|---|---|
| 테스트 | **37 통과 / 0 실패** (exit 0) |
| 도달 가능한 상태 | **2,097,216개** (큐 소진 · 잘림 없음) |
| 검사한 전이 | **73,402,560개** (허용 39,680,896 / 차단 33,721,664) |
| 불변식 | **9개 × 2,097,216상태 = 위반 0건** |
| 차단 사유 코드 | 21종 **전부** 실제 발생 (미발생 0종) |
| 서명 도달 상태 | 2,304개 · 근거 쌍 3종 · 전부 독립·배선 안 |
| 확정 도달 상태 | 저지대 233,024 · 부두 233,024 · **부식 초과안 0** |
| 저장 왕복 · 초기화 보존 표본 | 21,620개 (간격 97, 결정적 표본) |
| 전수 탐색 소요 | 21.6초 (전체 테스트 약 40~45초) |

### 불변식 9개

`INV1` 판독한 필수 단서 보존 · `INV2` 엔딩 3종 항상 선택 가능 · `INV3` 부식 초과 확정 없음 ·
`INV4` 프리뷰 없는 확정 없음 · `INV5` 배선 안 독립 2종 + 확정 기준선 없는 서명 없음 ·
`INV6` 피크 3개 + 잔차 ≤4분 없는 기준선 확정 없음 · `INV7` 선후 판정은 오차폭 규칙과 일치 ·
`INV8` 재산 보호 플래그는 확정 경로와만 일치 · `INV9` 피크 ≤3 · 슬롯 ≤2.

### 규칙 → 관측 대응

| interaction-rules.md | 모형이 낸 관측 |
|---|---|
| §2.3 잔차 4분 이하에서만 기준선 확정 | 정상 3피크 잔차 **3분** 확정 / 거짓 피크 포함 시 `RESIDUAL_OVER` |
| §2.3 피크 3개 필요 | 2피크는 잔차가 작아도 `PEAK_COUNT_SHORT` 로 차단 |
| §2.3 20분 간격은 확정, 겹치면 unknown | `pair-20=ordered` · `pair-6=unknown` (오차폭 합 8분) |
| §3 사본은 같은 출처 1개 | 5점 중 유효 근거 쌍 **정확히 3쌍**, 사본 쌍은 `SAME_ORIGIN` |
| §2.6 독립 매체 2종 | 다른 출처라도 같은 종류면 `SAME_MEDIUM` |
| §2.1 배선 범위 밖 근거 무효 | `OUT_OF_WIRING` · 배선 미확인은 `WIRING_UNCHECKED` |
| §2.5 초과 구성은 확정 전에 막힌다 | 부식 12 / 한도 9 → `CORROSION_OVER`, 확정 도달 상태 0개 |
| §5 확정 전 항상 프리뷰 | `NO_PREVIEW` · 경로 변경 시 프리뷰 무효화 |
| §0.3 필수 단서 불멸 | 되돌림·초기화 후에도 사본 5점 · 필수 단서 3/3 잔존 |
| §0.5 §0.6 §7 엔딩 3종 | 보호 플래그 3값 × 엔딩 3종 = 9조합 전부 선택 가능 |
| §6 손상 세이브는 덮어쓰지 않는다 | `CHECKSUM_FAILED` + 복구 3선택지, 현재 상태 무변경 |
| §6 미등록 상위 스키마 거부 | `SCHEMA_TOO_NEW`, 현재 상태 무변경 |

### HTML 산출물 자체 검사 `[OBSERVED]`

생성기가 매 실행마다 검사해 실패 시 exit 1 로 끝난다. 이번 실행은 전부 통과:
상시 배너 문구 존재 · 외부 `script src` 0 · 외부 스타일시트 0 · `img`/`iframe`/`video`/`audio` 0 ·
`@import` 0 · 원격 `url()` 0 · `fetch`/`XMLHttpRequest`/`WebSocket`/`importScripts` 0 ·
`alert`/`confirm`/`prompt` 0 · `autoplay` 0 · `lang="ko"` 존재 · 모형 원문 바이트 일치.

### 브라우저 기능 확인 `[OBSERVED]` — 시각 검수 아님

로컬 탭에 산출물을 주입해 **실행 여부만** 확인했다. 콘솔 오류·경고 **0건**,
6개 단계 섹션 전부 렌더(빈 섹션 0), 버튼 36개, range 1개, radiogroup 2개, 상태판 15행.
조작 시연에서 차단 사유가 화면 문자열로 그대로 표시됨을 확인했다:
`SEAL_INCOMPLETE` / `NO_ROUTE` / `RESIDUAL_OVER` / `CORROSION_OVER` / `SAME_ORIGIN` /
`SAME_MEDIUM` / `OUT_OF_WIRING` / `CHECKSUM_FAILED`(복구 3선택지 동반).
되돌림·초기화 후 상태판에서 사본 5점 · 필수 단서 3/3 · 선택 가능 엔딩 3종 유지 확인.

대비 실측(계산값, `getComputedStyle` 기반): 본문 16.26:1 · 배너 10.54:1 · 보조문구 7.94:1 ·
시폼 라벨 10.39:1 · 차단 사유 블록 14.81:1. 최소 글자 크기 **13.12px**(단계 번호 배지), 본문 16px.
포커스 가능한 컨트롤 37개, 상시 배너 렌더 확인, `aria-live="polite"` 상태 영역 존재.

`NOT-MEASURED` — 스크린 리더 실사용, 확대 400%, 키보드 전용 완주, 모바일 폭, 색각 이상 실사용,
성능·프레임·로드 시간. **시각 디자인 합격 판정은 하지 않았다.** 브라우저 검수는 상위 담당의 몫이다.

## 프로토타입 한정 상수 — 스펙과 다른 값 `[C3-F23 · RFC-P3-015]`

| 상수 | 프로토타입 값 | 스펙 값 | 왜 다른가 |
|---|---|---|---|
| `LIMITS.maxUndo` | **32** (`model.mjs:82`) | **상한 없음** | 전수 탐색기가 상태 큐를 유한하게 유지하기 위한 **탐색 편의 상수**다. 게임 스펙이 아니다 |

**정확한 동작** [OBSERVED: `model.mjs:530-532`]:

```js
function pushPast(past, core) {
  const next = [...past, cloneCore(core)];
  return next.length > LIMITS.maxUndo ? next.slice(next.length - LIMITS.maxUndo) : next;
}
```

즉 `maxUndo`는 되돌림 **이력 링의 길이**이며, 초과 시 **가장 오래된 항목을 버릴 뿐 되돌림을 거부하지 않는다**. 32단을 넘겨도 조작은 계속 되고 실패 코드도 발생하지 않는다. 33번째 이전으로 가고 싶을 때는 **체크포인트 재로드가 상시 가능**하므로(§`interaction-rules.md` §5·§6, `system-specs/save-undo.md`) 진행이 막히는 경로는 없다.

**정본 규정 (RFC-P3-015 F23)**: **스펙상 되돌림은 무제한이며, 체크포인트 재로드는 언제나 가능하다.** `maxUndo: 32`는 프로토타입 한정이고, Unity 구현·핸드오프 브리프에는 **"되돌림 상한 없음(체크포인트 재로드 항상 가능)"**으로 적는다. 이 상수를 게임 사양으로 인용하는 문서가 있으면 결함이다.

**미탐색으로 남는 것**: 32단 링에서 잘려나간 이력이 프로토타입의 전수 탐색 결과에 영향을 주는지는 확인하지 않았다. 되돌림·초기화는 새 코어 상태를 만들지 않아 탐색 알파벳에서 제외했기 때문이다(아래 정직성 경계 참조). Unity 구현에서 상한이 사라지면 이 항목 자체가 소멸한다.

## 관측과 목표의 구분

| 구분 | 항목 |
|---|---|
| `[OBSERVED]` | 2026-09-10 재측정한 4개 파일의 해시·크기, `model.mjs` 소스 인용(`:79` `:82` `:530-532` `:687`), HTML 산출물 부재 |
| `[CARRIED]` | 2026-09-09 실행분 — 테스트 수치, 상태·전이 개수, 차단 코드 21종 발생, Node v26.5.0, 콘솔 오류 0건, 대비 계산값. **현재 트리로 재현하지 않았다** |
| `[TARGET]` | 이 모형이 반영한 `interaction-rules.md` 의 확정 조건 자체(설계안이며 플레이 검증 0건) |
| `NOT-MEASURED` | 재미, 난이도, 학습 곡선, 플레이 시간, 조작감, 성능, 접근성 실사용, 실제 Unity 구현 가능성 |

## 정직성 경계

- **이 모형은 게임의 존재를 주장하지 않는다.** Unity 빌드 0건, 씬 0개, 에셋 0개, 플레이 표본 0건.
- **모형의 통과는 캠페인의 통과가 아니다.** 덮는 범위는 T0·C3 에서 뽑은 규칙으로 구성한
  가상의 훈련용 작업대 1개다. 8장 전체의 진행 불가 문제가 해결됐다는 주장이 아니며,
  `campaign.json` 의 나머지 스테이지는 이 모형이 건드리지 않았다.
- **플레이 시간에 대한 주장 없음.** `designMinutes: 480` 은 설계값이고 `observedMedianMinutes` 는
  여전히 `null`, `humanPlaytests` 는 여전히 `[]` 다. 이 작업은 그 값을 바꾸지 않았다.
- **퍼즐 깊이에 대한 주장 없음.** 여기 있는 것은 확정 조건의 참·거짓 판정뿐이고 설계의 질이 아니다.
- **전수 탐색의 경계를 명시한다.** 되돌림·초기화·저장/불러오기는 새로운 코어 상태를 만들지 않아
  탐색 알파벳에서 제외하고 표적 테스트로 덮었다. 오프셋은 대표값 4개 + 자동 맞춤만 전개했다.
  즉 "모든 상태"는 **정의된 조작 알파벳 35개 기준**이며 무한한 연속 입력 전체가 아니다.
- **저장 왕복·초기화 보존은 전수가 아니라 표본 21,620개**다(간격 97의 결정적 표본).
  불변식 9개만 2,097,216 상태 전수 검사다.
- **비용·수익 모형과 분리**되어 있다. 이 폴더에 금액·요율·매출 추정은 하나도 없고
  `product/economics.json` 을 읽지도 쓰지도 않았다.
- 네트워크·설치·계정·git commit/push **모두 미실행**. 워크스페이스 밖 쓰기는 지정된 artifacts 경로 1개뿐이다.
- 다른 레인의 소유 파일은 읽기만 했고 수정하지 않았다.

## 다음 소유자

`game-qa` 독립 검토(브라우저 접근성·키보드 완주·스크린 리더)와 Unity 구현 담당.
이 모형은 구현 시 **회귀 기준**으로 쓸 수 있다 — 확정 조건을 바꾸려면 `model.mjs` 의 불변식이 먼저 깨져야 한다.
