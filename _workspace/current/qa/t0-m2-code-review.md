---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# T0 M2 저장·분기·인용 코드 독립 QA

**[OBSERVED · 최신 후속 판정, 2026-09-10 08:34:08 UTC]** PlayMode3의 8/8 Passed·runner exit 0 및 실제 단언을 대조해 C7-F52·F53을 닫았다. 최신 생성/런타임 테이블 영수증과 인용 쌍 완료·재로드 증거를 연결해 C7-F50도 닫았다(§7). 마지막 reader/circuit 버튼 routing 정본 교정과 추가 회귀가 진행 중이므로 이 판정은 최종 빌드 또는 릴리스 PASS가 아니다.

**[OBSERVED · 후속 판정, 2026-09-10 08:26:54 UTC]** EditMode6의 XML·runner 로그·테스트 단언을 독립 대조했다. 18 NUnit testcases가 모두 통과했고 C7-F51·F54를 닫았다. C7-F52·F53은 UI 복구 실행 증거 대기로 open이다. 시스템이 발견한 타임스탬프 체크섬 결함 C7-F55를 실패/수정 후 영수증과 함께 closed로 등록했다. 상세 근거와 테스트 범위는 §6에 있다. 아래 §1–5는 최초 소스 검토 시점의 기록이다.

**[OBSERVED · 소스 검토] S2 수정 필요 사항 4건을 발견해 systems와 director에 전달했다.** 독립 리뷰 에이전트는 코드를 수정하거나 Unity를 실행하지 않았다. 아래 재현은 읽은 코드의 결정적 제어 흐름에서 도출한 회귀 시나리오이며, 실행된 Unity 테스트 결과가 아니다. systems가 동시에 수정을 진행하므로 행 번호는 아래 검토 스냅샷 기준이다. 새 실행 회귀 증거 전에는 결함을 닫지 않는다.

## 1. 검토 입력과 방법

`rtk run 'rg --files unity/Unknown/Assets | rg "(App|Save|Sim|EditMode|PlayMode|Tests).*\\.(cs|asmdef)$"'`로 파일 목록을 먼저 확인한 뒤 `mcp__node_repl__js`의 읽기 전용 filesystem API로 메서드와 해당 행을 읽었다. 대상은 `App/T0GameSession.cs`, `Save/{AtomicSaveStore,SaveCodec,JournalSave}.cs`, `Sim/{CommandJournal,PuzzleState,T0Definition,T0Simulation}.cs`, 출처 필드 전달을 위한 `Data/T0DataLoader.cs`, `Tests/EditMode/{T0ContractTests,T0M2Tests}.cs`, `Tests/PlayMode/T0PlayModeTests.cs`다. 경로 접두사는 `unity/Unknown/Assets/_Project/`다.

계약은 `systems/system-specs/save-undo.md`의 로드·복구/분기/실패 모드와 `systems/data-schemas/plates.md`의 인용 출처 필드다. 이전에 ACK한 에셋·저작 패킷의 재감사는 하지 않았다.

```yaml
measured:
  recorded_at: 2026-09-10T08:17:33Z
  reviewer_session: /root/t0_resource_qa
  method: read-only source/control-flow review and test-source comparison
  executed_unity_tests_by_this_reviewer: 0
  player_sessions_by_this_reviewer: 0
  new_defects: 4
  severity: S2
  gate_verdict: NOT-MEASURED
```

| 검토 스냅샷 | SHA-256 |
|---|---|
| `Sim/CommandJournal.cs` | `a91121152f078661519fcfd9ac6f08dbd05d69857aac49bdd6e5ed6284e2df46` |
| `Save/AtomicSaveStore.cs` | `b51186c9fc610787f9960675c6ca21938adcd1f3edf995936bf6410a2b09f6aa` |
| `App/T0GameSession.cs` | `a2eb36150a7157dd77aed0dca3579ae5d4130c5db5a713d6f0e609cef37efdfe` |
| `Tests/EditMode/T0M2Tests.cs` | `7be49f2459c4a6a685b14e3767d2111dd932170cb1f393f6c842f363dd246c24` |
| `Tests/PlayMode/T0PlayModeTests.cs` | `248907d02715770908d24237591f71299d865c5b45443a713c3725a2cf50a902` |

