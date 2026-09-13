---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# 독립 근거 이해 · 단일 변수 실험 제안

[TARGET] Main이 코어 루프 조사의 후속 검증 후보를 `game-design-theory` 계약으로 정리했다. 같은 루트의 사본을 묶는 읽기 전용 표현이 규칙 이해와 안내 없는 전이에 도움이 되는지 확인한다. 한 변수만 비교하며 새 증거·정답·자동 인용·진행을 추가하지 않는다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `game-design-hypothesis.json` | 9360 | `ed5cf0620947504a5df9cc5c81190a8807d8994dad1b6d22e7b48bbc2772f7c5` |

## 근거

- `../c1-signature-contract.md` §분리와 사본 계보, `../../systems/interaction-rules.md` §3.
- [맥락 내 학습과 반복 가능한 연습](https://gameaccessibilityguidelines.com/include-contextual-in-game-helpguidancetips/), [상호작용 튜토리얼](https://gameaccessibilityguidelines.com/include-interactive-tutorials/): Main이 2026-09-13 원문을 직접 읽었다. 연결선의 인과 효과를 입증하는 자료는 아니다.
- M22 기술 영수증은 동작의 근거이며 사람 이해/재미의 근거가 아니다.

[BOUNDARY] 실제 프로토타입·사람 모집·관찰은 실행하지 않았다. 사람 n=0, 효과 판정 inconclusive. 기존 12명/5유형 T0 계약을 통계적 A/B 유의성 보장으로 해석하지 않는다. 이 실험은 Base production gate 또는 전체 생산 승인을 대체하지 않는다.

## 구조 검증과 평가 오염 방지

- [OBSERVED 2026-09-13] `python3 skill://game-design-theory/scripts/validate-design-hypothesis.py _workspace/current/planning/core-loop-research-20260913/game-design-hypothesis.json` exit 0, `PASS: valid game design hypothesis contract`.
- 같은 검증기의 `--self-test` exit 0: 유효 계약 수용, falsifier 누락 거부, placeholder 거부 3건 통과. 전체 조합의 검증은 아니며 재미/학습 효과를 뜻하지 않는다.
- [CORRECTION] 현재 C1의 비교·근거 선택 ID는 고정돼 있다. 수동 선택이 현재 구현됐다는 초기 control 표현을 제거하고, 두 실험 조건에 공통인 미구현 과제로 명시했다. M8 재사용 범위부터 확인한다.
- [BOUNDARY] 파일럿과 정식 한 빌드의 12명/5유형 T0 평가를 분리한다. 같은 참가자의 후속 과제는 T0 자유 회상·H 기록 뒤에만 두며 출처 설명을 H-2 목표 설명으로 대체하지 않는다.
- 종합 판정과 Aside 조사 한계: `../../handoff/aside-core-loop-results-20260913.md`.
