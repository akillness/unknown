---
updated: 2026-09-09
cycle: 20260909-preproduction-c4
status: draft
supersedes: null
owner: game-production-director
---

# C4 상호작용·연출·Unity

## Research
game-ui-ux의화면/포커스/바인딩/검증행렬을실제스키마로적용했다. 설치된Unity6000.5.6f1을후보로확인했지만패키지해결/빌드/성능은실행하지않았다. C3의시간·도구의존성을엔진계약으로전환한다.

## Develop
19화면·6도구UI계약, 노드형2.5D카메라, 모델링/컨셉/애니/모션/VFX각레인계약, 밸런스·경제대체게이트를작성했다. 별도의HTML상호작용검증모형과순수Node리듀서37테스트를실행. 이것은Unity게임/T0슬라이스가아니다.

## Review
독립QA W4JD5JP4tpz3MSqQ: FIX4건. X키충돌/강제홀드/키보드경로누락, 비동기저장실패와1프레임입력제한충돌, 독립성의매체/출처혼용, T0범위불일치. qa/c4-review.md참조.

## Decision
시스템담당에게4건수정배정: X는프리뷰,해제Y(패널안)/Delete, 확인은두단계기본+홀드선택, 저장성공후권위상태승격/실패복구, originId+매체다양성, T0는hub+circuit/reader만25분. 조위정합은별도회색상자실험. 각레인새파일은이전판아카이브연결.

## Playtime discussion
키/저장장애로늘어난시간은체류가아니라결함이다. 필수홀드·저장대기로8시간을만들지않는다. 장전환은사건기반으로읽는속도를벌하지않는다. HTML모형이몇분에끝나도본편예상시간에산입하지않는다.

## Observed proof
부모세션재실행: 하네스47/47, 참조모형37/37. 단순선별값격자의상태탐색이지연속모든입력·전체캠페인의증명이아니다. Unity런타임G2/G4/G5/G6/G7은여전히미측정.

## R4 재검증 (2026-09-10, 본 세션)
- **Review**: `qa/c4-review.md` "재검증 (R4)" + 재검증 1·2. C4-F1~F4 해소 확인 후 신규 C4-F5~F22(총 22: closed 9 · open S3 4 · open-rfc 1, **S1/S2 open 0 은 스테일** — 정본은 `qa/defect-register.md`(RFC-C6-002); R7 종료 시 등록부 집계로 갱신). 핵심 신규: C4-F9 도구 제목이 interaction-rules ↔ gdd §4 ↔ style-guide §9 ↔ 용어집에서 다름(C6/C7 회차 systems 배정), C4-F14 UI 계약 JSON 오타 4건, C4-F16 Space 우선순위, C4-F22 재도출 명령 서술.
- **Decision**: QA 검증 통과분 승격(C3-F33) — `animation/animation-contract.md`, `systems/prototype/{README,prototype.meta}.md`, `economy/resources-and-fairness.md` → status current. `systems/interaction-rules.md`·`unity-implementation.md`·`game-ui-contract.*`는 S3 해소 후 승격.
- **Playtime discussion**: 변화 없음 — 문서 정합만 개선. 패드 실측·프레임·저장 캡처 0건.
- **Version evidence**: 제자리 개정(RFC-Q2), `freshness-check.sh` 0 finding / 96 artifacts.