## 2. 발견 사항과 필수 회귀

### C7-F51 · 분기 공통 조상까지 Undo하면 Redo가 막힘

**S2 · open.** `Sim/CommandJournal.cs:27–28`은 새 분기 명령에 새 `BranchId`를 붙인다. `:37`의 Redo는 `e.ParentSeq == HeadSeq && e.BranchId == BranchId`인 자식만 선택한다. 선택된 분기의 공통 조상 명령은 이전 branch ID를 유지하므로 이 조건을 통과하지 못한다.

재현: 유효한 명령 A/B/C를 seq 1/2/3(main)에 제출 → Undo하여 head 2 → D를 제출해 seq 4(branch-4, parent 2) → Undo 두 번으로 head 1 → Redo. 기대는 B(seq 2)를 거쳐 D(seq 4)로 다시 진행하는 것이다. 실제 제어 흐름은 seq 2의 branch가 main이므로 자식을 찾지 못하고, 스냅샷이 없는 짧은 저널에서는 false를 반환한다. 반복 가능한 `LoadRecord("rec-handover-brief")`로도 저널 경로 회귀를 구성할 수 있다.

필수 회귀는 분기 생성 후 공통 조상까지 두 단계 이상 Undo/Redo, 저장·로드 뒤 같은 경로, 압축된 스냅샷 경계에서 활성 분기 선택 보존이다. 기존 `JournalUndoRedoBranchesAndAutoCopiesAreImmutable`은 분기 직후의 Redo 불가만 검사하며 공통 조상에서 활성 분기로 다시 올라가는 경우가 없다. `save-undo.md:84–86`의 분기/Redo 계약을 따른다.

### C7-F52 · 새 복구 슬롯이 재시작 후 발견되지 않음

**S2 · open.** `App/T0GameSession.cs:193`의 `recovery-new-slot`은 Store를 기존 저장 경로 아래 무작위 `recovery-{guid}` 디렉터리로 바꾸지만 선택된 경로를 지속하지 않는다. 다음 `Initialize(:39–40)`은 기본 경로 또는 원래 `--t0-save-dir` 경로를 다시 연다. 슬롯 선택 UI도 검토 스냅샷에 없다.

재현: 미래 버전 또는 복구 불가능한 저장으로 부팅 → 복구 패널에서 새 슬롯 → T0 진행·저장 → 같은 실행 인자로 앱 재시작. 원래 실패 파일을 다시 열어 복구 패널에 들어가며 새 진행이 있는 하위 슬롯으로 돌아갈 경로가 없다. 기존 파일 바이트는 보존되지만 새 진행은 사용자에게 고립된다.

필수 수정은 안전한 슬롯 선택 포인터 또는 발견 가능한 슬롯 선택 경로다. 회귀는 새 슬롯 생성→저장 완료→전체 GameSession/플레이어 재시작→동일 진행·활성 슬롯 복원과 원본 실패 파일 해시 불변을 포함해야 한다. 원본 세이브를 덮어쓰는 방식으로 해결하지 않는다.

### C7-F53 · 복구 재시도 성공 후 읽기 전용 플래그가 남음

**S2 · open.** 로드 실패 시 `App/T0GameSession.cs:50`이 `saveReadOnly=true`를 설정한다. `:193`의 `recovery-retry` 성공 경로는 Journal과 화면만 갱신하고 이 플래그를 해제하지 않는다. `StartGame(:60)`은 다시 복구 패널로 돌아가고 `CommitAsync(:85)`도 계속 false를 반환한다. 저장 신원의 `saveId`/`createdUtc` 또한 성공한 문서에서 다시 취하지 않는다.

재현: 모든 후보가 실패해 복구 패널 진입 → 유효한 세이브/백업을 복원 → 재시도 성공 → 시작. 복구 성공 상태를 표시한 뒤에도 재개가 거부된다. 회귀는 동일 인스턴스에서 실패→유효 파일 교체→재시도→시작·새 확정 저장·다음 로드를 확인해야 한다. 재시도 Load에도 Journal validation을 제공해 유효 후보를 선택하고, 성공한 문서의 신원과 쓰기 가능 상태를 복원한다.

