# 조수기록국: 마지막 당직 (가제)

> **Working title — unapproved.** 국문 가제와 영문 코드네임은 상표·동명 게임 확인 전이며 폴더명·번들명·상점명에 쓰지 않습니다. 저장소·Unity 프로젝트 코드네임은 `Unknown`입니다.
>
> 이 저장소는 Steam 프리미엄 Unity 신작의 **사전제작(preproduction)** 저장소입니다. 컨셉·프리비주얼라이제이션 이미지와 개발 빌드의 게임 내 UI 캡처를 구분해 표기합니다. 사람 플레이·성능·판매 실측은 아직 **n = 0** 입니다.

![README hero — 은포항 야경 컨셉 (프리비즈, 게임플레이 아님)](docs/media/readme-hero.jpg)

## 한 줄 소개

폐국을 3주 앞둔 조수기록국의 마지막 야간 당직. 기록 복원사 **한서린**은 끊긴 염선 배선과 배수 경로를 **손으로 직접 바꾸고**, 그 결과로 달라진 항구를 다시 조사해 12년 전 **대조의 밤**에 사라진 **결손 4시간**의 진실을 청문 문서 한 건으로 확정합니다. 밤은 21:00에 시작해 05:00에 끝나고, 세 갈래 결말은 전부 본편 안에서 닫힙니다.

**장르** 작업대·공간 추리 어드벤처 (2.5D 고정 시점, 싱글플레이) · **엔진** Unity 6000.5.6f1 · **플랫폼 목표** PC (Steam) · **언어** 한국어 / 영어 · **전투·유료 재화·멀티플레이 없음**

## 개발 빌드 게임플레이 UI

![T0 개발 빌드의 판독기 화면 — 시간창을 조정하고 근거를 가설판에 인용하는 게임 내 UI 캡처](docs/media/t0-gameplay-capture-20260910.jpg)

**작업대·공간 추리 어드벤처**입니다. 허브의 사물 서랍·판독기·회로 조작을 오가며 자료를 선택하고, 시간창을 단계별로 조정해 근거를 가설에 인용합니다. 위 이미지는 Unity `Unknown T0` 개발 빌드에서 캡처한 게임 내 UI이며, 완성판 게임플레이나 사람 플레이 검증을 뜻하지 않습니다.

## 세 기둥

1. **행동으로 추리한다.** 단서를 모으는 데서 끝나지 않고 배선·경로·기록의 가설을 손으로 시험합니다.
2. **같은 장소가 다르게 읽힌다.** 새 지역을 늘리는 대신 허브 1곳 + 구역 4곳을 상태 변화로 재사용합니다.
3. **본편만으로 닫힌다.** 도시의 위기, 사건의 책임, 주인공의 선택이 본편에서 끝납니다. DLC는 다른 사건입니다.

## 플레이 컷씬 (프리비즈)

| 도구 조작 → 공간 변화 → 새 질문 (Higgsfield image-to-video, 10초) | 허브 당직실 그레이박스 턴테이블 (Blender) |
|---|---|
| ![프리비즈 컷씬 — 배선 연결 → 수위 하강 → 밸브 시험과 되돌림. 실제 게임플레이 아님](docs/media/previz-cutscene-video.gif) | ![허브 그레이박스 턴테이블 — 최종 아트 아님](docs/media/previz-hub-turntable.gif) |

컨셉 프레임 9장으로 만든 스토리보드 GIF는 [docs/media/previz-cutscene-concept.gif](docs/media/previz-cutscene-concept.gif) 에 있습니다. 두 GIF 모두 `docs/media/provenance.json`에 출처와 "NOT gameplay" 표기가 있습니다.

## 여섯 개의 동사 — 세계의 여섯 법칙과 1:1

