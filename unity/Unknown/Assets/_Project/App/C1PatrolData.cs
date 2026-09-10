using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;

namespace Tide.App
{
    public static class C1PatrolData
    {
        // A separate authored packet: generated T0 tables and their receipt remain byte-identical.
        public static T0Definition Attach(T0Definition t0,JObject packet)
        {
            Require((int?)packet["schemaVersion"]==1 && (string)packet["contractId"]=="c1-patrol-contract");
            Require((string)packet["scope"]?["beatId"]==C1PatrolDefinition.BeatId
                && (string)packet["scope"]?["checkpointId"]==C1PatrolDefinition.CheckpointId
                && (bool?)packet["scope"]?["fullChapter"]==false
                && (string)packet["authority"]?["decisionId"]=="RFC-CX-004");
            Require(packet["entry"]?["requiredCompletedBeatIds"]?.Values<string>().SequenceEqual(new[]{"t0-b3"})==true);
            var observations=(JArray)packet["observations"];
            Require(observations!=null && observations.Count==2);
            var log=observations.Single(o=>(string)o["id"]=="c1-b1-c1");
            var plate=observations.Single(o=>(string)o["id"]=="c1-b1-c2");
            Require((string)log["sourceType"]=="log" && (string)log["originId"]=="watchlog-bureau"
                && (string)plate["sourceType"]=="plate" && (string)plate["originId"]=="brine-log-gate3"
                && observations.All(o=>o["copiedFrom"]?.Type==JTokenType.Null && (string)o["rootOriginId"]==(string)o["originId"]));
            var bypass=packet["recovery"]?["freeBypass"];
            Require(new[]{"grantsObservation","acknowledgesCondition","grantsGateAccess","recordsCondition","completesBeat","commitsWorld"}.All(k=>(bool?)bypass?[k]==false));
            Require((bool?)packet["preview"]?["worldMutationAllowed"]==false && (bool?)packet["preview"]?["conditionAcknowledgementIsCandidateOnly"]==true);
            var initial=packet["preview"]?["initialConfiguration"];
            var safe=bypass?["targetPreview"];
            Require(initial?["lightingEnabled"]?.Type==JTokenType.Boolean && initial?["readerEnabled"]?.Type==JTokenType.Boolean);
            var configs=((JArray)packet["preview"]["combinations"]).Select(c=>new PatrolConfiguration(
                (bool)c["lightingEnabled"],(bool)c["readerEnabled"],(bool)c["valid"],(string)c["reasonKey"])).ToArray();
            var patrol=new C1PatrolDefinition(observations.Select(o=>(string)o["id"]),(string)packet["journalCondition"]["id"],
                configs,(bool)initial["lightingEnabled"],(bool)initial["readerEnabled"],(bool)safe["lightingEnabled"],(bool)safe["readerEnabled"]);
            Require(patrol.Configuration(false,true).Valid && !patrol.Configuration(true,true).Valid);
            var beat=new BeatDefinition(C1PatrolDefinition.BeatId,new[]{"t0-b3"},
                new[]{new CompletionRequirement("patrolAccessGranted",id:patrol.ConditionId)});
            return new T0Definition(t0.Records.Values,t0.Beats.Concat(new[]{beat}),t0.UncoveredAreas,t0.SystemIds,
                t0.StubToolIds,t0.ReadBudget,t0.ResolutionMinutes,t0.Overlay,patrol);
        }
        static void Require(bool valid){if(!valid)throw new InvalidOperationException("C1_PATROL_CONTRACT_INVALID");}
    }
}
