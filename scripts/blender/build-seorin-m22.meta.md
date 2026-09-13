---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# M22 · build-seorin-m22.py

[OBSERVED] RFC-CX-017/018 로컬 개발 통합 산출물. 정본 검증 범위와 실패/통과 구분은 `_workspace/current/systems/tech-verification/m22/verification.json`, 실제 화면 출처·편집 범위는 `docs/media/gameplay-m22/provenance.json`이 소유한다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `build-seorin-m22.py` | 47362 | `e90a36d07bcef80a9b2510ddf2c0e846627c6efc84fa31eb3b90f9bf1f4fb8e0` |

[BOUNDARY] 실제 Blender MCP에서 실행한 저작 레시피다. 외부 메시/애니메이션을 다운로드하지 않았다. 원본 Scene 비교는 45/45 통과, 상세 구조 비교는 기록이 있는 36/36 통과이며 초기 9건에는 상세 필드가 없다. 원본 .blend 덮어쓰기나 작업 세션의 dirty 불변을 주장하지 않는다. 원본 Blender 후보의 `runtimeEligible:false`, 미확인 상업 라이선스, 사람 플레이 n=0, 사용자 전용 commit/push 경계를 유지한다.