### C7-F54 · 미등록 저장 버전이 백업 복구로 우회됨

**S2 · open.** `Save/AtomicSaveStore.cs:79`는 없는 schemaVersion을 -1로 취급한다. `:81`에서 음수를 `InvalidOperationException`으로 거부하지만 `:92`의 일반 복구 catch가 이를 삼켜 다음 백업을 연다. 미래 버전 `>1`의 직접 Refused 반환과 동작이 다르다.

재현: 체크섬은 올바르지만 `schemaVersion`이 없거나 -1인 primary와 정상적인 v1 `save.bak`을 둔다 → `Load(validate: ...)`. 기대는 `SAVE_VERSION_REFUSED`와 읽기 전용이다(`save-undo.md:70`: 높은 버전 **또는 미등록**은 Refused). 검토 코드에서는 정상 backup을 쓰기 가능한 문서로 반환하고, 다음 자동 저장에서 알 수 없는 버전의 primary를 교체할 수 있다.

필수 회귀는 정상 백업 존재 여부와 무관한 unknown/future 거부, primary와 backup의 바이트 불변, 정수 타입 검증이다. 기존 `VersionZeroMigratesWithBackupAndVersionTwoIsRefused`는 0과 2만 다루고 누락·음수·비정수 schemaVersion은 검사하지 않는다. 버전 거부를 체크섬/재생 실패 복구 catch와 구분해야 한다.

## 3. 소스에서 확인한 구현 경계

| 영역 | 확인한 구현 | 아직 증명하지 않은 것 |
|---|---|---|
| 원자 저장 | 문서 DeepClone, SemaphoreSlim 직렬화, tmp 쓰기, `Flush(true)`, 교체 직전 취소 확인, `File.Replace`/`Move`, tmp 정리 | 실제 프로세스 강제 종료·전원 차단·디스크 가득·파일시스템 내구성 |
| 멱등성 | primary의 유효 checksum과 commitIdempotencyKey 대조; rename 뒤 실패 재시도용 키 보존 | 플레이어에서 rename 직후 중단·재실행·늦은 콜백 경합 |
| 복구 | checksum/재생 검증 후 primary→backup→checkpoint 순회; 유효 v2 직접 거부 | F52–F54의 사용자 복구 흐름 및 미등록 버전 |
| 저널 | 불변 PuzzleState, 부모 seq 기반 재생, 스냅샷/hash, 영구 보존 단서의 합집합, 분기 시 이전 entries 유지 | F51, 압축 후 복잡한 분기 탐색, 과거 head 상태에서 로그 상한 도달 |
| SavePending | candidate를 성공한 저장 후 게시; generation 확인 뒤에만 성공 영수증; 실패 시 원본 Journal 유지 | quit 직전 flush/commit 수명, 취소 직후 재확정·늦은 완료의 실제 플레이어 경합 |
| 인용 출처 | DataLoader가 명시적 `systemId`/`stationId`를 전달; plate만 system 필수, ledger null 허용; 출처 root 검증; `CiteToBoard`에서 명시 귀속 요구 | 실제 빌드의 생성 테이블 영수증·임포트·저장/로드 완료 |
| 인용 구간 | Cite event가 현재 start/end를 스냅샷으로 취하며 reducer가 별도 citationStart/citationEnd에 기록 | 데이터 버전 변경 시 출처/샘플 해석의 마이그레이션 보장 |

인용 출처 필드를 null 허용 여부와 무관하게 자동 추정하는 코드는 해당 경로에서 발견하지 않았다. sourceType/root lineage 독립성 검사와 plate/ledger 예외가 코드에 존재한다. 이것은 해당 계약 경로를 읽었다는 사실이며 C7-F50 실행 검증 완료나 게이트 PASS는 아니다.

## 4. 검토한 테스트와 실행 증거 경계

