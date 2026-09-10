---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# `handoff/` — 외부 실행자(Codex · GPT-6 Astra)용 인계 폴더

[OBSERVED 2026-09-10] T0 M2 구현 현황은 [`../production/codex-t0-m2-status.md`](../production/codex-t0-m2-status.md), 실제 Unity 검증은 [`../systems/tech-verification/t0-m2-native.md`](../systems/tech-verification/t0-m2-native.md)에서 먼저 확인한다. 테스트 XML, player 빌드, 네이티브 UI smoke는 각각의 영수증 범위로 읽으며 사람 플레이테스트·본편 생산 게이트를 대신하지 않는다.

이 폴더는 **하네스 밖의 실행자가 읽는 유일한 입구**다. 폴더 소유는 systems 레인, 승인은 디렉터다(개별 파일의 `owner:` 는 그 파일 frontmatter 가 갖는다 — `asset-runbook.md` 는 modeling 소유)
(`production/premium-preproduction-contract.md` 「Owners」·「Unity / 저장소 배치」).

## 0. 이 폴더가 전제하는 것 한 문장

Unity 프로젝트 `unity/Unknown`(Unity **6000.5.6f1**)은 **이미 생성되어 있고 `Assets/` 는 비어 있다**.
이번 인계의 목표는 **본편 생산이 아니라 T0 수직 슬라이스**(`hub` 1구역 + `circuit`·`reader` 2도구 · 설계 길이 25분) 하나다.
T0 사람 검증(`verification-plan.md`)을 통과하기 전에는 본 생산을 시작하지 않는다.

## 1. 읽는 순서 (이 순서를 지킬 것) `[C7-F4 재정렬 2026-09-10 R7 종료]`

**규칙: `status: current` 문서가 `status: draft` 문서보다 앞에 온다.** 이전 판은 `asset-runbook.md`(draft)를 3.5 에, `unity-implementation.md`·`interaction-rules.md`(둘 다 draft)를 5 에 두어 **draft 를 current 위에 세웠다**(C7-F4). 아래 표는 그 순서를 뒤집고 `status` 열을 명시해 다음 회차가 눈으로 확인할 수 있게 한다.
**어긋나면 `status: current` 가 이긴다.** draft 문서는 QA 승격 대기분이며, 그 안의 규칙을 current 문서보다 앞세우지 않는다.

| # | 파일 | status | 무엇을 얻는가 | 분량 |
|---|---|---|---|---|
| 1 | **`handoff/README.md`** (이 파일) | current | 폴더 규칙 · 되묻는 방법 · 하지 말 것 | 짧다 |
| 2 | **`handoff/codex-unity-brief.md`** | current | **착수에 필요한 전부** — 목표 · 저장소 규칙 · 폴더/asmdef · 데이터 매핑 · Sim 규칙 · 저장/되돌림 · 입력 · 연출 · 텔레메트리 · 인수 테스트 · 보고 양식 · 영문 요약 | 길다. 전부 읽는다 |
| 3 | `systems/architecture-contract.md` | **current** | **모듈 경계·불변식의 소유 문서.** asmdef 7분할의 정본이며 브리프 §③ 은 이 문서의 요약이다(RFC-S2 판정) | 필독 |
| 4 | `systems/system-specs/{wiring-trace,plate-readout}.md` | **current** | T0 두 도구의 상태기계·규칙·실패 모드·인수 기준 | 필독 |
| 5 | `systems/system-specs/save-undo.md` | **current** | 세이브·되돌림 상태기계. **재생 단위 = 명령**(C7-F7) · 로그 상한과 `SV-F6` 접힘 | 필독 |
| 6 | `systems/data-schemas/*.md` (6종) | **current** | 데이터 필드·튜닝 노브·불변식 | 필독 |
| 7 | `systems/ops/telemetry-contract.md` | **current** | 키 이름과 **정직성 규율**(설계 상수 ≠ 실측) | 필독 |
| 8 | `planning/campaign.json` + `planning/validate-campaign.mjs` | **current** | 저작 원본과 그 검증기(**규칙의 유일한 출처**). **집계·해시는 검증기 출력만 인용한다** | 필독 |
| 9 | `systems/pipeline/emit-tables.mjs` + `emit-tables.meta.md` | **current** | **런타임 테이블 생성기**. `--scope` 로 두 층을 낸다 — Unity `Data/Tables/{beats,hints}.json` + 영수증, 그리고 **T0 인스턴스 데이터**(아래 10) | 필독 |
| 10 | **`systems/data/t0/*.json` + 각 `.meta.md`** | **current** | **T0 퍼즐의 실제 값** — `beats`(완료 술어 포함) · `hints` · `tools` · `zones` · `records`. **손으로 만들지 않는다**(생성기 출력, C7-F1) | 필독 |
| 11 | **`handoff/verification-plan.md`** | current | T0 를 **누가 어떻게 검증하는가**(사람 12명/5유형 · 성능 캡처 · G4/G5/G6/G7 측정 방법) | 중간 |
| 12 | `worldview/glossary.md` | **current** | UI 문자열·에셋 이름에 쓸 수 있는 명사의 전부 | 참조 |
| 13 | `systems/unity-implementation.md` · `systems/interaction-rules.md` | **draft**(QA 승격 대기) | 구현 계약 · 입력/도구 규칙. **위 current 문서와 어긋나면 current 가 이긴다.** 브리프는 이 둘의 인용본이며, 브리프와 이 둘이 어긋나면 이 둘이 이긴다 | 참조 |
| 14 | `handoff/asset-runbook.md` (`owner: game-modeler`) | **draft** | **에셋 파이프라인** — 도구 실제 동작 · 폴더/명명/`provenance` 스키마 · 승격 절차 · 남은 47종. **이 폴더에서 systems 가 쓰지 않은 유일한 파일**이며 modeling 레인 소유다 [OBSERVED 2026-09-10] | 에셋 작업 전 필독 |

