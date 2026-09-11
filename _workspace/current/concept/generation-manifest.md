---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-concept-artist
---

# 2D 생성 매니페스트 — GTI 전량 생성

[OBSERVED] 도구: `gti` CLI (god-tibo-imagen, Codex 비공식 백엔드). 이 계정에서 허용된 모델은 `gpt-6-astra` **하나뿐**이며 다른 모델은 HTTP 400.
[OBSERVED] 래퍼: `scripts/gen-2d.sh <asset-id> <category> <size> <prompt-file>` → `assets/generated/2d/<category>/<id>.png` + 폴더별 `provenance.json`. `FORCE=1`이면 재생성.
[OBSERVED] 1장당 실측 90.9초(스모크 테스트 `tool-reader-hero`). 병렬 2, 호출 간 `sleep 2`, 실패 시 1회 재시도 후 `skipped`.

> **전량 컨셉/프리비즈다. 게임플레이가 아니다.** 모든 항목은 `runtimeEligible:false`로 시작하며 승격은 `production/decision-log.md` 감사로만.
> 모든 프롬프트는 `concept/style-guide.md` §2 팔레트 · §4 재질 · §5 카메라 문단을 **문자 그대로 반복**하고, §10 금지 목록을 NEGATIVE 절로 싣는다.
> 이미지 내 텍스트는 전면 금지(`no text, no letters, no numbers, no logos, no signage`). 상표 미확인 가제 문자열은 프롬프트·파일명 어디에도 없다(검증: 프롬프트 45개 전수 grep, 0건).

## 프롬프트 구성 (awesome-agent-skills 프레임 차용)
| 절 | 내용 | 출처 프레임 |
|---|---|---|
| `SUBJECT` | 이 컷이 무엇이고 **무엇이 변하는가** | content-creator "Hook Immediately / Provide Value" — 첫 문장이 가치를 말한다 |
| `STYLE` | 팔레트 8색 hex + 재질 4종 규칙, 45개 프롬프트에 동일 문단 | ux-designer "Consistency" (우선순위 5) |
| `CAMERA` | 2.5D 고정 노드 / 초상 / 평면 UI 3종 중 하나 | ux-designer "Visual Hierarchy" |
| `SEQUENCE CONSISTENCY` | previz 전용, 프레임 간 동일 스타일 강제 | ux-designer "Interaction states / edge cases" |
| `NEGATIVE` | style-guide §10 금지 목록 | ux-designer "Accessibility → 색 단독 부호화 금지" 포함 |

UI 3시트(`ui-tool-icon-sheet`, `ui-medium-badge-sheet`, `ui-status-badge-sheet`)는 ux-designer 페르소나의 접근성 우선순위에 따라 **색을 지운 1비트 실루엣에서도 구분**되도록 형태 부호를 프롬프트에 명시했다. 키아트·캡슐은 content-creator 프레임의 "축소 시에도 읽히는 후크" 규칙을 적용해 로고 여백을 비워둔다.

## 목록 (총 45장)

### concept/ — 인물·공간·도구 컨셉 (21장)

