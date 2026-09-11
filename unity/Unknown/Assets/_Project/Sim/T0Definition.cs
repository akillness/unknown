using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tide.Sim
{
    public sealed class RecordDefinition
    {
        public string Id { get; }
        public string SourceType { get; }
        public string OriginId { get; }
        public string RootOriginId { get; }
        public string CopiedFrom { get; }
        public string SystemId { get; }
        public string StationId { get; }
        public IReadOnlyList<string> ClueIds { get; }
        public IReadOnlyList<string> LineIds { get; }
        public IReadOnlyList<string> RowIds { get; }
        public IReadOnlyList<string> Phases { get; }
        public IReadOnlyList<string> VisibleAt { get; }
        public bool HasCitationProvenance => (SourceType!="plate" || !string.IsNullOrEmpty(SystemId)) && !string.IsNullOrEmpty(StationId);
        public RecordDefinition(string id, string sourceType, string originId, string rootOriginId,
            string copiedFrom, string systemId, string stationId, IEnumerable<string> clueIds,
            IEnumerable<string> lines, IEnumerable<string> rows, IEnumerable<string> phases, IEnumerable<string> visibleAt)
        {
            Id=id; SourceType=sourceType; OriginId=originId; RootOriginId=rootOriginId;
            CopiedFrom=copiedFrom; SystemId=systemId; StationId=stationId;
            ClueIds=Freeze(clueIds); LineIds=Freeze(lines); RowIds=Freeze(rows);
            Phases=Freeze(phases); VisibleAt=Freeze(visibleAt);
        }
        internal static IReadOnlyList<string> Freeze(IEnumerable<string> values) =>
            Array.AsReadOnly((values ?? Array.Empty<string>()).ToArray());
    }

    public sealed class CompletionRequirement
    {
        public string Type { get; }
        public string RecordId { get; }
        public string Id { get; }
        public string Value { get; }
        public string OtherValue { get; }
        public int Count { get; }
        public IReadOnlyList<string> Ids { get; }
        public CompletionRequirement(string type, string recordId=null, string id=null,
            string value=null, string otherValue=null, int count=0, IEnumerable<string> ids=null)
        { Type=type; RecordId=recordId; Id=id; Value=value; OtherValue=otherValue; Count=count; Ids=RecordDefinition.Freeze(ids); }
    }

    public sealed class BeatDefinition
    {
        public string Id { get; }
        public IReadOnlyList<string> Prerequisites { get; }
        public IReadOnlyList<CompletionRequirement> Requirements { get; }
        public BeatDefinition(string id, IEnumerable<string> prerequisites, IEnumerable<CompletionRequirement> requirements)
        { Id=id; Prerequisites=RecordDefinition.Freeze(prerequisites); Requirements=Array.AsReadOnly(requirements.ToArray()); }
    }

    public sealed class T0Definition
    {
        public IReadOnlyDictionary<string, RecordDefinition> Records { get; }
        public IReadOnlyList<BeatDefinition> Beats { get; }
        public IReadOnlyList<string> UncoveredAreas { get; }
        public IReadOnlyList<string> SystemIds { get; }
        public IReadOnlyList<string> StubToolIds { get; }
        public int ReadBudget { get; }
        public int ResolutionMinutes { get; }
        public CircuitOverlay Overlay { get; }
        public C1PatrolDefinition Patrol { get; }
        public C1SignatureDefinition Signature { get; }
        public T0Definition(IEnumerable<RecordDefinition> records, IEnumerable<BeatDefinition> beats,
            IEnumerable<string> uncoveredAreas, IEnumerable<string> systemIds, IEnumerable<string> stubs,
            int readBudget, int resolutionMinutes,CircuitOverlay overlay=null,C1PatrolDefinition patrol=null,C1SignatureDefinition signature=null)
        {
            Overlay=overlay; Patrol=patrol; Signature=signature;
            Records=new ReadOnlyDictionary<string, RecordDefinition>(records.ToDictionary(r=>r.Id, StringComparer.Ordinal));
            Beats=Array.AsReadOnly(beats.ToArray()); UncoveredAreas=RecordDefinition.Freeze(uncoveredAreas);
            SystemIds=RecordDefinition.Freeze(systemIds); StubToolIds=RecordDefinition.Freeze(stubs);
            ReadBudget=readBudget; ResolutionMinutes=resolutionMinutes;
        }

        public string ResolveRoot(string recordId)
        {
            var seen=new HashSet<string>(StringComparer.Ordinal);
            while (true)
            {
                if (!seen.Add(recordId) || !Records.TryGetValue(recordId, out var record))
                    throw new InvalidOperationException("Unresolvable record lineage: " + recordId);
                if (string.IsNullOrEmpty(record.CopiedFrom)) return record.OriginId;
                recordId=record.CopiedFrom;
            }
        }

        public bool IsIndependentPair(string first, string second)
        {
            return Records[first].SourceType != Records[second].SourceType &&
                ResolveRoot(first) != ResolveRoot(second);
        }
    }
}
