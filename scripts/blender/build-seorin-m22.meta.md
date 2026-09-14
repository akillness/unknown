---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# M22 / M24 · build-seorin-m22.py

[OBSERVED] RFC-CX-018의 기존 저작 경로를 유지한 RFC-CX-M24-20260914 디테일 개정이다. 실제 Blender 실행·보존 검사는 `_workspace/current/systems/tech-verification/m24/blender-session.json`, 새 기본 빌드·영상은 같은 폴더의 `verification.json` 및 `docs/media/gameplay-m24/provenance.json`을 따른다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `build-seorin-m22.py` | 50686 | `480503a1c856f881d63db6f2efbdbb3f1c90b6ff967ffd3221b87105272d84b7` |

[BOUNDARY] 실제 Blender MCP에서 실행한 저작 레시피다. 외부 메시/애니메이션 다운로드·유료 생성은 없다. 이전 r01과 레시피를 candidate-075에 먼저 보존했다. 원본 .blend 덮어쓰기나 사용자 세션 dirty 불변은 주장하지 않는다. 원본 `runtimeEligible:false`·미확인 상업 라이선스·사람 n=0은 유지한다. 이번 사용자 지시는 관련 자산·영상의 commit/일반 push를 명시적으로 승인했다.
