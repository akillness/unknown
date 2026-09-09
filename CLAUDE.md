# unknown — Game Ops Harness 운영 규칙 (repository rule file)

> 이 저장소는 **게임을 만들고, 운영하고, 확장하고, 업데이트하는** 13-역할 스튜디오 하네스로 운영된다.
> 세션은 잊지만 파일과 메모리는 남는다. 모든 세션은 이 문서 → `.mex/ROUTER.md` → `session-start.sh` 순서로 시작한다.
> Skill: `.claude/skills/game-ops-harness/SKILL.md` · Agents: `.claude/agents/game-*.md` · 이 파일은 사이클 산출물이다 (Step 7에서 재도출).

## 0. 세션 시작 (모든 세션, 예외 없음)

```bash
bash .claude/skills/game-ops-harness/scripts/session-start.sh "<이번 세션의 한 줄 과제>"
```
출력(열린 매니페스트 행, 최신 회고, 열린 RFC, G8 신선도, mex scope/timeline, graphify GRAPH_REPORT 요약, zg status, vault index)을 읽은 뒤에만 배정·작성한다. 채팅 기록은 상태가 아니다.

## 1. 역할 (13 + 디렉터) — 파일 기반 에이전트만 사용

| 레인 | 에이전트 | 소유 폴더 (`_workspace/current/`) | 리드 |
|---|---|---|---|
| 기획 | `game-planner` | `planning/` | 설계 레인 리드 |
| 밸런스 | `game-balance-designer` | `balance/` | |
| 시스템(코드) | `game-systems-designer` | `systems/` + 소스코드 | |
| 재화 | `game-economy-designer` | `economy/` | |
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
  `bash .claude/skills/game-ops-harness/scripts/archive-cycle.sh <run-id> <path>` 로 `git mv` 아카이빙하고,
  새 파일의 `supersedes:`가 그 아카이브 경로를 가리킨다. 이유: 이전 작업은 사라지지 않고 항상 참조 가능해야 한다.
- 삭제는 없다. 게이트나 요약을 깔끔하게 보이려고 산출물을 지우는 것은 결함이다.
- 주장에는 `[OBSERVED] [INFERENCE] [TARGET] [CARRIED]` 표기를 붙인다. 목표를 측정처럼 쓰지 않는다.
- `run-id` = `{YYYYMMDD}-{cycle-type}-{version}`; frontmatter 값이며 아카이브 시점에만 폴더명이 된다.
- 레이아웃과 스키마: `.claude/skills/game-ops-harness/references/workspace-contract.md`.

## 3. 사이클 = 업데이트 (운영·확장 중심)

| 타입 | 진입 | 필수 레인 | 게이트 |
|---|---|---|---|
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
