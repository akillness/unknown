# unknown — Game Ops Harness 운영 규칙 (repository rule file)

> 이 저장소는 **게임을 만들고, 운영하고, 확장하고, 업데이트하는** 14개 전문 역할 + 디렉터 하네스로 운영된다.
> 세션은 잊지만 파일과 메모리는 남는다. 모든 세션은 이 문서 → `.mex/ROUTER.md` → `session-start.sh` 순서로 시작한다.
> Skill: `.claude/skills/game-ops-harness/SKILL.md` · Agents: `.claude/agents/game-*.md` · 이 파일은 사이클 산출물이다 (Step 7에서 재도출).

## 0. 세션 시작 (모든 세션, 예외 없음)

```bash
bash .claude/skills/game-ops-harness/scripts/session-start.sh "<이번 세션의 한 줄 과제>"
```
출력(열린 매니페스트 행, 최신 회고, 열린 RFC, G8 신선도, mex scope/timeline, graphify GRAPH_REPORT 요약, zg status, vault index)을 읽은 뒤에만 배정·작성한다. 채팅 기록은 상태가 아니다.

## 1. 역할 (14 + 디렉터) — 파일 기반 에이전트만 사용

| 레인 | 에이전트 | 소유 폴더 (`_workspace/current/`) | 리드 |
|---|---|---|---|
| 기획 | `game-planner` | `planning/` | 설계 레인 리드 |
| 밸런스 | `game-balance-designer` | `balance/` | |
| 시스템(코드) | `game-systems-designer` | `systems/` + 소스코드 | |
| 재화 | `game-economy-designer` | `economy/` | |
| 제품 PM | `game-product-manager` | `product/` | 가격·수요·BM·GTM |
| 연출 | `game-presentation-director` | `presentation/` | 비주얼 레인 리드 |
| 시놉시스 | `game-synopsis-writer` | `synopsis/` | |
| 세계관 | `game-worldview-architect` | `worldview/` | 내러티브 레인 리드 |
| 컨셉 | `game-concept-artist` | `concept/` | |
| 이팩트 | `game-vfx-artist` | `vfx/` | |
| 에니메이션 | `game-animator` | `animation/` | |
| 모션 | `game-motion-designer` | `motion/` | |
| 모델링 | `game-modeler` | `modeling/` | |
| QA | `game-qa` | `qa/` | 전 레인 브로드캐스트 |
| 디렉터 | `game-production-director` | `intake/ production/ retrospectives/` | 오케스트레이터 |

- 역할을 `Agent` 프롬프트에 인라인으로 적지 않는다 — 항상 `.claude/agents/` 정의를 쓴다. 이유: 세션 간 재사용과 규칙 누적이 파일에서만 일어난다.
- 팀 깊이는 디렉터 → 전문가 2단계뿐. 팀원은 팀을 만들지 않는다. 리드는 "먼저 응답하는 사람"이지 하위 오케스트레이터가 아니다.
- `CLAUDE_CODE_EXPERIMENTAL_AGENT_TEAMS=1`이면 TeamCreate/SendMessage/TaskCreate, 아니면 순차 서브에이전트 + `messages/{seq}-{from}.md`.

## 2. 워크스페이스: 하나의 live 폴더, 하나의 아카이브 (최신화 계약)

- **쓰기는 `_workspace/current/`에만.** `_workspace/archive/<run-id>/`는 읽기 전용 역사 — 인용은 하되 편집·삭제 금지.
- 모든 산출물은 frontmatter를 가진다: `updated / cycle / status(current|superseded|draft) / supersedes / owner`.
  같은 논리 산출물에 `status: current`는 하나뿐이다. 새 버전이 옛 버전을 대체하면
  `bash .claude/skills/game-ops-harness/scripts/archive-cycle.sh --no-stage <run-id> <path>`로 인덱스를 보존해 아카이빙하고,
  새 파일의 `supersedes:`가 그 아카이브 경로를 가리킨다. 이유: 이전 작업은 사라지지 않고 항상 참조 가능해야 한다.
- 삭제는 없다. 게이트나 요약을 깔끔하게 보이려고 산출물을 지우는 것은 결함이다.
- 주장에는 `[OBSERVED] [INFERENCE] [TARGET] [CARRIED]` 표기를 붙인다. 목표를 측정처럼 쓰지 않는다.
- `run-id` = `{YYYYMMDD}-{cycle-type}-{version}`; frontmatter 값이며 아카이브 시점에만 폴더명이 된다.
- 레이아웃과 스키마: `.claude/skills/game-ops-harness/references/workspace-contract.md`.

