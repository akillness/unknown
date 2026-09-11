---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 시스템 스펙 — save-undo (세이브·체크포인트·무제한 되돌림·손상 복구)

전부 `[TARGET]`. **이 스펙의 위반은 플레이어 데이터 손실로 직결된다**(CLAUDE.md §9 불변식). 실측 0건.

| 항목 | 값 |
|---|---|
| 저장 슬롯 | 수동 3 + 자동 1 |
| 자동 저장 시점 | 장 경계, 구역 이동, **모든 확정 직전** |
| 되돌림 | **무제한**, 무료, 페널티 0 (명령 로그 포인터 이동). 상한 상수 없음 — 프로토타입 `model.mjs`의 `maxUndo: 32`는 탐색 편의값이며 사양이 아니다 (RFC-P3-015 F23) |
| 되돌림 최후 수단 | **체크포인트 재로드는 상시 가능**하다. 이력 입도가 낮아져도 진행이 막히지 않는다 |
| 스키마 버전 | `schemaVersion: 1` (신규) |
| 이야기 시계 | 하룻밤 21:00 → 05:00 단일 야간 [OBSERVED: worldview/timeline.md] |

## 0. 병행 초안과의 차이 (RFC-S3)

`systems/unity-implementation.md`(같은 레인 병행 산출물, 작업 중 cycle `c4` draft → `c5` draft로 갱신됨 [OBSERVED 2026-09-10]) 저장 스키마는 `"chapter": 3, "dayIndex": 2`를 갖고 있었다. **2026-09-10 R7에 해소**: `unity-implementation.md` §7 조각을 `save.md` §1·§3과 문자 단위로 맞추면서 `chapter`/`dayIndex`/`eventSeq`/`eventLogHash`를 걷어냈고, 그 조각이 스키마 정본이 아니라는 사실을 문서에 명시했다(C7-F7 편집의 부수). 확정된 캐논은 **단일 야간**(장별 21:00/21:30/22:10/23:00/00:00/01:00/02:00/03:30/05:00)이라 `dayIndex`는 항상 0인 죽은 필드다. 본 스펙은 `dayIndex` 대신 `stageId` + `storyClock`을 쓴다. 코드가 0줄이므로 지금 바꾸는 비용은 0이고, 출시 후에는 개명 금지 대상이 된다.

## 1. 입력

| 입력 | KB/마우스 | 패드 | 결과 |
|---|---|---|---|
| 수동 저장 | 메뉴 → 슬롯 | `A` | `SaveToSlot(n)` |
| 로드 | 메뉴 → 슬롯 | `A` | `LoadSlot(n)` |
| 되돌림 / 다시 | `Ctrl+Z` / `Ctrl+Y` | `LB+X` / `LB+B` | `Undo` / `Redo` |
| 체크포인트 목록 | 일시정지 → 기록 | `back` | 라벨: 장·조위 위상·플레이 시간 |
| 복구 패널 선택 | 3버튼 | `A` | `백업으로 열기` / `마지막 체크포인트` / `새로 시작` |

## 1-A. 키보드 파생 `[C4-F21]`

**신설 2026-09-10 R6 (C6/C7).** §1 표에서 KB/마우스 열이 포인터 조작만 적은 행의 키보드 단독 경로다. 규칙 id 는 `systems/interaction-rules.md` §1-3.4(D-1~D-7)를 인용한다. §0-8(키보드 단독 완결)이 전역 보장이고, 이 표는 그 보장이 **행 단위로** 어떻게 성립하는지를 적는다.

| 위 표의 행 | 파생 규칙 | 키보드 단독 경로 |
|---|---|---|
| 수동 저장 (메뉴 → 슬롯) | **D-5** | `Esc` 로 일시정지 메뉴 → 「저장」 초점 `Enter` → 슬롯 1~3 초점 `Enter` |
| 로드 (메뉴 → 슬롯) | **D-5** | 동상. 로드는 확인 1회(§1-1 확정 방식 설정을 따른다) |
| 체크포인트 목록 (일시정지 → 기록) | **D-5** | `Esc` → 「기록」 초점 `Enter` |
| 복구 패널 선택 (3버튼) | **D-1** | 세 버튼 사이를 방향키로 옮기고 `Enter`. **초기 초점은 「백업으로 열기」** 이며 파괴적 선택(「새로 시작」)에 초기 초점을 두지 않는다 |
| 되돌림 / 다시 | — | 이미 키보드 경로다(`Ctrl+Z` / `Ctrl+Y`) |

