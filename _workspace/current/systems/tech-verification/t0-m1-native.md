---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# T0 M1 — Unity 내부 규칙 실행 영수증

[OBSERVED] M1은 실제 Unity 6000.5.6f1에서 생성 테이블을 읽고 순수 C# 규칙을 실행하는 첫 구현이다. 아래 native editor 계약검사 21건이 성공했다. 이는 NUnit 실행, 플레이어 빌드, 플레이테스트 또는 전체 T0 DoD 통과가 아니다.

## 구현 및 관측

- `unity/Unknown/Assets/_Project/Sim/`: 불변 상태·DTO, 결정적 `Commit`/`Reduce`/`Preview`, 명령 재생, T0 완료 술어, 회로 표시와 근거 부착, reader 최초 사본/무료 재판독/원본 카운터, 출처 및 독립쌍 검사. 인용은 확정 당시 창의 시작·끝을 이벤트에 고정한다.
- `Data/`: 생성 영수증의 PASS, 전체 테이블 바이트 SHA, 생산자 영수증 계약 대조(`emittedUtc`만 제외). 저작 검증기는 재구현하지 않았다. ScriptableObject 진단 카탈로그 및 Zone/Record/Tool 저작 표현을 생성 데이터에서 임포트했다.
- `App/T0Bootstrap.cs`: 현재 출처 blocker가 있으면 전체 T0 시작을 거부한다. 진단용 규칙 실행은 Editor 검사에서만 한다. 미완료를 완료 UI로 표시하지 않는다.
- 생산 어셈블리 7개 + EditorTools 1개 + 테스트 어댑터 2개의 경계를 작성했다. Save/Input/Presentation/UI는 아직 코드가 없는 경계이므로 Unity의 empty assembly 경고가 남는다. 해당 시스템의 구현을 주장하지 않는다.
- generated `runtimeEligible` 값은 변경하지 않았다. 튜닝은 생성 JSON에서 읽는다. synthetic provenance는 양성 테스트에만 존재한다.

## 실행 명령과 결과

1. [OBSERVED] `rtk proxy node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out unity/Unknown/Assets/_Project/Data/Tables`
   - exit 0. 검사/해시 정본은 생성 `Data/Tables/tables-receipt.json`. 생성 원본은 편집하지 않았다.
2. [OBSERVED] `rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -executeMethod PackageBootstrap.Run -logFile /tmp/unknown-t0-package-bootstrap.log`
   - 필수 5종 resolve 실패. 원문: `t0-m1/logs/package-bootstrap.log`. 오류는 `ENOSPC: no space left on device`. 당시 available 219 MiB, 이번 호출의 `.tmp-*`는 0개로 자동 정리돼 있었다. 타 세션 파일이나 기존 캐시를 삭제하지 않았으며 공간 부족 상태에서 재시도하지 않았다. 5종 버전을 발명하거나 manifest에 가짜 pin을 넣지 않았다.
3. [OBSERVED] `rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -executeMethod Tide.EditorTools.T0Verification.RunBatch -logFile /tmp/unknown-t0-m1-native-checks-3.log`
   - **exit 0 · `T0_M1_CHECKS tests=21 failures=0`**.
   - 원문: `t0-m1/logs/native-checks-3.log`, 공개 단언 결과: `t0-m1/results/t0-m1-contract-checks.xml`.
   - **Unity Editor 내부 자체 계약검사**다. XML 형식은 JUnit이며 Unity Test Framework 결과로 부르지 않는다. NUnit 어댑터는 실제 test-framework 패키지가 resolve될 때만 컴파일된다.
   - 앞선 native 1회차는 NUnit 부재로 컴파일 실패, 2회차는 importer의 저장소 상대경로 오류로 실패했다. 둘을 수정한 뒤 3회차가 통과했다. 실패 원문도 보존한다.
4. [OBSERVED] `rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/Resources/Scripting/netcorerun/netcorerun /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/Resources/Scripting/DotNetSdk/sdk/8.0.318/Roslyn/bincore/csc.dll @Library/Bee/artifacts/200b0aEDbg.dag/Tide.Sim.rsp -out:/tmp/unknown-t0-negative.dll -refout:/tmp/unknown-t0-negative-ref.dll /tmp/unknown-t0-forbidden-engine.cs`
   - cwd `unity/Unknown`. Unity가 실제 생성한 Sim 컴파일 응답 파일에 `UnityEngine.Vector3` fixture를 추가했다. **예상대로 exit 1 / CS0246 (`UnityEngine` namespace 없음)**. 원문 `t0-m1/logs/engine-boundary-negative.log`, fixture는 `/tmp`에만 생성. 정상 Sim 산출물을 덮어쓰지 않았다.

## 검사 범위

[OBSERVED] 상태 입력 불변, 동일 명령열 해시, Preview 무부작용, 실패 이벤트 0개, 실제 b1/b2 **완료 술어** 충족, 선행조건 차단, 표시 해제 시 완료 철회, 테스트 영역 seeded 무작위 10k 명령의 근거 불감소·거부 상태 불변, 사본 보호, 데이터 기반 원본 상한 및 sandbox 비소모, 저작 샘플 창 검증, 합성 독립쌍/복사 계보/순환·고아 거부, 실제 누락 출처 인용 거부 및 b3 미완료, 확정 시점 창 고정 회귀, 생산자 실패·테이블 5종 변조·계약 변조 거부, timestamp 예외, 재생 실패 중단, 문화권 무관 해시를 검사했다.

