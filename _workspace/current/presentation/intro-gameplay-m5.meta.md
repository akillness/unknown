---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-presentation-director
---

# M5 연출 JSON 메타데이터

[TARGET] `intro-gameplay-m5.json`은 영상 두 편과 읽기 전용 인트로·지속 방향표 적용의 저작 계약이다. duration과 immersionTarget은 목표이며 실측 결과가 아니다. 정확한 생성 프롬프트·타임라인·접근성·불변식·인수 기준을 가진다. 실제 제공자 영수증과 QA 결과는 별도 산출물로 연결한다.

[OBSERVED] `observedVideoReview`는 두 실제 영상의 길이·컷·해시를 분리해 기록한다. `scenes[].beats`는 원래 저작 목표이며 실제 관측값으로 덮어쓰지 않았다. 검수 문서는 `intro-gameplay-m5-video-review.md`; 네이티브 검증은 아직 별도다.

[OBSERVED · 디렉터 승인] 최종 네이티브 캡션 계약은 **3125 ms / 2875 ms / 합계 6000 ms**다. JSON `nativeApplication.introContextPhases`와 `assetBindings`에 hub r03 및 영상 이후 GTI 금속 표면의 실제 경로·해시·참조 프레임을 기록한다. `current`는 승인된 설계 정본이라는 뜻이며 네이티브 인수 통과 상태가 아니다.
