---
updated: 2026-09-09
cycle: 20260909-preproduction-c3
status: draft
supersedes: null
owner: game-production-director
---

# C3 캠페인과 시간

## Research
C2의 6개 서사/규칙 결함과 스토어의 짧은 완결형 게임 표본을 대조했다. 작가는 수정된 바이블·연표·용어집을 직접 읽고 공개 순서를 검토했다.

## Develop
9구간33비트의 JSON을 작성. 21퍼즐, 각비트 단서/행동/완료조건/3힌트/복구/체크포인트. 대사6씬. 최초시간30/50/55/60/65/65/70/60/25.

## Review
독립QA VoLrTAq0pDxNklhF: SPEC-FIX5건. 숫자검사는재현됐지만162분비퍼즐저작량이빈약했고, 봉인호출과밸브대기로'사람의서명확인'을추론하는회귀가남았다. 자료복구/주인공지식/판#0회수도보강. 원문 qa/c3-review.md.

## Decision
시간패딩은기각. 튜토리얼25/에필로그10으로줄이고실제추리·조작구간에20분을재배정한25/50/55/65/65/70/75/65/10 안을재작성한다. 전체33비트에행동별예산/실제하위과제/추정근거를붙인다. 사건기록은밸브개폐와봉인완료접점으로정확히한정하고C1에서센서를미리소개한다.

## Playtime discussion
초기fast321/design480/deliberate672분은관측분포가아니다. QA가인용한420/360/600조건은채택된계약이아니므로그선에숫자를끼워맞추지않는다. 동작3개뿐인15분비트는제작과제가더필요하다는의미. 수정후에도480은저신뢰목표이며실제n0.

## Version evidence
이전본 _workspace/archive/20260909-preproduction-c3/{planning,synopsis,worldview}/. 수정판은동일current경로supersedes로연결. 작가실제응답과C5재검토로수정생존확인.

## 2차 · 세션 병합 후 종료 (2026-09-10, 본 세션)
- **Research**: 세션 P 초안(campaign.json C4 수리본·QA c1/c2/c4) 위에 세션 Q 의 C3 레인 후속본 40여 건이 얹혀 두 계보가 됐다. 사용자 판정으로 P 정본·Q 재도출.
- **Develop**: RFC-P3-008~015 판정 → 세계관 재기반(P 아카이브 c3 본문) → planner SPEC-REDO(`validate-campaign.mjs` 44검사 신설, `c1-b4` 매체 2종) → synopsis/systems/balance/economy/presentation 재도출.
- **Review**: 2차 QA 24건 → 재검증 1·2·3 에서 +12 발견, closed 27 · S1 0 · S2 3 · S3 1 · open-rfc 5 → 본 판정 묶음으로 배정. 원문 `qa/c3-review.md` §7~§9.
- **Playtime discussion**: 480분은 여전히 설계 예산이며 관측 0. 수용 키 `total_minus_afk_min`·목표 중앙값 450~540·철회 트리거 <420/<360 로 계약 통일(RFC-P3-011). `fast 322 / deliberate 673` 은 시나리오 경계.
- **Version evidence**: 세션 P 세계관·GDD 는 `_workspace/archive/20260909-preproduction-c3/`, current 문서 8건이 그 경로를 supersedes 로 인용. live `campaign.json` sha `fdabf1d4…`.
- **Next hypothesis (C4/C5)**: P 의 C4/C5 레인 문서를 QA 가 검증하면 `status: current` 로 승격해도 문서 간 모순이 0 인가.
