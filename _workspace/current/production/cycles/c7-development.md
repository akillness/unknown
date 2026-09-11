---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# C7 Codex Unity 핸드오프 · 리소스 파이프라인

## Research
외부 실행자(Codex GPT-6 Astra)가 추가 질문 없이 T0 수직 슬라이스에 착수할 수 있는가. 입력: `systems/{architecture-contract,interaction-rules,unity-implementation,data-schemas,system-specs,ops}`, `unity/Unknown`(6000.5.6f1, 배치 생성·헤드리스 열기 영수증 `production/receipts/unity-batchmode/`), 생성 리소스 provenance(2D 45 · 3D 23 · 영상 2 · docs/media 15).

## Develop
`handoff/README.md`(읽는 순서·RFC 되묻기 규칙·미결 표), `handoff/codex-unity-brief.md`(①~⑫ + 영문 요약: 목표 T0, 저장소 규칙, asmdef 7분할, 패키지, 데이터 매핑·임포터 I-1~I-10, Sim 규칙, Save/Undo, Input, Presentation, Telemetry, 인수 테스트 T-01~T-27 매핑·배치 명령·DoD·금지), `handoff/verification-plan.md`(12명/5유형 H-1~H-3, AFK 회고, 성능 캡처 보류), `handoff/asset-runbook.md`(GTI/Blender/Higgsfield/Mixamo 절차·provenance 4종·승격 감사·잔여 47종), `handoff/rfc-inbox/`. R7: **T0 인스턴스 데이터** `systems/data/t0/{beats,hints,records,tools,zones}.json`(+meta, `systems/pipeline/emit-tables.mjs` 생성, 검증기 `--t0` 5/5), `synopsis/t0-records.md`(기록 원본 저작).

## Review
반박 3렌즈(startability=false · contradiction=false · safety=true) → `qa/c6-review.md` C7-F1~F44. S1 1건(C7-F1 T0 인스턴스 데이터 부재) → R7 에서 closed. 착수 전 결정 4건(기준 HW 미정·Input System new+activeInputHandler 2·URP·asmdef 7) 판정. 잔여 S3 다수(문서 영수증 재현성·표기).

## Decision
브리프 정본 = `handoff/` 4문서 + 계약 "Base production gate". 실행자는 decision-log 를 쓰지 않고 `rfc-inbox` 로 제출. GLB 가 런타임 플레이스홀더 정본. Mixamo 조건부 미사용(RFC-P4-001). 승격: `unity-implementation.md`·`economics.meta.md`·덱 메타 → current(R7), `interaction-rules.md`·`game-ui-contract.meta.md`·`business-model.md` 는 R7b 한 줄 수정 후 승격 판정.

## Playtime discussion
T0 25분은 8시간을 증명하지 않는다. 사람 검증 표본 0/12. `observedMedianMinutes` null.

## Version evidence
`freshness-check.sh` 0 finding(118 artifacts, R7 QA 기준). 영수증: `systems/tech-verification/{r7-t0-data,r7-systems-receipt}.md`, `production/receipts/unity-batchmode/`.
