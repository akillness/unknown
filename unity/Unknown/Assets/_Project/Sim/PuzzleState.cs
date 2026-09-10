using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tide.Sim
{
    // Internal collections are copied at every transition; consumers only receive read-only copies.
    public sealed class PuzzleState
    {
        internal readonly HashSet<string> Facts;
        internal readonly Dictionary<string,string> Values;
        internal readonly Dictionary<string,int> Counts;
        public bool IsSandbox { get; }
        public string LoadedRecordId => Get("loaded");
        public IReadOnlyList<string> AutoKeptClues => Array.AsReadOnly(Facts.Where(f=>f.StartsWith("kept:", StringComparison.Ordinal)).Select(f=>f.Substring(5)).OrderBy(f=>f, StringComparer.Ordinal).ToArray());
        public IReadOnlyDictionary<string,int> ReadCounts => new ReadOnlyDictionary<string,int>(new Dictionary<string,int>(Counts, StringComparer.Ordinal));
        public IReadOnlyList<string> FactSnapshot => Array.AsReadOnly(Facts.OrderBy(v=>v,StringComparer.Ordinal).ToArray());
        public IReadOnlyDictionary<string,string> ValueSnapshot => new ReadOnlyDictionary<string,string>(new Dictionary<string,string>(Values,StringComparer.Ordinal));
        public static PuzzleState FromSnapshot(IEnumerable<string> facts,IReadOnlyDictionary<string,string> values,IReadOnlyDictionary<string,int> readCounts)
        {
            if(facts==null || values==null || readCounts==null || readCounts.Any(p=>p.Value<0)) throw new InvalidOperationException("Invalid state snapshot");
            return new PuzzleState(new HashSet<string>(facts,StringComparer.Ordinal),values.ToDictionary(p=>p.Key,p=>p.Value,StringComparer.Ordinal),
                readCounts.ToDictionary(p=>p.Key,p=>p.Value,StringComparer.Ordinal),false);
        }
        public PuzzleState(bool sandbox=false) : this(new HashSet<string>(StringComparer.Ordinal),
            new Dictionary<string,string>(StringComparer.Ordinal), new Dictionary<string,int>(StringComparer.Ordinal),sandbox) {}
        internal PuzzleState(HashSet<string> facts, Dictionary<string,string> values, Dictionary<string,int> counts, bool sandbox)
        { Facts=facts; Values=values; Counts=counts; IsSandbox=sandbox; }
        public bool Has(string fact) => Facts.Contains(fact);
        public string Get(string key) => Values.TryGetValue(key,out var value)?value:null;
        public int ReadCount(string recordId) => Counts.TryGetValue(recordId,out var count)?count:0;
        internal static PuzzleState KeepClues(PuzzleState state,IEnumerable<string> clues)
        {
            var facts=new HashSet<string>(state.Facts,StringComparer.Ordinal);
            foreach(var clue in clues) facts.Add("kept:"+clue);
            return new PuzzleState(facts,new Dictionary<string,string>(state.Values,StringComparer.Ordinal),
                new Dictionary<string,int>(state.Counts,StringComparer.Ordinal),state.IsSandbox);
        }
        public string StateHash
        {
            get
            {
                var b=new StringBuilder(IsSandbox?"sandbox;":"main;");
                Action<string> add=s=>b.Append((s??"").Length.ToString(CultureInfo.InvariantCulture)).Append(':').Append(s??"");
                foreach(var fact in Facts.OrderBy(v=>v,StringComparer.Ordinal)) { b.Append('F'); add(fact); }
                foreach(var item in Values.OrderBy(v=>v.Key,StringComparer.Ordinal)) { b.Append('V'); add(item.Key); add(item.Value); }
                foreach(var item in Counts.OrderBy(v=>v.Key,StringComparer.Ordinal)) { b.Append('C'); add(item.Key); add(item.Value.ToString(CultureInfo.InvariantCulture)); }
                using(var sha=SHA256.Create())
                    return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(b.ToString()))).Replace("-","").ToLowerInvariant();
            }
        }
    }

    public sealed class PuzzleCommand
    {
        public string CommandId { get; }
        public string SubjectId { get; }
        public string Value { get; }
        public string OtherValue { get; }
        public PuzzleCommand(string commandId,string subjectId=null,string value=null,string otherValue=null)
        { CommandId=commandId; SubjectId=subjectId; Value=value; OtherValue=otherValue; }
    }

    public sealed class PuzzleEvent
    {
        public PuzzleCommand Command { get; }
        public IReadOnlyList<string> ProtectedClues { get; }
        public string CitationStart { get; }
        public string CitationEnd { get; }
        internal PuzzleEvent(PuzzleCommand command,IEnumerable<string> clues=null,string citationStart=null,string citationEnd=null)
        { Command=command; ProtectedClues=RecordDefinition.Freeze(clues); CitationStart=citationStart; CitationEnd=citationEnd; }
    }

    public sealed class ValidationResult
    {
        public bool IsValid { get; }
        public ReasonCode? Reason { get; }
        // Developer-only data diagnostics. These must not be displayed as gameplay reasons.
        public string DataDiagnostic { get; }
        private ValidationResult(bool valid,ReasonCode? reason,string diagnostic)
        { IsValid=valid; Reason=reason; DataDiagnostic=diagnostic; }
        public static ValidationResult Success() => new ValidationResult(true,null,null);
        public static ValidationResult Reject(ReasonCode reason) => new ValidationResult(false,reason,null);
        public static ValidationResult InvalidData(string diagnostic) => new ValidationResult(false,null,diagnostic);
    }

    public sealed class CommandResult
    {
        public ValidationResult Validation { get; }
        public IReadOnlyList<PuzzleEvent> Events { get; }
        internal CommandResult(ValidationResult validation,IEnumerable<PuzzleEvent> events)
        { Validation=validation; Events=Array.AsReadOnly(events.ToArray()); }
    }
}
