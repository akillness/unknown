---
updated: 2026-09-18
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

## M22 — 기존 플레이 슬라이스 개선과 main 통합

[OBSERVED] 사용자 요청: 미커밋 변경 최신화, main 통합, 작업 워크트리 정리, 서브에이전트를 통한 게임 요소 분석·밸런스·연출·리소스 개선.

- cycle_type: preproduction (기존 C7 검증 슬라이스 후속; 본편 콘텐츠 생산/출시 승격 아님)
- version: M22
- [DECISION] 기존 M14~M21 작업을 보존한다. 밸런스는 무료 힌트 제안 임계/쿨다운을 분리하고 키보드·포인터의 활동 판정을 일치시킨다. 근거 없이 퍼즐 해답·소모량·180초 설계 수치를 바꾸지 않는다.
- [DECISION] 판독기 크랭크를 실제 판독 성공에 연결한다. 페이지 이동·입장만으로 판독을 연출하지 않으며, 모션 축소는 진행 중 동작에도 즉시 적용한다.
- [DECISION · 사용자 선택] 신규 r02는 진단 전용·승인 보류를 유지한다. 기본 화면은 기존 승인 M7 종이·청동 리소스의 대비와 표현을 개선한다. 사용권 미확인을 내부 승인으로 우회하지 않는다.
- [DECISION] Main이 공유 파일·최종 Unity 실행·리소스 승격·Git 통합을 단독 소유한다. 구현 서브에이전트는 파일 경계 안에서 수정하고 검증 실행은 하지 않는다.
- [BOUNDARY] 저장 스키마·증거/해금 규칙·캐논 불변. 사람 몰입/재미/플레이타임은 미측정이며 자동화 영수증으로 대체하지 않는다.

## M22 확장 — 유사작 조사와 주인공·손 동작

[OBSERVED] 최신 사용자 지시: 유사게임의 컨셉·톤을 조사·차용하되 게임플레이 구성을 우선하고, **Blender MCP로 주인공 캐릭터와 실제 조작의 손 동작·모션을 구현·적용**한다.

- [DECISION] 기존 고정 관찰점·비전투·캐논은 유지한다. 이번 명시적 인물/손 요청이 과거 RFC-A1의 손 없음 및 RFC-P4-001의 3D 인물 보류를 검증 슬라이스 범위에서 재개한다(RFC-CX-018).
- [DECISION] 한서린의 원래 컨셉(걷은 소매·좁은 작업앞치마·허리 수첩)을 독자적 Blender 메시·리그·클립으로 만든다. 외부 게임의 얼굴·자산·글귀·고유 UI를 복제하지 않는다.
- [DECISION] 관찰/문서 취급 → 회로 정렬 → 판독 → 대조 → 명시적 확정 흐름에 손의 접촉·복귀를 연결한다. 실패·취소·되돌림은 성공 모션을 발생시키지 않고, 렌더가 저장/판정 상태를 변경하지 않는다.
- [BOUNDARY] 신규 3D는 외부 생성 이미지와 별개인 프로젝트 저작물이다. 원본 사용자 Blender Scene은 보존하고 별도 스튜디오 Scene과 새 .blend/GLB/Unity용 FBX를 납품한다. 원본 r02 이미지 보류 및 상업 출시 미승인은 그대로다.

### 추가 품질 지시

[OBSERVED 2026-09-13] 사용자: “완성도있게 만들어”. 파일 생성이나 컴파일만으로 완료하지 않는다. 인물의 목/어깨/중립 손 자세, 도구와 손의 실제 접촉, 읽기 시야, 중간 취소·모션 축소·저장 실패를 네이티브 장면과 동작으로 확인하고 발견한 결함을 수정한다.

[BOUNDARY] `CLAUDE.md` §10은 git commit/push를 사용자 수행으로 유지한다. 현재 main 작업트리에 통합·검증하고 정확한 변경/검증/워크트리 상태를 인계하되, 에이전트가 커밋·푸시를 실행하거나 수행했다고 주장하지 않는다.

## M24 — 플레이 영상 갱신과 Blender 캐릭터 디테일

[OBSERVED] 사용자: “플레이영상 업데이트해서 깃푸시하고, 캐릭터 블랜더로 디테일 업데이트해줘”.

