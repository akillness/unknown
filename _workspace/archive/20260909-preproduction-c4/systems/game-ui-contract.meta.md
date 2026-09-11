---
updated: 2026-09-09
cycle: 20260909-preproduction-c4
status: superseded
supersedes: null
owner: game-systems-designer
describes: _workspace/current/systems/game-ui-contract.json
---

# game-ui-contract.json 메타

## 소유·출처
- 소유 레인: `game-systems-designer` (C4 상호작용·연출·Unity 담당 산출물).
- 스키마 출처: 계정 스킬 `game-ui-ux` v1.0 (`/Users/jangyoung/.aside/u/0/skills/user/game-ui-ux/`), `references/contract-example.json` 구조를 따르고 `scripts/validate-game-ui.py`로 검증.
- 내용 출처: `_workspace/current/planning/gdd.md`, `_workspace/current/worldview/worldview-bible.md`(불가침 6법·기록의 한계·엔딩 3종), `_workspace/current/qa/c2-review.md`(F3 잔차·F5 파괴 불가 경로), `_workspace/current/production/premium-preproduction-contract.md`(정직성 게이트·비전투 대체 검증).
- 형제 문서: `systems/unity-implementation.md`, `systems/interaction-rules.md`.

## 해시 (as of 2026-09-09 23:4x KST)
| 파일 | sha256 | 크기 |
|---|---|---|
| `game-ui-contract.json` | `f61ffa8895122e12e8a79aceb8a38f81607efd18880c161a9f523445c6c9b300` | 30,302 bytes |
| `unity-implementation.md` | `9a2ab6a8224f9a18…` (앞 16자리) | 6,077 chars |
| `interaction-rules.md` | `87ae669092172426…` (앞 16자리) | 4,441 chars |

JSON에는 frontmatter를 넣지 않았다(파서 보호). 메타는 이 파일이 단독으로 보유한다.

## 검증 실행 `[OBSERVED]`
명령(읽기 전용, 표준 라이브러리 전용, 기존 설치 python 사용):
```
python3 /Users/jangyoung/.aside/u/0/skills/user/game-ui-ux/scripts/validate-game-ui.py --self-test
python3 /Users/jangyoung/.aside/u/0/skills/user/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json
```
- 실행 환경: Python 3.13.12 (`python3 --version`), macOS, 네트워크 접근 없음, 유료 도구 없음.
- 자체 시험 결과: `PASS: valid accepted` / `PASS: missing back behavior rejected` / `PASS: placeholder rejected` — 3/3 PASS, exit 0.
- 계약 검증 결과: **1차 실행 error 7건**(짧은 문자열 7곳: `screens[14].initial_focus`, `screens[15].initial_focus`, `data_bindings[2].element`, `verification.matrix[1|3|5|8].locale`) → 문구 보강 후 **2차 실행 `PASS: valid game UI contract`, error 0건, exit 0**.
- 검증기는 대상 파일을 수정하지 않는다(읽기 전용). 스킬 디렉터리도 변경하지 않았다.

## 계약 수량 `[OBSERVED]`
| 항목 | 수 |
|---|---|
| screens | 19 |
| player_decisions | 7 |
| data_bindings | 9 |
| verification.matrix | 10 |
| verification.acceptance | 5 |
| decisions (설계 결정 기록) | 7 |

커버 범위: 부팅·타이틀·설정·입력 리매핑·저장 슬롯·손상 복구·구역 지도·조사 시점·작업대·도구 패널·증거함·가설판·힌트·확정 프리뷰·결과 체크포인트·일시정지·최종 제출·엔딩 패널·크레딧.

## 관측과 목표의 구분
| 구분 | 항목 |
|---|---|
| `[OBSERVED]` | Unity 에디터 `6000.5.6f1` 설치 확인(`ls /Applications/Unity/Hub/Editor/`, 2026-09-09 23:32 KST), Python 3.13.12, 검증기 실행 결과와 위 수량·해시 |
| `[TARGET]` | 화면 구성, 초점 순서, 확정 조건, 힌트 3단계, 해상도군 4종, 성능 예산, 슬라이스 20~30분, 480분 본편 |
| `NOT-MEASURED` | 프레임 시간, 로드 시간, 저장 쓰기 시간, 실제 패드 조작감, Steam Deck 동작, 플레이어 완주 시간, 접근성 실사용 검증 |

`verification.evidence`는 명시적으로 "증거 없음"을 선언한다. `verification.matrix` 10행은 **실행 계획**이며 관측 결과가 아니다. `evidence=[]` / `NOT-MEASURED` 원칙을 따른다.

## 정직성 경계
- 이 산출물은 **실행 가능한 구현 계약**이지 게임이 존재한다는 주장이 아니다. 빌드 0건, 플레이 표본 0건.
- G2/G4/G5/G6/G7은 여전히 `NOT-MEASURED`이며 이 문서는 어떤 게이트도 올리지 않는다.
- Unity 6000.5.6f1은 **설치가 확인된 후보**일 뿐 LTS라고 주장하지 않는다.
- 패키지 버전은 하나도 적지 않았다(`[PIN-AFTER-RESOLVE]`). 실제 resolve 전에 버전을 쓰는 것은 계약 위반이다.
- 1280x720 / 1920x1080 / 3440x1440 / 1280x800은 **제안 해상도군**이며 어느 것도 실기 검증되지 않았다.
- macOS는 검증 전용이고 성능 목표 대상이 아니다. 기준 하드웨어 자체가 미정이다.
- 유료 에셋·외부 API·네트워크 의존성 0건. git commit/push 미실행.

## 다음 소유자
`route_out.next_owner` = Unity UI 구현 + `game-qa` 독립 검토. 다음 단계는 수직 슬라이스 T0에서 `verification.matrix`를 실제로 실행해 빌드 해시·장치·입력·로케일·설정·이벤트 로그를 증거로 채우는 것이다.
