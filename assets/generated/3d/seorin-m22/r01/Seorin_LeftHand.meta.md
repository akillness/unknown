---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-modeler
---

# M22 / M24 Seorin_LeftHand — 원본 저작 영수증

[OBSERVED] Blender 5.1.2의 실제 MCP 연결에서 직접 저작·내보내기·렌더했다. `scripts/blender/build-seorin-m22.py` 및 같은 폴더의 `manifest.json`/`provenance.json`이 재현·리그·보존 검사를 소유한다. 외부 메시나 텍스처를 포함하지 않는다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `Seorin_LeftHand.glb` | 174956 | `9923f4210453c45e309b18d8d34f3dce9b0ad79108d45b7210d9cfcd135c509a` |
| `Seorin_LeftHand.fbx` | 773116 | `a4e9027d1cb70327a7355dabbd42cbfb03f98b03d898e12673ac5573ec96c9b1` |

[BOUNDARY] 원본 후보의 `runtimeEligible:false`를 유지한다. Unity 런타임 적용은 RFC-CX-018의 별도 네이티브 검토·decision-log 감사와 `M22Embodiment.asset` 승인 게이트로 구분한다. 프리뷰 PNG는 Blender 렌더이며 게임 캡처가 아니다. 기존 컨셉 참조의 외부 상업 라이선스나 인간 플레이 검증을 새로 주장하지 않는다. 이전 제작 후보는 `history/`에 보존했다.

[REVISION M24] 캐릭터 디테일 개정에 따른 재출력이다. 손 형상·리그·11 take·접촉 시간은 유지했다. 로컬 기본 빌드 재승인은 RFC-CX-M24-20260914 및 `_workspace/current/systems/tech-verification/m24/verification.json`을 따른다. M22 과거 증거는 소급하지 않는다.
