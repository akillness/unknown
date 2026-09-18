---
updated: 2026-09-18
cycle: 20260918-content-update-m25
status: current
supersedes: null
owner: game-production-director
---

# M25 회고 — Higgsfield 전 리소스 제작·적용, 안내 오버레이, 가독성, 빌드·배포

## 측정값

| 항목 | 값 | 출처 |
|---|---|---|
| 생성 | 이미지 18 + 반려 2, 영상 3 (Higgsfield CLI 1.1.25) | `concept/m25-higgsfield-jobs.json`, `assets/generated/{2d,video}/m25/provenance.json` |
| 크레딧 | 견적 합 159.0 · 자체 구간 차액 199.0 · 세션 밖 소모 ≥104.5 관측 | `systems/tech-verification/m25/verification.json` |
| 적용 | 임포트 18항목(SHA 대조) · 로컬 개발 프로필 승인 | `m25/import-audit.json`, decision-log 승격 감사 |
| 테스트 | EditMode 65/65 · PlayMode 134(133/0/1 조건부 skip) · 격리 boot 1/1 · 신규 6/6 | `m25/{editmode,playmode,boot}.xml` |
| 빌드 | 316파일 · 438,910,587B · digest `aaa5361d…` | `m25/build-inventory.json` |
| 네이티브 | 실제 창 캡처 5장, 오프닝 클립 재생 관측 | `docs/media/m25/` |
| 가독성 | TypeScale 8단(30/24/21/18/18/17/15/14) + 행간 1.15 | `UI/TypeScale.cs`, 계약 테스트 |

## 무엇이 바뀌었나 (Ground)

- 오프닝은 Higgsfield 은포항 야경 정지 이미지 + 5 s 클립(모션 축소·batchmode·실패 시 정지 이미지). 시작 화면 내비게이션 패널에 당직실 배경. 도구 휠·안내에 도구 아이콘 6, 안내에 인물 카드 2·구역 도판 4.
- `guide` 오버레이(F2·툴바·시작 화면)가 목표·세 단계·도구 절차·조작·규칙을 한 화면에 모은다. 회로/판독 안내 접두가 단계·남은 조건을 명시한다.
- 텍스트 계층이 이름 있는 상수로 고정됐고 제목·섹션·상태는 굵게, 헬퍼는 톤 다운.
- 배포 경로가 생겼다: `main` push + GitHub Release prerelease(미서명 개발 빌드 zip).

## 배운 것

- Higgsfield `gpt_image_2`는 "no signage" NEGATIVE만으로 작은 명판을 막지 못한다. SUBJECT에 「기둥은 맨 콘크리트」처럼 **표면 상태를 긍정 서술**해야 사라졌다(2회 재생성).
- 병렬 생성 시 `account status` 차액은 작업에 귀속할 수 없다. 영수증은 **견적 합 + 구간 차액 + 외부 소모 관측**으로 적는다.
- 배치모드 PlayMode에서 키 입력 테스트는 `editorInputBehaviorInPlayMode=AllDeviceInputAlwaysGoesToGameView` + `backgroundBehavior=IgnoreFocus`가 둘 다 필요하다(기존 T0PlayModeTests 관례).
- Unity 플레이어의 저장된 창 크기는 `-screen-width/-screen-height` 인자로 덮을 수 있고 Retina에서 창 pt×2 = 캡처 px.

## 미해결 리스크

- 상업 사용권 UNVERIFIED(모든 생성 백엔드). 사람 플레이 n=0, 성능·Windows 미측정.
- 문재화·오은정·표성찬 초상은 임포트됐으나 표시 슬롯이 없다(C1 인터뷰 화면은 익명 유지). 후속 회차에서 공개 상한에 맞춰 배치.
- 안내 본문의 세로 스크롤은 마우스 휠·Tab 초점 이동에 의존한다. PageDown 바인딩은 없다.
- `graphify update`가 이전 세션과 같이 실패하면 G8은 PARTIAL로 남는다(아래 memory_sync).

## 다음 진입 결정

- 다음 회차는 사람 플레이테스트(n≥3) 또는 Windows 빌드 중 하나를 먼저 연다. 리소스 추가 생성보다 측정이 우선이다.

## memory_sync

- ROUTER `Current Project State` 갱신, `.mex/events/m25-higgsfield-20260918.md` 기록.
- mex-agent: 세션 시작 시 `skipped`(PATH `mex`는 TeX) — 이번에도 실행하지 않았다.
- graphify / zg / vault: 이 파일 작성 후 실행 결과를 `retrospectives/m25-higgsfield-resources-20260918.md` 하단 「실행 영수증」에 추가한다.

## 실행 영수증 (memory_sync)

- `freshness-check.sh`: 구조 검사 0 findings / 696 artifacts (exit 0). `--since 2026-09-18`은 631 findings — 이번 사이클이 손대지 않은 산출물의 `updated`가 사이클 시작 이전이라는 뜻이며 M25 범위의 결함이 아니다. **G8 = PARTIAL**(구조 PASS, 시점 신선도 전량 미갱신, mex skipped, zg 실패).
- graphify: `graphify update .` **성공** — 45,902 nodes · 51,485 edges · 4,872 communities, `graphify-out/graph.json`·`GRAPH_REPORT.md` 갱신(이전 세션의 ModuleNotFoundError는 재현되지 않았다).
- zg: `zg index`(증분) 실패 `[ZVEC_INTERNAL_ERROR] FtsRocksdbReducer: source postings is not BitPacked`, 이후 `zg status`가 `Failed to open zvec collection storage`를 돌려준다. 공유 데몬(pid 7334, 다른 하네스 소유)이 살아 있고 인덱스 재생성은 사용자 승인 사항이라 `--rebuild`/`--drop`을 실행하지 않았다. **후속: 사용자 승인 후 `zg index --rebuild`.**
- mex-agent: skipped (PATH `mex`는 TeX). `.mex/ROUTER.md` 상태와 `.mex/events/m25-higgsfield-20260918.md`는 직접 갱신.
- vault: Obsidian 미실행 → 직접 쓰기 폴백. `wiki/reports/2026-09-18-unknown-m25-higgsfield-resources.md`, `wiki/projects/unknown/decisions.md` D-019, `index.md` 한 줄, `log.md` 한 줄.
