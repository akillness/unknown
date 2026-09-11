---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
describes: production/receipts/unity-batchmode/{create,open-validate}.full.log
---

# Unity 배치모드 영수증 [OBSERVED 2026-09-10]

| 실행 | 명령 | 종료 코드 | 근거 행 |
|---|---|---|---|
| 프로젝트 생성 | `Unity -batchmode -nographics -quit -createProject unity/Unknown -logFile create.log` (6000.5.6f1) | 0 | `create.full.log` "Exiting batchmode successfully now!" |
| 헤드리스 열기 검증 | `Unity -batchmode -nographics -quit -projectPath unity/Unknown -logFile open-validate.log` | 0 | `open-validate.full.log` (같은 문구) |

- 로그는 `unity/Unknown/Logs/`(gitignore)가 아니라 세션 스크래치에 기록됐기 때문에 저장소에 없었다(C7 반박 검토자 지적 정당). 여기 전문을 보존한다.
- 라이선스 클라이언트 경고("Access token is unavailable")는 비치명이며 배치 실행을 막지 않았다.
- 이 영수증은 "프로젝트가 열린다"만 증명한다. 코드 0줄·테스트 0건·성능 캡처 0건.

## 재실행 [OBSERVED 2026-09-10 09:2x KST]
| 실행 | 종료 코드 | 근거 |
|---|---|---|
| 헤드리스 열기 검증 2차 (`open-validate-2.full.log`) | 0 | "Exiting batchmode successfully now!" 1건 — 1차(open-validate)는 성공 문구 없이 34행에서 끝났으므로 미확인이었고, 2차가 처음으로 **헤드리스 열림을 증명**한다. 당시 실행 중이던 GUI 에디터는 2022.3.32f1(다른 버전)이었고 이 프로젝트에 락 파일은 없었다. |
