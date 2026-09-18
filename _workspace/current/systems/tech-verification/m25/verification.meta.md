---
updated: 2026-09-18
cycle: 20260918-content-update-m25
status: current
supersedes: null
owner: game-systems-designer
---

# M25 검증 팩 (RFC-CX-M25-20260918)

| 파일 | 내용 |
|---|---|
| `verification.json` | 제공자 영수증(잔액 전후·견적 합·외부 소모 관측), 생성 목록(provenance 요약, 반려 2건, 텍스트 부재 100% 검수), Unity 임포트/승인/테스트/빌드/네이티브 캡처, 타이포 스케일 계약, 경계 |
| `import-audit.json` | `M25ResourceProjectBuilder.Import` 생성 — 18항목 원본 경로·SHA·Unity 경로·크기 |
| `build-inventory.json` | 최종 `Unknown.app` 316파일/438,913,631B 전수 SHA + digest `08e71660…` |
| `editmode.xml` `boot.xml` `playmode.xml` | NUnit 결과 원본. `playmode.xml`은 최종 트리에서 마지막으로 실행한 전체 PlayMode(테스트 6종 추가 후) |

- [OBSERVED] 최종 트리: EditMode 65/65 · PlayMode 136(135/0/1 조건부 skip) · 격리 boot 1/1 · M25 테스트 8/8. 첫 전달(v0.25.0-dev)은 134/133/0/1 · 6/6.
- [OBSERVED] 빌드 로그 `T0_MAC_BUILD Succeeded bytes=438913631`(최종, digest `08e71660…`, Release v0.25.1-dev). 첫 전달 438,910,587B/`aaa5361d…`(v0.25.0-dev)는 `verification.json` `build.previous`에 보존.
- 미측정: 사람 플레이 n=0, 성능, Windows, 전체 캠페인. 상업 사용권 UNVERIFIED.
