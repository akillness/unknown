---
updated: 2026-09-09
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-production-director
---

# Production Brief

```yaml
cycle_type: preproduction      # hotfix | balance-patch | content-update | season | preproduction
version: c3-c7                 # 이번 세션에 닫는 회차 범위 (C1·C2는 아카이브 완료)
entry_phase: P1
required_lanes: [planner, worldview, synopsis, systems, balance, economy, presentation, concept, modeling, animation, motion, vfx, product-manager, qa]
main_question: '한 허브 + 4구역 + 6도구의 작업대 공간 추리가 480분 설계 예산·측정 가능한 슬라이스·외부 실행자(Codex) 핸드오프까지 문서로 닫히는가'
next_beat: 'C7 종료 = handoff/ 브리프 + 2D 에셋 세트 + README 프리비즈 GIF + unity/Unknown 스켈레톤'
source_signal: '사용자 2차 요청 2026-09-09 (초안 개발 + 5회 리뷰 + Unity in-repo + 리소스 생성 + README GIF)'
```

## 사용자 요청 원문 요약
[OBSERVED] "규칙확인하고 워크스페이스 내용및 리서치내용 바탕으로 게임초안개발하고 5번리뷰 개선작업진행하자. 이후에 codex gpt6 astra 한테 유니티로 게임 개발하고 검증할꺼야. 리소스는 higgsfield mcp, maxiam, blender mcp 이용해서 리소스만들고 awesome gpt, gti 이용해서 2d 리소스는 모두만들어" + "유니티 프로젝트도 같은 폴더에 만들고 깃푸시할꺼야. 리드미도 게임소개와 플레이 컷씬 gif로 적용해서 이미지 등록해야해"

## 정지선
- 상세: `production/premium-preproduction-contract.md` (C3 개정) Release safety.
- 사용자 직접 수행: git commit/push, Mixamo 다운로드, Steam 계정 작업, Higgsfield 유료 크레딧 승인(계정 상태 확인 후 별도).
