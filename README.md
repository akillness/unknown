# 조수기록국: 마지막 당직 (가제)

> **Working title — unapproved.** 국문 가제와 영문 코드네임은 상표·동명 게임 확인 전이며 폴더명·번들명·상점명에 쓰지 않습니다. 저장소·Unity 프로젝트 코드네임은 `Unknown`입니다.
>
> 이 저장소는 Steam 프리미엄 Unity 신작의 **사전제작(preproduction)** 저장소입니다. 아래 이미지·GIF는 전부 **컨셉·프리비주얼라이제이션**이며 **실제 게임플레이 캡처가 아닙니다.** 플레이·성능·판매 실측은 아직 **n = 0** 입니다.

![README hero — 은포항 야경 컨셉 (프리비즈, 게임플레이 아님)](docs/media/readme-hero.jpg)

## 한 줄 소개

폐국을 3주 앞둔 조수기록국의 마지막 야간 당직. 기록 복원사 **한서린**은 끊긴 염선 배선과 배수 경로를 **손으로 직접 바꾸고**, 그 결과로 달라진 항구를 다시 조사해 12년 전 **대조의 밤**에 사라진 **결손 4시간**의 진실을 청문 문서 한 건으로 확정합니다. 밤은 21:00에 시작해 05:00에 끝나고, 세 갈래 결말은 전부 본편 안에서 닫힙니다.

**장르** 작업대·공간 추리 어드벤처 (2.5D 고정 시점, 싱글플레이) · **엔진** Unity 6000.5.6f1 · **플랫폼 목표** PC (Steam) · **언어** 한국어 / 영어 · **전투·유료 재화·멀티플레이 없음**

## 세 기둥

1. **행동으로 추리한다.** 단서를 모으는 데서 끝나지 않고 배선·경로·기록의 가설을 손으로 시험합니다.
2. **같은 장소가 다르게 읽힌다.** 새 지역을 늘리는 대신 허브 1곳 + 구역 4곳을 상태 변화로 재사용합니다.
3. **본편만으로 닫힌다.** 도시의 위기, 사건의 책임, 주인공의 선택이 본편에서 끝납니다. DLC는 다른 사건입니다.

## 플레이 컷씬 (프리비즈)

| 도구 조작 → 공간 변화 → 새 질문 (Higgsfield image-to-video, 10초) | 허브 당직실 그레이박스 턴테이블 (Blender) |
|---|---|
| ![프리비즈 컷씬 — 배선 연결 → 수위 하강 → 밸브 시험과 되돌림. 실제 게임플레이 아님](docs/media/previz-cutscene-video.gif) | ![허브 그레이박스 턴테이블 — 최종 아트 아님](docs/media/previz-hub-turntable.gif) |

컨셉 프레임 9장으로 만든 스토리보드 GIF는 [docs/media/previz-cutscene-concept.gif](docs/media/previz-cutscene-concept.gif) 에 있습니다. 두 GIF 모두 `docs/media/provenance.json`에 출처와 "NOT gameplay" 표기가 있습니다.

## 여섯 개의 동사 — 세계의 여섯 법칙과 1:1

| 동사 | 대응 법 | 연습(sandbox)에서 | 확정(commit)에서 |
|---|---|---|---|
| ![배선 추적](docs/media/verb-circuit.jpg) **배선 추적** `circuit` | 배선된 것만 남는다 | 계통선을 따라 센서 범위 안/밖을 비교 | 확정 없음 — 배선 밖 근거를 무효로 만든다 |
| ![판독](docs/media/verb-reader.jpg) **판독** `reader` | 원본은 닳지만 사본은 남는다 | 검증 사본을 무제한 재생·확대 | 판독 결과를 가설판에 출처와 함께 고정 |
| ![조위정합](docs/media/verb-alignment.jpg) **조위정합** `alignment` | 정합 전 시계는 믿지 않는다 | 공통 피크 3개를 잡으며 잔차를 실시간 확인 | 잔차 ≤ 4분일 때 두 자료의 시간축을 잇는다 |
| ![배수 편성](docs/media/verb-routing.jpg) **배수 편성** `routing` | 이번 조수에는 보호 용량이 부족하다 | 두 경로를 대기 상태로 편성해 결과를 미리 본다 | 우선순위를 확정해 구역 상태를 실제로 바꾼다 |
| ![부식 시험](docs/media/verb-corrosion.jpg) **부식 시험** `corrosion` | 소금은 비용으로 보인다 | 구성안을 가상 시험대에 무제한 올린다 | 한도 안의 구성만 배수 편성 확정으로 넘긴다 |
| ![이중서명](docs/media/verb-seal.jpg) **이중서명** `seal` | 원본 책임과 제출을 나눈다 | 결론 카드에 근거 슬롯을 채워 본다 | 서로 다른 매체 2종 + 배선 범위 안 + 시간 근거로 제출 |

