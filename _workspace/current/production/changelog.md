---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Changelog

## v0.1.0-pre (사전제작 C1~C7, 2026-09-09 → 2026-09-10) — 코드 0줄, 실측 n=0

### production / director
- 세션 병합 판정 RFC-P3-008~015, C3 종료 판정(RFC-Q1/Q2, F25/F30/F31/F33/F22, RFC-W4, A37), RFC-P4-001(Mixamo), C6/C7 판정 묶음(RFC-C7-001, RFC-S2~S6, RFC-N6, RFC-B6, RFC-M1/M3, RFC-C6-001/002) — `production/decision-log.md`.
- 계약 C3 개정: Time acceptance(RFC-P3-011), Evidence storage 제자리 갱신 규칙(RFC-Q2), Base production gate(C6-F9), Asset pipeline, Unity/저장소 배치 — `production/premium-preproduction-contract.md`.
- 회차 기록 `production/cycles/c{3..7}-development.md`, 영수증 `production/receipts/unity-batchmode/`, 대장 `production/cycle-ledger.json`(`scripts/regen-cycle-ledger.py` 파생), 충돌 기록 `conflicts.md`.

### planning
- `campaign.json` 계보 B 확정 + `validate-campaign.mjs`(49검사, `--pairs`, `--t0`) 신설; zoneId 33비트, RFC-W4 문장 이동, `c1-b4` 매체 2종, hints[0] 1단 재작성(H-04), `c1-b2`·`c3-b1` proofRequired, t0-b1 목표 문장, completion 술어 — RFC-P3-008/012/013, C3-F11/F22/F29/F35, RFC-N6, C6-F1/F3/F17, RFC-C7-001.
- `gdd.md` C3 후속본(동사 6·2층·힌트·저장·접근성·범위), `content-matrix.md`·`campaign-time-budget.md` live 재도출, `feature-specs/verb-01~06`·`recap-panel.md`, `game-draft-v1.md`(C6).

### worldview
- 세션 P c3 본문으로 재기반(RFC-P3-010), 캐논 시각 H-1:24/H-1:04/H+0:12(RFC-P3-013), T0 공개 상한(RFC-P3-012), B# 단일 정의(C3-F30), 용어집 125행+도구 표시명 4건(C3-F13, RFC-S6), 일관성 감사 pass 37/violation 0.

### synopsis
- `chapter-beats.md`·`continuity.md`·`synopsis.md` live 재도출, K 표 검증기 파생(A37), R2 의도 4장/6장 분리(RFC-W4), S6 대사·S1 판 #0 정의(C3-F7/F21), `t0-records.md`(RFC-C7-001).

### systems
- `architecture-contract.md`(7분할 asmdef, sim/render, 세이브 불변식), `system-specs/` 8종, `data-schemas/` 6종(+zoneId, 부식 저장 없음, dayIndex 제거), `ops/telemetry-contract.md`(`total_minus_afk_min`), `interaction-rules.md`·`unity-implementation.md`·`game-ui-contract.*` R1~R4 반영·정본 표시명·접근성 15키, `pipeline/emit-tables.mjs` + `data/t0/*`(C7-F1), `handoff/` 4문서 + `rfc-inbox/`.

### balance / economy
- 부식 = 전역 상한 9(RFC-P3-009), 원본 상태 카운터 `readCounts`/`readBudget` 3(C3-F35), 힌트 180초 단일 제안(RFC-P3-015), 난이도 지수 관측 2열(C3-F31), 위험 R-T0-1(C6-F5); `currency-map`·`sink-source-ledger`·`reward-bands`·`negotiation-record` 정본 인용 교체(C3-F15/F16/F27).

### presentation / product
- 덱 36장 법 문구·폐기 용어 정정 재생성(C3-F24, C5-F5), `media-direction.md`; `business-model.md` 포지셔닝·가격 후보·생성형 AI 공개 위험(C6-F2/F6/F8), `economics.meta.md`·`assumption-tests`·`skill-application`·`steam-registration-guide` current.

### concept / modeling / animation / motion / vfx
- `style-guide.md`·`generation-manifest.md`·프롬프트 45 + 재생성 2(C4-F11, RFC-M1); `asset-manifest.md` 47종·`pipeline.md`·`specs/hub-watchroom.md`, 그레이박스 GLB 7/FBX 1/렌더 13; `rig-requirements.md`·`anim-list.md`; `motion-contract.md`·`vfx-budget.md` 영수증 게이팅 정합(C4-F7).

### assets / docs / unity / scripts
- `assets/generated/2d`(45, GTI gpt-6-astra) · `3d`(Blender 5.1.2) · `video`(Higgsfield 2클립, 25 credits) · `previz` GIF, 전건 provenance·`runtimeEligible:false`. `docs/media/` 15 파생본 + README. `unity/Unknown/` 6000.5.6f1 스켈레톤(배치 생성·헤드리스 열기 영수증). `scripts/{gen-2d.sh,gen-video-higgsfield.sh,make-previz-gif.sh,refresh-2d-provenance.py,regen-cycle-ledger.py}`.

### 미해결 (등록부 기준, R7 종료)
- S1 0 · S2: C4 1 · C5 1(C5-F2 대장 — 재생성 완료, QA 확인 대기) · C6 2 · C7 1(R7b 처리 중) · S3/S4 다수(표기·영수증 재현성). 런타임 게이트 전부 NOT-MEASURED.