## 3. 사이클 = 업데이트 (운영·확장 중심)

| 타입 | 진입 | 필수 레인 | 게이트 |
|---|---|---|---|
| `preproduction` | P0→P1→P2 | 전 전문 역할 + 제품 PM | 문서 D1~D5, 실제 G는 별도 |
| `hotfix` | P4 (QA 선탑재) | qa, systems, 해당 레인 | G6, G8 (+수치 변경 시 G2/G3) |
| `balance-patch` | P2 | balance, economy, systems(data-only), qa, planner | G2, G3, G7, G8 |
| `content-update` | P1 | 내러티브 제외 전 레인 (+필요시 synopsis/concept) | G1–G5, G7, G8 |
| `season` | P2 세계관 우선 | 13 레인 전부 | G1–G8 |

사이클당 타입은 하나. 범위가 커지면 늘리지 말고 더 큰 타입으로 **재-인테이크**한다.
단계: `P0 sync → P1 scope → P2 foundation([세계관→시놉→컨셉] ∥ [시스템↔밸런스↔재화]) → P3 production(연출 스펙 먼저 → 모델링∥애니∥모션∥VFX; 시스템은 코드+영수증) → P4 QA loop(FIX ≤2) → P5 close`.
상세: `references/cycle-types.md`, 게이트 임계값: `references/quality-gates.md` (이 문서의 요약보다 우선).

## 4. 역할 간 논의 (RFC 프로토콜)

1. 변경을 여는 레인이 `production/decision-log.md`에 `RFC-{n}` 블록(레인, 질문, 제안, 증거 경로)을 append-only로 쓰고 영향 레인에 SendMessage.
2. 영향 레인은 `references/dependency-matrix.md`의 ● 항목에 대해 `ack | counter(증거) | block` 으로 응답.
3. 한 번의 교환으로 해소되지 않으면 디렉터가 **수치·증거로** 판정하고 기록.
4. 해소는 소유 산출물 갱신(fresh frontmatter) + `mex log "RFC-{n}: …"` 로 끝난다. 파일이 바뀌지 않은 RFC는 일어나지 않은 것이다.
- 공유 진실 파일(키프레임 ms, hit-stop, 용어집, 연출 스펙, 밸런스 시트, 게이트 측정)은 소유자만 편집하고 나머지는 인용한다.

## 5. 메모리 스택 (mex → llm-wiki → graphify → zg)

| 계층 | 경로 | 담는 것 | 검증 |
|---|---|---|---|
| mex | `.mex/` (`ROUTER.md`, `context/`, `patterns/`, `graph.db`, events) | **코드/데이터가 어떻게 동작하는가** — 아키텍처, 스택, 컨벤션, 결정 타임라인 | `mex check` |
| llm-wiki | `~/vaults/llm-wiki` (obsidian-cli vault `llm-wiki`) → `wiki/projects/unknown/`, `wiki/reports/`, `wiki/queries/` | **왜 그렇게 결정했는가** — 근거, 대안, 서베이, 플레이테스트 종합, 회고 | lint + 사람 |
| graphify | 저장소 루트 `graphify-out/` (`graph.json`, `GRAPH_REPORT.md`) | 코드 분해·정렬: 모듈, 허브, 커뮤니티, 영향 흐름 | `graphify update .` |
| zg | `.zvec-grep/` | 코드+문서 검색면 | `zg status` |

- **라우팅 규칙**: `mex check`가 코드로 검증할 수 없는 지식은 llm-wiki로 간다. 모르겠으면 사실은 mex, 이유는 vault, 둘 다 `decision-log.md`에서 링크.
- **코드 작업은 검색 우선(시스템 레인 필수 순서)**: `mex graph scope` → `zg query "<의도>"` / `zg query --rg -F "<심볼>"` → `graphify query` → 편집 → `graphify update .` → `mex graph && mex check` → `mex log`. 파일 전체를 열어 탐색하지 않는다; 그래프가 모르는 변경은 미완이다.
- vault의 `graphify-out/`(프롬프트/지식 그래프)와 저장소의 `graphify-out/`(코드 그래프)는 다른 그래프다. 서로 덮어쓰지 않는다.
- `LLM_WIKI_VAULT`가 다른 프로젝트를 가리킬 수 있으므로 하네스는 `GAME_OPS_VAULT`(기본 `~/vaults/llm-wiki`)만 본다.
- Obsidian이 실행 중이면 `obsidian-cli vault=llm-wiki create|append|read|search`, 아니면 직접 쓰기 후 `index.md`/`log.md` 갱신.
- 명령 모음: `references/memory-stack.md`.

