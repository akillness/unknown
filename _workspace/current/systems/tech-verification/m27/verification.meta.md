---
updated: 2026-09-18
cycle: 20260918-balance-patch-m27
status: current
supersedes: null
owner: game-systems-designer
---

# M27 검증 팩 (RFC-CX-M27-20260918)

| 파일 | 내용 |
|---|---|
| `verification.json` | 조사·데이터·런타임·제공자(글리프 3장, 10.5 크레딧)·테스트·QA 루프 회귀 포착·빌드·경계 |
| `import-audit.json` | `M27ResourceProjectBuilder.Import` — 3항목 SHA·Unity 경로·크기 |
| `build-inventory.json` | 최종 `Unknown.app` 316파일/445,068,423B 전수 SHA + digest(`db8cdb7b…`류; 전문은 파일) |
| `editmode.xml` `playmode.xml` `boot.xml` | NUnit 결과 원본 — QA 루프 `20260919T164740Z`(조용한 트리)에서 복사 |
| `qa-loop-20260919T163626Z.red.triage.md` | 루프가 잡은 회귀 3건의 분류(같은 세션 해소) |
| `qa-loop-20260919T164740Z.{summary.json,triage.md}` | 최종 GREEN 사이클 |

- [OBSERVED] EditMode 65/65 · PlayMode 148(147/0/1 조건부 skip) · 격리 boot 1/1 · M27 6/6.
- 미측정: 사람 플레이 n=0, 성능, Windows(모듈 부재). 상업 사용권 UNVERIFIED. 수치 밴드 무변경.