모든 조작은 **연습(무제한·무료·세계 불변)** 과 **확정(프리뷰 → 저장 성공 후에만 세계 변경)** 두 층으로만 존재합니다. 힌트 3단계는 무료·무제한이고 엔딩·평가에 영향이 없습니다. 실시간 타이머는 없습니다.

## 세계

은포항(銀浦)은 대조차 9.2 m의 반폐쇄 만입니다. 방조제 → 갑문·수문 → 양수장의 3중 방어를 **염선**(브라인 도관)의 압력·염도가 회로처럼 잇고, 그 신호는 **염판**에 12시간 링으로 각인됩니다. 염판은 밸브 개폐·압력·염도·수위·문 개폐·호출만 남기고 얼굴·의도·대화 내용·사람의 위치는 남기지 않습니다. 과거는 재생되지 않고 **서로 다른 매체 2종의 대조로만 추론**됩니다.

![허브 당직실 무드 컨셉](docs/media/space-hub.jpg)

세계관 바이블·연표·용어집: [`_workspace/current/worldview/`](_workspace/current/worldview/) · 캠페인 33비트(정본 JSON): `_workspace/current/planning/campaign.json`

## 사전제작 상태 (2026-09-10, R7 종료 당시)

| 항목 | 상태 |
|---|---|
| 사전제작 사이클 | C1 시장·범위, C2 인과·세계관, C3 캠페인·시간, C4 상호작용·Unity, C5 상품·생산 — **각 회차 독립 QA 검토 + 수정 완료** (`_workspace/current/qa/c{1..5}-review.md`), C6 통합 초안(5렌즈 판정단), C7 Codex 핸드오프(반박 3렌즈) — `qa/c6-review.md` |
| 결함 현황 | 열린 **S1 0**. 정본은 `_workspace/current/qa/defect-register.md`, 회차별 집계는 `production/cycle-ledger.json` |
| 설계 분량 | 9장 33비트, 설계 예산 480분 **[TARGET]**, 검증기 `planning/validate-campaign.mjs` 49/49 PASS — 관측 완주 시간은 **미측정(null)** |
| 통합 초안 | `_workspace/current/planning/game-draft-v1.md` (12절 + English summary) |
| Codex 핸드오프 | `_workspace/current/handoff/` — 브리프·검증 계획·리소스 런북·RFC 인박스; T0 인스턴스 데이터 `_workspace/current/systems/data/t0/` |
| 2D 컨셉 리소스 | 45장 (인물·공간·도구·UI·키아트·캡슐·README·프리비즈), GTI, 전부 `runtimeEligible:false` |
| 3D | 허브 당직실 그레이박스 + 도구 6종 블록아웃 (GLB 7 / FBX 1, 144 tris), Blender 5.1.2 |
| 영상 | Higgsfield image-to-video 프리비즈 2클립 (5초·720p) |
| Unity 프로젝트 | `unity/Unknown/` — Unity 6000.5.6f1 빈 프로젝트 (코드 0줄, 배치 생성·헤드리스 열기 영수증 `_workspace/current/production/receipts/unity-batchmode/`) |
| 실제 빌드 · 플레이테스트 · 성능 | **없음 / n = 0** — 본 생산은 계약의 "Base production gate" 네 조건 충족 후에만 |

정직성 규칙: 표를 더해 480분이 나왔다는 사실은 8시간을 플레이했다는 증거가 아닙니다. 게이트 측정치는 [`_workspace/current/qa/gate-measurements.md`](_workspace/current/qa/gate-measurements.md) 에만 있고, 실측이 없는 게이트는 `NOT-MEASURED`로 남습니다.

## 저장소 구조

기획 상세 문서와 Unity 프로젝트는 현재 로컬 작업 트리에서 준비 중입니다. 아래의 기획·구현 경로와 실행 명령은 해당 로컬 작업 트리를 기준으로 합니다. GitHub에 공개된 README와 소개 미디어만 내려받아서는 게임을 실행할 수 없습니다.

```
CLAUDE.md                      저장소 운영 규칙 (13역할 + PM 하네스, 게이트 G1~G8, 사이클 계약)
_workspace/current/            살아 있는 사전제작 산출물 (레인별 폴더, frontmatter 필수)
_workspace/archive/            대체된 이전 판본 (읽기 전용, supersedes 로 연결)
_workspace/current/handoff/    Codex(GPT-6 Astra) Unity 구현 핸드오프 브리프 · 검증 계획 · 리소스 런북
unity/Unknown/                 Unity 6000.5.6f1 프로젝트 (in-repo)
assets/generated/{2d,3d,video,previz}/   생성 리소스 + provenance.json (승격 전 runtimeEligible:false)
docs/media/                    README 용 이미지·GIF (파생본) + provenance.json
scripts/                       gen-2d.sh (GTI) · gen-video-higgsfield.sh · make-previz-gif.sh · refresh-2d-provenance.py
```

## Unity에서 열기