## 6. 게이트 (수치만 통과한다)

G1 세계관 일관성 · G2 밸런스 밴드 · G3 경제 건전성 · G4 연출/몰입 · G5 에셋/이펙트 예산 · G6 운영 안정성 · G7 피처 수용/코어루프 · **G8 신선도·메모리**(`freshness-check.sh` exit 0 + memory_sync 영수증).
PASS / FIX(≤2) / REDO. 열린 S1 결함, 측정 누락, 숫자 자리의 `[TARGET]`/`[INFERENCE]`는 PASS를 막는다. QA가 재고 디렉터가 판정하며 모든 판정은 `qa/gate-measurements.md#g{n}`을 링크한다.

## 7. 사이클 종료 (디렉터 체크리스트)

1. 게이트 표 + `retrospectives/cycle-{n}-retrospective.md` (측정값, 미해결 리스크, 다음 진입 결정, `memory_sync:` 영수증 블록).
2. `archive-cycle.sh`로 대체된 산출물 이동; `freshness-check.sh` exit 0 확인.
3. `mex log` → `mex check` → (drift 시) `mex sync` → vault 리포트 `wiki/reports/{date}-unknown-{type}-{version}.md` + `index.md` 한 줄 → 코드 변경 시 `graphify update .` → 레이아웃 변경 시 `zg index --rebuild`.
4. 레인·도구·불변식이 바뀌었으면 **이 파일을 재도출**한다. 규칙 파일은 사이클 산출물이다.
5. `production/changelog.md`에 버전별·레인별 변경을 산출물+RFC 인용과 함께 적는다.

## 8. Git 안전 (동시 세션 가정)

- 편집 전과 커밋 직전에 `git status --short`; 예상치 못한 변경은 다른 세션의 작업으로 취급.
- 명시적 pathspec으로만 스테이징. `git add -A/.` 금지. force-push 금지. 다른 세션의 변경을 되돌리지 않는다.
- 사용자가 요청할 때만 커밋/푸시. `.zvec-grep/`, `.mex/graph.db`, `graphify-out/cache/`, `graph.html`은 무시 대상.

## 9. 불변식 (위반 시 실제 손상이 생기는 것들)

- 저장 데이터 필드 이름 변경은 플레이어 세이브를 고아로 만든다 → 마이그레이션 없이는 거부.
- 렌더/연출 코드는 시뮬레이션 스냅샷을 읽되 시뮬레이션 상태를 쓰지 않는다.
- 밸런스/재화 숫자는 데이터 테이블에만 산다; 코드는 노브를 노출할 뿐 튜닝을 하드코딩하지 않는다.
- 출시된 서사·설정은 캐논이다; 레트콘은 `season` 사이클 + 연속성 노트로만.
- 생성/구매 에셋은 `provenance.json`과 `runtimeEligible:false`로 시작하며, 승격은 decision-log 감사로만.


## 10. Steam 프리미엄 신작 사전제작 (2026-09-09 추가)

현재 요청은 라이브 운영이 아니라 **Unity 신작의 사전제작**이다. 이 범위에서는
`_workspace/current/production/premium-preproduction-contract.md`를 함께 읽는다.
기존 13개 역할을 없애지 않고 `.claude/agents/game-product-manager.md`를 추가해
제품 가치·시장 검증·가격·GTM을 게임 내 재화 설계와 분리한다.

- `preproduction` 사이클: 최소 5회의 조사→개발→독립 검토→플레이타임 논의. 회차마다 이전 문서·변경·근거·미측정 항목을 남긴다.
- 문서 D 게이트 통과와 실제 게임 G 게이트 통과는 별개다. 480분 표를 더했다고 8시간을 플레이한 것은 아니다.
- 이 신작에 전투·유료 재화가 없으면 해당 승률/인플레이션 게이트는 이유 있는 N/A다. 대신 퍼즐 도달성·힌트·저장 복구·본편 완결성·DLC 비의존성을 검사한다.
- 범용 `mex`가 TeX 동명 바이너리일 수 있다. 정체를 검증한 mex-agent만 실행하고, 도구가 없으면 skipped 영수증을 남긴다.
- JSON/CSV/HTML 등 비-Markdown 산출물은 같은 basename의 `.meta.md`로 메타데이터를 갖춘다. Markdown frontmatter를 바이너리/구조화 파일에 삽입하지 않는다.
- Steam 계약·실명/은행/세금 제출·등록비 결제·유료 도구/외주·상점 공개와 git commit/push는 이번 기획 요청만으로 실행하지 않는다. 단, 사용자가 2차 요청(2026-09-09)으로 명시한 리소스 생성 도구(GTI·Blender MCP·Higgsfield CLI)는 사용하되 크레딧 소모를 영수증으로 남긴다. git commit/push는 여전히 사용자가 수행한다.