한 줄 요약이 필요하면 `production/premium-preproduction-contract.md` → `planning/gdd.md` 순으로 읽는다.
**`_workspace/archive/` 는 읽기 전용 역사다.** 인용은 하되 편집·삭제하지 않는다.

## 2. 브리프에 없는 결정은 RFC 로 되묻는다 (이 폴더의 핵심 규칙)

브리프가 답하지 않는 결정을 만나면 **추측해서 구현하지 않는다.** 다음 순서로 되묻는다.

1. 정말 없는지 확인한다 — `handoff/codex-unity-brief.md` 검색 → `systems/` 소유 문서 검색 → `production/decision-log.md` 검색.
2. **`_workspace/current/handoff/rfc-inbox/RFC-CX-{n}.md` 에 파일 하나로 제출한다** `[C7-F12 정정 2026-09-10 R7 종료]`. 양식·번호 규칙·이 폴더로 오는 것/오지 않는 것의 전문은 **`handoff/rfc-inbox/README.md`** 가 소유하며, 이 항목은 그 문서를 인용한다.

> 이전 판은 제출처를 `messages/{seq}-codex-*.md` 로 적어 `rfc-inbox/README.md` §4 가 「알려진 문서 불일치」로 고지한 상태였다. 디렉터 판정 `production/decision-log.md` 「C6-F11 … + 실행자 규칙」 **⑥**(「RFC 는 `handoff/rfc-inbox/RFC-CX-{n}.md` 로 제출하고 디렉터가 append 한다」)이 정본이므로 **이 파일을 그쪽에 맞춘다**. `rfc-inbox/README.md` §4 의 불일치는 이 편집으로 닫힌다.

```markdown
### RFC-CX-{n} · {한 줄 제목}
- lanes: {영향 레인들}
- question: {예/아니오 또는 A/B 로 답할 수 있는 한 문장}
- proposal: {실행자가 지금 채택하고 싶은 안 + 그 이유}
- evidence: {읽은 파일 경로와 행/절. 없으면 "없음"이라고 적는다}
- blocking: {yes = 판정 전까지 코드가 못 나간다 / no = 임시안으로 진행 가능}
- measured: {[OBSERVED] 명령과 그 출력. 실측이 없으면 "n=0"}
- decided_by: (비워 둔다 — 디렉터가 채운다)
```

**진행 보고·질문 스레드**는 여전히 `_workspace/current/messages/{seq}-codex-{주제}.md` 다. RFC 와 보고를 같은 파일에 섞지 않는다.

3. `blocking: no` 이면 **임시안을 코드 주석 `// RFC-CX-{n} 임시안`** 으로 표시하고 진행한다. 판정이 오면 주석을 지우고 값을 바꾼다.
4. `blocking: yes` 이면 그 부분을 **컴파일되는 스텁**으로 남기고 다른 작업으로 넘어간다. 추측한 값을 데이터 테이블에 넣지 않는다.
5. 디렉터 판정은 `production/decision-log.md` 에 append-only 로 기록된다. 판정 뒤에는 **판정문을 인용해** 구현한다.

**되묻지 않고 정해도 되는 것**: 변수·클래스 이름, 내부 자료구조, 폴더 안의 파일 분할, 테스트 픽스처 이름, 코드 주석.
**반드시 되물어야 하는 것**: 저장 필드 이름·타입, 밸런스/경제 숫자, 도구·구역·비트 id, UI 표시 문자열, 캐논(세계관) 서술, 패키지 추가, 에셋 승격, 범위 확대.

