---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: null
owner: game-production-director
---

# C5 상품·생산·회귀

## Research
세션 P의 C5 초안(`product/*`, `production/production-estimate.*`, `presentation/steam-game-plan.html` 36장, `systems/unity-implementation.md`)을 C3 정본(live `campaign.json`·validate-campaign.mjs·worldview·balance·economy·계약)과 대조했다. 실제 생성 리소스(2D 45장·3D 그레이박스·영상 2클립)와 견적의 "신작 자산 47종"은 별개(생성물은 컨셉/프리비즈)임을 검토 축에 넣었다.

## Develop
독립 QA 검토 `qa/c5-review.md`(R5): C5-F1~F11(S2 4 · S3 6 · S4 1). 가격 산식(9,000원 하한·런치 할인 40%·본당 수취·손익분기)은 재계산 일치, "권장" 승격 0, 8시간→가격 논리 0. 결함은 덱↔business-model 수치 병존(F8), B1 근거 문구 스테일(F9), economics.json 재생성 경로(F7), 폐기 용어 "매체 경로" 잔존(F5, 12곳/9파일), cycle-ledger 상태 스테일 등 정합 문제.

## Review
수정 루프 2회(systems·modeling·presentation → systems). 종료 시 집계는 `qa/defect-register.md` 가 정본(RFC-C6-002) — 당시 회차 문서의 "S1/S2 open 0" 은 스테일이었다, S3 open 5(C4-F14/F16/F21/F22 · C5-F5). 승격 대기: `product/business-model.md`(F8·F9 한 줄), `product/economics.meta.md`(F7), `product/{assumption-tests,steam-registration-guide,skill-application}.md`(QA 통과, 승격 미실행 → C6/C7 회차 처리).

## Playtime discussion
가격·판매·위시리스트·전환 실측 n=0. 손익분기 표는 예측이 아니라 역산. 생성 리소스 45장은 "생산 자산 완료"가 아니며 견적 인일에 반영하지 않는다.

## Decision
C1~C5 다섯 회차의 독립 검토·수정이 파일로 남았다(사용자 요구 "5회 리뷰·개선" 충족). 잔여 S3와 승격은 C6(통합 초안)·C7(핸드오프) 회차에서 닫는다.
