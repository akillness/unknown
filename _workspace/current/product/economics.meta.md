---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: null
owner: game-product-manager
---

# Economics model

전부 미측정 시나리오 입력. 경로 economics.json. 계산은 `.claude/skills/game-ops-harness/scripts/validate-preproduction.mjs`에서 재생성한다(economics.json의 `prices`·`launchDiscount`·`vatKR`·`refundRate`·`chargebackRate`·`developerShareAssumption`·`perUnitReserve`·`cashBudgets`를 읽어 `net`·`breakEvenCash`·런치플로어 검사를 재계산 — 코드 L65~L71, 실행 확인 `qa/c5-review.md` M9 exit 0). `scripts/validate-preproduction.mjs`(저장소 루트)는 **존재하지 않는다** — QA C5-F7이 지목한 오기를 정정한다(이전 판본이 잘못된 경로를 적었다). JSON 수치는 공식 정책 VAT외에는 시장값이 아닌 가정이다. 소비자가 최소9000원은 사용자 출시 조건이고 미래할인까지 확대 적용 여부는 추가 결정.