| 동사 | 대응 법 | 연습(sandbox)에서 | 확정(commit)에서 |
|---|---|---|---|
| ![배선 추적](docs/media/verb-circuit.jpg) **배선 추적** `circuit` | 배선된 것만 남는다 | 계통선을 따라 센서 범위 안/밖을 비교 | 확정 없음 — 배선 밖 근거를 무효로 만든다 |
| ![판독](docs/media/verb-reader.jpg) **판독** `reader` | 원본은 닳지만 사본은 남는다 | 검증 사본을 무제한 재생·확대 | 판독 결과를 가설판에 출처와 함께 고정 |
| ![조위정합](docs/media/verb-alignment.jpg) **조위정합** `alignment` | 정합 전 시계는 믿지 않는다 | 공통 피크 3개를 잡으며 잔차를 실시간 확인 | 잔차 ≤ 4분일 때 두 자료의 시간축을 잇는다 |
| ![배수 편성](docs/media/verb-routing.jpg) **배수 편성** `routing` | 이번 조수에는 보호 용량이 부족하다 | 두 경로를 대기 상태로 편성해 결과를 미리 본다 | 우선순위를 확정해 구역 상태를 실제로 바꾼다 |
| ![부식 시험](docs/media/verb-corrosion.jpg) **부식 시험** `corrosion` | 소금은 비용으로 보인다 | 구성안을 가상 시험대에 무제한 올린다 | 한도 안의 구성만 배수 편성 확정으로 넘긴다 |
| ![이중서명](docs/media/verb-seal.jpg) **이중서명** `seal` | 원본 책임과 제출을 나눈다 | 결론 카드에 근거 슬롯을 채워 본다 | 서로 다른 매체 2종 + 배선 범위 안 + 시간 근거로 제출 |

모든 조작은 **연습(무제한·무료·세계 불변)** 과 **확정(프리뷰 → 저장 성공 후에만 세계 변경)** 두 층으로만 존재합니다. 힌트 3단계는 무료·무제한이고 엔딩·평가에 영향이 없습니다. 실시간 타이머는 없습니다.

## 세계

은포항(銀浦)은 대조차 9.2 m의 반폐쇄 만입니다. 방조제 → 갑문·수문 → 양수장의 3중 방어를 **염선**(브라인 도관)의 압력·염도가 회로처럼 잇고, 그 신호는 **염판**에 12시간 링으로 각인됩니다. 염판은 밸브 개폐·압력·염도·수위·문 개폐·호출만 남기고 얼굴·의도·대화 내용·사람의 위치는 남기지 않습니다. 과거는 재생되지 않고 **서로 다른 매체 2종의 대조로만 추론**됩니다.

![허브 당직실 무드 컨셉](docs/media/space-hub.jpg)

세계관 바이블·연표·용어집: [`_workspace/current/worldview/`](_workspace/current/worldview/) · 캠페인 33비트(정본 JSON): `_workspace/current/planning/campaign.json`

## 사전제작 상태 (2026-09-10, R7 종료 당시)

다음 표는 R7 당시 기록입니다. 이후 T0 M2 실행·빌드와 자동 테스트 결과는 아래 「T0 M2 구현과 검증」에 별도로 기록합니다.

| 항목 | 상태 |
|---|---|
| 사전제작 사이클 | C1 시장·범위, C2 인과·세계관, C3 캠페인·시간, C4 상호작용·Unity, C5 상품·생산 — **각 회차 독립 QA 검토 + 수정 완료** (`_workspace/current/qa/c{1..5}-review.md`), C6 통합 초안(5렌즈 판정단), C7 Codex 핸드오프(반박 3렌즈) — `qa/c6-review.md` |
| 결함 현황 | 열린 **S1 0**. 정본은 `_workspace/current/qa/defect-register.md`, 회차별 집계는 `production/cycle-ledger.json` |
| 설계 분량 | 9장 33비트, 설계 예산 480분 **[TARGET]**, 검증기 `planning/validate-campaign.mjs` 49/49 PASS — 관측 완주 시간은 **미측정(null)** |
| 통합 초안 | `_workspace/current/planning/game-draft-v1.md` (12절 + English summary) |
| Codex 핸드오프 | `_workspace/current/handoff/` — 브리프·검증 계획·리소스 런북·RFC 인박스; T0 인스턴스 데이터 `_workspace/current/systems/data/t0/` |
| 2D 컨셉 리소스 | 45장 (인물·공간·도구·UI·키아트·캡슐·README·프리비즈), GTI, 전부 `runtimeEligible:false` |
| 3D | 허브 당직실 그레이박스 + 도구 6종 블록아웃 (GLB 7 / FBX 1, 144 tris), Blender 5.1.2 |
| 영상 | Higgsfield image-to-video 프리비즈 2클립 (5초·720p) |
| Unity 프로젝트 | `unity/Unknown/` — Unity 6000.5.6f1, T0·C1 개발용 씬·코드·리소스와 저장/복구 포함. 구간별 구현·검증 범위는 아래 보고서 참조 |
| 실제 빌드 · 플레이테스트 · 성능 | macOS 개발 빌드와 에이전트 조작 스모크 있음. 사람 플레이테스트·성능 실측은 **n = 0**. 본 생산 진입은 별도 Base production gate 적용 |

