---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# 콘셉트 기반 몰입 개선: 조사 → 판정 → 반영

## Scope

[OBSERVED] OMP `01a09984-ff92-7000-b047-a64aca285590`가 `~/orca/unknown`에서 M14~M22 통합, 힌트 입력/idle, 판독기 크랭크, 기존 승인 작업면 개선을 진행 중이다. 사용자는 OMP에서 2026-09-13 06:59:53 UTC에 **기존 승인 리소스 개선**을 선택했다. Aside는 해당 코드/재질을 덮어쓰지 않는다. 이후 OMP에 전달된 Blender 인물·손 모션 작업도 이 패스에서 복제하지 않는다.

이 작업은 기존 `20260909-preproduction-c7`의 좁은 후속 개선이다. 신규 챕터·정답·캐논·캐릭터·게임 시간·승인 권한을 추가하지 않는다. 커밋/푸시/워크트리 정리는 OMP 소유이며 Aside는 실행하지 않는다.

## Evidence and adoption

| 조사 발견 | 판정 | 실제 적용/경계 |
|---|---|---|
| Frictional 4-Layers: 이야기를 진행시키기 위한 행동보다 이야기 때문에 하는 행동 | 채택 | 오프닝의 일반적 동사 안내를 마지막 당직의 상황 + 다음 관찰 행동으로 변경 |
| Kelsey Beachum GDC 2021: 질문마다 관찰 가능한 앵커 | 제한 채택 | 목록/서랍 대조를 지시하되 개수 차이와 판의 의미는 먼저 공개하지 않음 |
| Pope Obra Dinn: 즉시 개별 정답 확인은 대입을 유도 | 유지 | 기존 서로 다른 2매체 대조·안전한 확정 유지. 원작의 3건 묶음·회중시계는 복제하지 않음 |
| Golden Idol: 근접 피드백은 좌절을 줄이는 대신 난도와 길이도 줄일 수 있음 | 보류 | 실측 없이 허용 오차를 넓히지 않음. 기존 4/8분 설계는 미구현 조위정합용이며 T0 밸런스 개선 완료로 보고하지 않음 |
| `nearMissLimitMinutes` 미러 누락 | 보류 | 소비하는 C# 경로가 없는 노브 추가는 실행 효과가 없다. 생성 테이블 직접 편집도 금지. 조위정합 구현 시 `tools.md` 원천→emitter→테스트로 함께 반영 |
| Xbox XAG 102: 환경 재질과 텍스트 대비를 분리 | 채택 | 새 패널은 중앙 저대비/어두운 여백. 런타임 글자는 별도 단색 배킹 검증을 통과해야 함 |
| XAG 103/117: 색 단독 표시 회피와 모션 선택권 | 유지 | 새 지속 펄스·블룸·카메라 흔들림·강제 시간 실패는 추가하지 않음. 모든 새 이미지는 정적 후보 |
| 목표 fallback에 후반 단서가 포함됨 | 방어적 수정 | 정상 데이터에서는 현재 발동 0건. 누락/가림 시에도 초반 스포일러를 내지 않는 일반 문구로 변경. 현재 노출 버그를 고쳤다고 주장하지 않음 |

출처 전문·짧은 인용·조사 당시 코드 좌표: `research-story.md`, `research-balance.md`, `research-presentation.md`. 연구 제안과 최종 판정이 다르면 이 표가 우선한다.

## Applied copy contract

| 필드 | 반영 문구 |
|---|---|
| firstTitle | 마지막 당직 |
| firstCaption | 폐국 전날 밤, 마지막 당직을 맡았다. |
| secondTitle | 목록과 서랍 |
| secondCaption | 목록과 서랍을 대조해, 무엇을 남길지 정한다. |
| caseObjective.ko fallback | 현재 기록을 대조하고 다음 근거를 확인 |

반영 파일: `unity/Unknown/Assets/_Project/Resources/M5Direction.asset`, `Presentation/M5DirectionProfile.cs` 기본값, `Resources/T0Strings.json`의 `caseObjective.ko` 단일 값. 이미지·재질·승인값·두 컷 길이·motto·intro·조작·힌트·저장·시뮬레이션은 보존한다.

[OBSERVED] 독립 synopsis/worldview 리뷰가 위 문구를 ACK했다. 필수 counter인 `.asset`↔C# 기본값 동기화를 채택했다. 그 리뷰는 실행 테스트가 아니며 “PlayMode 회귀 0” 같은 예측을 런타임 영수증으로 사용하지 않는다.

## Rejected proposals and corrections

- 초안의 “목록 세 줄보다 한 점 더”는 **3줄=5점 / 실물6점**을 혼동시키므로 기각했다. 차이1점은 T0 발견의 보상이라 오프닝에서 선지급하지 않는다. 이는 B01 상한 위반이라고 잘못 이름 붙이지 않는다.
- 오프닝에 결손4시간·정전·사건 인과·서명 이름·후반 시각을 넣지 않는다.
- 텍스처 위에 글자를 직접 굽지 않는다. 이미지 속 의미 없는 종이 무늬도 정식 단서가 될 수 없다.
- 최초 GTI `quiet-watchroom-opening`은 요청한 새 구도·닫힌 장부·독립된 읽기 여백을 충족하지 못했다. 개방된 종이의 미세 패턴도 남아 **수정 필요, 런타임/최종 아트로 사용하지 않는다**. 원본·실제 수정 프롬프트·해시는 보존한다.
- GTI 2차 `nav-quiet-field`는 기존 M20 작업면과 다른 사건 열용 정적 재질 후보다. 표식 스트립은 코드의 도형으로 만들 수 있어 별도 생성하지 않는다.
- 몰입도 상승률, 플레이시간 달성, 재미/접근성 인증 수치를 만들지 않는다.

## Runtime and rights boundary

신규 GTI 후보는 모두 `runtimeEligible:false`, `commercialReleaseEligible:false`. 사용료/내부 이미지 생성 횟수는 API에서 확인되지 않았으므로 unknown이다. CLI 호출 수와 최종 산출물 수를 이미지 생성 도구 내부 호출 수로 등치하지 않는다.

[blocked] 새 아트의 기본 런타임 적용은 사용권/사용자 적용범위 결정과 네이티브 화면 검증이 남아 있다. 기존 승인 아트는 변경하지 않는다.

[blocked] Unity 점유가 해소된 뒤 6000.5.6f1로 집중 검증을 실행했으나, Aside 실행 환경의 라이선스 클라이언트가 유효한 권한을 찾지 못해 exit198로 테스트 전에 종료했다. XML은 없고 PlayMode는 시작하지 않았다. 이 패스의 네이티브 실행 영수증은 별도로 필요하며 OMP의 이전 결과를 재사용하지 않는다.

[OBSERVED] 최종 소스·데이터 검증 17/17, campaign 50/50, 독립 QA scoped ACK. `status: current`는 이 좁은 반영/보류 결정의 정본이라는 뜻이며 전체 게임 또는 새 아트 런타임 승인이라는 뜻이 아니다. QA가 지적한 테스트 메타 누락과 C#/JS 금지어 차이는 수정했다. OMP 소유 테스트의 옛 fallback literal·과거 문서 설명은 인수 파일 AIM-01/02로 남겼다.

[OBSERVED] 새 패널 중앙 84% 원본 픽셀의 종이색 최저 대비 7.76:1, 약화색은 4.20:1로 미달이다. 약화색을 그대로 얹지 않는다. 실제 UI 검증과 구분하며 결과는 `systems/tech-verification/aside-immersion-20260913/source-image-metrics.json`이 소유한다.