| asset-id | 요청 size | 용도 | 프롬프트 파일 | 결과 |
|---|---|---|---|---|
| `char-seorin-sheet` | 1024x1536 | 인물 전신 컨셉 시트 (3/4 뷰, 중립 배경) | `prompts/char-seorin-sheet.txt` | `OK` |
| `char-jaehwa-sheet` | 1024x1536 | 인물 전신 컨셉 시트 (3/4 뷰, 중립 배경) | `prompts/char-jaehwa-sheet.txt` | `OK` |
| `char-eunjeong-sheet` | 1024x1536 | 인물 전신 컨셉 시트 (3/4 뷰, 중립 배경) | `prompts/char-eunjeong-sheet.txt` | `OK` |
| `char-seongchan-sheet` | 1024x1536 | 인물 전신 컨셉 시트 (3/4 뷰, 중립 배경) | `prompts/char-seongchan-sheet.txt` | `OK` |
| `char-doyeon-sheet` | 1024x1536 | 인물 전신 컨셉 시트 (3/4 뷰, 중립 배경) | `prompts/char-doyeon-sheet.txt` | `OK` |
| `char-seorin-portrait` | 1024x1024 | 대화 UI 초상 (평상 표정) | `prompts/char-seorin-portrait.txt` | `OK` (실제 1145x1374) |
| `char-jaehwa-portrait` | 1024x1024 | 대화 UI 초상 (평상 표정) | `prompts/char-jaehwa-portrait.txt` | `OK` (실제 1145x1374) |
| `char-eunjeong-portrait` | 1024x1024 | 대화 UI 초상 (평상 표정) | `prompts/char-eunjeong-portrait.txt` | `OK` (실제 1024x1536) |
| `char-seongchan-portrait` | 1024x1024 | 대화 UI 초상 (평상 표정) | `prompts/char-seongchan-portrait.txt` | `OK` (실제 1145x1374) |
| `char-doyeon-portrait` | 1024x1024 | 대화 UI 초상 (평상 표정) | `prompts/char-doyeon-portrait.txt` | `OK` (실제 1024x1536) |
| `space-hub-watchroom-mood` | 1536x1024 | 공간 2.5D 고정 시점 무드 프레임 | `prompts/space-hub-watchroom-mood.txt` | `OK` |
| `space-gate-three-mood` | 1536x1024 | 공간 2.5D 고정 시점 무드 프레임 | `prompts/space-gate-three-mood.txt` | `OK` (2026-09-10 재생성, 실제 1536x1024) |
| `space-lowland-mood` | 1536x1024 | 공간 2.5D 고정 시점 무드 프레임 | `prompts/space-lowland-mood.txt` | `OK` (실제 1672x941) |
| `space-wharf-mood` | 1536x1024 | 공간 2.5D 고정 시점 무드 프레임 | `prompts/space-wharf-mood.txt` | `OK` |
| `space-pumphouse-one-mood` | 1536x1024 | 공간 2.5D 고정 시점 무드 프레임 | `prompts/space-pumphouse-one-mood.txt` | `OK` |
| `tool-circuit-hero` | 1024x1024 | 도구 히어로 프롭 (작업대 확대뷰) | `prompts/tool-circuit-hero.txt` | `OK` (실제 1536x1024) |
| `tool-reader-hero` | 1024x1024 | 도구 히어로 프롭 (작업대 확대뷰) | `prompts/tool-reader-hero.txt` | `OK` (실제 1312x1199) |
| `tool-alignment-hero` | 1024x1024 | 도구 히어로 프롭 (작업대 확대뷰) | `prompts/tool-alignment-hero.txt` | `OK` (실제 1536x1024) |
| `tool-routing-hero` | 1024x1024 | 도구 히어로 프롭 (작업대 확대뷰) | `prompts/tool-routing-hero.txt` | `OK` (실제 1312x1199) |
| `tool-corrosion-hero` | 1024x1024 | 도구 히어로 프롭 (작업대 확대뷰) | `prompts/tool-corrosion-hero.txt` | `OK` (실제 1536x1024) |
| `tool-seal-hero` | 1024x1024 | 도구 히어로 프롭 (작업대 확대뷰) | `prompts/tool-seal-hero.txt` | `OK` (실제 1312x1199) |

### ui/ — 인터페이스 아트 (4장)

| asset-id | 요청 size | 용도 | 프롬프트 파일 | 결과 |
|---|---|---|---|---|
| `ui-workbench-frame` | 2048x1152 | 작업대 프레임 (6도구 슬롯·증거함·가설판 영역) | `prompts/ui-workbench-frame.txt` | `OK` (실제 1536x1024) |
| `ui-tool-icon-sheet` | 1024x1024 | 도구 아이콘 시트 (6아이콘 3x2 그리드) | `prompts/ui-tool-icon-sheet.txt` | `OK` (실제 1536x1024) |
| `ui-medium-badge-sheet` | 1024x1024 | 매체 배지 3종 시트 (염판/일지/대장) | `prompts/ui-medium-badge-sheet.txt` | `OK` (실제 1774x887) |
| `ui-status-badge-sheet` | 1024x1024 | 상태 배지 시트 (정상/대기/침수/복구/고장) | `prompts/ui-status-badge-sheet.txt` | `OK` (실제 2056x765) |

### keyart/ — 키아트 (2장)

| asset-id | 요청 size | 용도 | 프롬프트 파일 | 결과 |
|---|---|---|---|---|
| `keyart-watchroom-wide` | 2048x1152 | 키아트 가로 (당직실 판독기 앞 주인공) | `prompts/keyart-watchroom-wide.txt` | `OK` (실제 1672x941) |
| `keyart-watchroom-vertical` | 1024x1536 | 키아트 세로 (동일 주제, 상단 로고 여백) | `prompts/keyart-watchroom-vertical.txt` | `OK` |

### capsule/ — Steam 캡슐 후보 (2장, 로고 미포함)

| asset-id | 요청 size | 용도 | 프롬프트 파일 | 결과 |
|---|---|---|---|---|
| `capsule-header-candidate` | 2048x1152 | Steam header 비율 캡슐 후보 (좌측 1/3 로고 여백) | `prompts/capsule-header-candidate.txt` | `OK` (실제 1672x941) |
| `capsule-library-candidate` | 1024x1536 | Steam library 세로 캡슐 후보 (하단 1/3 로고 여백) | `prompts/capsule-library-candidate.txt` | `OK` |

