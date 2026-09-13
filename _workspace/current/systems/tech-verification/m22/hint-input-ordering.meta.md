---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# M22 · hint-input-ordering.xml

[OBSERVED] 실제 InputSystem.QueueStateEvent의 Enter/Esc 및 패드 B 경로 3/3 통과. 직접 Back/Activate 호출로 닫기를 대체하지 않고 ScreenChanged=0·surface 보존·Journal.HeadSeq 불변을 확인했다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `hint-input-ordering.xml` | 4904 | `87fc169e7c76e1d2c55bd282404910c567ba245520269c2195005e438f88c87e` |

[BOUNDARY] 기존 157개 사례 중 3개의 단정을 강화한 한정 재실행이다. 고유 사례 수에 더하지 않는다. 런타임 코드는 바꾸지 않았으며 이전 전체 회귀와 네이티브 빌드 증거는 그대로 유효하다.
