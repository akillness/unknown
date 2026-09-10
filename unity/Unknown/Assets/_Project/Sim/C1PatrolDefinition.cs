using System;
using System.Collections.Generic;
using System.Linq;

namespace Tide.Sim
{
    // RFC-CX-004: C1 preview never shares T0's circuit keys or command meanings.
    public sealed class PatrolConfiguration
    {
        public bool Lighting { get; }
        public bool Reader { get; }
        public bool Valid { get; }
        public string ReasonKey { get; }
        public PatrolConfiguration(bool lighting, bool reader, bool valid, string reasonKey)
        { Lighting=lighting; Reader=reader; Valid=valid; ReasonKey=reasonKey; }
    }

    public sealed class C1PatrolDefinition
    {
        public const string BeatId="c1-b1";
        public const string CheckpointId="cp-c1-b1";
        public IReadOnlyList<string> Observations { get; }
        public IReadOnlyList<PatrolConfiguration> Configurations { get; }
        public string ConditionId { get; }
        public bool InitialLighting { get; }
        public bool InitialReader { get; }
        public bool RecoveryLighting { get; }
        public bool RecoveryReader { get; }
        public C1PatrolDefinition(IEnumerable<string> observations, string conditionId,
            IEnumerable<PatrolConfiguration> configurations, bool initialLighting, bool initialReader,
            bool recoveryLighting, bool recoveryReader)
        {
            Observations=RecordDefinition.Freeze(observations); ConditionId=conditionId;
            Configurations=Array.AsReadOnly(configurations.ToArray());
            InitialLighting=initialLighting; InitialReader=initialReader;
            RecoveryLighting=recoveryLighting; RecoveryReader=recoveryReader;
            if(Observations.Count!=2 || Observations.Distinct().Count()!=2 || string.IsNullOrEmpty(conditionId)
                || Configurations.Count!=4 || Configurations.Select(c=>c.Lighting+":"+c.Reader).Distinct().Count()!=4
                || Configurations.Count(c=>c.Valid)!=1 || !Configuration(recoveryLighting,recoveryReader).Valid)
                throw new InvalidOperationException("C1_PATROL_CONTRACT_INVALID");
        }
        public PatrolConfiguration Configuration(bool lighting,bool reader) =>
            Configurations.Single(c=>c.Lighting==lighting && c.Reader==reader);
        public static string Boolean(bool value)=>value?"true":"false";
        public static bool Enabled(PuzzleState state,string branch)=>state.Get("c1:preview:"+branch)=="true";
        public ValidationResult Validate(PuzzleState state,PuzzleCommand command,bool t0Complete)
        {
            if(!t0Complete)return ValidationResult.InvalidData("c1.patrol.blocked.t0Incomplete");
            if(command.CommandId=="EnterPatrol")return Check(!state.Has("c1:entered"),"C1_ALREADY_ENTERED");
            if(!state.Has("c1:entered") || state.Has("c1:gateAccess"))return ValidationResult.InvalidData("C1_NOT_EDITABLE");
            switch(command.CommandId)
            {
                case "ObservePatrol": return Check(Observations.Contains(command.SubjectId),"C1_UNKNOWN_OBSERVATION");
                case "SetPatrolBranch": return Check((command.SubjectId=="lighting" || command.SubjectId=="reader")
                    && (command.Value=="true" || command.Value=="false"),"C1_INVALID_BRANCH");
                case "AcknowledgePatrol": return Check(command.SubjectId==ConditionId && (command.Value=="true" || command.Value=="false"),"C1_INVALID_CONDITION");
                case "RecoverPatrol": return ValidationResult.Success();
                case "ConfirmPatrol":
                    if(!Observations.All(id=>state.Has("c1:observed:"+id)))return ValidationResult.InvalidData("c1.patrol.blocked.observations");
                    var config=Configuration(Enabled(state,"lighting"),Enabled(state,"reader"));
                    if(!config.Valid)return ValidationResult.InvalidData(config.ReasonKey);
                    if(command.SubjectId!=ConditionId || state.Get("c1:conditionCandidate")!=ConditionId)
                        return ValidationResult.InvalidData("c1.patrol.blocked.condition");
                    return Check(command.Value==Boolean(config.Lighting) && command.OtherValue==Boolean(config.Reader),"C1_STALE_PREVIEW");
                default: return ValidationResult.InvalidData("C1_UNKNOWN_COMMAND");
            }
        }
        static ValidationResult Check(bool valid,string diagnostic)=>valid?ValidationResult.Success():ValidationResult.InvalidData(diagnostic);
        public static bool Handles(string command)=>command=="EnterPatrol" || command=="ObservePatrol" || command=="SetPatrolBranch"
            || command=="AcknowledgePatrol" || command=="RecoverPatrol" || command=="ConfirmPatrol";
        public PuzzleCommand Materialize(PuzzleCommand command)
        {
            if(command.CommandId=="EnterPatrol")return new PuzzleCommand(command.CommandId,value:Boolean(InitialLighting),otherValue:Boolean(InitialReader));
            if(command.CommandId=="RecoverPatrol")return new PuzzleCommand(command.CommandId,value:Boolean(RecoveryLighting),otherValue:Boolean(RecoveryReader));
            return command;
        }
        internal static void Reduce(PuzzleCommand command,HashSet<string> facts,Dictionary<string,string> values)
        {
            switch(command.CommandId)
            {
                case "EnterPatrol": facts.Add("c1:entered"); goto case "RecoverPatrol";
                case "RecoverPatrol":
                    values["c1:preview:lighting"]=command.Value; values["c1:preview:reader"]=command.OtherValue;
                    if(command.CommandId=="RecoverPatrol")facts.Add("c1:bypassPreview");
                    break;
                case "ObservePatrol": facts.Add("c1:observed:"+command.SubjectId); break;
                case "SetPatrolBranch": values["c1:preview:"+command.SubjectId]=command.Value; break;
                case "AcknowledgePatrol":
                    if(command.Value=="true")values["c1:conditionCandidate"]=command.SubjectId;
                    else values.Remove("c1:conditionCandidate");
                    break;
                case "ConfirmPatrol":
                    values["c1:committed:lighting"]=command.Value; values["c1:committed:reader"]=command.OtherValue;
                    facts.Add("c1:journal:"+command.SubjectId); facts.Add("c1:gateAccess"); facts.Add("checkpoint:"+CheckpointId);
                    break;
            }
        }
    }
}
