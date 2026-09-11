---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: _workspace/archive/20260909-preproduction-c2/production/premium-preproduction-contract.md
owner: game-production-director
---

# Premium Preproduction Contract (C3 개정)

## Scope
[OBSERVED] 사용자 요청(2026-09-09, 1차): Steam 최신 인기 경향 검토, Unity, 8시간 완결형 본편, 원화 출시 정가 최소 9,000원, 인터랙티브한 플레이, 세계관·서사·연출·UI·튜토리얼·DLC, 최소 5회 실제 기획 개선 사이클, 등록/수익화 HTML 슬라이드.

[OBSERVED] 사용자 요청(2026-09-09, 2차 — 이번 세션): 규칙 확인 → 워크스페이스·리서치 기반 **게임 초안 개발** → **5회 리뷰·개선** → 이후 Codex(GPT-6 Astra)에게 Unity 개발·검증 위임. 리소스는 Higgsfield MCP·"maxiam"·Blender MCP로, 2D 리소스는 "awesome gpt"·GTI로 **전부** 제작. Unity 프로젝트는 **이 저장소 안**에 만들고 git push 예정. 루트 README에 게임 소개와 플레이 컷씬 GIF·이미지를 등록.

[INFERENCE] "maxiam"은 로컬 스킬·MCP 어디에도 없다. 리깅·모션 문맥상 Adobe **Mixamo**(웹, API 없음)로 해석하고 수동 단계로 남긴다. "awesome gpt"는 로컬 `awesome-agent-skills`(ux-designer/content-creator 페르소나) + GTI(`god-tibo-imagen`, Codex 백엔드 이미지 생성)로 해석한다. 두 해석 모두 사용자가 정정하면 바꾼다.

[OBSERVED] baseline b79577b에는 하네스만 있고 게임·시장·플레이 측정은 없다. C1(시장·범위)·C2(인과·서사)는 `_workspace/archive/20260909-preproduction-c{1,2}/`에 있고, C2 산출물(gdd·세계관·연표·용어집·본 계약)은 `status: superseded`로 아카이브됐으나 `current/`에 후속본이 없었다. C3가 후속본을 재도출한다(supersedes 연결).

## Cycle plan (이번 세션: 5회 리뷰·개선 = C3~C7)
기존 preproduction 정의(C1~C5)를 지우지 않는다. C1·C2 완료분을 인정하고, 사용자의 "5회"를 이번 세션에서 **C3~C7 다섯 회차의 독립 검토·수정**으로 충족한다. 회차마다 입력·산출·검토 발견·수정·잔여 위험·플레이타임 논의가 파일로 남는다.

| 회차 | 이름 | 개발 레인 | 독립 검토 | 닫는 질문 |
|---|---|---|---|---|
| C3 | 캠페인·시간·서사 비트 | worldview, synopsis, planner, systems, balance, economy | QA + 반박 3렌즈(도달성/시간착시/서사모순) | 33비트가 480분 설계 예산과 행동 예산으로 역산되는가 |
| C4 | 상호작용·연출·Unity 슬라이스·에셋 | presentation, concept, modeling, animation, motion, vfx, systems, planner | QA + 반박 3렌즈(접근성/에셋예산/sim-render) | 20~30분 슬라이스가 측정 가능한 인수 기준을 갖는가 |
| C5 | 상품·생산·회귀 | product-manager, planner, director, qa | QA 전문서 회귀 + 반박(가격/생산량/일관성) | 문서 간 숫자·이름 모순이 0인가 |
| C6 | 통합 게임 초안 v1 | planner(편집) + director | 5렌즈 판정단(플레이어/퍼블리셔/엔지니어/서사/프로듀서) | 한 문서로 게임을 설명·판단할 수 있는가 |
| C7 | Unity 핸드오프·리소스 파이프라인 | systems, modeler, concept, presentation | Codex-준비도 반박 + QA | 외부 실행자가 추가 질문 없이 착수할 수 있는가 |