정직성 규칙: 표를 더해 480분이 나왔다는 사실은 8시간을 플레이했다는 증거가 아닙니다. 게이트 측정치는 [`_workspace/current/qa/gate-measurements.md`](_workspace/current/qa/gate-measurements.md) 에만 있고, 실측이 없는 게이트는 `NOT-MEASURED`로 남습니다.

## 저장소 구조

`unity/Unknown/`에는 실행에 필요한 씬·코드·입력·생성 테이블·T0 리소스와 패키지 설정을 포함합니다. Unity 6000.5.6f1에서 프로젝트를 직접 열거나 아래 `BuildMac` 명령으로 빌드할 수 있습니다. 개발용 `Prepare`는 포함된 테이블을 읽고 부모 작업 트리의 `campaign.json` SHA를 검증합니다. 저작 원본부터 테이블을 다시 생성하는 작업은 별도의 `emit-tables.mjs`와 해당 저작 입력 파일이 모두 필요합니다.

```
CLAUDE.md                      저장소 운영 규칙 (13역할 + PM 하네스, 게이트 G1~G8, 사이클 계약)
_workspace/current/            살아 있는 사전제작 산출물 (레인별 폴더, frontmatter 필수)
_workspace/archive/            대체된 이전 판본 (읽기 전용, supersedes 로 연결)
_workspace/current/handoff/    Codex(GPT-6 Astra) Unity 구현 핸드오프 브리프 · 검증 계획 · 리소스 런북
unity/Unknown/                 Unity 6000.5.6f1 프로젝트 (in-repo)
assets/generated/{2d,3d,video,previz}/   생성 리소스 + provenance.json (승격 전 runtimeEligible:false)
docs/media/                    README 용 이미지·GIF (파생본) + provenance.json
scripts/                       gen-2d.sh (GTI) · gen-video-higgsfield.sh · make-previz-gif.sh · refresh-2d-provenance.py
```

## Unity에서 실행·빌드

1. Unity Hub에서 `unity/Unknown/`을 추가하고 **Unity 6000.5.6f1**로 엽니다.
2. `Assets/_Project/Scenes/boot.unity`를 열고 Play를 누릅니다. 포함된 씬과 데이터로 허브·회로·판독·설정·저장 흐름을 실행합니다.
3. macOS 플레이어는 저장소 루트에서 다음 명령으로 빌드합니다.

```bash
UNITY_EDITOR="/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity"
"$UNITY_EDITOR" -batchmode -nographics -quit \
  -projectPath "$PWD/unity/Unknown" \
  -executeMethod Tide.EditorTools.T0ProjectBuilder.BuildMac \
  -logFile /tmp/unknown-t0-build.log
```

산출물은 `unity/Unknown/Builds/T0-mac/Unknown.app`입니다. `BuildMac`은 포함된 씬·테이블을 사용하므로 먼저 `Prepare`를 실행할 필요가 없습니다. 자동 테스트는 Unity Test Runner의 EditMode·PlayMode에서 실행할 수 있습니다.

## T0 M2 구현과 검증