EditMode 소스에는 회로 FSM, 전체 T0 명령 경로, ledger의 null systemId, 단서 보존, 단일 분기, snapshot/fold, payload/hash 손상, 정규화 checksum, 쓰기 단계별 예외 주입, rename 뒤 멱등성, 취소, backup/checkpoint 복구, v0/v2 경계 검사가 있다. PlayMode 소스에는 키보드/패드 입력, T0 UI 경로, 확인 모드, SavePending 중 Undo·늦은 영수증 억제, 저장 실패 후 재시도가 있다.

**이 리뷰는 그 테스트를 실행하지 않았다.** 테스트 코드의 존재를 통과 결과로 세지 않는다. 특히 F51–F54를 포괄하는 새 회귀, 실제 Unity 결과 XML/로그, 플레이어 재시작 증거가 결함 닫힘의 필요조건이다. 모델러·컨셉·밸런스·사람 플레이테스트·기준 하드웨어 성능 판정은 이 코드 리뷰 범위 밖이다.

## 5. 통보와 상태

네 발견 모두 `/root/t0_systems`와 director `/root`에 파일·행·재현 조건·필수 회귀를 통보했다. 디렉터가 QA canonical register 소유 범위를 확장했으며 `qa/defect-register.md`에 C7-F51–F54를 append한다. status 셀은 `**open**` 단독값으로 유지한다. 코드 수정이 보이더라도 새 실행 회귀 영수증을 확인하기 전에는 닫지 않는다.

`feedback-requested-by: 2026-09-10`

**[OBSERVED · 문서 검증, 2026-09-10 08:19:48 UTC]** `rtk run 'bash .claude/skills/game-ops-harness/scripts/freshness-check.sh'` → exit 0, `freshness: 0 finding(s) across 137 markdown artifact(s) under _workspace/current`. 독립 읽기 검사에서 C7-F51–F54는 각각 한 행·일곱 셀이며 status는 모두 `**open**` 단독값이었다. 보고서 필수 frontmatter와 단일 H1도 확인했다. 구조 검사이며 memory_sync·staleness·런타임 게이트 검증은 아니다. 파생 cycle-ledger 재생성은 director의 통합 단계에 남겼다.

## 6. EditMode6 실행 증거에 따른 후속 판정

QA는 Unity를 추가 실행하지 않고 systems가 실행한 `/tmp/unknown-t0-m2-editmode-4.xml/.log`와 `-6.xml/.log`를 직접 읽었다. 테스트 파일 수정 시각은 08:21:34 UTC로 EditMode6 실행보다 앞선다. 검증은 XML 개별 `test-case`의 `result`, runner 종료 코드, 해당 C# 테스트의 실제 단언을 함께 대조했다.

```yaml
measured:
  reviewed_at: 2026-09-10T08:26:54Z
  evidence_method: XML test-case results plus runner log and exact source assertions
  reviewer_session: /root/t0_resource_qa
  runner_command_evidence: Unity -runTests -testPlatform EditMode -testResults <receipt-path>
  editmode4:
    start: 2026-09-10T08:19:55Z
    end: 2026-09-10T08:19:57Z
    total_nunit_testcases: 18
    passed: 8
    failed: 10
    runner_exit_code: 2
  editmode6:
    start: 2026-09-10T08:24:06Z
    end: 2026-09-10T08:24:07Z
    total_nunit_testcases: 18
    passed: 18
    failed: 0
    skipped: 0
    runner_exit_code: 0
  unity_runs_started_by_this_reviewer: 0
```

`GeneratedT0ContractChecksPass`는 이 18개 중 **한 개**의 NUnit testcase다. 내부 native contract 검사 21건을 추가 NUnit testcase로 더하지 않는다. EditMode6은 PlayMode·네이티브 플레이어 조작·사람 플레이테스트 영수증이 아니다.

