---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# M22 · hint-focus-restoration.xml

[OBSERVED] `OfferedHintIsFocusableWithoutStealingFocusAndOpensOnlyOnActivation` 1/1 통과. 기존 일반 초점(`handover`)을 기억하고, `hint-offer-dismiss`에 초점을 둔 뒤 실제 InputSystem Enter press/release로 닫았다. `CurrentFocusId`의 이전 일반 초점 복귀, 화면 무재구축, Journal.HeadSeq 불변, shell 유지, 숨겨진 offer의 초점 목록 제거를 함께 확인했다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `hint-focus-restoration.xml` | 3379 | `e53ad52a4b121de423df9196a265d585daa71174b74a71bdc4110f475113a948` |

[OBSERVED] 실행 시각 2026-09-13 12:51:40–12:51:41Z, 사례 실행 1.2200813초. 테스트 소스 SHA-256은 `39fd06d37f17a87a454bc2dd6d0f2141781bedc5a864238f0861d2f364dab465`다.

[BOUNDARY] 기존 157건 중 한 사례의 추가 불변식이며 158건으로 합산하지 않는다. 닫기 동작은 직접 Back/Activate 호출이 아니다. 초점 준비는 직접 Focus 호출이므로 키보드 탐색 증거는 기존 별도 사례를 따른다. 이번 증거는 PlayMode InputSystem 관찰이며 네이티브 화면의 초점 복귀·물리 패드·사람 플레이 검증이 아니다. 런타임 소스와 기존 네이티브 빌드는 변경하지 않았다.
