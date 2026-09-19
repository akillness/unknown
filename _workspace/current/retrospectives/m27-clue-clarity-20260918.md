---
updated: 2026-09-18
cycle: 20260918-balance-patch-m27
status: current
supersedes: null
owner: game-production-director
---

# M27 회고 · 실마리 명료화 (RFC-CX-M27-20260918)

## 한 줄

"클루 명확·확장 가능·중간 지점·진행 보장/가이드·밸런스·리소스 UI"를 **데이터 계약 하나**(요구조건 라벨·캡션·단계 → 검증기 T0-06)와 **투영 하나**(`IsSatisfied` 위의 체크리스트)로 풀었다. 카드·안내·부제·복원 상태문이 같은 투영을 읽고, 런타임에 비트 리터럴을 더하지 않았다. 사람 플레이 n=0이므로 재미·이해 개선을 주장하지 않는다.

## 잘 된 것

- **조사를 먼저 세 갈래로 나눠 읽었다.** 캠페인 데이터·런타임·밸런스/UI 감사가 각각 "카드 카운터가 t0-b3에 고정", "`IsSatisfied`가 이미 공개 API", "밴드 하나 추가는 작업면을 줄인다"를 짚어 줬고, 그대로 설계 근거가 됐다.
- **확장성을 검증기에 넣었다.** 새 비트의 술어에 라벨이 없으면 `--t0` 6/6이 아니라 FAIL이다. 카드 금지어 목록을 검증기와 테스트가 같은 정본으로 본다.
- **QA 루프가 제 몫을 했다.** 첫 GREEN 뒤 넣은 밴드가 회귀 3건(클릭 좌표 이동 ×2, 판독면 대비)을 냈고 루프가 잡아 같은 세션에 고쳤다. 사람이 전체 스위트를 돌리지 않았는데도.
- **결손 끝점 규칙을 값 없이 말했다.** "시작 = 첫 결손 눈금, 끝 = 기록이 돌아온 첫 눈금"은 절차이지 정답이 아니다.

## 실패와 배운 것

- **내 python 치환이 테스트 헬퍼를 자기 재귀로 만들었다** (`void Start(){Start();}`) → Unity가 시작 직후 SIGSEGV(스택 오버플로), 원인 추적에 여러 실행을 썼다. 배운 것: 리터럴 치환은 정의부를 먼저 제외한다; "실행 직후 139 + 반복 프레임" = 관리 코드 재귀부터 의심.
- **정지 락파일.** 크래시가 남긴 `Temp/UnityLockfile`이 다음 실행도 죽였다. QA 루프의 에디터 열림 검사는 프로세스+락파일을 함께 보므로 무사했지만, 수동 실행은 락파일을 먼저 지운다.
- **밴드를 더하면 어디선가 빼야 한다.** 작업면 .05를 잃자 서명지 클릭 테스트가 좌표로 실패했다. 카드 슬랙(.185→.15)으로 되돌렸다.
- **AspectRatioFitter는 부모를 채운다.** 칩 안에서 글리프가 중앙에 겹쳐 그려졌다. `InquiryFigure`처럼 셀 → 자식 구조로 고쳤다. 첫 캡처가 아니었으면 출고됐을 것이다.

## 경계(측정 안 한 것)

사람 플레이 n=0 · 수치 밴드 무변경 · 성능 · Windows 모듈 부재 · 상업 사용권 UNVERIFIED · L1 힌트 문면 규칙(C6-F3)·`revealsValues` 기계 판정 보류 · 빌드 비결정성(같은 트리 재빌드 +28 B) 관찰 계속.

## memory_sync

- freshness-check: 사이클 시작 기준 실행(결과는 ROUTER "Receipts").
- mex-agent: `skipped`(PATH `mex`는 TeX) — 변경 없음.
- vault: Obsidian 미실행 시 직접 파일 쓰기 폴백(`wiki/reports/2026-09-18-unknown-m27-clue-clarity.md`, `decisions.md` D-022, `index.md`, `log.md`).

## 실행 영수증 (memory_sync)

- `freshness-check.sh --since 2026-09-18` 실행(결과 ROUTER Receipts 참조). mex-agent skipped. vault 직접 파일 쓰기(`wiki/reports/2026-09-18-unknown-m27-clue-clarity.md`, D-022, index, log).