| 결함 | 실행된 테스트와 실제 단언 | 후속 상태 |
|---|---|---|
| C7-F51 | `RedoFollowsSelectedBranchAcrossSharedAncestors` (`T0M2Tests.cs:23`): 새 분기 후 두 번 Undo, Redo 두 번 각각 true, head가 공통 조상 b와 선택 분기 d의 seq와 일치. EditMode6 Passed | **closed** — 원래 보고한 짧은 저널 공통 조상 실패를 해소. 분기+압축+재로드 조합 전체를 검증했다는 뜻은 아님 |
| C7-F52 | 새 복구 슬롯 생성 후 App/플레이어 재시작 경로에 해당하는 EditMode testcase 없음 | **open** — PlayMode/플레이어 영수증 대기 |
| C7-F53 | 같은 GameSession의 복구 실패→재시도 성공→시작/새 저장 경로에 해당하는 EditMode testcase 없음 | **open** — UI 복구 영수증 대기 |
| C7-F54 | `UnknownVersionsRefuseEvenWithValidBackup` (`:24`): null/-1/1.5/문자열 1 각각 SAVE_VERSION_REFUSED 및 primary 텍스트 불변. `VersionZeroMigratesWithBackupAndVersionTwoIsRefused` (`:38`): v0 원본 백업 존재와 migration, v2 거부·바이트 불변. EditMode6 모두 Passed | **closed** — unknown/future를 직접 Refused로 반환하는 정수/범위 guard도 `AtomicSaveStore.cs:80`에서 확인 |
| C7-F55 | `CanonicalSaveRootAndChecksumAreOrderIndependent` (`:32`): 생성된 createdUtc 문자열과 Encode→Decode 뒤 문자열이 동일하고 체크섬 decode 성공. 멱등성·backup·migration 회귀도 EditMode6 Passed | **closed** — 아래 실패 원인과 수정 경계 참조 |

**F54 테스트 범위 제한:** unknown 테스트는 첫 null 사례에서 유효 v1 backup을 가진다. 반복문의 다음 사례에서는 이전 invalid primary가 backup으로 회전된다. 따라서 네 타입 모두를 각각 유효 backup과 조합해 실행했다고 보고하지 않는다. 누락된 속성을 별도로 삭제한 fixture, backup 파일 자체의 전후 바이트 비교도 현재 testcase의 단언에 없다. 실행된 거부 이유/primary 불변과 직접 반환 guard는 원래 보고한 fallback 우회 원인의 해소를 뒷받침한다.

### C7-F55 · ISO 타임스탬프 자동 변환으로 자체 생성 세이브 체크섬이 달라짐

**S2 · closed, systems 발견.** 수정 전 `SaveCodec.Decode`의 `JObject.Parse(text)`는 ISO `createdUtc`를 날짜 값으로 자동 해석할 수 있었다. Encode가 문자열로 해시한 표기와 Decode가 재직렬화한 날짜 표기가 달라지면 유효한 자체 생성 저장을 SAVE_CHECKSUM_FAILED로 거부한다. 그 결과 정상 저장 로드·backup 복구·rename 뒤 멱등성 대조까지 실패할 수 있다. 수정본 `SaveCodec.cs:28–31`은 `JsonTextReader`에 `DateParseHandling.None`을 적용해 스키마의 문자열을 보존한다.

EditMode4에서 `AfterRenameRetryIsIdempotent`는 false 기대에 true, `TruncationRecoversBackupThenCheckpointWithoutDeletingBytes`는 save.bak 기대에 recovery, unknown 버전 테스트는 SAVE_VERSION_REFUSED 기대에 SAVE_CHECKSUM_FAILED로 실패했다. 이 저장 관련 실패와 수정 전후 코드가 원인 진단의 근거다. **당시 `CanonicalSaveRootAndChecksumAreOrderIndependent` 자체는 Passed였으며 createdUtc 왕복 단언이 없었다.** 따라서 EditMode4에 직접 타임스탬프 회귀 실패가 있었다고 바꾸어 기록하지 않는다. 열 개 실패 전체를 이 결함 하나로 귀속하지도 않는다. 별도 synthetic fixture/null 표현/취소 예외 유형 실패가 포함돼 있었다.

EditMode6에서는 새 createdUtc 왕복 단언, `AfterRenameRetryIsIdempotent`, 세 `InterruptedWritePreservesPreviousSave` 사례(BeforeWrite/DuringWrite/BeforeRename), 취소, backup/checkpoint, 버전 검사가 모두 Passed다. AfterRename은 별도 멱등성 테스트이므로 「예외 주입 네 단계」라는 범위와 「InterruptedWrite 세 parameterized testcases」를 구분한다. 이 회귀는 생성된 UTC 타임스탬프와 현재 직렬화 계약을 입증하며 모든 시간대/과거 스키마의 일반 마이그레이션 보장은 아니다.

