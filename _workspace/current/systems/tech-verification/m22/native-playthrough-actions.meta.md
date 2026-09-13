---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# M22 · native-playthrough-actions.json

[OBSERVED] RFC-CX-017/018 로컬 개발 통합 산출물. 정본 검증 범위와 실패/통과 구분은 `_workspace/current/systems/tech-verification/m22/verification.json`, 실제 화면 출처·편집 범위는 `docs/media/gameplay-m22/provenance.json`이 소유한다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `native-playthrough-actions.json` | 100811 | `01b6c88e2963ed5202805639293fd11be8a258fb5c36a03855bd0c9b9299e99b` |

[BOUNDARY] 테스트 영수증은 실행 범위만 증명한다. `playmode-before-fix.xml`은 수정 전 실패 기록이며, 전체 PlayMode의 격리 부팅 skip은 별도 `boot.xml`에서 통과했다. MP4는 실제 개발 빌드 화면을 창 장식만 잘라 30fps로 인코딩한 것이며 게임 성능 실측·생성 영상·사람 플레이가 아니다. 원본 Blender 후보의 `runtimeEligible:false`, 미확인 상업 라이선스, 사람 플레이 n=0, 사용자 전용 commit/push 경계를 유지한다.
