---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M23 · 실행 XML

최종은 editmode.xml64/64 + playmode-release-final.xml112/113(격리 boot1 skip) + boot-release.xml1/1, 고유177개 통과다. Node7/7·캠페인50/50·증거 그래프18/18은 NUnit 개수와 합치지 않는다. 아래 RED와 중간 실패도 삭제하지 않았다.

| XML | pass | fail | skip | SHA-256 |
|---|---:|---:|---:|---|
| `editmode.xml` | 64 | 0 | 0 | `aea467e625d5850268029d2338af82945d89c8c22d1278fdaeb45272afc9b1cb` |
| `playmode-release-final.xml` | 112 | 0 | 1 | `8477623234a03fe8480ecb2827e116a0db4429872f32b64bcbb5c0937e537df1` |
| `boot-release.xml` | 1 | 0 | 0 | `8bc3c01e51ee532cff2ea18135a1c582ec486eef23905468994a34e3b1d51665` |
| `review-feedback-red.xml` | 0 | 2 | 0 | `fbfe0b217180b92d67083505a806695057c88b451c7793f4a29078f3e9a16566` |
| `review-boundary.xml` | 2 | 1 | 0 | `9fc87d45759c6757e5932fa8a56dc7f4ee81c65d05c71921d82c703288dc57c6` |
| `feedback-visibility-red.xml` | 0 | 1 | 0 | `cc8369ba8e49b395dad0a2345b6c1ff5e1bc86f602500588083d8de18cae7c1d` |
| `playmode-release.xml` | 111 | 1 | 1 | `339a55ce3a639e650f219470eb992317b37bc63c7b5ca5e6fbe40f054f972046` |

[BOUNDARY] review-feedback-red는 Q1/Q2 원재현, review-boundary는 늦은 실패 경계, feedback-visibility-red는 Q5 원재현이다. playmode-release의 Band comparison clipping은 첫 고정 알림안이 Navigation을 줄인 실패이며 최종에서 오른쪽 작업면만 분리해 해결했다. 반복 실행을 고유 개수로 더하지 않는다.
