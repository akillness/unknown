---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-modeler
---

# M22 / M24 Seorin_Character — 원본 저작 영수증

[OBSERVED] Blender 5.1.2의 실제 MCP 연결에서 직접 저작·내보내기·렌더했다. `scripts/blender/build-seorin-m22.py` 및 같은 폴더의 `manifest.json`/`provenance.json`이 재현·리그·보존 검사를 소유한다. 외부 메시나 텍스처를 포함하지 않는다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `Seorin_Character.fbx` | 1066396 | `50b502c3f451078bcbd25e8fe943ab2e6908514439a18f464993bf389efdf4f9` |
| `Seorin_Character.glb` | 649068 | `a95b49afd3e6786c9e35420a63a7706f24e0a7d7f6df62a427f3c8842d238646` |
| `Seorin_Character.blend` | 560941 | `a7d1ef36a0cedaac702de1d229b5ea3ab3b5a141d9f236f631b4f43eebc50872` |

[BOUNDARY] 원본 후보의 `runtimeEligible:false`를 유지한다. Unity 런타임 적용은 RFC-CX-018의 별도 네이티브 검토·decision-log 감사와 `M22Embodiment.asset` 승인 게이트로 구분한다. 프리뷰 PNG는 Blender 렌더이며 게임 캡처가 아니다. 기존 컨셉 참조의 외부 상업 라이선스나 인간 플레이 검증을 새로 주장하지 않는다. 이전 제작 후보는 `history/`에 보존했다.

[REVISION M24] 얼굴 윤곽·눈/코/입·연속 헤어 셸과 가닥·소매/앞치마 마감을 개정했다. 실제 19,618 tri/50본, 기존 20k 상한 이내다. 이번 로컬 기본 빌드 재승인은 RFC-CX-M24-20260914 및 `_workspace/current/systems/tech-verification/m24/verification.json`을 따른다. 손 형상·리그·11 take·접촉 시간과 M22 과거 증거는 보존한다.
