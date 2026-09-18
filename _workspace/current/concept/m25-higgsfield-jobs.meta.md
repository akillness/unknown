---
updated: 2026-09-18
cycle: 20260918-content-update-m25
status: current
supersedes: null
owner: game-concept-artist
---

# m25-higgsfield-jobs.json — M25 Higgsfield 생성 계약 (프롬프트 정본)

21개 작업(배경 7 · 초상 5 · 도구 아이콘 6 · 모션 3). 각 프롬프트는 `style-guide.md` §2 팔레트·§4 재질·§5 카메라·§10 금지(NEGATIVE)를 압축해 반복한다. 참조 입력은 RFC-CX-M25-20260918이 M25 한정으로 바인딩한 원본 c4 시트뿐이다. 실행: `scripts/gen-higgsfield.py <this> [--only id,…] [--parallel N] [--dry-run] [--force]`. 로그 `m25-gen-images.log`·`m25-gen-video.log`. `bg-quay`는 플라크/명판 재발로 두 번 재생성했다(반려본 `assets/generated/2d/m25/rejected/`).
