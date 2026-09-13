---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M23 · 최종 소스 EditMode 실행

- 대상: `editmode-source-final.xml`
- 생산 소스: `7e2c383759cc81530efb970d05e0a3e3d6d23124`. 실행 후 런타임 소스를 변경하지 않았다.
- 실행: Unity6000.5.6f1, batchmode/nographics, `-runTests -testPlatform EditMode`.
- UTC: 2026-09-13T17:17:11Z–17:17:14Z. NUnit duration2.5588068초, 프로세스 exit0.
- 결과:65 pass /0 fail /0 skip. 이 중 `Tide.Tests.*` 프로젝트64개만 프로젝트 합계에 포함한다.
- 제외: `AddressableAssets.DocExampleCode.TestStub.RequiredTest`1개는 외부 예제다. 프로젝트65개 통과로 주장하지 않는다.
- 집계: 최종 생산 소스의 PlayMode122·격리 boot1과 fullname 합집합187. 이전 EditMode 이월/RED 반복/skip은 더하지 않는다.

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `editmode-source-final.xml` | 51463 | `63179940afc56e4f8e7221f0a4eb6aa0c7c5c7d6b3d9c7b72fcf516af97c6fe6` |

실행 XML과 결과 경계는 `verification.json`의 `evidenceConvergence`와 연결된다. `test-results.meta.md`는 여러 실행의 색인이고, 이 동명 파일이 개별 XML의 메타데이터다. 사람 평가·Windows·전체 캠페인 완료를 입증하지 않는다.
