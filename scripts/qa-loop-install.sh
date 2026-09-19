#!/usr/bin/env bash
# qa-loop-install.sh - install / remove / inspect the 3-hour QA loop launchd agent (user domain, no sudo).
#   scripts/qa-loop-install.sh            install (copies the plist into ~/Library/LaunchAgents and bootstraps it)
#   scripts/qa-loop-install.sh --now      install and kick one cycle off immediately
#   scripts/qa-loop-install.sh --status   print launchd state (interval, last exit, run count)
#   scripts/qa-loop-install.sh --uninstall
set -euo pipefail
LABEL=io.github.akillness.unknown.qa-loop
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
SRC="$ROOT/scripts/launchd/$LABEL.plist"; DST="$HOME/Library/LaunchAgents/$LABEL.plist"; DOMAIN="gui/$(id -u)"
case "${1:-}" in
  --status) launchctl print "$DOMAIN/$LABEL" 2>/dev/null | grep -E "state|interval|last exit|runs|program " || echo "not loaded"; exit 0;;
  --uninstall) launchctl bootout "$DOMAIN/$LABEL" 2>/dev/null || true; rm -f "$DST"; echo "removed $LABEL"; exit 0;;
esac
mkdir -p "$HOME/Library/LaunchAgents" "$ROOT/unity/Unknown/Builds/qa-loop"
plutil -lint "$SRC" >/dev/null
launchctl bootout "$DOMAIN/$LABEL" 2>/dev/null || true
cp "$SRC" "$DST"
launchctl bootstrap "$DOMAIN" "$DST"
launchctl print "$DOMAIN/$LABEL" | grep -E "state|interval" | sed 's/^/  /'
echo "installed $LABEL (every 10800 s); ledger: _workspace/current/qa/loop/ledger.md"
[ "${1:-}" = "--now" ] && { launchctl kickstart "$DOMAIN/$LABEL"; echo "kickstarted; follow unity/Unknown/Builds/qa-loop/launchd.log"; }
exit 0