## 3. 실행자가 쓰는 곳 / 쓰지 않는 곳

| 쓴다 | 쓰지 않는다 |
|---|---|
| `unity/Unknown/**`(코드·에셋·설정) | `_workspace/current/` 의 다른 레인 폴더 |
| `_workspace/current/systems/tech-verification/{name}.md`(검증 영수증) | `_workspace/current/production/decision-log.md`(디렉터 소유 — RFC 는 `messages/` 로) |
| **`_workspace/current/handoff/rfc-inbox/RFC-CX-{n}.md`(RFC 제출)** `[C7-F12]` | `_workspace/archive/**`(읽기 전용) |
| `_workspace/current/messages/{seq}-codex-*.md`(진행 보고·질문 스레드) | `_workspace/current/systems/data/t0/**`(**생성기 출력** — 손으로 고치지 않는다. 값이 틀렸으면 저작 문서를 고치고 생성기를 다시 돌린다) |
| `unity/Unknown/.gitignore` 보강 | `planning/campaign.json`(planner 소유 · 임포터는 **읽기만**) |
|  | `worldview/glossary.md`(worldview 소유) |

## 4. 현재 열려 있는 것 (착수 전에 알아야 하는 미결) `[2026-09-10 R7 종료 갱신]`

| id | 내용 | 상태 | 실행자에 대한 영향 |
|---|---|---|---|
| RFC-S2 | 어셈블리 분할 | **판정됨 — 7분할** (`decision-log.md` 「RFC-S2 / C7-F4」) | 브리프 §③ 트리대로 만든다. 되묻지 않는다. `unity-implementation.md` §2 정정 완료 |
| RFC-S3 | 세이브에서 `chapter`/`dayIndex` 제거 | **판정됨 — 제거 승인** (동 문서 「RFC-S3」) | 저장하지 않는다. 스테이지 진행은 `storyPhase`. **마이그레이션 대상 아님**(v1 이전 제거) |
| RFC-S5 | 확정 사본의 루트 승계 예외 | **판정됨 — 예외 없음** (동 문서 「RFC-S5」) | 사본은 원본의 루트 `originId` 를 물려받는다. 독립성 판정에 예외 분기를 만들지 않는다 |
| RFC-S6 | 도구 표시명 4건 용어집 등재 | **부분 판정** — 표시명 6종 정본 확정, 용어집 등재는 worldview 대기 | **T0 는 KO 전용.** EN 필드는 **선택**(없으면 KO 폴백)이며 `T-12`/`I-9` 는 EN 부재로 실패하지 않는다. fail-closed 대상은 **용어집 등재 여부**뿐 |
| C4-F20 | 「확정 `Enter` 길게 0.4 s」 표기 | **해소** (`tide-alignment.md` L30 · `dual-seal.md` L30 · `interaction-rules.md` §1-2 `RB` 행 정정) | 확정 기본값은 `two-step`. **홀드를 전제로 구현하지 않는다.** 지속시간 분기 바인딩 **0건** |
| C7-F1 | T0 인스턴스 데이터 | **해소** — `systems/data/t0/` 생성 | 값을 발명하지 않는다. 생성기 출력을 임포트하고, 부족하면 RFC 로 되묻는다 |
| PRE-1 | 기준 하드웨어(`hw_profile_id`) | **미정** | 성능 수치를 **PASS/FAIL 로 판정하지 않는다**. 캡처는 하되 결론은 미룬다 |
| OPEN-S8 | `zones.json` 의 Blender→Unity 좌표 변환식이 `[INFERENCE]` | **열림** | `hub-greybox.glb` 임포트 결과와 대조해 확인하고(`T-Z1`) 어긋나면 RFC |
| OPEN-S10 | `hub-uncovered-1/2/3` 표시 이름(RFC-N9) | **worldview 대기** | 기술 id 로만 쓰고 **UI 문자열로 표시하지 않는다** |
| OPEN-S11 | `hub-view-drawer` 대상 프롭(사물 서랍) 미모델링 | **modeling 대기** | 노드는 데이터에 있으나 메시가 없다. 플레이스홀더로 진행하고 승격하지 않는다 |

## 5. 이 폴더가 주장하지 않는 것

- **게임이 존재한다고 주장하지 않는다.** 빌드 0회 · 플레이 표본 n=0 · 프레임타임 캡처 0건이다.
- 브리프의 성능·용량·시간 수치는 전부 `[TARGET]` 이며 어떤 게이트도 올리지 않는다.
- 생성된 프리비즈 이미지·영상은 **게임플레이가 아니다.** 파일명·캡션에 `previz` 를 유지한다.
