# Dependency Matrix — who must be notified when a lane changes

Row = lane that changed. Columns = lanes that must `ack | counter | block` in the RFC
before the change is `status: current`. ● required, ○ informational.

| changed ↓ / notify → | plan | bal | sys | eco | pres | syn | world | con | vfx | anim | mot | mod | qa |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **planning (기획)** | – | ● | ● | ● | ● | ○ | ○ | ○ | ○ | ○ | ○ | ○ | ● |
| **balance (밸런스)** | ○ | – | ● | ● | | | | | | | ● | | ● |
| **systems (시스템)** | ○ | ● | – | ● | ○ | | | | ● | ● | ● | ○ | ● |
| **economy (재화)** | ● | ● | ● | – | ○ | ○ | | | | | | | ● |
| **presentation (연출)** | ○ | | ○ | | – | ● | ○ | ○ | ● | ● | ● | ● | ● |
| **synopsis (시놉시스)** | ○ | | | ○ | ● | – | ● | ● | | ○ | | | ○ |
| **worldview (세계관)** | ● | | | | ○ | ● | – | ● | ○ | | | ○ | ● |
| **concept (컨셉)** | | | | | ● | ○ | ● | – | ● | ● | | ● | ○ |
| **vfx (이팩트)** | | | ● | | ● | | | ○ | – | ● | ● | ○ | ● |
| **animation (에니메이션)** | | | ● | | ● | | | | ● | – | ● | ● | ● |
| **motion (모션)** | | ● | ● | | ● | | | | ● | ● | – | | ● |
| **modeling (모델링)** | | | ○ | | ● | | ○ | ● | ● | ● | | – | ● |
| **qa** | ● | ● | ● | ● | ● | ● | ● | ● | ● | ● | ● | ● | – |

QA broadcasts to every affected lane by construction. A change whose required acks are
missing stays `status: draft` and cannot feed a gate.

## Shared-truth files (edit only by owner; others cite)
- `animation/anim-list.md` key-event frames → read by vfx, motion
- `motion/feel-tuning.md` hit-stop / reaction band → read by vfx, animation, balance
- `worldview/glossary.md` names → read by everyone; asset & feature names derive from it
- `presentation/presentation-spec.md` intent + timeline → read by vfx, animation, motion, modeling
- `balance/balance-sheet.md` + `economy/reward-bands.md` → mirrored to data by systems
- `qa/gate-measurements.md` → cited by every gate verdict
