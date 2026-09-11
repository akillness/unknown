---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-presentation-director
---

# M6 영상 저작 JSON 메타데이터

[OBSERVED] `cinematic-gameplay-m6.json`은 디렉터가 요청한 시네마틱30–40초/방법45–60초 영화 제작의 실행 계약이다. 현재 구현된 T0/C1 선행조건과 저장 발행 조건은 코드/저작 데이터로 확인했다. `shots`의6초·`films`의36/54초·오디오 수치는 **목표**다.

[TARGET] 공식 Higgsfield MCP의 실제 도구/모델/요금/작업ID는 제공자 실행 영수증에서 확정한다. 4×6초=24초 raw를 두 편에서 재사용하며 부족한 설명 시간은 실제 네이티브 화면과 수동 타이포그래피로 만든다. 새 GTI keyframe은 필요한 경우만 추가하며 원본 lineage를 기록한다.

`current`는 승인된 저작 범위이며 생성/렌더/네이티브/권리/몰입 PASS가 아니다. 초기 저작 시점의15개 `acceptance`는 모두 `pending`이었다; 원래 목표를 실제 제공자 측정값으로 덮어쓰지 않는다. 실제 M6 파일/해시/컷 측정/채택·기각은 디렉터의 제작 영수증에 연결한다.

[OBSERVED · 디렉터 결정] `executionAdaptation`은 실제 native 원본이 새 T0b3/C1b1 수락을 포함하지 않는 점을 반영한다. 원래 `films`는 저작 목표로 보존하며 앞선 해금은 수동 규칙 설명, 실제 예시는 복원 후 이동/저장 결과로 제한한다. M6-P05/P15 적용 변경과 구간 가림은 독립 editorial-review에 기록했다. 초기 결정 자체는 최종 렌더 PASS가 아니며 아래 최종 제작·검수 상태가 현재 판정이다.


## 최종 제작·검수 상태 — 2026-09-11

[OBSERVED] 공식 Higgsfield MCP4건 완료와 실제 비용/파일은 `systems/tech-verification/cinematic-gameplay-m6/mcp-production-receipt.json`에 기록했다. 최종 시네마틱36초/1080프레임, 방법54초/1620프레임을 독립 해시·메타데이터 및 편집 프레임 표본으로 확인했다. 실제 컷은 `docs/media/cinematic-gameplay-m6/edit-timeline.json`, 판정은 `systems/tech-verification/cinematic-gameplay-m6/editorial-review.json`이다. 원래 shot/film/audio 수치는 저작 목표로 보존한다.

[OBSERVED] JSON `acceptance`는 개별 판정으로 갱신했다. 모든 기준의 일괄 PASS는 아니다. 새 T0b3/C1b1 저장 전이 목표는 규칙 설명·복원 예시로 조정했고, 주요32px 자막은 확인했으나 연속3초 완전 가독시간·수치 대비는 미측정이다. 오디오는 로컬 저작 필터 노이즈를−27LUFS 목표로 믹스했으며 생성 오디오 채택·청취 승인·저모션 별도판은 완료로 처리하지 않는다. G4 인간 몰입, runtime 성능, 상업 권리와 게임 적용은 별도 미검증이다.
