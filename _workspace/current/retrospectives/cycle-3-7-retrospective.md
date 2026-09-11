---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# 회고 — 사전제작 C3~C7 (2026-09-09 → 2026-09-10)

## 0. 이 회고가 말하지 않는 것
빌드 0줄 · 사람 플레이 n=0 · 시뮬 0회 · 성능 캡처 0건. 아래 모든 "통과"는 **문서 정합**이며 G1~G8 런타임 PASS 가 아니다. 결함 상태의 정본은 `qa/defect-register.md`, 회차별 집계는 `production/cycle-ledger.json`(`scripts/regen-cycle-ledger.py` 파생)이다. 아래 수치는 재검증 6(R7d) 종료 시점 스냅샷이다.

## 1. 게이트 표 (QA 최종, `qa/gate-measurements.md` "최종 게이트 표 (R7 종료)" 인용)

| 게이트 | 판정 | 문서 수준 값 [OBSERVED] | 런타임 | 근거 |
|---|---|---|---|---|
| G1 세계관 일관성 | PASS 불가(문서 violation 0) | 검증기 49/49, 일관성 감사 pass 37 / violation 0, 도구 표시명 용어집 6/6 | NOT-MEASURED | `qa/gate-measurements.md#g1` |
| G2 밸런스 밴드 | N/A(전투 없음) + 대체 NOT-MEASURED | 난이도 지수 [4,5,5,4,4,5,7,4,3] 재현, 힌트 1단 H-04 0위반 | n=0 | `#g2` · `balance/balance-sheet.md` §7 |
| G3 경제 건전성 | N/A(유료 재화 없음) + 대체 NOT-MEASURED | 유일 소모성 = routing 부식 상한 9, 기능 인질 금지, DLC 비의존 | 판매 n=0 | `#g3` |
| G4 연출/몰입 | NOT-MEASURED | vfx·motion·animation 계약이 "영수증 이후 발행" 동일 문면 | 프레임 0건 | `#g4` |
| G5 에셋/이펙트 예산 | NOT-MEASURED | GLB 7 · FBX 1 · 2D 45 · 영상 2, 전건 runtimeEligible:false, 승격 0 | Unity 임포트 0회 | `#g5` |
| G6 운영 안정성 | NOT-MEASURED | 저장 3파일 분리·원자적 rename·SV-F6, 참조 모형 37/0 | 크래시 주입 0회 | `#g6` |
| G7 피처 수용/코어루프 | NOT-MEASURED | T0 확정 명령 = reader 인용 고정, T0 인스턴스 데이터 `--t0` 5/5, C-07 17/17 | 표본 0/12, observedMedianMinutes null | `#g7` |
| G8 신선도·메모리 | **PARTIAL** | `freshness-check.sh` exit 0 / 0 finding / 120 artifacts; memory_sync 영수증 §4 | 문서 게이트 | `#g8` |

**PASS 0/8.** 어떤 게이트도 이 회차에서 올라가지 않았다. 열린 **S1 = 0**. 하네스 검증기 `validate-preproduction.mjs` SPEC-PASS 514/514.

## 2. 무엇이 있었나 (측정값)

| 항목 | 값 [OBSERVED] |
|---|---|
| 회차 | C3(재검증 3회) · C4(재검증+2루프) · C5(검토+2루프) · C6(5렌즈 판정단) · C7(반박 3렌즈) · R7/R7b/R7c 종료 수정 |
| 결함 등록부 | 총 157 · closed 98 · 열린 **S1 0** · S2 1(C6-F7, 디렉터: 인용 RFC id 대조 잔여) · S3+ 53 · open-rfc 5 (재검증 6, 대장·덱 s19 63칸 일치) |
| 캠페인 검증기 | `validate-campaign.mjs` 49/49 PASS · `--pairs` 17/17 · `--t0` 5/5 · 하네스 `validate-preproduction.mjs` SPEC-PASS 514/514 |
| 승격(draft→current) | animation-contract, prototype/README·meta, resources-and-fairness, unity-implementation, economics.meta, deck-outline, steam-game-plan.meta, interaction-rules, assumption-tests, skill-application, steam-registration-guide |
| 리소스 | 2D 45(GTI gpt-6-astra) · 3D 그레이박스(144 tris, GLB 7/FBX 1, 렌더 13) · 영상 2(Higgsfield, 25 credits) · README 미디어 15 |
| Unity | `unity/Unknown` 6000.5.6f1, 배치 생성 + 헤드리스 열기 2차 성공 영수증 |
| 디렉터 RFC | RFC-P3-008~015, C3 종료 판정, RFC-P4-001, C6/C7 판정 묶음(RFC-C7-001 등) — `production/decision-log.md` |

