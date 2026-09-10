namespace Tide.Sim
{
    // Exact gameplay reason set from codex-unity-brief section 5-7.
    public enum ReasonCode
    {
        OUT_OF_COVERAGE,
        ANCHOR_INCOMPLETE,
        ZONE_LOCKED,
        ORIGINAL_DEGRADED,
        MEDIA_DUPLICATE,
        INDETERMINATE,
        RESIDUAL_EXCEEDED,
        NO_SHARED_TIDE_EVENT,
        NOT_ALIGNED,
        ANCHOR_SPREAD_INSUFFICIENT,
        DUAL_PROTECTION_FORBIDDEN,
        ROUTE_CYCLE,
        CORROSION_LIMIT_EXCEEDED,
        NO_VALID_ROUTING,
        WITNESS_DECLINED,
        SAVE_CHECKSUM_FAILED,
        SAVE_VERSION_REFUSED,
        WRITE_FAILED,
        REPLAY_HASH_MISMATCH,
        LOG_CAP_EXCEEDED,
        DUPLICATE_COMMIT_SUPPRESSED,
        REPLAY_VALIDATE_FAILED,
        NOT_IMPLEMENTED_IN_T0
    }
}

