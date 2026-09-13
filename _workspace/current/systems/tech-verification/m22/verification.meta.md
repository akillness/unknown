---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# M22 · verification.json

[OBSERVED] RFC-CX-017/018 로컬 개발 통합과 입력·중단 계약 보완 영수증. 고유 157건 통과 및 현재 빌드/소스 지문은 `verification.json`, 교정 후 150% 힌트 UI는 `docs/media/gameplay-m22/native-contract-provenance.json`이 소유한다. 기존 모션 미디어와 이전 154건 영수증은 교정 전 지문으로 별도 보존한다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `verification.json` | 25310 | `bb826ee2ad712aaa8f7f2f6b6d13859c60c93848be1be35640ba20c3114687d4` |

[BOUNDARY] 테스트 영수증은 실행 범위만 증명한다. 최종 PlayMode의 serialized boot skip은 `boot-contract-corrections.xml`에서 통과했고, `playmode-before-fix.xml` 및 이전 XML은 과거 기록이다. 프로필 재생성은 제거된 임시 Editor 스모크이며 문구 고정 영구 테스트가 아니다. 새 UI와 기존 MP4는 서로 다른 네이티브 빌드다. 원본 runtimeEligible:false, 상업 라이선스 미확인, 사람 플레이·성능 n=0 및 사용자 전용 commit/push 경계를 유지한다.

[INPUT ORDER] `hint-input-ordering.xml`의 3/3은 기존 사례에 Enter/Esc/패드 B 닫기의 무재구축·무게임명령 단정을 추가한 재실행이다. 고유 합계는 157이며 런타임 소스와 네이티브 assembly는 변경하지 않았다.

[FOCUS RETURN] `hint-focus-restoration.xml`의 1/1은 이전 일반 초점을 기억한 뒤 실제 Enter로 offer를 닫아 `CurrentFocusId` 복귀를 확인한 기존 사례의 추가 검증이다. 직접 Focus는 닫기 대상 준비에만 사용했다. 고유 합계 157·기존 네이티브 빌드는 그대로이며, 네이티브 시각적 초점 복귀를 증명한 것으로 확대하지 않는다.