- cycle_type: preproduction, 기존 C7 슬라이스의 아트·미디어 후속. 새 구역/본편 생산 아님.
- [DECISION] 한서린의 원본 컨셉과 비대칭 단발·작업복 정체성을 지킨다. 얼굴 윤곽·머리결·옷깃/접힘·앞치마/장비 마감을 Blender에서 개선한다. 기존 Generic 본 이름·계층·6개 의미 클립·접촉점·동작 시간은 유지한다.
- [TARGET] 캐릭터20k triangle/65본 이하, 양손 합12k/각22본 이하, 최대4본 웨이트. 기존 원본 장면/출력은 보존하며 외부 자산·유료 생성·라이선스 승격은 하지 않는다.
- [DECISION] Unity 임포트와 실제 네이티브 외형/동작 확인 뒤 같은 업데이트 빌드로 플레이 영상을 캡처한다. 실제 앱 창만 녹화하고 생성 프레임·전체 데스크톱·마이크·연속 완주처럼 보이는 저장 시편 접합은 금지한다. 편집/시편/빌드 출처를 표시한다.
- [OWNERSHIP] Main이 Blender 실행·Unity 통합·녹화·명시 경로 stage/commit/일반 push를 소유한다. 사용자 최신 요청이 이번 Git 전달을 명시 승인한다. 사람·Base·Windows·전체 캠페인 gate는 유지한다.

## M25 — Higgsfield 전 리소스 제작·적용, 튜토리얼·가독성, 빌드·배포·push·README (2026-09-18)

[OBSERVED] 사용자: "힉스필드이용해서 오브젝트, 캐릭터, 모션, 배경 등 모든 리소스 컨셉에 맞게 생성해서 적용하고 업데이트해줘. 튜토리얼과 가이드부분도 강화하고, 가독성을 위해 텍스트는 변별력있게 구성해줘." 이어서 "빌드하고 배포까지 해야해. 깃푸시까지, 리드미도 개선된 내용을 바탕으로 전체 새롭게 업데이트해야해".

- cycle_type: content-update (P1 진입). 기존 T0~C1-b2 슬라이스의 리소스·가이드·가독성 갱신이며 새 구역/본편 생산이 아니다.
- [DECISION · 사용자 지정] 이번 사이클의 신규 리소스 제공자는 **Higgsfield CLI**(이미지·영상·필요 시 3D)다. 이 최신 지시가 RFC-CX-006(이미지=GTI)보다 우선한다(RFC-CX-M25-20260918). 과거 GTI/Blender/Higgsfield 산출물의 출처 기록은 보존하고 재표기하지 않는다.
- [DECISION] 시각 원전은 `concept/style-guide.md`와 원본 컨셉 시트다(RFC-CX-009 유지). 현재 플레이 화면·프리팹·3D 프리뷰·M5/M6 파생물은 참조 입력으로 쓰지 않는다. 신규 참조 바인딩은 decision-log의 M25 블록이 명시한 원본 c4 시트에 한정한다.
- [DECISION] 적용 범위: (1) 오프닝 배경·시작 화면 배경(RawImage), (2) 인물 초상 5인 1:1 슬롯(안내 화면 인물 카드 · T0 공개 범위 내 이름·공적 역할만), (3) 도구 6종 아이콘(도구 휠·안내 카드), (4) 오프닝 모션 클립(모션 축소 시 정지 이미지로 대체, 재생 실패 시 정지 이미지 유지), (5) 구역 4곳 배경(안내 화면 참고 도판·README). 저장 스키마·퍼즐 데이터·`Data/Tables/*.json`·캠페인 정본은 바꾸지 않는다.
- [DECISION] 튜토리얼/가이드: 새 `guide` 오버레이(F2 · 툴바 「안내」)가 현재 단계의 목표·도구 사용 절차·조작·규칙(힌트 무료·되돌림·2단계 확정)을 한 화면에 모은다. 회로/판독 안내 접두는 단계·남은 조건을 구조화한다. 힌트 본문(캠페인 정본)은 손대지 않는다.
- [DECISION] 가독성: `T0Interface`에 명명된 타이포 스케일(Display 30 · Title 24 · Section 18 Bold · Body 21 · Label 18 Bold · Helper 15 · Meta 14)과 행간을 도입한다. 계층 순서 계약(body > label > helper, label만 Bold)은 기존 PlayMode 계약 테스트로 유지한다. 게이트 off 리터럴은 유지한다.
- [TARGET] 신규 이미지 ≤ 30장, 영상 ≤ 3클립, Higgsfield 크레딧 ≤ 300. 실제 소모는 전후 `higgsfield account status` 차액으로 decision-log에 기록한다.
- [OWNERSHIP] Main(디렉터 역할)이 생성·임포트·승격 감사(로컬 개발 프로필 한정, 상업 사용권은 UNVERIFIED 유지)·Unity 검증·빌드·GitHub Release(개발 빌드 zip)·명시 경로 stage/commit/일반 push·README 전면 갱신을 소유한다. 사용자 최신 요청이 빌드·배포·push를 명시 승인한다.
- [BOUNDARY] force push·타인 변경 되돌림·Steam/실명/금융·상점 공개는 하지 않는다. 사람 플레이테스트 n=0, 성능·Windows·전체 캠페인·상업 게이트는 유지한다. 55개 미추적 로그/XML은 스테이징하지 않는다.

