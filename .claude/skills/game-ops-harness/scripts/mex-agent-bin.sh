#!/usr/bin/env bash
# mex-agent-bin.sh — sourceable resolver for the *real* mex-agent CLI.
#
# Why this exists: on this machine `mex` on PATH is TeX Live's pdfTeX format
# driver (/opt/homebrew/bin/mex -> texlive). Running it leaves texput.log in the
# repo and can block on stdin. Never invoke a bare `mex` from PATH without proof
# of identity.
#
# Contract:
#   resolve_mex_agent  -> 0 and sets MEX_BIN, or 1 and sets MEX_SKIP_REASON
# Resolution order:
#   1. $MEX_AGENT_BIN (explicit configuration) — still identity-probed.
#   2. Known installed mex-agent paths.
#   3. `mex-agent` / `mex` on PATH — accepted ONLY if the identity probe passes.
# Never installs anything and never touches the network.

MEX_BIN=""
MEX_SKIP_REASON=""

# Probe an executable's identity without ever running it bare. Runs in a scratch
# directory with stdin closed so a TeX binary cannot hang or litter the repo.
_mex_identity_ok() {
  _mib_bin="$1"
  [ -n "$_mib_bin" ] && [ -x "$_mib_bin" ] || return 1
  _mib_dir="$(mktemp -d 2>/dev/null)" || return 1
  _mib_out="$( { cd "$_mib_dir" && "$_mib_bin" --version; } </dev/null 2>&1 | head -20
              { cd "$_mib_dir" && "$_mib_bin" --help; } </dev/null 2>&1 | head -60 )"
  rm -rf "$_mib_dir"

  # Negative gate: TeX family (pdfTeX/XeTeX/LuaTeX/kpathsea/Web2C).
  if printf '%s\n' "$_mib_out" | grep -qiE 'tex live|pdftex|luatex|xetex|kpathsea|web2c|texname|\.fmt file|\\FIRST-LINE'; then
    return 1
  fi
  # Positive gate: must look like the mex-agent CLI surface.
  if printf '%s\n' "$_mib_out" | grep -qiE 'mex-agent|project memory|code graph|mex[[:space:]]+(setup|graph|check|sync|log|wiki|impact|timeline|scope|context)'; then
    return 0
  fi
  return 1
}

resolve_mex_agent() {
  MEX_BIN=""
  MEX_SKIP_REASON=""

  if [ -n "${MEX_AGENT_BIN:-}" ]; then
    if [ ! -x "${MEX_AGENT_BIN}" ]; then
      MEX_SKIP_REASON="MEX_AGENT_BIN is not an executable file: ${MEX_AGENT_BIN}"
      return 1
    fi
    if _mex_identity_ok "${MEX_AGENT_BIN}"; then
      MEX_BIN="${MEX_AGENT_BIN}"
      return 0
    fi
    MEX_SKIP_REASON="MEX_AGENT_BIN failed the mex-agent identity probe (TeX or unknown binary): ${MEX_AGENT_BIN}"
    return 1
  fi

  for _mib_cand in \
    "$HOME/.npm-global/bin/mex-agent" \
    "$HOME/.local/bin/mex-agent" \
    "$HOME/.bun/bin/mex-agent" \
    /opt/homebrew/bin/mex-agent \
    /usr/local/bin/mex-agent \
    "$(command -v mex-agent 2>/dev/null || true)" \
    "$(command -v mex 2>/dev/null || true)"
  do
    [ -n "$_mib_cand" ] || continue
    [ -x "$_mib_cand" ] || continue
    if _mex_identity_ok "$_mib_cand"; then
      MEX_BIN="$_mib_cand"
      return 0
    fi
  done

  MEX_SKIP_REASON="mex-agent not found or failed identity probe (PATH \`mex\` is TeX on this machine). Set MEX_AGENT_BIN=/path/to/mex-agent to enable."
  return 1
}
