---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M8 외부 자료 검토 영수증

[OBSERVED] `source-review.json`은 사용자 지정 WeChat 글의 공개 본문을 HTTP 200으로 읽은 기록이다. 발행 시각은 페이지의 Unix timestamp를 UTC로 변환했다. 추출 본문의 SHA-256을 남겼으며 원문 전체와 제3자 이미지는 재배포하지 않는다.

[OBSERVED] 글은 罗斯基가 Naavik 보고서를 번역·정리했다고 설명하는 2차 자료다. 판매·이용자·비용·체류시간 숫자를 별도 원출처와 대조하지 않았으므로 프로젝트의 시장 예측, 성과 근거, 런타임 측정으로 쓰지 않는다. 원문도 이용자 구성 변화 등 교란 요인을 설명하며 이탈의 단일 원인을 단정하지 않는다.

[INFERENCE] 상황을 대조하는 행동, 자기 언어로 표현하는 가설, 문맥 보존은 이 게임의 추리 루프에 적용할 수 있다. M8는 오프라인 검토 노트로 먼저 구현한다. 모델 추론이나 자유문장 의미 평가를 구현했다는 뜻이 아니다. 적용 범위와 비채택 항목은 JSON에 분리했다.

[CARRIED] 시각 원전은 RFC-CX-009와 `concept/concept-first-m7-sources.json`이다. 기사 이미지, 현재 프리팹, 런타임 리소스와 M5/M6 파생 미디어는 새 아트 입력으로 채택하지 않는다.