## Honesty gates
문서 검증 D1~D5와 실제 게임 G1~G8은 다르다. 시간 합계가 480분이어도 G7 플레이 검증을 PASS하지 않는다. 실제 빌드가 없으면 G2/G4/G5/G6/G7은 NOT-MEASURED. PM의 수익/판매/위시리스트/전환율은 목표·가정이며 관측치로 쓰지 않는다. 시장성은 매출 보장이 아니다. **생성된 프리비즈 GIF·이미지는 "게임플레이"가 아니며 README에 그렇게 표기한다.**

## Premium overrides
전투 승률·TTK·유료 무료 격차·일간 인플레이션은 비전투 완결형 게임에 부적합하다. 전투가 없다는 확인이 있을 때만 N/A로 명시하고, 퍼즐 도달 가능성·진행 막힘·선택 상태·무료 힌트·본편 완결성·DLC 의존성 검증으로 대체한다. 누락을 PASS로 위장하지 않는다.

## Time acceptance (C1 F4 · RFC-P3-011 반영)
[TARGET] 설계 분량 합계(`designMinutes` = 480, 문서 상수)와 관측 완주 통계(`observed_*`)는 **다른 키**다. 판정 키는 `total_minus_afk_min`(AFK 구간은 회고로 판별, 60초 무입력을 자동 제외하지 않음). **목표 밴드**: 처음 플레이한 일반 이용자(공략 없음)의 완주 중앙값 **450~540분**(세션 P 결정 유지). **철회 트리거**(통과선이 아니다): 중앙값 < 420 또는 하위 25% < 360이면 "8시간" 주장을 철회하거나 콘텐츠를 증설한다 — 목표를 낮춰 자동 통과시키지 않는다. `fastMinutes` 합(322)·`deliberateMinutes` 합(673)은 시나리오 경계이며 표본 통계와 비교하지 않는다(p75 조건 없음). 표본 최소 12명/5유형, 탈락 포함 보고. 선택 콘텐츠·NG+·수집 100%·로딩·일시정지·자리비움·재시작 대기는 본편 목표에 합산하지 않는다. 근거: `production/decision-log.md` RFC-P3-011.
## Unity / 저장소 배치 (2차 요청 반영)
- Unity 프로젝트는 `unity/Unknown/` (저장소 내부, 코드네임 = 저장소명). 가제 "조수기록국"/"TIDE ARCHIVE"는 상표·동명 확인 전 폴더명·번들명·상점명에 쓰지 않는다.
- Unity **6000.5.6f1**(설치 확인됨)로 생성. 2022.3 LTS는 쓰지 않는다. `Library/ Temp/ Logs/ obj/ UserSettings/`는 .gitignore.
- 실행자: Codex(GPT-6 Astra)는 `_workspace/current/handoff/`의 브리프만 읽고 착수한다. 브리프에 없는 결정은 RFC로 되묻는다. 시스템 레인 코드 규칙(검색 우선, 그래프 갱신, sim/render 분리, 데이터 테이블 튜닝)은 Codex에게도 적용된다.
- git commit/push: 사용자가 직접 수행한다. 하네스는 push-ready 상태(.gitignore, README, 미디어 등록)까지만 만든다.