**[OBSERVED · 2026-09-10]** T0의 허브, 회로 오버레이·근거, 판독 구간·인용, 키보드/게임패드 입력, 설정과 저장·복구를 연결했습니다. 저장 성공 영수증 이후 확정 결과를 반영하며, undo/redo와 읽기 전용 복구 흐름을 포함합니다.

| 검증 | 결과 |
|---|---|
| Unity NUnit EditMode | **18/18 통과**, 실패·건너뜀 0 |
| Unity NUnit PlayMode | **15/15 통과**, 실패·건너뜀 0 |
| 최종 macOS 플레이어 빌드 | `BuildMacFramingFix` 성공 · `Builds/T0-mac-framing/Unknown.app` · 314,105,987 bytes |
| 네이티브 macOS 스모크 | 최초 빌드: 포인터로 T0 완료·앱 종료/재실행 복구 확인. 수정 빌드: 150% 설정·복구 상태·서랍 가림·판독 그래프 스크롤 회귀 통과 |
| 사람 플레이 시간·물리 게임패드·기준 기기 성능 | 별도 검증 대기 |

M1의 내부 계약 검사 21개는 EditMode wrapper 1건에 포함되며 테스트 수에 중복 합산하지 않습니다. [자동 검증 명령·원본 결과](_workspace/current/systems/tech-verification/t0-m2-native.md)와 [네이티브 플레이어 스모크](_workspace/current/systems/tech-verification/t0-m2-player-smoke.md)를 구분합니다. 실제 포인터로 처음부터 T0 완료까지 진행하고 프로세스 종료·재실행을 확인한 뒤, 수정 빌드에서는 복구된 상태와 두 시각 결함을 집중 재검증했습니다. 이 결과는 수정 빌드 전체 재플레이나 물리 게임패드 검증을 뜻하지 않습니다.

Blender r03 서랍은 실제 Unity 임포트·재질·배치 검토를 거쳐 **T0 씬 한정**으로 연결했습니다. Higgsfield 도장 소리는 생성 후보이며 청취 검수 전이라 런타임 재생을 비활성 상태로 유지합니다. MuAPI의 공식 인터페이스는 확인했으나 인증된 로컬 연결이 없어 이번 리소스 생성에는 사용하지 못했습니다.

T0 M2 당시 구현 범위입니다. **25분은 설계 목표**이며 측정된 플레이 시간이 아닙니다. 전체 9장 캠페인, G4/G5/G6, 상품 출시 준비가 완료됐다는 뜻도 아닙니다.

## C1 M3 구현과 검증

**[OBSERVED · 2026-09-11]** C1 「두 개의 필적」의 첫 구간 「순찰로의 두 분기」(c1-b1)를 구현했습니다. 완료한 T0 저장에서 이어서 진입하며, 당직일지와 수문 계통판의 두 단서를 관찰하고 조명·판독 분기를 조정합니다. 무료 우회는 미리보기만 바꾸고, 출처 귀속 조건과 수문 접근은 명시적 확정과 저장 성공 후 함께 반영됩니다.

Blender 5.1.2에서 원본 계통판을 제작했습니다: 정적 메시 6개, 삼각형 2,092개, 1024² 맵 4장. 아래 이미지는 **Blender 미리보기**입니다. 이 정적 패널의 Unity 사용을 승인했으며, 실행 증거는 [M3 검증 보고서](_workspace/current/production/codex-c1-m3-status.md)에 기록합니다.


검증: **EditMode 27/27 · PlayMode 28/28 PASS**. macOS 빌드와 실제 v1 저장 불러오기 → C1 확정 → 앱 종료·재실행 복원을 확인했습니다. 저장 v2로 이전하면서 기존 36개 명령과 원본 v1 백업을 보존합니다. 150% 글자 크기에서 스크롤과 완료 화면도 확인했습니다.

![C1 수문 계통판 Blender 미리보기](docs/media/c1-patrol-panel-blender-r01.png)

범위는 c1-b1까지입니다. C1 전체 네 구간, 설계상 50분 플레이타임, 재미·성능·G4/G5 완료를 입증하지 않습니다. 리소스 제작 이력은 [작업자 검토](_workspace/current/production/c1-panel-operator-review.md)와 [provenance](assets/generated/3d/c1-patrol-panel-r01/provenance.json)에 남깁니다.

