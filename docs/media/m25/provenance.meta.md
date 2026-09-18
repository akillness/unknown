---
updated: 2026-09-18
cycle: 20260918-content-update-m25
status: current
supersedes: null
owner: game-production-director
---

# docs/media/m25 — 출판 영수증 (RFC-CX-M25-20260918)

`provenance.json`은 `scripts/`가 아닌 세션 내 생성기(PIL/ffmpeg)로 작성됐으며 손으로 편집하지 않는다. 항목별 `derived_from`·`output_sha256`·`claim`이 정본이다.

| 파일 | 종류 | 출처 |
|---|---|---|
| `start-screen.png` `guide-overlay.png` `guide-overlay-tall.png` `opening-motion-frame.png` `circuit-teaching-header.png` | **실제 macOS 개발 빌드 창 캡처** (빌드 digest `aaa5361d…`, 316파일/438,910,587B). OS 창틀만 잘라냈고 속도·프레임·UI를 합성하지 않았다. 에이전트 키 입력, 격리 `--t0-save-dir`, 사람 플레이 아님 | `unity/Unknown/Builds/T0-mac/Unknown.app` |
| `mo-opening-harbor.mp4` `mo-hub-watchroom.mp4` `mo-reader-operation.mp4` `opening-harbor.gif` | Higgsfield `seedance_2_0` 이미지→영상 **프리비즈**(5 s, 720p, 무음). 게임플레이 아님 | `assets/generated/video/m25/` |
| `cast-portraits.jpg` `tool-icons.jpg` `zones.jpg` `opening-harbor.jpg` `hub-watchroom.jpg` | Higgsfield `gpt_image_2` / `nano_banana_flash` 생성물의 축소 파생본. 컨셉/README용, 게임플레이 아님 | `assets/generated/2d/m25/` |

- 런타임 승격 상태: 원본 `assets/generated/**/provenance.json`은 `runtimeEligible:false`를 유지한다. Unity 측 `Resources/M25Resources.asset`만 로컬 개발 프로필로 `runtimeApproved:true`(승격 감사: `production/decision-log.md` RFC-CX-M25-20260918 하위 블록). 상업 사용권은 UNVERIFIED.
- 미측정: 사람 플레이테스트 n=0, 성능, Windows.
