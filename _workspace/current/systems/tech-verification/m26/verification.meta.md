---
updated: 2026-09-18
cycle: 20260918-content-update-m26
status: current
supersedes: null
owner: game-systems-designer
---

# M26 검증 팩 (RFC-CX-M26-20260918)

| 파일 | 내용 |
|---|---|
| `verification.json` | Aside 조사 세션·채택/보류/비채택, Higgsfield 영수증(9장, 견적 37.5·관측 37.5·세션 밖 소모 54.0), 생성 목록, sim/임포트/승인, 테스트, 빌드, 네이티브 캡처, 경계 |
| `import-audit.json` | `M26ResourceProjectBuilder.Import` 생성 — 9항목 원본 경로·SHA·Unity 경로·크기 |
| `build-inventory.json` | 최종 `Unknown.app` 316파일/444,954,066B 전수 SHA + digest `a4864536…` (릴리스 `v0.26.0-dev`) |
| `qa-loop-20260919T135355Z.{summary.json,triage.md}` | 3시간 QA 루프의 GREEN 사이클(조용한 트리·표준 우선순위) — 이 팩의 NUnit XML은 그 실행에서 복사 |
| `editmode.xml` `playmode.xml` `boot.xml` | NUnit 결과 원본(최종 트리, QA 루프 `20260919T135355Z`) |

- [OBSERVED] EditMode 65/65 · PlayMode 142(141/0/1 조건부 skip) · 격리 boot 1/1 · M26 테스트 6/6(영수증 `R` 키 포함).
- [OBSERVED] `T0_MAC_BUILD Succeeded bytes=444954066`. 네이티브 캡처 5장은 캡션 수정 직전 빌드(444,948,527B/`7e210806…`)에서 찍었고 최종 빌드와의 차이는 매체 도판 캡션 문자열과 영수증 `R` 키 경로뿐이다.
- [OBSERVED] QA 루프 첫 사이클(`20260919T133959Z`)은 오염 행(스로틀링·실행 중 스크립트 편집·boot 접두사)이며 원장에 그대로 남긴다. 상세: `production/decision-log.md` RFC-CX-M26-QA-20260918.
- 미측정: 사람 플레이 n=0, 성능, Windows(모듈 부재). 상업 사용권 UNVERIFIED.