## 3. 미해결 위험 (다음 회차 입력)
1. **런타임 전무** — T0 를 Codex 가 구현하고 사람 12명/5유형 검증(H-1~H-3)을 통과하기 전까지 G4~G7 은 열리지 않는다. 계약 "Base production gate" 4조건.
2. **R-T0-1 조작 밀도** — 표·칸 하위과제 21/33(63.6%), 손 조작감 미측정. T0 에서 `manipulation_share` 관측.
3. **허브 비중 45.4%(218분)** — zoneId 정정의 결과. 기둥 2(같은 장소가 다르게 읽힌다)의 연출·셸 상태 단계 예산 재검토(RFC-N7 후속).
4. **corrosion·routing 재등장 공백**(각 3/33 비트) — RFC-P3-002, T0 후 재판정.
5. **생성물 라이선스 UNVERIFIED · Steam 생성형 AI 공개 항목 [TARGET]** — 승격·상점 제출 전 확인.
6. **가제/영문명 상표 미확인**, 기준 HW 미정(성능 판정 보류), `hint_offer_per_session_max` null.
7. 남은 한 줄 잔여: `systems/game-ui-contract.meta.md` L61 영수증 문장(C7-F45 S4), `product/business-model.md` H1 제목 — 각 소유 레인이 다음 세션 첫 편집에서 정정 후 승격.

## 4. memory_sync 영수증 [OBSERVED 2026-09-10]
```yaml
memory_sync:
  mex: skipped            # PATH `mex` = TeX Live; mex-agent 미설치 (scripts/mex-agent-bin.sh 정체 검사 실패). .mex/ROUTER.md·context/decisions.md D-006~D-011 은 직접 갱신
  llm_wiki: written       # ~/vaults/llm-wiki/wiki/reports/2026-09-10-unknown-preproduction-c3-c7.md · wiki/projects/unknown/decisions.md D-006~D-014 · index.md 1행 (Obsidian 미실행 → 직접 쓰기 폴백)
  graphify: updated       # graphify update . → graph.json/GRAPH_REPORT.md 갱신 (2026-09-10, 352 files) exit 0
  zg: rebuilt             # zg index --rebuild → "Workspace index: succeeded" exit 0
  claude_md: re-derived   # CLAUDE.md §10.1 (세션 병행 안전·RFC-Q2·캠페인 정본·Unity·리소스 파이프라인·승격 규칙·T0 데이터)
  freshness: "0 finding / 121 artifacts / exit 0 (scope: frontmatter + supersedes only)"
```

## 5. 다음 진입 결정
- **다음 회차 = T0 구현(Codex GPT-6 Astra)**. 입력은 `_workspace/current/handoff/` 4문서 + `systems/data/t0/` + `handoff/rfc-inbox/`. 사이클 타입은 `preproduction` 유지(본 생산 아님).
- 구현 완료의 게이트 입력은 실행자 자기보고가 아니라 QA 가 같은 배치 명령을 독립 재실행한 영수증이다(브리프 ⑩-3 주의).
- 사용자 직접 수행: git commit/push(pathspec 명시), Mixamo(해당 시), Steam 계정 작업, Higgsfield 추가 크레딧 승인.

## 6. 프로세스 교훈 (memory 로 저장됨)
- 쓰기 워크플로 전에 다른 세션 확인; `TaskStop` 은 진행 중 하위 에이전트를 죽이지 않는다.
- 같은 사이클 제자리 갱신 ≠ 대체(RFC-Q2).
- QA 가 레인을 "a / b" 로 적으면 디스패치는 첫 비-디렉터 레인 — 이번 R6 루프가 20여 건을 놓친 원인.
- 재도출 주장에는 명령과 출력을 붙인다; "결론은 맞고 영수증이 거짓"인 결함 유형(C7-F35/F38/F45)이 회차마다 재생산됐다.