- 복구 패널은 오버레이 표면이므로 §1-3.1 배달 우선순위에서 가장 위다. 그 뒤의 도구 패널·셸 입력은 배달되지 않는다.

## 2. 상태기계 — 저장 쓰기

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Idle` | `SaveRequested` | `Serializing` | 스냅샷 + 명령 로그 직렬화 |
| `Serializing` | 성공 | `WritingTemp` | `save.tmp` 기록 |
| `WritingTemp` | flush + fsync 성공 | `Rotating` | 기존 정본 → `save.bak` |
| `Rotating` | 성공 | `Renaming` | `rename(tmp, save.json)` (원자적) |
| `Renaming` | 성공 | `Idle` | `saved` 토스트, `save_write_ms` 기록 |
| 임의 단계 | 실패(디스크 가득·권한·전원) | `WriteFailed` | **정본 무변경**, 사유 표시, 재시도 제안 |

정본 자리에 부분 파일이 오는 경로가 **존재하지 않는다**. 이것이 이 스펙의 첫 번째 하드 요구다.

## 3. 상태기계 — 로드·복구

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Idle` | `LoadSlot` | `ReadingPrimary` | `save.json` 읽기 |
| `ReadingPrimary` | 체크섬 OK, `schemaVersion == 1` | `Replaying` | 스냅샷 + 로그 재생 |
| `ReadingPrimary` | 체크섬 OK, `schemaVersion < 1` | `Migrating` | `save.v0.bak` 보존 후 마이그레이터 체인 |
| `ReadingPrimary` | `schemaVersion > 1` 또는 미등록 | `Refused` | **파일 바이트 무변경**, 복구 패널 |
| `ReadingPrimary` | 체크섬 실패 | `ReadingBackup` | `save.bak` 시도 |
| `ReadingBackup` | 성공 | `Replaying` | "백업에서 복구됨" 고지 |
| `ReadingBackup` | 실패 | `ReadingCheckpoint` | 마지막 자동 체크포인트 |
| `ReadingCheckpoint` | 실패 | `RecoveryPanel` | **읽기 전용**. 3개 선택지만. 손상 파일 덮어쓰기 금지 |
| `Migrating` | 성공 | `Replaying` | 새 파일로 기록, 원본 보존 |
| `Migrating` | 실패 | `RecoveryPanel` | 원본 무변경 |
| `Replaying` | 로그 재생 완료, 해시 일치 | `Ready` | 게임 재개 |
| `Replaying` | 해시 불일치 | `RecoveryPanel` | 조작·손상 의심 기록 |

## 4. 상태기계 — 되돌림 / 브랜치

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `AtHead` | `Undo` | `InPast` | `headSeq -= 1`, 로그 항목 **삭제하지 않음** |
| `InPast` | `Redo` | `AtHead`/`InPast` | `headSeq += 1` |
| `InPast` | 새 명령 제출 | `AtHead(newBranch)` | **분기 생성**, 이전 이력 접근 가능 유지 |
| any | `Fork(sandbox)` | `Sandbox` | O(1), 상태 복사 없음 |
| `Sandbox` | `Commit` | `AtHead` | main에서 **재검증** 후 적용 |
| `Sandbox` | `Discard` | `AtHead` | main 무변화, `sandboxDiscardedCount += 1` |

## 5. 규칙