### 10.2 2026-09-10 사용자 후속 지시 — 리소스 제공자

- 사용자 지시: "you use muapi and heiggsfield for resources". 이후 리소스 생성 제공자는 **MuAPI와 Higgsfield**다. 이 선택은 아래 10.1의 GTI 전량 생성 지시보다 우선한다(RFC-CX-002).
- 과거 GTI/Blender/Higgsfield 산출물의 실제 출처와 재현 기록은 보존한다. 신규 생성에서 GTI로 자동 대체하지 않는다.
- 모델·지원 형식·인증·비용은 해당 제공자의 현재 도구와 스키마로 확인한다. 제공자 선택과 실제 연결 완료를 구분하고, 미연결 제공자로 생성했다고 기록하지 않는다.
- 각 결과의 실제 제공자·모델·프롬프트·참조 입력·작업 ID·해시 및 확인된 크레딧 사용량을 provenance에 기록한다. 기존 런타임 승격·라이선스 검증 규칙은 유지한다. 실행 절차는 `_workspace/current/handoff/asset-runbook.md`를 따른다.

### 10.1 2026-09-10 재도출 (세션 병합 · Unity in-repo · 리소스 파이프라인)
- **세션 병행 안전**: 쓰기 워크플로를 띄우기 전에 `git status --short`·`ListAgents`·`ps -o pid,lstart,command -p $(pgrep -f 'claude --')`로 다른 세션을 확인한다. 세션 시작 이후 예상 밖 레인 파일이 생겼으면 멈추고 사용자에게 어느 세션이 마무리를 맡는지 묻는다. 병합은 `production/decision-log.md`에 RFC로 먼저 판정하고(명시 결정 > 후속 재도출), 레인 수정은 그 RFC id를 인용한다. `TaskStop`은 진행 중인 하위 에이전트를 죽이지 않으므로 전사본 mtime·워크스페이스 쓰기를 몇 분간 확인한다.
- **같은 사이클 안의 제자리 갱신은 대체가 아니다(RFC-Q2)**: frontmatter `cycle` 값이 바뀌지 않으면 `supersedes: null` 유지, 아카이브 의무 없음. 아카이브는 `cycle` 값이 바뀌는 대체 또는 사이클 종료 시점에만.
- **캠페인 정본과 인용 키**: `planning/campaign.json`이 33비트·73단서·분 배분의 단일 출처이고 `node _workspace/current/planning/validate-campaign.mjs`(44+검사)가 집계·sha의 유일한 측정 명령이다. 문서 간 인용 키는 campaign id(`t0-b1`…`e0-b2`)뿐이며 B01~B33은 `worldview/timeline.md` §7의 파생 색인이다. 고정 sha 숫자를 문서에 재기재하지 않는다.
- **Unity**: 프로젝트는 `unity/Unknown/`(저장소 내부, Unity **6000.5.6f1**, 코드네임 = 저장소명). 가제·영문명은 폴더/번들/상점명에 쓰지 않는다. `Library/ Temp/ Logs/ obj/ UserSettings/`는 .gitignore. 외부 실행자(Codex GPT-6 Astra)는 `_workspace/current/handoff/`만 읽고 착수하며, 브리프에 없는 결정은 RFC로 되묻는다.
- **리소스 파이프라인**(전부 `runtimeEligible:false`, 폴더별 `provenance.json`, 승격은 decision-log 감사로만): 2D = GTI(`gti`, 이 계정은 `--model gpt-6-astra`만 허용) via `scripts/gen-2d.sh` → `assets/generated/2d/<category>/`; 3D 블록아웃 = Blender MCP(사용자 오브젝트 삭제 금지, 새 .blend에 저장) → `assets/generated/3d/`; 영상 = `higgsfield` CLI(MCP는 이 저장소에 미등록; 실행 전 `higgsfield account status`, 소모 크레딧은 decision-log에 영수증); Mixamo는 3D 휴머노이드가 생기기 전까지 조건부 미사용(RFC-P4-001). README 미디어는 `docs/media/` 파생본이며 **프리비즈이지 게임플레이가 아니다** — 파일명·캡션·provenance `claim`에 명시한다. 생성 이미지 안의 텍스트·가제 문자열은 금지(프롬프트 NEGATIVE 필수).
- **T0 데이터·검증**: `_workspace/current/systems/data/t0/*.json`은 `systems/pipeline/emit-tables.mjs`가 `campaign.json`+`synopsis/t0-records.md`에서 생성하며 손으로 쓰지 않는다. `node _workspace/current/planning/validate-campaign.mjs --t0 _workspace/current/systems/data/t0`가 유일한 검사. T0의 확정 명령은 `reader` 인용 고정이다(RFC-C7-001). 결함 상태의 정본은 `qa/defect-register.md`이고 `scripts/regen-cycle-ledger.py`가 대장을 파생한다(RFC-C6-002). QA가 레인을 "a / b"로 적으면 첫 비-디렉터 레인이 소유자다.
- **C4/C5 승격 규칙(C3-F33)**: 세션 P가 남긴 C4/C5 레인 문서는 QA 검증(R4/R5)을 통과한 파일만 소유 레인이 같은 `cycle` 값으로 `status: current`로 올린다. 검증 전 인용에는 "(C4/C5 검증 대기)"를 붙인다.


