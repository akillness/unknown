---
updated: 2026-09-10
cycle: 20260909-preproduction-c6
status: current
supersedes: null
owner: game-production-director
---

# C6 통합 게임 초안 v1

## Research
C1~C5 정본(live `campaign.json` + 검증기, worldview, systems, balance, economy, product, 견적, 덱)을 한 문서로 설명·판단할 수 있는지가 닫는 질문이었다. 입력: `qa/c{3,4,5}-review.md` 잔여, 디렉터 판정 묶음(RFC-P3-008~015, C3 종료 판정).

## Develop
`planning/game-draft-v1.md`(12절 + 영문 요약) 신설. 6법 정본 문구·부식 상한 9·two-step 확정·`total_minus_afk_min`·campaign id 인용 키를 준수하고 숫자는 소유 문서 인용으로만.

## Review
5렌즈 판정단(플레이어 4 · 퍼블리셔 4.5 · 엔지니어 4 · 서사 5.5 · 프로듀서 5.5, 다음 단계 가부 1/5) → `qa/c6-review.md` C6-F1~F41(S2 17). 핵심: 첫 30분 목표 부재, 상품 약속 "결론이 바뀌는" vs 비분기 캠페인, 힌트 1단 위반, 세션 재개 요약 부재, T0 확정 명령 부재, T0 공개 상한·캐논 시각 누락, 포지셔닝·가격·생성형 AI 공개 위험 누락, 인용 RFC 미등재.

## Decision
디렉터 판정 묶음(2026-09-10 09:10): 상품 약속 정본 문구, T0 확정 = reader 인용 고정(RFC-C7-001), 조작 밀도는 위험 R-T0-1 로 등록, 본 생산 게이트를 계약 "Base production gate" 한 곳으로 통합, 미등재 RFC 전건 기재. R7 에서 planner·product·balance·synopsis 가 반영, QA 재검증 3 에서 C6 S2 17건 중 15건 closed, 잔여 2건(C6-F12·C6-F25)은 R7b 한 줄 수정.

## Playtime discussion
변화 없음 — 설계 480분·관측 null. 초안이 시간을 증명하지 않음을 §10·§11.3 에 명시. 허브 비중이 zoneId 정정으로 45.4%(218분)로 올라간 것은 좌표 수정이지 콘텐츠 변화가 아니나 기둥 2 부담이 커졌다(RFC-N7 후속).

## Version evidence
제자리 개정(RFC-Q2). 검증기 49/49 PASS, `--pairs` 17/17, `--t0` 5/5.