## Asset pipeline (2차 요청 반영)
| 종류 | 도구 | 상태(2026-09-09 관측) | 규칙 |
|---|---|---|---|
| 2D 전부(컨셉·UI·텍스처·키아트·캡슐·README 이미지) | GTI `gti` CLI (Codex 백엔드, `~/.codex/auth.json` 존재) | 설치됨. 비공식 백엔드, 중단 가능 | 프롬프트는 `concept/style-guide.md` 준수, 산출은 `assets/generated/2d/` + `provenance.json` |
| 2D 보조/영상/3D 메시 | Higgsfield CLI `higgsfield` | 설치됨. 이 프로젝트에 MCP 미등록(Abyssal-Surge에만 등록). 인증·크레딧 미확인 | 크레딧 소모는 실행 전 `higgsfield account status`로 확인, 실패 시 skipped 영수증 |
| 3D 블록아웃·렌더·GIF 프레임 | Blender MCP (Blender 실행 중, 애드온 구버전·폴백 동작) | 연결됨 | 파괴적 조작 금지, 씬은 새 .blend에 저장 |
| 휴머노이드 리깅·모션 | Mixamo(웹) | 수동. API 없음 | `animation/rig-requirements.md`가 Mixamo 호환 규격을 명시, 다운로드는 사용자 |
| README 컷씬 GIF | GTI 프레임 + ffmpeg/ImageMagick(설치됨) 또는 Blender 렌더 | 가능 | 파일명·캡션에 `previz`(프리비즈) 명시, "gameplay" 금지 |

모든 생성 에셋은 `runtimeEligible:false`로 시작하고 `provenance.json`(도구·모델·프롬프트·해시·날짜·라이선스)을 동반한다. 승격은 decision-log 감사로만.

## Base production gate (C6-F9 통합 · 2026-09-10)
본 생산(T0 이후 콘텐츠 생산)은 다음 **네 조건이 모두** 파일로 증명될 때만 시작한다. 어느 하나라도 없으면 시작하지 않는다.
1. **T0 사람 검증 통과** — `handoff/verification-plan.md` H-1(첫 조작 ≤ 60초 중앙값)·H-2(목표 설명 10/12)·H-3(진행 불가 0/12), 표본 12명/5유형, 탈락 포함 보고, AFK 회고 판별.
2. **조위정합 스파이크 개념 검증** — 별도 회색상자 실험 결과(`systems/tech-verification/`).
3. **STOP 규칙** — T0 실제 구현 소요가 슬라이스 견적의 150% 를 넘으면 STOP 하고 범위(도구 6→4, 장 구조)를 재설계한다. 8시간을 유지할 콘텐츠가 없으면 출시 약속을 수정 승인받는다(재사용 걷기 시간 금지).
4. **열린 S1 0 · G8 exit 0** — `qa/defect-register.md`·`freshness-check.sh` 원문.
이 절이 유일한 정본이며 초안·브리프·검증 계획은 이 절을 인용만 한다.

## Release safety
이 작업은 로컬 기획·하네스 개선·문서·프리비즈 에셋 제작 승인이다. Steam 계정 계약/신원/은행/세금 제출, Direct 비용 결제, 유료 외주, 외부 모집·상점 공개, git commit/push는 별도 승인. Unity 전체 콘텐츠 생산은 문서만으로 자동 시작하지 않는다. 검증용 최소 슬라이스를 먼저 만든다.

## Owners
기존 13개 전문 역할을 유지하고 독립 game-product-manager를 추가한다. PM은 제품 가치·가격·GTM·손익, economy는 게임 내 자원과 공정성 담당. PM이 QA 결과를 수정하거나 QA가 구매 의사를 만들어내지 않는다. `handoff/`는 systems가 소유하고 director가 승인한다.

## Evidence storage
기존 archive는 수정 금지. 대체 문서는 archive-cycle 경유, 현재 버전은 supersedes 연결. JSON/CSV/HTML/PNG/GIF는 frontmatter를 넣어 파서를 깨지 않고 같은 basename의 .meta.md 또는 폴더 단위 provenance.json에 소유자·해시·출처·관측/목표를 기록한다. 실제 측정이 없으면 evidence=[]/NOT-MEASURED.

**같은 사이클 안의 제자리 갱신(RFC-Q2)**: frontmatter `cycle` 값이 바뀌지 않는 개정은 "대체"가 아니며 `supersedes: null` 을 유지한다. 아카이브 의무는 `cycle` 값이 바뀌는 대체 또는 사이클 종료 시점에만 발생한다. 개정 이력은 문서의 변경 로그 절과 `qa/*-review.md` 재검증 절이 담당한다.