## C1 M4 — 겹쳐 붙은 서명지

C1 두 번째 구간 `c1-b2`를 구현했습니다. 습도 시험과 원상 복구, 두 장 분리·개별 사본 보존, 하단 미해결 영역 표시, 판 #0과의 명시적 비교를 연결했습니다. 확정 저장이 성공한 뒤에만 완료·체크포인트·사본·근거 관계를 함께 반영합니다. 이전 v1/v2 저장 호환과 읽기 전용 화면 규칙도 검증 범위에 포함합니다.

Blender **r02 판독기·트레이**는 3,920 triangles / 4 meshes / 4 materials이며, Higgsfield의 **1024² 빈 종이 질감**을 사용합니다. 종이의 접착 소금과 아래쪽 가림은 별도 UI 상태로 구성해 단서를 이미지에 굽지 않습니다. 아래 그림은 **Unity 네이티브 리소스 검수 캡처**로, 플레이 화면이나 성능 측정이 아닙니다.

![C1 판독기와 습도 트레이 — Unity 네이티브 리소스 검수](docs/media/c1-signature-reader-native-r02.png)

포함된 리소스로 M4 macOS 빌드를 재현하려면 다음 명령을 사용합니다.

```bash
"$UNITY_EDITOR" -batchmode -nographics -quit \
  -projectPath "$PWD/unity/Unknown" \
  -executeMethod Tide.EditorTools.C1SignatureProjectBuilder.BuildMac \
  -logFile /tmp/unknown-c1-m4-build.log
open -a "$PWD/unity/Unknown/Builds/C1-M4-mac/Unknown.app"
```

**[OBSERVED · 2026-09-11]** 분리한 배포 소스에서 **EditMode 36/36 · PlayMode 39/39 · 직렬화 부팅 1/1 PASS**. 실제 v2 저장의 기존 42개 명령과 백업을 보존해 v3로 이전했고, 완료 후 최종 macOS 앱을 재시작해 두 사본·가림·완료 상태를 확인했습니다. 최종 빌드는 348,163,868 bytes입니다. 사람 플레이·성능 검증은 별도입니다.

구현·검증 영수증은 [M4 보고서](_workspace/current/production/codex-c1-m4-status.md), 리소스 생성·수정·승격은 [작업자 검토](_workspace/current/production/c1-signature-operator-review.md)에 기록합니다. 범위는 `c1-b2`까지이며, C1 전체·설계 플레이시간·사람 플레이테스트·성능·G4/G5 완료를 뜻하지 않습니다. MuAPI 생성과 신규 음향의 청취 검수는 완료되지 않았습니다.

## M5 — 영상에서 게임 연출로

[인트로·플레이 영상과 실제 게임 녹화 보기](docs/media/intro-gameplay-m5/index.html)

Higgsfield로 인트로와 C1 플레이 연출 영상을 각각 제작하고, 측정한 컷과 시선 흐름을 Unity에 적용했습니다. 신규 게임은 건너뛰기·설정이 가능한 6초 인트로로 시작하며, 기존 세이브는 바로 이어집니다. C1의 관찰·시험·기록 안내는 선택한 도구와 실제 기록 상태를 따릅니다.

영상의 임의 손잡이 동작·카메라 이동·가림 영역 삭제는 채택하지 않았습니다. GTI로 제작한 당직실 배경과 완성된 플레이 영상의 프레임에서 파생한 UI 표면을 사용합니다. 생성 영상은 프리비즈이며 게임 실행에는 필요하지 않습니다.

최종 통합 검증: **EditMode36/36 · PlayMode54/54 · 부팅1/1**. 실제 인트로·플레이 녹화와 완료 세이브 재시작을 확인했습니다.

검증과 출처: [M5 실행·영상 기록](_workspace/current/production/intro-gameplay-m5-status.md), [연출 검토](_workspace/current/presentation/intro-gameplay-m5-video-review.md).