### 영수증 해시

| 파일 | SHA-256 |
|---|---|
| `/tmp/unknown-t0-m2-editmode-4.xml` | `3c79c93f5828dd63e102351a715463dec66cabb747a46c490ecd0e2ce3f3a8aa` |
| `/tmp/unknown-t0-m2-editmode-4.log` | `98e67980b02f1ae3894ad02427c5bb9e2d2a125b74a1d62510595b7e5e012a14` |
| `/tmp/unknown-t0-m2-editmode-6.xml` | `12885cfc24f66e567dbe9ddd7795635dcdd924318800ce6513e9c0bd8ef46967` |
| `/tmp/unknown-t0-m2-editmode-6.log` | `b899923182666195cbb95c4165b8d2d4ea3a168af20d826b5d80704a95c34d61` |
| `Tests/EditMode/T0M2Tests.cs` | `161e112fac7cfcd40495673ed89ac3f375f4d76a4e592e254eab21fad310d513` |
| `Save/SaveCodec.cs` | `5cb3bd937ef70bbaffa7caff5a97b0abb99fa8abd5234f0ec026b8c965b7a79e` |
| `Save/AtomicSaveStore.cs` | `76453654cb9e52483d8d9816bdbd195c8ffa804c63c1fb40f73451c269185738` |
| `Sim/CommandJournal.cs` | `e680928e4ef12ed337c50879215cdb5dae32922f99ae2f2d30bcc0a443f3923a` |

임시 XML/log의 영구 보존은 systems/director 소유 영수증 폴더로 요청했다. 본 QA는 원본 영수증이나 Unity 코드를 변경하지 않았다. `feedback-requested-by: 2026-09-10`.

## 7. PlayMode3 복구·인용 실행 영수증

QA는 `/tmp/unknown-t0-m2-playmode-3.xml/.log`와 실제 테스트 단언, 생성 테이블/런타임 참조를 읽기 전용으로 대조했다. 테스트 소스 수정 시각 08:30:30 UTC는 아래 실행 이전이다. 이후 routing 변경의 테스트 결과를 이 영수증으로 대신하지 않는다.

```yaml
measured:
  reviewed_at: 2026-09-10T08:34:08Z
  test_start: 2026-09-10T08:30:49Z
  test_end: 2026-09-10T08:30:53Z
  runner: Unity -runTests -testPlatform PlayMode
  total_nunit_testcases: 8
  passed: 8
  failed: 0
  skipped: 0
  runner_exit_code: 0
  reviewer_started_unity_runs: 0
  native_player_process_restarts_measured: 0
```

| 결함 | 실행된 testcase와 실제 단언 | 판정과 경계 |
|---|---|---|
| C7-F52 | `RecoveryNewSlotIsSelectableAfterProcessEquivalentRestart` (`T0PlayModeTests.cs:44`): v2 primary로 복구 패널 → 새 하위 슬롯 → hb-l1 진행 저장 → Restart → `select-slot-1` → 시작 → 진행 fact 유지, 원래 v2 primary 바이트 동일 | **closed.** 새 슬롯이 발견 가능한 선택 경로로 복원됨. 자동 선택을 주장하지 않으며 실제 OS 프로세스 종료 검증도 아님 |
| C7-F53 | `ValidatedRetryClearsRefusalAndPreservesSaveIdentity` (`:45`): v2 거부 뒤 유효 backup을 복원 → retry/start → 복구 표면 이탈 → 새 CommitAsync true → 로드 문서의 saveId/createdUtc가 원본과 동일 | **closed.** 같은 App 인스턴스의 재시도 성공 뒤 쓰기 가능 상태와 저장 신원 복원 입증 |
| C7-F50 | `PointerUiCompletesCanonicalT0AndReopensSavedState` (`:40`): canonical plate와 ledger 각각 read/window/cite/confirm → t0-b3 완료, 성공 영수증 두 개 → 저장 로드/Journal decode의 StateHash 동일 | **closed.** 최신 명시 귀속이 로드된 런타임에서 실제 두 자료 인용과 재로드 가능. 테스트 이름만으로 모든 물리 포인터 조작을 입증하지 않음 |