## 11. 검증 도구의 실제 경계
- archive-cycle --root/--no-stage: 추적 여부와 무관하게 인덱스 보존. 옵션 없는 추적 파일은 git mv가 경로를 스테이징한다. 없는 소스·경로이탈·symlink·아카이브 덮어쓰기는 거부. superseded 수정은 이동 전, 이동 후 역사 편집 없음.
- freshness-check --since <날짜>의0은 Markdown구조만. since생략=시점신선도미측정, memory_sync별도. G8전체PASS로 읽지 않는다.
- mex-agent없으면TeX mex를 실행/설치하지 않는다. 문서/검증모형은degraded모드로 진행,G8PARTIAL.
- 사용자5회사이클 요구는 기존FIX≤2로 축소하지 않는다. 프리미엄 비전투N/A/문서D게이트는§10계약 우선.
- 일부 역할초안은 부모세션이 역할정의로 저작했다. 독립QA/ggumi응답만 독립 검토로 기록하며 가상의동의·회의·사람플레이를 만들지 않는다. 참조모형Node테스트≠Unity빌드/재미/8시간검증.

## 이미지 생성 제공자 (2026-09-11 사용자 지시)

- 신규 이미지 생성은 `god-tibo-imagen` 스킬과 `gti`를 사용한다. 이 최신 지시는 이전 MuAPI/Higgsfield 이미지 제공자 지정을 대체한다(RFC-CX-006).
- Blender 3D 제작과 Higgsfield 영상 경로는 유지한다. 이미 생성·승격된 리소스와 실제 출처 기록은 보존한다.
- 로컬 인증은 값을 출력하지 않고 확인하며 `--dry-run` 후 생성한다. 요청/실제 크기·요청 모델과 실제 모델의 확인 범위·프롬프트·해시·확인된 비용을 기록한다. 신규 결과는 `runtimeEligible:false`로 시작한다.
- 현행 절차와 검증 범위: `_workspace/current/production/image-provider-gti-20260911.md`.


## 영상 선행 아트 제작 기준 (2026-09-11 사용자 수정 · RFC-CX-009)

- 시각 원전은 기존 세계관과 초기 원본 컨셉 아트다. 현재 플레이 화면, Unity 프리팹/런타임 리소스, 현재3D/Blender 프리뷰, 그에 의존한 M5·M6 파생 이미지·영상은 새 영상/텍스처의 참조로 사용하지 않는다. 기록은 보존한다.
- 제작 순서는 **세계관·원본 컨셉 → 시네마틱/플레이 경험 목표 영상 → GTI 텍스처·리소스 → 프로젝트 프리팹 재구성**이다. 현재 프리팹의 모양을 영상 목표에 역으로 강제하지 않는다.
- 참조 허용 목록과 해시는 `_workspace/current/concept/concept-first-m7-sources.json`이 관리한다. 새 GTI 파생 이미지의 영상 입력 승인은 별도 출처 검토로 연결하며 런타임 승격과 구분한다.
- 영상은 목표 연출이다. 생성된 손/기계 움직임, 변형된 결정 패턴, 문 개방·저장·해금은 구현/물리 검증 증거가 아니다. 모델 형태와 표면 상세는 원본 및 검토된 GTI 키프레임을 함께 대조해 재구성한다.
- 신규 텍스처·2D 리소스 생성은 GTI를 사용한다. 기존 런타임 리소스를 입력으로 재생성하지 않는다. 후속 제작 계약은 `_workspace/current/handoff/concept-first-m7-resources.json`을 따른다.