| id | 규칙 |
|---|---|
| SV-R1 | 지속 필드 **개명 금지**. 마이그레이터 없이 개명하면 세이브가 고아가 된다 → 거부 |
| SV-R2 | `schemaVersion`은 정수 단조 증가. 더 높은 버전은 열지도 덮어쓰지도 않는다 |
| SV-R3 | 쓰기는 tmp → fsync → 회전 → rename. 4단계 중 어디서 죽어도 정본이 유효하다 |
| SV-R4 | 체크섬은 `sha256(정규화 JSON 본문)`. 필드 순서는 키 사전순으로 고정(결정론) |
| SV-R5 | 손상 파일은 **절대 덮어쓰지 않는다**. 복구 패널은 읽기 전용이다 |
| SV-R6 | 200 커밋마다 스냅샷을 남겨 로그 재생 시간을 상한한다 |
| **SV-R11** | **재생 단위는 명령 하나다** [C7-F7 · 2026-09-10 R7]. 세이브에 남는 것은 `CommandEntry(seq, branchId, parentSeq, commandId, **payload**, payloadHash, committed)`이며 **이벤트는 저장되지 않는다**(메모리 안의 파생물). 재생 절차·순수성 조건은 `data-schemas/save.md` §3.0~§3.1 |
| **SV-R12** | **확정 트랜잭션마다 `commitIdempotencyKey`** 를 발급해 세이브에 함께 기록한다. 재시도·지연 완료·rename 후 크래시에서 중복 적용을 막는 유일한 수단이다(`SV-F8` · 인수 `B-SV7`) |
| SV-R7 | DLC 플래그가 없어도 본편 로드·엔딩 3종 도달이 성립한다. DLC 삭제 시 본편 진행 보존 |
| SV-R8 | 자동 저장은 확정 **직전**이다. 확정 직후가 아니다(선택 재시도를 보장하기 위해) |
| SV-R9 | 연습(sandbox)은 저장되지 않는다. UI가 이 사실을 확정적으로 고지한다 |
| SV-R10 | 세이브 파일은 로컬 전용. 클라우드·서버 전송 없음 |

## 6. 실패 모드

| id | 상황 | 시스템 반응 | 플레이어 손실 |
|---|---|---|---|
| SV-F1 | 쓰기 중 전원 차단 | 다음 로드에서 정본(구버전) 정상 로드 | 마지막 저장 이후 진행 |
| SV-F2 | 정본 체크섬 실패 | `save.bak` → 체크포인트 → 복구 패널 순 | 최소화, 최악에도 새로 시작 선택권 |
| SV-F3 | 신버전 세이브를 구버전 실행 | `Refused`, 파일 바이트 불변 | 없음 |
| SV-F4 | 디스크 가득 | `WriteFailed` + 정본 무변경 + 사유 | 없음 |
| SV-F5 | 로그 재생 해시 불일치 | 복구 패널 + `save_hash_mismatch` 기록 | 체크포인트로 복귀 |
| SV-F6 | 명령 로그 상한 초과(**바이트 또는 개수, 먼저 닿는 쪽**) | 오래된 구간을 스냅샷으로 접고 되돌림 입도를 체크포인트 단위로 낮춤 | 세밀한 되돌림 입도(고지) |
| SV-F7 | 슬롯 덮어쓰기 오조작 | 덮어쓰기는 별도 확인 + 직전 파일을 `slotN.bak`으로 회전 | 없음 |
| **SV-F8** | 확정 중 재시도·지연 완료·rename 후 크래시 | `commitIdempotencyKey` 대조로 **같은 커밋을 두 번 적용하지 않는다**. 키가 다른 늦은 콜백은 폐기하고 UI를 바꾸지 않는다 | 없음 |
| **SV-F9** | 재생 중 `Validate` 실패(데이터 손상·버전 불일치) | **조용히 건너뛰지 않고** `RecoveryPanel`로 간다. 손상 파일 무변경 | 마지막 유효 체크포인트 이후 |

## 7. 데이터 스키마 참조

- `systems/data-schemas/save.md` — 전체 필드, 타입, 마이그레이션 규칙, 개명 금지 목록
- `systems/data-schemas/beats.md` — `checkpoint` id(33개, 비트마다 1개) [OBSERVED]
- `systems/architecture-contract.md` §6, §8

## 8. 텔레메트리 필드