`Restart` helper(`:43`)는 `FlushSaves` 뒤 기존 GameObject를 Destroy하고 새 T0GameSession을 같은 directory와 `Resources.Load<T0RuntimeConfig>("T0Runtime")`로 Initialize한다. 세션 수명/디스크 재로드 회귀이며 플레이어 프로세스를 실제 종료·재실행한 테스트는 아니다.

`Click` helper(`:33`)는 `Interface.Activate(actionId)`를 직접 호출한다. 인용 테스트의 `SetWindow`는 `SubmitImmediate`로 들어간다. 따라서 이것을 실제 마우스 hit target, 키보드/패드의 모든 reader 버튼 경로, 마지막 routing 정본 교정의 통과로 표현하지 않는다. 동일 suite의 키보드 시작·gamepad 시작·확인 모드·SavePending Undo·실패 후 재시도 테스트도 Passed지만 각 단언의 범위를 넘어 일반화하지 않는다.

### C7-F50의 원본→생성기→런타임 연결

**[OBSERVED]** `systems/data/t0/records.json`과 Unity `Data/Tables/records.json`은 바이트 단위로 동일하다. 현재 synopsis 원문 SHA-256은 생성 파일의 `recordsDocSha256` 및 receipt의 `authoringSources.records.sha256`과 일치하고, runtime records SHA-256은 receipt의 records 항목과 일치한다. receipt는 `emitter: systems/pipeline/emit-tables.mjs`, `scope: t0`, `dryRun: false`다.

| recordId | systemId | stationId |
|---|---|---|
| `rec-plate-standard-hub` | `system-hub` | `station-bureau-standard` |
| `rec-tide-ledger-bureau` | JSON null | `station-bureau-standard` |

나머지 세 기록에는 해당 필드가 추가되지 않았다. 원래 sourceType/rootOriginId를 유지한다. 비교 기준 연결의 저작 의미에 대한 기존 소스 QA ACK는 그대로다. 이 검사는 새 관측 생산지 사실을 승인하지 않는다.

`Resources/T0Runtime.asset`의 records와 catalog 참조 GUID를 각각 실제 records meta와 catalog meta에 대조했다. catalog의 tables 배열에도 해당 records 참조가 존재한다. `T0CatalogAsset.Load(:22–26)`는 그 TextAsset 바이트를 모아 `T0DataLoader.LoadVerified`에 실제 receipt 및 producerReceipt와 함께 전달한다. 앞선 EditMode6의 `ApprovedLedgerNullSystemIsValidAndOtherRecordsStayUnassigned`도 Passed다. 이 연결과 PlayMode3의 실제 인용 쌍 완료가 이전 소스 QA의 「생성기 미반영 스냅샷」을 현재 상태에서 해소한다.

| 증거 파일 | SHA-256 |
|---|---|
| `/tmp/unknown-t0-m2-playmode-3.xml` | `6a467c7bb8f1411e7793dbfe516f5ea9ad6820ca033e2c31cb5953d7ee31598b` |
| `/tmp/unknown-t0-m2-playmode-3.log` | `d82a9b44f4c42aabe2911a40b152cb042226bbb376e527ccd85880df59c21c1b` |
| `Tests/PlayMode/T0PlayModeTests.cs` | `6eb692f91f6a8c906a67ee64e5b526b1052a77a655cbff3ee30c7cb40604b6e6` |
| `Data/Tables/records.json` | `7703643983f733a49f173296530710bb719d5ec663fa6ba863a25c6c9d7989cd` |
| `Data/Tables/tables-receipt.json` | `5f5aa4a8640b1b0e5a11dfefa8ec04ac29510ca06f880c82214ba6810d6f1bb7` |

원본 XML/log의 영구 보존 위치는 systems가 `systems/tech-verification/t0-m2/`로 마련 중이다. QA는 이번 후속 판정을 director와 systems에 통보했다. `feedback-requested-by: 2026-09-10`.
