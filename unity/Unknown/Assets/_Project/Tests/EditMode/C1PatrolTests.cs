#if TIDE_TEST_FRAMEWORK
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using Tide.Save;
using Tide.Sim;
using UnityEngine;

namespace Tide.Tests
{
    public sealed class C1PatrolTests
    {
        string directory;
        T0Definition definition;
        T0Simulation simulation;
        byte[] legacy;
        JObject packet;
        [SetUp] public void Setup()
        {
            directory=Path.Combine(Path.GetTempPath(),"c1-edit-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
            packet=JObject.Parse(Resources.Load<TextAsset>("C1PatrolContract").text);
            definition=C1PatrolData.Attach(Resources.Load<T0RuntimeConfig>("T0Runtime").catalog.Load(),packet);
            simulation=new T0Simulation(definition);
            legacy=File.ReadAllBytes(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/T0CompletedV1.json"));
        }
        [TearDown] public void Teardown(){Directory.Delete(directory,true);}
        CommandJournal T0()=>JournalSave.Decode(SaveCodec.Decode(System.Text.Encoding.UTF8.GetString(legacy)),simulation);
        CommandJournal Entered(){var journal=T0();Submit(journal,new PuzzleCommand("EnterPatrol"));return journal;}
        void Submit(CommandJournal journal,PuzzleCommand command){var v=journal.Submit(command);Assert.IsTrue(v.IsValid,v.DataDiagnostic);}
        void Observe(CommandJournal journal){foreach(var id in definition.Patrol.Observations)Submit(journal,new PuzzleCommand("ObservePatrol",id));}
        PuzzleCommand Confirm()=>new PuzzleCommand("ConfirmPatrol",definition.Patrol.ConditionId,"false","true");
        void Ready(CommandJournal journal){Observe(journal);Submit(journal,new PuzzleCommand("RecoverPatrol"));Submit(journal,new PuzzleCommand("AcknowledgePatrol",definition.Patrol.ConditionId,"true"));}

        [Test] public void LegacyPhysicalPlayerSaveMigratesWithoutChangingCommandsOrIdentity()
        {
            var path=Path.Combine(directory,"save.json");File.WriteAllBytes(path,legacy);
            var before=SaveCodec.Decode(System.Text.Encoding.UTF8.GetString(legacy));var state=T0().State.StateHash;
            var loaded=new AtomicSaveStore(directory).Load(validate:d=>JournalSave.Decode(d,simulation));
            Assert.IsTrue(loaded.Migrated);Assert.AreEqual(3,(int)loaded.Document["schemaVersion"]);
            Assert.IsTrue(JToken.DeepEquals(before["commandLog"],loaded.Document["commandLog"]));
            Assert.IsTrue(JToken.DeepEquals(before["progress"],loaded.Document["progress"]));
            Assert.AreEqual((string)before["saveId"],(string)loaded.Document["saveId"]);
            Assert.AreEqual((string)before["createdUtc"],(string)loaded.Document["createdUtc"]);
            Assert.AreEqual(state,JournalSave.Decode(loaded.Document,simulation).State.StateHash);
            CollectionAssert.AreEqual(legacy,File.ReadAllBytes(path));CollectionAssert.AreEqual(legacy,File.ReadAllBytes(path+".v1.bak"));
        }
        [Test] public void FutureSchemaRefusesBeforeBackupFallback()
        {
            var store=new AtomicSaveStore(directory);var valid=JournalSave.Encode(T0(),"valid",20000,6291456);
            store.WriteAsync(valid).GetAwaiter().GetResult();var future=(JObject)valid.DeepClone();future["schemaVersion"]=4;future["commitIdempotencyKey"]="future";
            store.WriteAsync(future).GetAwaiter().GetResult();File.WriteAllBytes(Path.Combine(directory,"save.bak"),legacy);var raw=File.ReadAllBytes(Path.Combine(directory,"save.json"));
            Assert.AreEqual("SAVE_VERSION_REFUSED",store.Load().Failure);CollectionAssert.AreEqual(raw,File.ReadAllBytes(Path.Combine(directory,"save.json")));CollectionAssert.AreEqual(legacy,File.ReadAllBytes(Path.Combine(directory,"save.bak")));
        }
        [Test] public void EntryRequiresCompletedT0AndDoesNotCompleteC1()
        {
            Assert.IsFalse(simulation.Validate(new PuzzleState(),new PuzzleCommand("EnterPatrol")).IsValid);
            var journal=T0();Assert.IsTrue(simulation.IsComplete(journal.State,"t0-b3"));Assert.IsFalse(simulation.IsComplete(journal.State,C1PatrolDefinition.BeatId));
            var prior=journal.State;Submit(journal,new PuzzleCommand("EnterPatrol"));
            Assert.IsTrue(journal.State.Has("c1:entered"));Assert.IsFalse(simulation.IsComplete(journal.State,C1PatrolDefinition.BeatId));
            foreach(var value in prior.ValueSnapshot)Assert.AreEqual(value.Value,journal.State.Get(value.Key));
            CollectionAssert.IsSubsetOf(prior.FactSnapshot,journal.State.FactSnapshot);
            Assert.IsTrue(C1PatrolDefinition.Enabled(journal.State,"lighting"));Assert.IsTrue(C1PatrolDefinition.Enabled(journal.State,"reader"));
        }
        [Test] public void EveryInvalidConfigurationAndMissingRequirementRejectsWithoutWorldEffects()
        {
            var journal=Entered();Assert.IsFalse(journal.Submit(Confirm()).IsValid);Observe(journal);
            Submit(journal,new PuzzleCommand("AcknowledgePatrol",definition.Patrol.ConditionId,"true"));
            foreach(var config in definition.Patrol.Configurations.Where(c=>!c.Valid))
            {
                Submit(journal,new PuzzleCommand("SetPatrolBranch","lighting",C1PatrolDefinition.Boolean(config.Lighting)));
                Submit(journal,new PuzzleCommand("SetPatrolBranch","reader",C1PatrolDefinition.Boolean(config.Reader)));
                var before=journal.State.StateHash;Assert.IsFalse(journal.Submit(Confirm()).IsValid);Assert.AreEqual(before,journal.State.StateHash);
            }
            Submit(journal,new PuzzleCommand("RecoverPatrol"));Submit(journal,new PuzzleCommand("AcknowledgePatrol",definition.Patrol.ConditionId,"false"));
            Assert.IsFalse(journal.Submit(Confirm()).IsValid);Assert.IsFalse(journal.State.Has("c1:gateAccess"));
        }
        [Test] public void FreeBypassOnlyRecoversPreviewAndPreservesAlreadyObservedClue()
        {
            var journal=Entered();var clue=definition.Patrol.Observations[0];Submit(journal,new PuzzleCommand("ObservePatrol",clue));
            Submit(journal,new PuzzleCommand("RecoverPatrol"));Submit(journal,new PuzzleCommand("RecoverPatrol"));
            Assert.IsTrue(journal.State.Has("c1:observed:"+clue));Assert.IsFalse(journal.State.Has("c1:observed:"+definition.Patrol.Observations[1]));
            Assert.IsNull(journal.State.Get("c1:conditionCandidate"));Assert.IsFalse(journal.State.Has("c1:gateAccess"));
            Assert.IsFalse(journal.State.Has("c1:journal:"+definition.Patrol.ConditionId));Assert.IsFalse(journal.State.Has("checkpoint:"+C1PatrolDefinition.CheckpointId));
            Assert.IsFalse(simulation.IsComplete(journal.State,C1PatrolDefinition.BeatId));
        }
        [Test] public void ConfirmationIsOneReplayableUndoableChapterTransaction()
        {
            var journal=Entered();Ready(journal);var before=journal.State.StateHash;Submit(journal,Confirm());
            Assert.AreEqual("false",journal.State.Get("c1:committed:lighting"));Assert.AreEqual("true",journal.State.Get("c1:committed:reader"));
            Assert.IsTrue(journal.State.Has("c1:journal:"+definition.Patrol.ConditionId));Assert.IsTrue(journal.State.Has("c1:gateAccess"));
            Assert.IsTrue(journal.State.Has("checkpoint:"+C1PatrolDefinition.CheckpointId));Assert.IsTrue(simulation.IsComplete(journal.State,C1PatrolDefinition.BeatId));
            var final=journal.State.StateHash;Assert.IsTrue(journal.Undo());Assert.AreEqual(before,journal.State.StateHash);
            Assert.IsTrue(journal.Redo());Assert.AreEqual(final,journal.State.StateHash);
            var encoded=JournalSave.Encode(journal,"c1",20000,6291456);Assert.AreEqual("C1",(string)encoded["stageId"]);
            Assert.AreEqual(final,JournalSave.Decode(encoded,simulation).State.StateHash);Assert.IsFalse(journal.Submit(Confirm()).IsValid);
        }
        [Test] public void PacketRejectsMissingInitializationAndForbiddenBypassGrants()
        {
            var original=Resources.Load<T0RuntimeConfig>("T0Runtime").catalog.Load();var broken=(JObject)packet.DeepClone();
            broken["preview"]["initialConfiguration"]=null;Assert.Throws<InvalidOperationException>(()=>C1PatrolData.Attach(original,broken));
            broken=(JObject)packet.DeepClone();broken["recovery"]["freeBypass"]["completesBeat"]=true;
            Assert.Throws<InvalidOperationException>(()=>C1PatrolData.Attach(original,broken));
        }
        [Test] public void EitherMissingClueBlocksAnOtherwiseReadyConfirmation()
        {
            foreach(var missing in definition.Patrol.Observations)
            {
                var journal=Entered();foreach(var id in definition.Patrol.Observations.Where(id=>id!=missing))Submit(journal,new PuzzleCommand("ObservePatrol",id));
                Submit(journal,new PuzzleCommand("RecoverPatrol"));Submit(journal,new PuzzleCommand("AcknowledgePatrol",definition.Patrol.ConditionId,"true"));
                var before=journal.State.StateHash;Assert.IsFalse(journal.Submit(Confirm()).IsValid);Assert.AreEqual(before,journal.State.StateHash);
            }
        }
        [Test] public void EveryInvalidPreviewRecoversFreelyAndDoubleFoldsRestoreEachBranch()
        {
            foreach(var invalid in definition.Patrol.Configurations.Where(c=>!c.Valid))
            {
                var journal=Entered();Submit(journal,new PuzzleCommand("SetPatrolBranch","lighting",C1PatrolDefinition.Boolean(invalid.Lighting)));
                Submit(journal,new PuzzleCommand("SetPatrolBranch","reader",C1PatrolDefinition.Boolean(invalid.Reader)));
                var before=journal.State.StateHash;
                foreach(var branch in new[]{"lighting","reader"})
                {
                    bool enabled=C1PatrolDefinition.Enabled(journal.State,branch);Submit(journal,new PuzzleCommand("SetPatrolBranch",branch,C1PatrolDefinition.Boolean(!enabled)));
                    Submit(journal,new PuzzleCommand("SetPatrolBranch",branch,C1PatrolDefinition.Boolean(enabled)));Assert.AreEqual(before,journal.State.StateHash);
                }
                Submit(journal,new PuzzleCommand("RecoverPatrol"));Assert.IsFalse(C1PatrolDefinition.Enabled(journal.State,"lighting"));Assert.IsTrue(C1PatrolDefinition.Enabled(journal.State,"reader"));
                Assert.IsFalse(definition.Patrol.Observations.Any(id=>journal.State.Has("c1:observed:"+id)));
                Assert.IsNull(journal.State.Get("c1:conditionCandidate"));Assert.IsFalse(simulation.IsComplete(journal.State,C1PatrolDefinition.BeatId));
            }
        }
    }
}
#endif
