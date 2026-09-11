---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M6 film editing and technical validation

The Python scripts render authored typography, colour grading, exact 30fps segment boundaries, spoiler-value overlays and a locally authored filtered-noise ambience. They do not alter Unity or call providers. Run render-films.py, then validate-films.py through RTK from this repository. --typeset-only reuses locally generated render-cache and is only suitable when source segment filters and timing have not changed. The cache is reproducible and excluded from Git.

validation.json binds the final two MP4 files, full decode results, frame-exact cuts, sound levels and local gallery assets. Browser verification observed both players decoding 1280x720;36/54second metadata; final gameplay-method time advancing to25.76seconds with readyState4, then paused. No human audition or human immersion study was performed. Source AAC is preserved but not used.

The final films are cinematic previz with separately labeled M5 native inserts. Initial source review found absent T0/C1 live save transitions; the edit uses rule cards instead. The producer/reviewer loop corrected body font sizes and an initial two-frame cumulative native-cut drift. The final ready-to-saved boundary is frame1410 (47seconds). Auxiliary source/eyebrow labels are16/18px; main body and action captions are at least32px.

Decode logs preserve command text with trailing whitespace normalized for Git.