[OBSERVED] QA가 발견한 두 항목을 반영했다. 인용 뒤 preview 창만 고쳐서 완료되는 결함은 citation snapshot으로 수정했다. 처음의 10k 교대 stress를 seeded 무작위 합법/거부 입력과 매 스텝 단언으로 강화했다. 외부 QA가 최종 원문/XML을 별도 확인했다.

## 미완료 및 차단 경계

- `RFC-CX-001`: 실제 인용 레코드의 `systemId`/`stationId`가 없으므로 두 인용 및 실제 b3 완료, 전체 T0 시작이 차단된다. 정의되지 않은 새 `ReasonCode`를 만들지 않고 개발용 data diagnostic을 반환한다. 해당 진단은 플레이어 문자열이 아니다.
- 회로 Overlay/3점 AnchorOverlay 상태기계·좌표 판정, 전체 도구 UI, 입력/rebind, 3개 씬 가산 로드, 저장/undo/브랜치/원자 rename/SavePending, telemetry/localization 통합은 **미구현**이다. b2 검사는 표시/근거 **완료 술어** 테스트이며 전체 circuit 조작 경로 완주가 아니다.
- `T-07` 상태공간 softlock-free, `T-08`~저장 인수, `T-24/25` 입력 완주, 전체 importer R1~R4, URP 설정 및 필수5종 pin, Player build는 **미검증/미완료**다. 필수 패키지 설치 실패를 테스트 통과로 상쇄하지 않는다.
- 최신 RFC-S6의 KO 필수·EN 선택 판정이 정본이다. 브리프 뒤쪽의 EN 없으면 실패라는 잔존 문구를 새 규칙으로 채택하지 않았다. M1은 전체 localization 검증을 수행하지 않았다.
- 사람 n=0. 성능/재미/몰입/G4~G7 판정은 하지 않는다.

## 검색·그래프·메모리

- [OBSERVED] **통합 상태 PARTIAL**: graphify에는 새 Sim 코드가 포함됐다. Unity 생성 캐시를 제외한 재갱신과 mex 메모리 영수증은 미완료다. `resolve_mex_agent` identity probe는 `mex-agent not found or failed identity probe`를 반환했다. PATH의 TeX `mex`는 실행하지 않았다. `mex graph scope`/`graph`/`check`/`log`는 도구 부재로 미수행이며 **[UNGRAPHED]는 mex 메모리 영수증에만 해당**한다.
- [OBSERVED] 기존 인덱스에 `zg query` (T0 simulation/data intent), `zg query --rg` (`stationId` 등 정확 필드), `graphify query` (T0 data/receipt/citation 흐름)를 실행했다.
- `zg` 생성/rebuild/drop은 실행하지 않았다. 새 명시 승인 없이 전체 인덱스를 재생성하지 않는 최신 AGENTS 규칙을 따랐다.
- [OBSERVED] root director의 `graphify update .`는 exit 0. 보존 로그 `t0-m1/logs/graphify-update.log`와 메타 `graphify-update.meta.md` 참조. 현재 `graphify-out/graph.json`은 **13,592 nodes / 17,350 edges**이며 Sim 소스 **41 nodes**를 포함한다. `git show HEAD:graphify-out/graph.json`의 커밋 기준은 744 nodes / 738 edges / Sim 0 nodes였다. 현재 Sim 노드의 source_file은 `PuzzleState.cs`, `T0Definition.cs`, `ReasonCode.cs`, `T0Simulation.cs` 4파일임을 JSON에서 직접 확인했다.
- [OBSERVED] 첫 성공 그래프에는 `unity/Unknown/Library/` 노드 **7,578개**도 포함됐다. root가 `.graphifyignore`를 작성하고 ignore 판정이 cache=True / Sim=False임을 확인했다. 이후 `graphify update . --force`는 **ENOSPC exit 1**로 실패했다. 실패 로그 `t0-m1/logs/graphify-source-only.log`와 메타 `graphify-source-only.meta.md` 참조. **캐시가 제외된 그래프가 완성됐다고 주장하지 않는다.**
- [OBSERVED] 실패 후 현재 graph.json과 root의 `/tmp/unknown-t0-m1-graph-before-ignore.json.gz` 압축 해제 바이트는 동일하다(SHA-256 `70b6c08f0680cd8aee538dab0925736e399fa67081d5d3cf9c67a1a45a4e965a`). 즉 소스 포함 성공 그래프가 보존돼 있다. 후속 확인은 읽기/로그 복사만 했으며 재빌드나 추가 정리를 실행하지 않았다. graph/mex의 PARTIAL 상태를 G8 전체 PASS로 올리지 않는다.

## 컨벤션 확인

1. [OBSERVED] 새 기술 영수증/RFC에 current frontmatter 있음. 기존 문서를 대체/삭제하지 않음.
2. freshness-check는 root의 통합 작업에서 실행한다.
3. 위 관측 수치는 원문 로그/XML/생성 영수증을 인용한다.
4. 새 출처 결정은 ACK 대기 상태이며 그 부분만 차단했다.
5. 그래프/메모리 범위와 위임·도구 부재를 위 절에 기록했다.
6. mex check는 미수행이며 도구 부재를 숨기지 않는다.
