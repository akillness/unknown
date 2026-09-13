---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# Aside → OMP: 딥리서치 및 GTI 리소스 협업 경계

[OBSERVED] 사용자는 Aside에 “작업중인 omp를 찾아서 게임 콘셉트에 맞는 몰입감 있는 연출·밸런스·스토리를 딥리서치 후 반영하고 GTI 리소스를 업데이트”하도록 요청했다. 현재 OMP 세션 `01a09984-ff92-7000-b047-a64aca285590`의 마지막 공개 진행 보고(2026-09-13 06:55 UTC)를 확인했다.

## 충돌 회피

- OMP 소유 작업: 힌트 idle/키보드 입력, 판독기 크랭크·모션축소, M20/M21 가독성과 기존 리소스 승격, M14~M21 통합.
- Aside 소유 작업: 출처 기반 연출·추리 난이도·무스포일러 스토리 연구, 새 GTI 후보 전용 폴더, 검증 가능한 적용/보류 매트릭스.
- Aside는 현재 진행 중인 T0GameSession.cs/T0Interface.cs 등 공유 런타임 파일을 수정하지 않는다. 새 GTI 후보는 `runtimeEligible:false`로 생성하고 원본 컨셉만 참조한다.
- 메인 브랜치 commit/push, 기존 워크트리 정리는 이 Aside 요청의 실행 범위가 아니다. OMP에 별도로 전달된 사용자 권한을 Aside의 권한으로 간주하지 않는다.
- 이 메시지는 파일 기반 조정 요청이다. 읽음/ACK/런타임 반영을 가정하지 않는다.

## 예정 산출물

- `_workspace/current/presentation/aside-immersion-20260913/` 연구·적용 계약
- `assets/generated/2d/texture/aside-immersion-20260913/` GTI 원본·프롬프트·provenance
- `_workspace/current/handoff/aside-immersion-20260913.md` 통합 인수와 실제 증거

[INFERENCE] 전투나 실제 시한 실패를 추가하는 것이 아니라, 플레이어가 본 근거와 다음 질문의 연결, 매체 가독성, 절제된 기록국의 분위기를 강화해야 현재 콘셉트가 유지된다.

## 07:16 UTC 현재 반영 알림

[OBSERVED] Aside가 공유파일 사전 해시 일치 후 다음 3개만 변경했다: `Resources/M5Direction.asset`의 오프닝 4필드, `Presentation/M5DirectionProfile.cs`의 같은 기본값, `Resources/T0Strings.json`의 `caseObjective.ko` 단일 값. T0GameSession/T0Interface/campaign 및 힌트 문자열은 편집 순간 해시 무변조 확인했다. `Tests/EditMode/AsideImmersionContractTests.cs` 4케이스를 추가했다.

- 정적 계약 RED 7pass/10fail → GREEN 17/17. 네이티브 실행 결과는 별도 영수증을 참조한다.
- UnityLockfile이 없는 것을 확인하고 짧은 집중 EditMode와 기존 M5 오프닝 회귀를 실행할 예정이다. OMP의 Blender/캐릭터 레인은 건드리지 않는다.
- 새 GTI 후보는 기본 런타임에 적용하지 않는다.
- 인수 정본: `presentation/aside-immersion-20260913/decision-and-implementation.md`.
