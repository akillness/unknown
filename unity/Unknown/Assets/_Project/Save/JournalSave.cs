using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;
namespace Tide.Save {
 public static class JournalSave {
  static JArray Entries(CommandJournal journal)=>new JArray(journal.Entries.Select(e=>{
   var payload=new JObject{["subjectId"]=e.Command.SubjectId,["value"]=e.Command.Value,["otherValue"]=e.Command.OtherValue};
   return new JObject{["seq"]=e.Seq,["parentSeq"]=e.ParentSeq,["branchId"]=e.BranchId,["commandId"]=e.Command.CommandId,["payload"]=payload,["payloadHash"]=SaveCodec.Hash(SaveCodec.Canonical(payload)),["committed"]=true};}));
  static string ChainHash(JArray entries){var hash="";foreach(var e in entries)hash=SaveCodec.Hash(hash+SaveCodec.Canonical(e));return hash;}
  public static JObject Encode(CommandJournal journal,string key,int entryCap,int byteCap,string saveId=null,string createdUtc=null){
   var entries=Entries(journal);
   while(entries.Count>entryCap||System.Text.Encoding.UTF8.GetByteCount(SaveCodec.Canonical(entries))>byteCap){if(!journal.FoldOldest())throw new InvalidOperationException("LOG_CAP_EXCEEDED");entries=Entries(journal);}
   var snapshots=new JArray(journal.Snapshots.Select(s=>new JObject{["seq"]=s.Seq,["stateHash"]=s.State.StateHash,["stateBlob"]=new JObject{["facts"]=new JArray(s.State.FactSnapshot),["values"]=JObject.FromObject(s.State.ValueSnapshot),["readCounts"]=JObject.FromObject(s.State.ReadCounts)}}));
   return new JObject{["schemaVersion"]=3,["saveId"]=saveId??Guid.NewGuid().ToString(),["createdUtc"]=createdUtc??DateTime.UtcNow.ToString("O"),["stageId"]=journal.State.Has("c1:entered")?"C1":"T0",["beatId"]=C1SignatureDefinition.Has(journal.State,"entered")?C1SignatureDefinition.BeatId:journal.State.Has("c1:entered")?C1PatrolDefinition.BeatId:"t0-b1",["storyClock"]="21:00",["commitIdempotencyKey"]=key,["settingsRef"]="settings.json",
    ["progress"]=new JObject{["autoKeptClueIds"]=new JArray(journal.State.AutoKeptClues),["discoveredClueIds"]=new JArray(),["readCounts"]=JObject.FromObject(journal.State.ReadCounts),["bypassUsed"]=new JArray(journal.State.Has("c1:bypassPreview")?new[]{C1PatrolDefinition.BeatId}:Array.Empty<string>()),["alignedPairs"]=new JArray(),["committedRouting"]=null,["propertyProtection"]=null,["sealedConclusions"]=new JArray(),["submissionPerspective"]=null,["hintLevelUsed"]=new JObject(),["checkpoints"]=new JArray(new[]{C1PatrolDefinition.CheckpointId,C1SignatureDefinition.CheckpointId}.Where(id=>journal.State.Has("checkpoint:"+id))),["dlcFlags"]=new JObject()},
    ["commandLog"]=new JObject{["headSeq"]=journal.HeadSeq,["branchId"]=journal.BranchId,["entries"]=entries,["snapshots"]=snapshots,["logHash"]=ChainHash(entries),["sandboxDiscardedCount"]=0,["entryCap"]=entryCap,["byteCap"]=byteCap}};
  }
  public static CommandJournal Decode(JObject doc,T0Simulation simulation,int snapshotInterval=200){
   var log=(JObject)doc["commandLog"]??throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");
   var entries=(JArray)log["entries"]??throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");if((string)log["logHash"]!=ChainHash(entries))throw new InvalidOperationException("REPLAY_HASH_MISMATCH");
   var decoded=entries.Select(e=>{var payload=(JObject)e["payload"]??throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");if((string)e["payloadHash"]!=SaveCodec.Hash(SaveCodec.Canonical(payload)))throw new InvalidOperationException("REPLAY_HASH_MISMATCH");return new JournalEntry((long)e["seq"],(long)e["parentSeq"],(string)e["branchId"],new PuzzleCommand((string)e["commandId"],(string)payload["subjectId"],(string)payload["value"],(string)payload["otherValue"]));}).ToArray();
   var snapshots=((JArray)log["snapshots"]).Select(s=>{var blob=(JObject)s["stateBlob"];var state=PuzzleState.FromSnapshot(blob["facts"].Values<string>(),blob["values"].ToObject<Dictionary<string,string>>(),blob["readCounts"].ToObject<Dictionary<string,int>>());if(state.StateHash!=(string)s["stateHash"])throw new InvalidOperationException("REPLAY_HASH_MISMATCH");return new JournalSnapshot((long)s["seq"],state);}).ToArray();
   var journal=new CommandJournal(simulation,snapshotInterval);journal.Restore(decoded,(long)log["headSeq"],(string)log["branchId"],doc["progress"]?["autoKeptClueIds"]?.Values<string>(),snapshots);
   if(!JToken.DeepEquals(JObject.FromObject(journal.State.ReadCounts),doc["progress"]?["readCounts"]))throw new InvalidOperationException("REPLAY_HASH_MISMATCH");return journal;
  }
 }
}
