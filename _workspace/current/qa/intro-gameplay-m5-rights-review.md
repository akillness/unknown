---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M5 scene still — output rights and provenance review

[OBSERVED] OpenAI’s contractual output-ownership clause is verified. **GTI backend authorization, actual image-model identity and contract applicability remain unverified.** This review does not promote an asset or certify an unrestricted commercial licence.

Source: [OpenAI Terms of Use](https://openai.com/policies/terms-of-use/), effective **January 1, 2026**, retrieved **2026-09-11 UTC** by `mcp__scrapling__get` (HTTP 200). Review timestamp: 2026-09-11T06:32:23Z. No account credentials were accessed; no agreement was accepted.

## Exact relevant clauses

> Ownership of content. As between you and OpenAI, and to the extent permitted by applicable law, you (a) retain your ownership rights in Input and (b) own the Output. We hereby assign to you all our right, title, and interest, if any, in and to Output.

> Similarity of content. Due to the nature of our Services and artificial intelligence generally, output may not be unique and other users may receive similar output from our Services. Our assignment above does not extend to other users’ output or any Third Party Output.

> Our [Business Terms](https://openai.com/policies/business-terms/) govern use of ChatGPT Enterprise, our APIs, and our other services for businesses and developers.

Under **What you cannot do**, the page lists:

> Automatically or programmatically extract data or Output (defined below).

> Represent that Output was human-generated when it was not.

## Bounded interpretation

- Ownership is **as between the user and OpenAI**, to the extent applicable law permits, and covers only OpenAI’s rights, if any. It does not establish copyrightability, exclusivity, ownership of third-party material or immunity from infringement claims. The same page places responsibility for Content and necessary Input permissions on the user and disclaims service/non-infringement warranties.
- The ownership clause is a contractual basis for using covered Output, including incorporating it into a game, subject to the applicable agreement and other rights. It is not a separate GTI licence or a guarantee that every image returned through a third-party wrapper is covered. API/business use has a separate agreement; the actual service and contract cannot be inferred from the wrapper’s name.
- The extraction restriction makes route applicability material. No backend traffic, credentials or account contract was inspected here, so this review establishes neither GTI compliance nor a violation. User authorization to use GTI does not itself establish OpenAI’s support for that route. No official GTI integration support, availability guarantee, SLA, image-model identity or zero-cost claim follows from these Terms.

## Workspace application

`CLAUDE.md` §이미지 생성 제공자 (2026-09-11), `handoff/asset-runbook.md` and `production/image-provider-gti-20260911.md` record the new explicit GTI image authorization. This supersedes the older image-provider choice; existing rights and promotion rules remain. The production contract describes GTI’s Codex route as unofficial and potentially interruptible.

For this still, preserve the exact original prompt/reference inputs, output bytes and SHA-256, request/job receipt, requested and actual dimensions, generation date and known cost. Record **requested orchestration model `gpt-6-astra`** separately from **actual image model: unreported**; do not relabel the orchestration model as the image generator. Unknown cost stays unknown.

The task describes a new original scene; this review does not independently certify originality or inspect its pixels. Root must link the final asset path/hash and inspect for third-party marks, generated text, canon leaks and native readability before applying this review to that artifact. `provenance.json` and initial `runtimeEligible:false` remain required; runtime promotion belongs to the separate director decision-log audit. **Higgsfield video remains previz, not runtime.**
