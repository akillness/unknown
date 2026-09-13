---
updated: 2026-09-13
cycle: 20260909-preproduction-c3
status: draft
supersedes: null
owner: game-modeler
---

# M22 Seorin_Character — 원본 저작 영수증

[OBSERVED] Blender 5.1.2의 실제 MCP 연결에서 직접 저작·내보내기·렌더했다. `scripts/blender/build-seorin-m22.py` 및 같은 폴더의 `manifest.json`/`provenance.json`이 재현·리그·보존 검사를 소유한다. 외부 메시나 텍스처를 포함하지 않는다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `Seorin_Character.fbx` | 995468 | `6bbd9508e6946db5b05ea28db072977da626393d87149471a4e787d308e050e7` |
| `Seorin_Character.glb` | 513020 | `02c127dfdb48ee301df9b7b6f92f759b0d5be04a56bb736b34adeb5bc5ce2100` |
| `Seorin_Character.blend` | 480337 | `e44afbebd9601a5b64df5f4d644fc0b01e8a91b63a3766257480a88430669ae2` |

[BOUNDARY] 원본 후보의 `runtimeEligible:false`를 유지한다. Unity 런타임 적용은 RFC-CX-018의 별도 네이티브 검토·decision-log 감사와 `M22Embodiment.asset` 승인 게이트로 구분한다. 프리뷰 PNG는 Blender 렌더이며 게임 캡처가 아니다. 사용한 기존 컨셉 참조의 외부 상업 라이선스나 인간 플레이 검증을 새로 주장하지 않는다. 이전 제작 후보는 `history/`에 보존했다.