## 개발용 준비와 정본 재생성

기획·구현 작업은 `CLAUDE.md` → `.mex/ROUTER.md` → `_workspace/current/handoff/README.md` 순서로 계약을 확인합니다. `Prepare`는 프로젝트에 포함된 테이블을 읽고 부모 작업 트리의 `_workspace/current/planning/campaign.json` SHA를 검증한 뒤 씬을 준비합니다. 저작 문서에서 테이블을 생성하는 명령이 아니며, 이 경로에는 부모 캠페인 파일이 필요합니다.

```bash
node _workspace/current/planning/validate-campaign.mjs
node _workspace/current/planning/validate-campaign.mjs --t0 _workspace/current/systems/data/t0
"$UNITY_EDITOR" -batchmode -nographics -quit \
  -projectPath "$PWD/unity/Unknown" \
  -executeMethod Tide.EditorTools.T0ProjectBuilder.Prepare \
  -logFile /tmp/unknown-t0-prepare.log
```

정본부터 테이블을 재생성하는 도구는 `_workspace/current/systems/pipeline/emit-tables.mjs`이며, 캠페인·출처·회로 등 필요한 저작 입력 파일을 모두 준비해야 합니다. 이 전체 재생성, `Prepare`, 포함된 프로젝트의 직접 실행·`BuildMac`은 서로 다른 경로입니다. 새 리소스의 런타임 승격에는 출처 기록과 해당 범위의 감사가 필요합니다.

## 리소스 출처와 라이선스

신규 리소스 생성에는 **MuAPI·Higgsfield·Blender**를 사용합니다(2026-09-10 사용자 지정). 아래 목록은 기존 산출물의 실제 생성 출처입니다. 새 결과도 제공자·모델·입력·해시를 기록하고 별도 감사 후에만 런타임으로 승격합니다.

- 2D: `god-tibo-imagen`(GTI, Codex 백엔드, 모델 `gpt-6-astra`) — 프롬프트는 `_workspace/current/concept/prompts/`, 출처는 각 폴더의 `provenance.json`.
- 3D: Blender 5.1.2 — 기존 MCP 그레이박스와 신규 CLI 서랍의 출처를 구분합니다. 스크립트 `assets/generated/3d/scripts/`.
- 영상: Higgsfield `seedance_2_0_mini` — `assets/generated/video/provenance.json`.
- 생성물의 상업 이용 가능 여부는 **각 백엔드 약관 확인 전까지 UNVERIFIED** 입니다. 모든 항목이 `runtimeEligible:false`로 시작합니다.
- 실존 재난·피해자·타 작품 설정을 차용하지 않았습니다. 수문·염선 기술은 창작이며 현실 안전 매뉴얼이 아닙니다.

---

*Generated content is pre-visualization, not gameplay. Playtest n = 0. See `CLAUDE.md` for the studio contract and `_workspace/current/production/premium-preproduction-contract.md` for what this repository does and does not promise.*

### 이번 T0 리소스 제작

![Blender로 제작한 T0 서랍 재질 검토 프리뷰](docs/media/t0-drawer-blender-r03.png)

Blender CLI로 서랍을 제작하고 프리뷰·Unity 임포트 검토를 거쳐 색공간·부식·염분 표현과 씬 배치를 확인했습니다. r03은 메시 2개·삼각형 156개·1024px 텍스처 4개이며 T0 씬 한정으로 연결했습니다. 원본과 이전 버전을 보존하고, [원본 provenance의 승인 범위](assets/generated/3d/hub-view-drawer-r03/provenance.json)를 구분합니다. 위 이미지는 독립 에셋 렌더로 게임플레이 화면이 아닙니다.

Higgsfield Seed Audio로 원본 도장 효과음도 생성했습니다. 24kHz 스테레오 WAV, 3.5초이며 클리핑은 없습니다. 청취 검수 대기 후보로 보존하며 런타임 재생은 비활성 상태입니다. MuAPI는 공식 인터페이스를 확인했으며 현재 인증 연결은 미완입니다.