## M26 — 유사게임 딥리서치 → 디벨롭 → Higgsfield 리소스 갱신 → 빌드·배포 (2026-09-18)

[OBSERVED] 사용자: "유사게임 딥리서치후 디벨롭해서 빌드후 배포 ㄱ, 리소스는 힉스필드이용해서 업데이트 ㄱ" (`aside-browser` 스킬 호출).

- cycle_type: content-update (P1 진입). 범위는 여전히 T0(3비트)→C1-b2 슬라이스이며 후반 장·NPC 증언·3제출은 Base gate 뒤로 유지한다(R8/R9/R10 gate-blocked).
- [DECISION · 조사] Aside(`aside exec --effort ultrabrowse`, 세션 `szZUom6G8YnfSs9Q`)가 비교작 8종 이상(Obra Dinn·Golden Idol·Roottrees·Sennaar·Lorelei·Strange Horticulture·Her Story/Immortality·Pentiment·Paradise Killer·Shadows of Doubt·Unheard·Papers Please·Outer Wilds 로그)의 **첫 10~30분 교습법과 증거/가설 UI**를 스토어·리뷰·개발자 강연에서 읽고 `planning/aside-similar-games-research-20260918.md`에 보고한다. 2026-09-13 조사(R1~R10)와 M23 달성 범위를 먼저 읽어 **중복 권고를 배제**한다. 저장소 편집은 그 보고서 1건뿐, Git·구매·신규 로그인 없음.
- [DECISION · 디벨롭] 보고서 S2 권고 중 캐논 제약(비전투·고정 시점·도구 6·무료 힌트·2단계 확정·매체 2종·공개 상한) 안에서 T0~C1 슬라이스에 바로 구현 가능한 상위 항목만 고른다. 정답 대행·자동 모순선·후반 콘텐츠·사람 패널 전제 항목은 채택하지 않는다. 채택 항목은 decision-log RFC-CX-M26-20260918에 적고 각각 PlayMode/EditMode 테스트로 고정한다.
- [DECISION · 리소스] 보고서 S3 목록을 Higgsfield CLI(`gpt_image_2`/`nano_banana_flash`/`seedance_2_0`)로 생성한다. RFC-CX-M25 절차 그대로: `scripts/gen-higgsfield.py` + `concept/m26-higgsfield-jobs.json` → `assets/generated/{2d,video}/m26/` → SHA 대조 임포터 → 승격 감사 → 로컬 개발 프로필 승인. 크레딧 영수증은 견적 합 + 전후 잔액.
- [TARGET] 신규 이미지 ≤ 20장, 영상 ≤ 2클립, 크레딧 ≤ 200.
- [OWNERSHIP] Main(디렉터·planner·systems 역할)이 조사 감독·채택 판정·구현·테스트·빌드·GitHub Release(prerelease)·명시 경로 commit/일반 push를 소유한다. 사용자 요청이 빌드·배포·push를 명시 승인한다.
- [BOUNDARY] force push·타인 변경 되돌림·Steam/실명/금융·상점 공개 없음. 사람 플레이 n=0·성능·Windows(모듈 부재) 미측정 유지. 조사 결과가 사람 데이터를 대체한다고 적지 않는다.
