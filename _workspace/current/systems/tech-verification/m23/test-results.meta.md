---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M23 · 실행 XML

초기35ebdc5는177, 첫 교정5d7b18c는186개이며 각 원본 XML/실패/지문을 보존한다. 현행은 마지막 절의187개다. Node/캠페인/그래프 수는 NUnit 개수와 합치지 않는다.

| XML | pass | fail | skip | SHA-256 |
|---|---:|---:|---:|---|
| `editmode.xml` | 64 | 0 | 0 | `aea467e625d5850268029d2338af82945d89c8c22d1278fdaeb45272afc9b1cb` |
| `playmode-release-final.xml` | 112 | 0 | 1 | `8477623234a03fe8480ecb2827e116a0db4429872f32b64bcbb5c0937e537df1` |
| `boot-release.xml` | 1 | 0 | 0 | `8bc3c01e51ee532cff2ea18135a1c582ec486eef23905468994a34e3b1d51665` |
| `review-feedback-red.xml` | 0 | 2 | 0 | `fbfe0b217180b92d67083505a806695057c88b451c7793f4a29078f3e9a16566` |
| `review-boundary.xml` | 2 | 1 | 0 | `9fc87d45759c6757e5932fa8a56dc7f4ee81c65d05c71921d82c703288dc57c6` |
| `feedback-visibility-red.xml` | 0 | 1 | 0 | `cc8369ba8e49b395dad0a2345b6c1ff5e1bc86f602500588083d8de18cae7c1d` |
| `playmode-release.xml` | 111 | 1 | 1 | `339a55ce3a639e650f219470eb992317b37bc63c7b5ca5e6fbe40f054f972046` |

[BOUNDARY] review-feedback-red는 Q1/Q2 원재현, review-boundary는 늦은 실패 경계, feedback-visibility-red는 Q5 원재현이다. playmode-release의 Band comparison clipping은 첫 고정 알림안이 Navigation을 줄인 실패이며 최종에서 오른쪽 작업면만 분리해 해결했다. 반복 실행을 고유 개수로 더하지 않는다.

## 5d7b18c 첫 교정 XML

당시186은 correction-editmode.xml의 프로젝트64 + correction-playmode-final.xml121 + correction-boot-final.xml1의 집계다. 이 EditMode는 최종 geometry 수정 전 실행을 이월했으므로 동일 소스 수렴 증거가 아니다. EditMode65번째 외부 예제는 제외한다. RED12회는 신규9개 고유 사례의 반복/확장 실행이다. 최신 소스의 재실행은 마지막 절에 기록한다.

| XML | pass | fail | skip | SHA-256 |
|---|---:|---:|---:|---|
| `correction-boundary-red.xml` | 0 | 8 | 0 | `5f32e6724ae01269764bc9fade8cbcb64b4e083a990d992b282956510992da13` |
| `correction-comparison-card-red.xml` | 0 | 1 | 0 | `0914911c0a8df8bb92d280a00611ed71185f68267e9bc3cdb2fc3aefadf464e4` |
| `correction-comparison-case-thread-red.xml` | 0 | 1 | 0 | `7b9bfcd08308f80c7cf353b5a349e2b425b22f1bf9d7c1aefdd1eb65427529a3` |
| `correction-practice-timeline-red.xml` | 0 | 1 | 0 | `20428901b4036e23188bd8825edbd2e6654db18e09189fecd11db8dbaa3b4c85` |
| `correction-practice-peak-bounds-red.xml` | 0 | 1 | 0 | `625568b7203fae54870b970cdcf898444263789a4b61a885a3572d081845919a` |
| `correction-boundary-green.xml` | 120 | 0 | 1 | `3f50ecff87700eb688f136f8fad2278841a21c35028de62016a91fe8132e47ef` |
| `correction-editmode.xml` | 65 | 0 | 0 | `ec23261192b41259b6fcaea3c815df3a425a5f88b40434b73dd45ebe1f5fef10` |
| `correction-reader-green.xml` | 121 | 0 | 1 | `3849ff9f962af0e6386f9e877c3acf6cfef39d30277443b30f63830fe2629acb` |
| `correction-practice-contrast-green.xml` | 121 | 0 | 1 | `001992d8f551ed5eaf567366c68c7f501dbcd17b7abcf0dd833bcaf57aad399e` |
| `correction-playmode-final.xml` | 121 | 0 | 1 | `a65e9eb1f89e0556c6142a7f5f41871d2fa896e7403e44fb8ffa4a17afe168ae` |
| `correction-boot-final.xml` | 1 | 0 | 0 | `c817f248ecbac2ba4e243c8b2d90632a552c0704fdfd76707c0832c357ef9464` |

## 마지막 off-reader 수명 교정

최신 소스7e2c383에서 EditMode를 재실행했다:65 pass/0 fail/0 skip 중 프로젝트64와 외부 Addressables 예제1이다. 외부 예제를 제외한64 +같은 최종 생산 소스의 PlayMode122 +격리 boot1 =187 고유 통과다. 후속 회귀10개가 통과했고 인자 누락 boot의 skip은 제외한다. 이전177/186/첫187 집계의 EditMode 이월을 현행 동일 소스 증거와 혼동하지 않는다.

| XML | pass | fail | skip | SHA-256 |
|---|---:|---:|---:|---|
| `off-reader-pin-red.xml` | 0 | 1 | 0 | `b720220070df53b9ffb87de0dfb9dd5fea53fc0870b298766ee04db373d7dc34` |
| `off-reader-playmode-final.xml` | 122 | 0 | 1 | `d758c19498107f738a28b8837f9017b7e2c3036a455acda2d26d0ffa4cc787a9` |
| `off-reader-boot-isolated.xml` | 1 | 0 | 0 | `93681177370cf95250a98e9caf1ecd2a23b2a62ba8b18b95c46d13142d72766b` |
| `off-reader-boot-invocation-skipped.xml` | 0 | 0 | 1 | `1b93aa0b532c20181c8cd325a38e3ff8ac614d6320e6c16925f245418fa427ef` |
| `editmode-source-final.xml` | 65 | 0 | 0 | `63179940afc56e4f8e7221f0a4eb6aa0c7c5c7d6b3d9c7b72fcf516af97c6fe6` |

최신 EditMode 실행은2026-09-13T17:17:11Z–17:17:14Z이며 프로젝트64개를 fullname 집합으로 분리해 PlayMode/격리 boot와 합집합187을 확인했다. 원본 XML은51463바이트다.