| 키 | 타입 | 의미 |
|---|---|---|
| `undo_count` | int | 공통 키. `headSeq` 하향 횟수 |
| `redo_count` | int | |
| `branch_created` | int | 되돌림 후 재조작으로 생긴 분기 |
| `sandbox_discarded` | int | 폐기된 연습 브랜치 |
| `sandbox_time_min` / `commit_time_min` | float | 공통 키 |
| `checkpoint_created` | int | |
| `checkpoint_loaded` | int | |
| `save_write_ms` | float | 쓰기 소요(fsync 포함) |
| `save_load_ms` | float | 로드 + 재생 소요 |
| `save_recovery_path` | enum | `primary` \| `backup` \| `checkpoint` \| `panel` |
| `save_hash_mismatch` | int | **0이어야 하는 값** |
| `save_refused_version` | int | 신버전 거부 횟수 |

`save_recovery_path != primary` 인 모든 건은 `ops/telemetry-contract.md` §6 세이브 손상 복구 로그에 원인·파일 크기·직전 이벤트와 함께 기록한다.

## 9. 성능 예산 [TARGET] — NOT-MEASURED

| 항목 | 목표 | 비고 |
|---|---|---|
| 세이브 쓰기(fsync 포함) | ≤ 200 ms | 프레임 블로킹 금지, 백그라운드 스레드 |
| 세이브 로드 + 로그 재생 | ≤ 1500 ms | 스냅샷 200커밋 간격 전제 |
| 되돌림 1스텝 | ≤ 16 ms | 프레임 내 |
| 체크포인트 생성 | ≤ 100 ms | |
| 세이브 파일 크기 | ≤ 8 MB | 명령 로그 몫 ≤ 6 MiB(`byteCap`) + 엔트리 수 가드 **20,000**(`entryCap`). 「50,000」은 `payload` 없던 시절의 짝이라 폐기했다 — 재산정 근거는 `data-schemas/save.md` §3.2 [C7-F7] |
| 자동 저장 프레임 스파이크 | ≤ 4 ms | 직렬화는 워커, 메인은 스냅샷 복사만 |

## 10. 인수 기준

### 문서 단계 (D)
| id | 기준 |
|---|---|
| D-SV1 | 정본에 부분 파일이 오는 경로가 상태기계에 없는가 |
| D-SV2 | 상위 버전 거부가 "파일 무변경"으로 명시됐는가 |
| D-SV3 | 개명 금지 대상 필드 목록이 스키마 문서에 존재하는가 |
| D-SV4 | 자동 저장이 확정 **직전**으로 명시됐는가 |
| D-SV5 | `dayIndex` 대신 `stageId`/`storyClock`을 쓰는 근거가 캐논과 일치하는가 |
| **D-SV6** | 재생 단위가 **문서 전체에서 하나**인가(명령 소싱 · 이벤트 미저장). 두 모델이 공존하지 않는가 [C7-F7] |
| **D-SV7** | `commitIdempotencyKey`가 스키마 §1과 개명 금지 목록 §4에 **둘 다** 등재됐는가 |

### 빌드 후 (B)
| id | 기준 | 방법 |
|---|---|---|
| B-SV1 | 부분 기록 세이브 주입 시 `.bak`으로 복구하고 정본 바이트 불변 | PlayMode + 파일 해시 |
| B-SV2 | `schemaVersion=0` 세이브가 마이그레이션 후 v1 만족 | 픽스처 테스트 |
| B-SV3 | `schemaVersion=2` 세이브 거부 + 바이트 불변 | 픽스처 테스트 |
| B-SV4 | 되돌림 포인터를 0까지 내린 뒤 재적용 시 원래 해시 복귀 | 결정론 테스트 |
| B-SV5 | 쓰기 중 프로세스 강제 종료 100회 후 정본 손상 0건 | 반복 킬 테스트 |
| B-SV6 | 세이브 쓰기가 메인 스레드를 4 ms 이상 막지 않음 | 프레임타임 캡처 |
| **B-SV7** | 같은 `commitIdempotencyKey`로 두 번 성공해도 명령이 한 번만 적용됨 | 주입 테스트(`T-17`·`T-19`) |
| **B-SV8** | `payload` 직렬화 왕복 후 재생 상태 해시가 원본과 동일하고 **세이브 파일에 이벤트 0건** | 결정론 테스트(`S-I11`) |
| **B-SV9** | 실제 평균 엔트리 바이트를 재어 `entryCap`을 재파생 | `save_file_bytes / entries_count` · `command_count` (현재 **n = 0**) |
