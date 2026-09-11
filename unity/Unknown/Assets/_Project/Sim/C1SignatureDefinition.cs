using System;
using System.Collections.Generic;
using System.Linq;

namespace Tide.Sim
{
    public sealed class SignatureHumidity
    {
        public string Id { get; }
        public bool Safe { get; }
        public SignatureHumidity(string id,bool safe){Id=id;Safe=safe;}
    }
    // RFC-CX-005. Only commands write state; presentation consumes these immutable snapshots.
    public sealed class C1SignatureDefinition
    {
        public const string BeatId="c1-b2",CheckpointId="cp-c1-b2",Prefix="c1:signature:";
        public IReadOnlyList<string> Observations { get; }
        public IReadOnlyList<string> Copies { get; }
        public IReadOnlyList<SignatureHumidity> Humidity { get; }
        public string RegionId { get; }
        public string ComparisonId { get; }
        public string LeftClue { get; }
        public string RightClue { get; }
        public C1SignatureDefinition(IEnumerable<string> observations,IEnumerable<string> copies,IEnumerable<SignatureHumidity> humidity,
            string region,string comparison,string left,string right)
        {
            Observations=RecordDefinition.Freeze(observations);Copies=RecordDefinition.Freeze(copies);Humidity=Array.AsReadOnly(humidity.ToArray());
            RegionId=region;ComparisonId=comparison;LeftClue=left;RightClue=right;
            if(Observations.Count!=2||Copies.Count!=2||Copies.Distinct().Count()!=2||Humidity.Count!=3||Humidity.Count(h=>h.Safe)!=1
                ||!Observations.Contains(left)||!Observations.Contains(right)||left==right||string.IsNullOrEmpty(region)||string.IsNullOrEmpty(comparison))
                throw new InvalidOperationException("C1_SIGNATURE_CONTRACT_INVALID");
        }
        public static bool Has(PuzzleState state,string key)=>state.Has(Prefix+key);
        public static string Get(PuzzleState state,string key)=>state.Get(Prefix+key);
        public bool AllObserved(PuzzleState state)=>Observations.All(id=>Has(state,"observed:"+id));
        public bool AllCopied(PuzzleState state)=>Copies.All(id=>Has(state,"copy:"+id));
        public bool SafeTrial(PuzzleState state)=>Get(state,"trial")=="safe"&&Humidity.Any(h=>h.Id==Get(state,"humidity")&&h.Safe);
        public bool Ready(PuzzleState state)=>AllObserved(state)&&AllCopied(state)&&Has(state,"separated")&&Has(state,"marked:"+RegionId)
            &&Has(state,"compared:"+ComparisonId)&&Has(state,"proofSelected");
        static ValidationResult Check(bool valid,string reason)=>valid?ValidationResult.Success():ValidationResult.InvalidData(reason);
        public ValidationResult Validate(PuzzleState state,PuzzleCommand c,bool priorComplete)
        {
            if(!priorComplete)return ValidationResult.InvalidData("c1.signature.blocked.prerequisite");
            if(c.CommandId=="EnterSignature")return Check(!Has(state,"entered"),"C1_SIGNATURE_ALREADY_ENTERED");
            if(!Has(state,"entered")||Has(state,"committed"))return ValidationResult.InvalidData("C1_SIGNATURE_NOT_EDITABLE");
            switch(c.CommandId)
            {
                case "ObserveSignature":return Check(Observations.Contains(c.SubjectId),"C1_SIGNATURE_UNKNOWN_OBSERVATION");
                case "SetSignatureHumidity":return Check(Humidity.Any(h=>h.Id==c.SubjectId),"C1_SIGNATURE_UNKNOWN_HUMIDITY");
                case "TrialSignature":return Check(Has(state,"observed:"+LeftClue)&&Humidity.Any(h=>h.Id==Get(state,"humidity")),"c1.signature.blocked.trial");
                case "SeparateSignature":return Check(Has(state,"separated")||Has(state,"backup:"+LeftClue)&&SafeTrial(state),"c1.signature.blocked.separate");
                case "CopySignature":return Check(Has(state,"separated")&&Copies.Contains(c.SubjectId),"c1.signature.blocked.copy");
                case "MarkSignature":return Check(AllCopied(state)&&c.SubjectId==RegionId,"c1.signature.blocked.mark");
                case "CompareSignature":return Check(AllObserved(state)&&AllCopied(state)&&c.SubjectId==ComparisonId,"c1.signature.blocked.compare");
                case "SelectSignatureProof":return Check(AllObserved(state)&&Has(state,"compared:"+ComparisonId)&&c.SubjectId==LeftClue&&c.Value==RightClue,"c1.signature.blocked.proof");
                case "ResetSignatureTrial":return ValidationResult.Success();
                case "ConfirmSignature":return Check(Ready(state)&&c.SubjectId==ComparisonId&&c.Value==RegionId,"c1.signature.blocked.confirm");
                default:return ValidationResult.InvalidData("C1_SIGNATURE_UNKNOWN_COMMAND");
            }
        }
        public static bool Handles(string id)=>new[]{"EnterSignature","ObserveSignature","SetSignatureHumidity","TrialSignature","SeparateSignature","CopySignature","MarkSignature","CompareSignature","SelectSignatureProof","ResetSignatureTrial","ConfirmSignature"}.Contains(id);
        public PuzzleCommand Materialize(PuzzleState state,PuzzleCommand c)=>c.CommandId=="TrialSignature"
            ?new PuzzleCommand(c.CommandId,value:Humidity.Single(h=>h.Id==Get(state,"humidity")).Safe?"safe":"risk"):c;
        internal static void Reduce(PuzzleCommand c,HashSet<string> facts,Dictionary<string,string> values)
        {
            switch(c.CommandId)
            {
                case "EnterSignature":facts.Add(Prefix+"entered");break;
                case "ObserveSignature":facts.Add(Prefix+"observed:"+c.SubjectId);facts.Add(Prefix+"backup:"+c.SubjectId);break;
                case "SetSignatureHumidity":values[Prefix+"humidity"]=c.SubjectId;values.Remove(Prefix+"trial");break;
                case "TrialSignature":values[Prefix+"trial"]=c.Value;break;
                case "SeparateSignature":facts.Add(Prefix+"separated");break;
                case "CopySignature":facts.Add(Prefix+"copy:"+c.SubjectId);break;
                case "MarkSignature":facts.Add(Prefix+"marked:"+c.SubjectId);break;
                case "CompareSignature":facts.Add(Prefix+"compared:"+c.SubjectId);break;
                case "SelectSignatureProof":facts.Add(Prefix+"proofSelected");break;
                case "ResetSignatureTrial":values.Remove(Prefix+"humidity");values.Remove(Prefix+"trial");break;
                case "ConfirmSignature":
                    facts.Add(Prefix+"committed");facts.Add(Prefix+"copiesFiled");facts.Add(Prefix+"regionFiled:"+c.Value);
                    facts.Add(Prefix+"bandFiled:"+c.SubjectId);facts.Add(Prefix+"proofFiled");facts.Add("checkpoint:"+CheckpointId);break;
            }
        }
    }
}
