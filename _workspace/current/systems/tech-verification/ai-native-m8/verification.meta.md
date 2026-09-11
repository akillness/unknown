---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M8 검증 영수증 규약

[OBSERVED] 이 폴더의 JSON/XML/log는 [검증 보고서](verification.md)와 [집계](verification.json)에 연결된다. source-review.json/source-independent-review.md는 자료 검토이며 native 실행과 구분한다. 최종 전달 소스는 delivery-source-manifest.json의13개 오버레이로 한정한다.

[OBSERVED] attempt1/2의 실패, attempt3의 미완 실행, 부트의 필수 인자 누락에 따른 ignored 결과를 삭제하지 않았다. baseline-source-manifest.json과 baseline-reduced-motion.xml은 M8가 없는 기준 커밋의 기존 실패 재현이다. EditMode·PlayMode·baseline·부트 결과를 합산하거나 모두 통과했다고 표시하지 않는다.

[CARRIED] 원문 전체·제3자 이미지를 재배포하지 않는다. 사람 플레이테스트·물리 IME/컨트롤러 검수·새 아트 생성·전체 캠페인 완성은 별도다. raw native log의 원래 공백과 오류 이력은 보존한다.

[OBSERVED] player-smoke-before-fixture.log는 초기 검수 저장 준비 전에 종료한 setup-only 실행이다. 최종 실제 창 검수는 player-smoke.log/player-smoke.json이며 native-tab-before-note.json과 native-tab-final-note.json은 서로 다른 격리 fixture의 수정 전·후 입력 결과다.
