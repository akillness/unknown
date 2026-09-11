using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;

namespace Tide.App
{
    public static class C1SignatureData
    {
        public static T0Definition Attach(T0Definition prior,JObject packet)
        {
            Require((int?)packet["schemaVersion"]==1&&(string)packet["contractId"]=="c1-signature-contract"
                &&(string)packet["scope"]?["beatId"]==C1SignatureDefinition.BeatId
                &&(string)packet["scope"]?["checkpointId"]==C1SignatureDefinition.CheckpointId
                &&(string)packet["authority"]?["decisionId"]=="RFC-CX-005");
            var observations=(JArray)packet["observations"];var copies=(JArray)packet["copies"]?["records"];
            Require(observations?.Count==2&&copies?.Count==2);
            var left=observations.Single(o=>(string)o["id"]=="c1-b2-c1");var right=observations.Single(o=>(string)o["id"]=="c1-b2-c2");
            Require((string)left["sourceType"]=="log"&&(string)left["rootOriginId"]=="signature-annex"
                &&(string)right["sourceType"]=="plate"&&(string)right["rootOriginId"]=="plate-zero"
                &&observations.All(o=>(string)o["originId"]==(string)o["rootOriginId"]&&o["copiedFrom"]?.Type==JTokenType.Null)
                &&copies.All(o=>(string)o["originId"]==(string)o["id"]&&(string)o["sourceType"]=="log"&&(string)o["rootOriginId"]=="signature-annex"&&(string)o["copiedFrom"]=="signature-annex"));
            Require((bool?)packet["confirmation"]?["saveSuccessRequired"]==true&&(bool?)packet["confirmation"]?["atomic"]==true
                &&(bool?)packet["confirmation"]?["immediateSubmissionMayCommit"]==false
                &&(bool?)packet["obscuredRegion"]?["restoreTextAllowed"]==false
                &&(bool?)packet["obscuredRegion"]?["remainsObscuredAfterSeparation"]==true
                &&(bool?)packet["proof"]?["sameRootPairAccepted"]==false&&(bool?)packet["proof"]?["sameTypePairAccepted"]==false);
            var signature=new C1SignatureDefinition(observations.Select(o=>(string)o["id"]),copies.Select(o=>(string)o["id"]),
                packet["humidity"]["levels"].Select(h=>new SignatureHumidity((string)h["id"],(bool)h["safeSeparation"])),
                (string)packet["obscuredRegion"]["id"],(string)packet["comparison"]["id"],(string)left["id"],(string)right["id"]);
            var beat=new BeatDefinition(C1SignatureDefinition.BeatId,new[]{C1PatrolDefinition.BeatId},new[]{new CompletionRequirement("signatureFiled")});
            return new T0Definition(prior.Records.Values,prior.Beats.Concat(new[]{beat}),prior.UncoveredAreas,prior.SystemIds,
                prior.StubToolIds,prior.ReadBudget,prior.ResolutionMinutes,prior.Overlay,prior.Patrol,signature);
        }
        static void Require(bool value){if(!value)throw new InvalidOperationException("C1_SIGNATURE_CONTRACT_INVALID");}
    }
}