1. Unity Hub → Add project → `unity/Unknown` (에디터 6000.5.6f1).
2. 구현은 `_workspace/current/handoff/README.md` 의 읽는 순서를 따릅니다: `codex-unity-brief.md`(T0 = 허브 + `circuit`/`reader` 25분, asmdef 7분할, Input System, URP, T0 확정 = 판독 인용 고정) → `systems/data/t0/*.json` 임포트 → 인수 테스트 T-01~T-27 → `verification-plan.md`. 브리프에 없는 결정은 `handoff/rfc-inbox/`에 RFC로 제출합니다.
3. 생성 리소스는 `assets/generated/`에서 `unity/Unknown/Assets/`로 **감사(decision-log) 후에만** 승격합니다.

## 구현 시작점과 검증

**로컬 T0 M1 구현 경과 (2026-09-10):** 불변 시뮬레이션 상태, 명령 재생, 회로 표시·근거, 판독 사본·인용 구간 고정, 생성 데이터 영수증/해시 검증을 구현했습니다. 실제 Unity 6000.5.6f1의 자체 Editor 계약 검사 **21건이 통과**했고, `Tide.Sim`에서 `UnityEngine` 참조가 컴파일 단계에서 거부되는 것도 확인했습니다.

전체 T0는 아직 실행 가능한 게임으로 완성되지 않았습니다. 실제 데이터의 출처 ID 누락(`C7-F50`, `RFC-CX-001`)으로 마지막 인용 확정이 차단되며, 필수 패키지 설치는 디스크 공간 부족으로 실패했습니다. 화면·입력·저장/복구·전체 도구 상태기계는 후속 구현 범위입니다. 위 21건은 NUnit/PlayMode 테스트나 플레이어 빌드 결과가 아닙니다. 상세 원본은 로컬 `_workspace/current/systems/tech-verification/t0-m1-native.md`와 `_workspace/current/qa/t0-m1-review.md`에 있습니다.

현재 개발 범위는 **T0 수직 슬라이스**입니다. `hub` 한 구역에서 `t0-b1` 탐색 → `t0-b2` 배선 추적(`circuit`) → `t0-b3` 판독·인용 고정(`reader`)으로 이어집니다. **25분은 설계 목표**이며 실제 플레이 시간은 아직 측정하지 않았습니다. 본편 생산 착수 조건은 `_workspace/current/production/premium-preproduction-contract.md`의 `production gate`를 따릅니다.

작업 전 `CLAUDE.md` → `.mex/ROUTER.md` → `_workspace/current/handoff/README.md` 순서로 규칙을 확인합니다. 런타임 수치는 정본 데이터에서 읽고, `Tide.Sim`은 Unity 엔진을 참조하지 않으며, 화면은 시뮬레이션 스냅샷만 읽습니다. 생성 에셋의 `runtimeEligible:false` 상태는 별도 감사 전까지 유지합니다.

저장소 루트에서 설계와 T0 데이터를 검증합니다(Node.js와 Bash 필요).

```bash
bash .claude/skills/game-ops-harness/scripts/session-start.sh "T0 implementation"
node _workspace/current/planning/validate-campaign.mjs
node _workspace/current/planning/validate-campaign.mjs --t0 _workspace/current/systems/data/t0
bash .claude/skills/game-ops-harness/scripts/freshness-check.sh --root "$PWD"
```

이 명령은 설계·데이터·문서 검증입니다. Unity 컴파일, EditMode/PlayMode 테스트, 사람 플레이 검증은 별도로 수행하며 실행 명령과 원본 결과를 `_workspace/current/systems/tech-verification/`에 남깁니다. 아직 실행하지 않은 검증은 통과로 표시하지 않습니다.

## 리소스 출처와 라이선스

신규 리소스 생성에는 **MuAPI와 Higgsfield**를 사용합니다(2026-09-10 사용자 지정). 아래 목록은 기존 산출물의 실제 생성 출처입니다. 새 결과도 제공자·모델·입력·해시를 기록하고 별도 감사 후에만 런타임으로 승격합니다.

- 2D: `god-tibo-imagen`(GTI, Codex 백엔드, 모델 `gpt-6-astra`) — 프롬프트는 `_workspace/current/concept/prompts/`, 출처는 각 폴더의 `provenance.json`.
- 3D: Blender 5.1.2 (MCP) 프리미티브 그레이박스 — 스크립트 `assets/generated/3d/scripts/`.
- 영상: Higgsfield `seedance_2_0_mini` — `assets/generated/video/provenance.json`.
- 생성물의 상업 이용 가능 여부는 **각 백엔드 약관 확인 전까지 UNVERIFIED** 입니다. 모든 항목이 `runtimeEligible:false`로 시작합니다.
- 실존 재난·피해자·타 작품 설정을 차용하지 않았습니다. 수문·염선 기술은 창작이며 현실 안전 매뉴얼이 아닙니다.

---

*Generated content is pre-visualization, not gameplay. Playtest n = 0. See `CLAUDE.md` for the studio contract and `_workspace/current/production/premium-preproduction-contract.md` for what this repository does and does not promise.*
