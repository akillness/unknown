using System;
namespace Tide.Data
{
    // Wire types mirror generated JSON. Tuning is imported, never defaulted in code.
    [Serializable] public sealed class Rows<T> { public int schemaVersion; public string scope; public T[] rows; }
    [Serializable] public sealed class BeatJson { public string id; public string[] prerequisites; public PredicateJson completionPredicate; }
    [Serializable] public sealed class PredicateJson { public RequirementJson[] requires; }
    [Serializable] public sealed class RequirementJson
    {
        public string type,recordId,slotId,decisionId,rowId,clueId,sourceType,originId,rootOriginId,startPhase,endPhase;
        public string[] lineIds,rowIds,areaIds,values;
        public int count,mediumsPerArea;
    }
    [Serializable] public sealed class RecordJson
    {
        public string recordId,sourceType,originId,rootOriginId,copiedFrom,systemId,stationId;
        public string[] clueIds,visibleAt;
        public LineJson[] lines;
        public RowJson[] rows;
        public SampleJson[] samples;
    }
    [Serializable] public sealed class LineJson { public string lineId; }
    [Serializable] public sealed class RowJson { public string rowId; }
    [Serializable] public sealed class SampleJson { public string phase; public float t; }
    [Serializable] public sealed class ZonesJson { public int resolutionMinutes; public ZoneJson[] rows; }
    [Serializable] public sealed class ZoneJson { public string zoneId; public string[] systemIds,uncoveredAreaIds; }
    [Serializable] public sealed class ToolsJson { public KnobsJson knobs; public ToolJson[] rows; public StubJson[] stubs; public OverlayJson circuitOverlay; }
    [Serializable] public sealed class PointJson { public double x,y; }
    [Serializable] public sealed class AnchorJson { public string anchorId,displayNameKo; public PointJson target,overlay; }
    [Serializable] public sealed class OverlayJson { public double gridStep; public PointJson initialOffset; public AnchorJson[] anchors; }
    [Serializable] public sealed class KnobsJson { public KnobJson readBudget; }
    [Serializable] public sealed class KnobJson { public string value; }
    [Serializable] public sealed class ToolJson { public string toolId; }
    [Serializable] public sealed class StubJson { public string toolId; }
    [Serializable] public sealed class ReceiptJson
    { public string emittedUtc,scope; public SourceJson source; public ValidatorJson validator; public TableJson[] tables; }
    [Serializable] public sealed class SourceJson { public string path,sha256; public long bytes; }
    [Serializable] public sealed class ValidatorJson { public string verdict; public int exitCode,fail,pass,checks; }
    [Serializable] public sealed class TableJson { public string file,sha256,derivation; public long bytes; public int rows; }
}
