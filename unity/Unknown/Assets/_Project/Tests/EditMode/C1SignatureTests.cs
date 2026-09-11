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
    public sealed class C1SignatureTests
    {
        T0Definition data;T0Simulation sim;JObject packet;byte[] prior;string directory;
        [SetUp] public void Setup()
        {
            packet=JObject.Parse(Resources.Load<TextAsset>("C1SignatureContract").text);
            data=C1SignatureData.Attach(C1PatrolData.Attach(Resources.Load<T0RuntimeConfig>("T0Runtime").catalog.Load(),JObject.Parse(Resources.Load<TextAsset>("C1PatrolContract").text)),packet);sim=new T0Simulation(data);
            prior=File.ReadAllBytes(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"));directory=Path.Combine(Path.GetTempPath(),"c1-signature-edit-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
        }
        [TearDown] public void TearDown(){Directory.Delete(directory,true);}
        CommandJournal Prior()=>JournalSave.Decode(SaveCodec.Decode(System.Text.Encoding.UTF8.GetString(prior)),sim);
        void Do(CommandJournal j,string id,string subject=null,string value=null){var v=j.Submit(new PuzzleCommand(id,subject,value));Assert.IsTrue(v.IsValid,v.DataDiagnostic);}
        CommandJournal Enter(){var j=Prior();Do(j,"EnterSignature");return j;}
        void Copies(CommandJournal j){Do(j,"ObserveSignature",data.Signature.LeftClue);Do(j,"SetSignatureHumidity","low");Do(j,"TrialSignature");Do(j,"SeparateSignature");foreach(var c in data.Signature.Copies)Do(j,"CopySignature",c);}
        void Ready(CommandJournal j){Copies(j);Do(j,"ObserveSignature",data.Signature.RightClue);Do(j,"MarkSignature",data.Signature.RegionId);Do(j,"CompareSignature",data.Signature.ComparisonId);Do(j,"SelectSignatureProof",data.Signature.LeftClue,data.Signature.RightClue);}
        PuzzleCommand Confirm()=>new PuzzleCommand("ConfirmSignature",data.Signature.ComparisonId,data.Signature.RegionId);
        [Test] public void ActualPriorV2SaveMigratesWithoutChangingReplayIdentityOrOriginalBytes()
        {
            File.WriteAllBytes(Path.Combine(directory,"save.json"),prior);var before=SaveCodec.Decode(System.Text.Encoding.UTF8.GetString(prior));var hash=Prior().State.StateHash;
            var load=new AtomicSaveStore(directory).Load(validate:d=>JournalSave.Decode(d,sim));Assert.IsTrue(load.Migrated);Assert.AreEqual(3,(int)load.Document["schemaVersion"]);
            foreach(var key in new[]{"commandLog","progress","saveId","createdUtc"})Assert.IsTrue(JToken.DeepEquals(before[key],load.Document[key]),key);
            Assert.AreEqual(hash,JournalSave.Decode(load.Document,sim).State.StateHash);CollectionAssert.AreEqual(prior,File.ReadAllBytes(Path.Combine(directory,"save.json")));CollectionAssert.AreEqual(prior,File.ReadAllBytes(Path.Combine(directory,"save.json.v2.bak")));
        }
        [Test] public void EntryRequiresPatrolAndDoesNotCompleteOrGrantCopies()
        {
            Assert.IsFalse(sim.Validate(new PuzzleState(),new PuzzleCommand("EnterSignature")).IsValid);var j=Enter();Assert.IsTrue(sim.IsComplete(j.State,"c1-b1"));Assert.IsFalse(sim.IsComplete(j.State,"c1-b2"));Assert.IsFalse(data.Signature.AllCopied(j.State));Assert.IsNull(C1SignatureDefinition.Get(j.State,"humidity"));
        }
        [Test] public void TrialRequiresObservationAndExplicitChoiceAndSelectionInvalidatesTrial()
        {
            var j=Enter();Assert.IsFalse(j.Submit(new PuzzleCommand("TrialSignature")).IsValid);Do(j,"ObserveSignature",data.Signature.LeftClue);Assert.IsFalse(j.Submit(new PuzzleCommand("TrialSignature")).IsValid);
            Do(j,"SetSignatureHumidity","low");Assert.IsFalse(j.Submit(new PuzzleCommand("SeparateSignature")).IsValid);Do(j,"TrialSignature");Assert.IsTrue(data.Signature.SafeTrial(j.State));Do(j,"SetSignatureHumidity","high");Assert.IsFalse(data.Signature.SafeTrial(j.State));Assert.IsNull(C1SignatureDefinition.Get(j.State,"trial"));
        }
        [Test] public void RiskTrialsNeverDamageSourceAndResetNeverGrantsEvidence()
        {
            var j=Enter();Do(j,"ObserveSignature",data.Signature.LeftClue);foreach(var level in new[]{"medium","high"}){Do(j,"SetSignatureHumidity",level);Do(j,"TrialSignature");Assert.AreEqual("risk",C1SignatureDefinition.Get(j.State,"trial"));Assert.IsFalse(j.Submit(new PuzzleCommand("SeparateSignature")).IsValid);}
            Do(j,"ResetSignatureTrial");Assert.IsTrue(C1SignatureDefinition.Has(j.State,"backup:"+data.Signature.LeftClue));Assert.IsFalse(data.Signature.AllCopied(j.State));Assert.IsFalse(data.Signature.Ready(j.State));Assert.IsFalse(j.State.Has("checkpoint:cp-c1-b2"));
        }
        [Test] public void CopiesAreExplicitIdempotentAndCannotFormIndependentProof()
        {
            var j=Enter();Copies(j);Do(j,"CopySignature",data.Signature.Copies[0]);Assert.AreEqual(2,j.State.FactSnapshot.Count(f=>f.StartsWith(C1SignatureDefinition.Prefix+"copy:")));
            Assert.IsFalse(j.Submit(new PuzzleCommand("SelectSignatureProof",data.Signature.Copies[0],data.Signature.Copies[1])).IsValid);Assert.IsFalse(j.Submit(Confirm()).IsValid);
        }
        [Test] public void ConfirmationNeedsMaskComparisonAndExplicitDistinctSourceProof()
        {
            var j=Enter();Copies(j);Do(j,"ObserveSignature",data.Signature.RightClue);Assert.IsFalse(j.Submit(Confirm()).IsValid);Do(j,"CompareSignature",data.Signature.ComparisonId);Do(j,"SelectSignatureProof",data.Signature.LeftClue,data.Signature.RightClue);Assert.IsFalse(j.Submit(Confirm()).IsValid);Do(j,"MarkSignature",data.Signature.RegionId);Assert.IsTrue(sim.Validate(j.State,Confirm()).IsValid);
        }
        [Test] public void AtomicCompletionUndoRedoReplayAndResetPreserveUnresolvedRegion()
        {
            var j=Enter();Ready(j);Do(j,"ResetSignatureTrial");Assert.IsTrue(data.Signature.Ready(j.State));var priorHash=j.State.StateHash;Assert.IsTrue(j.Submit(Confirm()).IsValid);var complete=j.State.StateHash;Assert.IsTrue(sim.IsComplete(j.State,"c1-b2"));Assert.IsFalse(j.Submit(Confirm()).IsValid);
            Assert.IsTrue(j.Undo());Assert.AreEqual(priorHash,j.State.StateHash);Assert.IsFalse(j.State.Has("checkpoint:cp-c1-b2"));Assert.IsTrue(j.Redo());Assert.AreEqual(complete,j.State.StateHash);
            var replay=JournalSave.Decode(JournalSave.Encode(j,"signature",20000,6291456),sim);Assert.AreEqual(complete,replay.State.StateHash);Assert.IsTrue(C1SignatureDefinition.Has(replay.State,"regionFiled:"+data.Signature.RegionId));
        }
        [Test] public void UnsupportedIdsCannotMutatePrerequisiteReadyState()
        {
            var j=Enter();Ready(j);var hash=j.State.StateHash;var head=j.HeadSeq;
            foreach(var c in new[]{new PuzzleCommand("SetSignatureHumidity","unknown"),new PuzzleCommand("ObserveSignature","unknown"),new PuzzleCommand("CopySignature","unknown"),new PuzzleCommand("MarkSignature","unknown"),new PuzzleCommand("CompareSignature","unknown"),new PuzzleCommand("SelectSignatureProof","unknown",data.Signature.RightClue),new PuzzleCommand("SelectSignatureProof",data.Signature.LeftClue,"unknown")})
            {Assert.IsFalse(j.Submit(c).IsValid,c.CommandId);Assert.AreEqual(hash,j.State.StateHash);Assert.AreEqual(head,j.HeadSeq);}
        }
        [Test] public void PacketRejectsSameRootProofAndResolvableCopySelfReference()
        {
            var bad=(JObject)packet.DeepClone();bad["observations"][1]["rootOriginId"]="signature-annex";Assert.Throws<InvalidOperationException>(()=>C1SignatureData.Attach(C1PatrolData.Attach(Resources.Load<T0RuntimeConfig>("T0Runtime").catalog.Load(),JObject.Parse(Resources.Load<TextAsset>("C1PatrolContract").text)),bad));
            bad=(JObject)packet.DeepClone();bad["copies"]["records"][0]["copiedFrom"]="c1-b2-copy-1";Assert.Throws<InvalidOperationException>(()=>C1SignatureData.Attach(data,bad));
            bad=(JObject)packet.DeepClone();bad["copies"]["records"][0]["rootOriginId"]="different-root";Assert.Throws<InvalidOperationException>(()=>C1SignatureData.Attach(data,bad));
        }
    }
}
#endif