### readme/ — README 이미지 (7장)

| asset-id | 요청 size | 용도 | 프롬프트 파일 | 결과 |
|---|---|---|---|---|
| `readme-hero` | 2048x1152 | README 히어로 배너 (항만 방어계통 전경) | `prompts/readme-hero.txt` | `OK` (실제 1983x793) |
| `readme-verb-circuit` | 1536x1024 | README 동사 설명 프레임 (circuit: 동사 -> 공간 변화) | `prompts/readme-verb-circuit.txt` | `OK` |
| `readme-verb-reader` | 1536x1024 | README 동사 설명 프레임 (reader: 동사 -> 공간 변화) | `prompts/readme-verb-reader.txt` | `OK` |
| `readme-verb-alignment` | 1536x1024 | README 동사 설명 프레임 (alignment: 동사 -> 공간 변화) | `prompts/readme-verb-alignment.txt` | `OK` (실제 1916x821) |
| `readme-verb-routing` | 1536x1024 | README 동사 설명 프레임 (routing: 동사 -> 공간 변화) | `prompts/readme-verb-routing.txt` | `OK` |
| `readme-verb-corrosion` | 1536x1024 | README 동사 설명 프레임 (corrosion: 동사 -> 공간 변화) | `prompts/readme-verb-corrosion.txt` | `OK` |
| `readme-verb-seal` | 1536x1024 | README 동사 설명 프레임 (seal: 동사 -> 공간 변화) | `prompts/readme-verb-seal.txt` | `OK` (2026-09-10 재생성, 실제 1536x1024) |

### previz/ — README 컷씬 프리비즈 프레임 (9장)

| asset-id | 요청 size | 용도 | 프롬프트 파일 | 결과 |
|---|---|---|---|---|
| `previz-f01-circuit-connect` | 1536x1024 | README 컷씬 프리비즈 프레임 (f01) | `prompts/previz-f01-circuit-connect.txt` | `OK` (실제 1672x941) |
| `previz-f02-water-drop` | 1536x1024 | README 컷씬 프리비즈 프레임 (f02) | `prompts/previz-f02-water-drop.txt` | `OK` (실제 1672x941) |
| `previz-f03-compare-records` | 1536x1024 | README 컷씬 프리비즈 프레임 (f03) | `prompts/previz-f03-compare-records.txt` | `OK` (실제 1672x941) |
| `previz-f04-valve-trial` | 1536x1024 | README 컷씬 프리비즈 프레임 (f04) | `prompts/previz-f04-valve-trial.txt` | `OK` (실제 1672x941) |
| `previz-f05-valve-revert` | 1536x1024 | README 컷씬 프리비즈 프레임 (f05) | `prompts/previz-f05-valve-revert.txt` | `OK` (실제 1672x941) |
| `previz-f06-flood-cost` | 1536x1024 | README 컷씬 프리비즈 프레임 (f06) | `prompts/previz-f06-flood-cost.txt` | `OK` (실제 1672x941) |
| `previz-f07-three-places` | 1536x1024 | README 컷씬 프리비즈 프레임 (f07) | `prompts/previz-f07-three-places.txt` | `OK` (실제 1672x941) |
| `previz-f08-signing-desk` | 1536x1024 | README 컷씬 프리비즈 프레임 (f08) | `prompts/previz-f08-signing-desk.txt` | `OK` (실제 1672x941) |
| `previz-f09-signing-close` | 1536x1024 | README 컷씬 프리비즈 프레임 (f09) | `prompts/previz-f09-signing-close.txt` | `OK` (실제 1672x941) |

## previz 체인 규칙
`previz-f01-circuit-connect`를 먼저 단독 생성한 뒤, `f02`~`f09`는 `scripts/gen-2d.sh` 대신
`gti --image <f01.png> --prompt <프레임 프롬프트> --size 1536x1024 --model gpt-6-astra`를 직접 호출하고
provenance 는 `gen-2d.sh`와 **동일 스키마**로 python 수동 append 한다(`claim` 끝에 `previz chain ref=previz-f01-circuit-connect.png` 표기).
시퀀스는 `presentation/video-study.md` 채택 순서를 따른다: 배선 연결·수위 하강(0~6초) → 두 기록 대조(6~18초) → 밸브 시험과 되돌림(18~32초) → 침수 선택의 대가(32~45초) → 세 장소 대비(45~55초) → 서명대(55~60초, 제목 자리).

