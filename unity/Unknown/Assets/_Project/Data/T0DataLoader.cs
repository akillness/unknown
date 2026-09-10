using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tide.Sim;
using UnityEngine;

namespace Tide.Data
{
    public static class T0DataLoader
    {
        public static T0Definition LoadVerified(IReadOnlyDictionary<string,byte[]> files,byte[] receipt,byte[] trustedReceipt)
        {
            ReceiptVerifier.Verify(files,receipt,trustedReceipt);
            var records=Parse<Rows<RecordJson>>(files,"records.json").rows;
            var beats=Parse<Rows<BeatJson>>(files,"beats.json").rows;
            var zones=Parse<ZonesJson>(files,"zones.json");
            var tools=Parse<ToolsJson>(files,"tools.json");
            if(!int.TryParse(tools.knobs?.readBudget?.value,NumberStyles.Integer,CultureInfo.InvariantCulture,out var budget))
                throw new InvalidOperationException("Generated readBudget is missing");
            if(zones.resolutionMinutes<=0) throw new InvalidOperationException("Generated resolution is missing");
            var definition=new T0Definition(records.Select(r=>new RecordDefinition(r.recordId,r.sourceType,r.originId,
                r.rootOriginId,r.copiedFrom,string.IsNullOrEmpty(r.systemId)?null:r.systemId,string.IsNullOrEmpty(r.stationId)?null:r.stationId,r.clueIds,r.lines?.Select(l=>l.lineId),
                r.rows?.Select(l=>l.rowId),r.samples?.Select(s=>s.phase),r.visibleAt)),
                beats.Select(b=>new BeatDefinition(b.id,b.prerequisites,b.completionPredicate.requires.Select(Map))),
                zones.rows.SelectMany(z=>z.uncoveredAreaIds),zones.rows.SelectMany(z=>z.systemIds),
                tools.stubs.Select(s=>s.toolId),budget,zones.resolutionMinutes,
                tools.circuitOverlay==null?null:new CircuitOverlay(tools.circuitOverlay.gridStep,null,
                    tools.circuitOverlay.initialOffset.x,tools.circuitOverlay.initialOffset.y,
                    tools.circuitOverlay.anchors.Select(a=>new CircuitAnchor(a.anchorId,a.displayNameKo,a.target.x,a.target.y,a.overlay.x,a.overlay.y))));
            // These are runtime reference checks, not a second campaign validator.
            foreach(var record in definition.Records.Values)
            {
                var root=definition.ResolveRoot(record.Id);
                if(!string.IsNullOrEmpty(record.RootOriginId) && record.RootOriginId!=root)
                    throw new InvalidOperationException("Runtime lineage disagrees with generated root: "+record.Id);
            }
            return definition;
        }

        public static IReadOnlyList<string> CitationBlockers(T0Definition definition) =>
            Array.AsReadOnly(definition.Beats.SelectMany(b=>b.Requirements).Where(r=>r.Type=="citationPinned")
                .Select(r=>r.RecordId).Distinct().Where(id=>!definition.Records[id].HasCitationProvenance)
                .Select(id=>id+": systemId/stationId absent; RFC-CX-001").ToArray());

        private static T Parse<T>(IReadOnlyDictionary<string,byte[]> files,string name) =>
            JsonUtility.FromJson<T>(Encoding.UTF8.GetString(files[name]));
        private static CompletionRequirement Map(RequirementJson r)
        {
            switch(r.type)
            {
                case "recordLinesViewed": return new CompletionRequirement(r.type,r.recordId,ids:r.lineIds);
                case "recordRowsViewed": return new CompletionRequirement(r.type,r.recordId,ids:r.rowIds);
                case "slotLoaded": return new CompletionRequirement(r.type,r.recordId,r.slotId);
                case "decisionRecorded": return new CompletionRequirement(r.type,id:r.decisionId,ids:r.values);
                case "uncoveredAreasMarked": return new CompletionRequirement(r.type,ids:r.areaIds,count:r.count);
                case "eachAreaHasEvidence": return new CompletionRequirement(r.type,count:r.mediumsPerArea);
                case "citationPinned": return new CompletionRequirement(r.type,r.recordId,r.clueId);
                case "autoCopyCreated": return new CompletionRequirement(r.type,r.recordId,count:r.count);
                case "independentPair": return new CompletionRequirement(r.type);
                case "gapEndpointsFixed": return new CompletionRequirement(r.type,r.recordId,value:r.startPhase,otherValue:r.endPhase);
                default: throw new InvalidOperationException("Unsupported generated predicate: "+r.type);
            }
        }
    }
}