## 실행 결과 [OBSERVED] (2026-09-10)
- **45 / 45 생성 성공, skipped 0.** provenance 45항목이 6개 카테고리에서 각각 파일과 1:1 일치하며 `runtimeEligible:false` 아닌 항목 0.
- 재생성 11건: 인물 10장(1차가 캐논과 다른 국적으로 생성 → 프롬프트에 한국인·3/4 각 명시 후 `FORCE=1`), `previz-f02`(f01 구도를 과하게 상속해 변화가 안 읽힘).
- **`gti --size` 는 강제되지 않는다**: 45장 중 29장이 요청과 다른 해상도로 반환됐다(위 표의 "실제" 표기). Steam 캡슐·UI 프레임은 별도 크롭·리사이즈 공정이 필요하며 그 공정은 아직 0건이다.
- 파생물 1건: `assets/generated/previz/previz-cutscene-concept.gif` (previz 9프레임 → ffmpeg 7.1.1, 1.4fps, 960×540). 별도 `provenance.json` 을 가지며 `runtimeEligible:false`, claim 에 "NOT gameplay footage" 명시.
- 검수 결과·재생성 우선순위: `sheets/README.md`.

## 재생성 이력 — C7 종료 수정 회차 (2026-09-10) [OBSERVED]

배정: `qa/defect-register.md` **C4-F11** + decision-log **RFC-M1**. 프롬프트 텍스트를 먼저 고치고 `FORCE=1`로 같은 asset-id 를 덮어썼다.
`gti --size` 는 이번 2장에 대해서는 **요청과 실제가 일치**했다(1536x1024). 45장 중 29장 불일치라는 §0.1 관측은 그대로 유효하다 — 이 2건이 그 비율을 바꾸지 않는다(불일치 29건은 다른 자산들이다).

| asset-id | 실행 명령 | 프롬프트 변경 | 결과 |
|---|---|---|---|
| `readme-verb-seal` | `FORCE=1 scripts/gen-2d.sh readme-verb-seal readme 1536x1024 _workspace/current/concept/prompts/readme-verb-seal.txt` | SUBJECT 의 "handwritten booklet" → "pages are blank except for abstract dashed marks and ruled lines"; NEGATIVE 선두에 `no handwriting, no glyphs, no letters, blank pages, abstract dashed marks only` + cursive/script/calligraphy/hangul/hanja/latin 추가 | `httpStatus 200`, 1536x1024, 3,080,168 B. 책자 100% 확대 검수에서 글자꼴 0 → **§10-4 해소** |
| `space-gate-three-mood` | `FORCE=1 scripts/gen-2d.sh space-gate-three-mood concept 1536x1024 _workspace/current/concept/prompts/space-gate-three-mood.txt` | SUBJECT 를 "poured-concrete sluice housing + 수직 슬라이드 강판 문짝 + 상부 권양기(geared hoist drum)·리프트 체인·격자 발판·파이프 난간"으로 재기술, 배경은 "빈 하늘과 낮은 방조제뿐"으로 고정; NEGATIVE 에 `no castle, no fortress, no battlements, no crenellations, no turrets, no watchtower, no stone arch, no arched bridge, no viaduct, no aqueduct, no masonry blockwork, no medieval or european-fantasy architecture` 추가 | `httpStatus 200`, 1536x1024, 2,616,965 B. 성곽/아치교 모티프 소멸, 피어 위로 솟는 구조물 0 → **RFC-M1 모델링 착수 차단 해소** |

- provenance 갱신: `scripts/gen-2d.sh` 가 항목을 교체 기록 → `python3 scripts/refresh-2d-provenance.py` → `refreshed 45/45 entries`. 두 항목 모두 `runtimeEligible:false`, `license: UNVERIFIED` 유지. `readme/` 7항목·`concept/` 21항목 모두 id 중복 0.
- `docs/media/` 파생본(`verb-seal.jpg` 등)과 루트 `README.md` 는 **컨셉 레인이 건드리지 않았다** — 디렉터 소유. 파생본이 갱신되기 전까지 README 에 걸린 이미지는 구버전(`d259d1df…`)이다.

## 실패 처리
- 1회 재시도 후에도 실패하면 위 표의 상태를 `skipped`로 남기고 파일을 만들지 않는다. 텍스트 매니페스트는 그대로 남으므로 모델링·VFX는 이미지 없이도 착수할 수 있다(`[NO-IMAGE]`).
- 백엔드는 비공식 경로이며 예고 없이 끊길 수 있다. 끊기면 이 매니페스트가 재생성 계약서다.

## 미측정
- 인게임 적합성(실제 Unity 프레임 안에서의 가독성) 0건.
- 색약 사용자 검증 0명. §2.1 대체 세트는 [TARGET]이다.
- 라이선스: 생성 백엔드 ToS 미확인. `provenance.json`의 `license` 필드는 `UNVERIFIED`.
