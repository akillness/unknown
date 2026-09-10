using System;
using System.Collections.Generic;
using System.Linq;
namespace Tide.Sim {
 public sealed class JournalEntry {
  public long Seq {get;} public long ParentSeq {get;} public string BranchId {get;} public PuzzleCommand Command {get;}
  public JournalEntry(long seq,long parentSeq,string branchId,PuzzleCommand command){Seq=seq;ParentSeq=parentSeq;BranchId=branchId;Command=command;}
 }
 public sealed class JournalSnapshot {
  public long Seq {get;} public PuzzleState State {get;}
  public JournalSnapshot(long seq,PuzzleState state){Seq=seq;State=state;}
 }
 public sealed class CommandJournal {
  readonly T0Simulation simulation; readonly int interval;
  readonly List<JournalEntry> entries=new List<JournalEntry>();
  readonly List<JournalSnapshot> snapshots=new List<JournalSnapshot>();
  readonly HashSet<string> permanentClues=new HashSet<string>(StringComparer.Ordinal);
  long nextSeq=1;
  public long HeadSeq {get;private set;} public string BranchId {get;private set;}="main";
  public PuzzleState State {get;private set;}=new PuzzleState();
  public IReadOnlyList<JournalEntry> Entries=>entries.AsReadOnly();
  public IReadOnlyList<JournalSnapshot> Snapshots=>snapshots.AsReadOnly();
  public bool CoarseUndo=>snapshots.Any(s=>s.Seq>0&&entries.All(e=>e.Seq!=s.Seq));
  public CommandJournal(T0Simulation sim,int snapshotInterval=200){simulation=sim;interval=snapshotInterval;if(interval<=0)throw new ArgumentOutOfRangeException(nameof(snapshotInterval));}
  public ValidationResult Submit(PuzzleCommand command){
   var result=simulation.Commit(State,command);if(!result.Validation.IsValid)return result.Validation;
   if(HeadSeq!=nextSeq-1)BranchId="branch-"+nextSeq;
   var entry=new JournalEntry(nextSeq++,HeadSeq,BranchId,command);entries.Add(entry);HeadSeq=entry.Seq;
   State=result.Events.Aggregate(State,T0Simulation.Reduce);PreserveClues();
   if(entry.Seq%interval==0)snapshots.Add(new JournalSnapshot(entry.Seq,State));return result.Validation;
  }
  public bool Undo(){
   if(HeadSeq==0)return false;var entry=entries.FirstOrDefault(e=>e.Seq==HeadSeq);
   HeadSeq=entry!=null?entry.ParentSeq:snapshots.Where(s=>s.Seq<HeadSeq&&entries.All(e=>e.Seq!=s.Seq)).Select(s=>s.Seq).DefaultIfEmpty(0).Max();Rebuild();return true;
  }
  public bool Redo(){
   var lookup=entries.ToDictionary(e=>e.Seq);var tip=entries.LastOrDefault(e=>e.BranchId==BranchId);JournalEntry next=null;
   while(tip!=null&&tip.Seq>HeadSeq){if(tip.ParentSeq==HeadSeq){next=tip;break;}lookup.TryGetValue(tip.ParentSeq,out tip);}
   if(next!=null)HeadSeq=next.Seq;
   else{var checkpoint=snapshots.Where(s=>s.Seq>HeadSeq&&entries.All(e=>e.Seq!=s.Seq)).OrderBy(s=>s.Seq).FirstOrDefault();if(checkpoint==null)return false;HeadSeq=checkpoint.Seq;}
   Rebuild();return true;
  }
  public CommandJournal Clone(){var c=new CommandJournal(simulation,interval);c.entries.AddRange(entries);c.snapshots.AddRange(snapshots);c.HeadSeq=HeadSeq;c.BranchId=BranchId;c.nextSeq=nextSeq;c.permanentClues.UnionWith(permanentClues);c.State=State;return c;}
  public void Restore(IEnumerable<JournalEntry> source,long head,string branch,IEnumerable<string> kept,IEnumerable<JournalSnapshot> saved=null){
   entries.Clear();entries.AddRange(source);snapshots.Clear();snapshots.AddRange(saved??Array.Empty<JournalSnapshot>());
   if(entries.Any(e=>e.Seq<=0||e.ParentSeq<0||e.ParentSeq>=e.Seq||string.IsNullOrEmpty(e.BranchId))||entries.Select(e=>e.Seq).Distinct().Count()!=entries.Count||snapshots.Select(s=>s.Seq).Distinct().Count()!=snapshots.Count||snapshots.Any(s=>s.Seq<=0))throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");
   HeadSeq=head;BranchId=branch;permanentClues.Clear();permanentClues.UnionWith(kept??Array.Empty<string>());
   nextSeq=Math.Max(entries.Select(e=>e.Seq).DefaultIfEmpty(0).Max(),snapshots.Select(s=>s.Seq).DefaultIfEmpty(0).Max())+1;
   var parents=new HashSet<long>(entries.Select(e=>e.ParentSeq));var targets=entries.Where(e=>!parents.Contains(e.Seq)).Select(e=>e.Seq);
   foreach(var seq in targets)BuildAt(seq,true);Rebuild();
  }
  public bool FoldOldest(){
   var path=ActivePath();if(path.Count<2)return false;var cut=path[Math.Min(interval,path.Count/2)-1];var state=BuildAt(cut.Seq,true);
   snapshots.RemoveAll(s=>s.Seq==cut.Seq||(s.Seq<=cut.Seq&&entries.Any(e=>e.Seq==s.Seq)&&path.All(e=>e.Seq!=s.Seq)));
   snapshots.Add(new JournalSnapshot(cut.Seq,state));snapshots.Sort((a,b)=>a.Seq.CompareTo(b.Seq));
   var valid=new HashSet<long>(snapshots.Where(s=>s.Seq<=cut.Seq).Select(s=>s.Seq)){0};entries.RemoveAll(e=>e.Seq<=cut.Seq);
   foreach(var e in entries.ToArray())if(valid.Contains(e.ParentSeq))valid.Add(e.Seq);else entries.Remove(e);
   return true;
  }
  List<JournalEntry> ActivePath(){var lookup=entries.ToDictionary(e=>e.Seq);var path=new List<JournalEntry>();long head=HeadSeq;while(lookup.TryGetValue(head,out var e)){path.Add(e);head=e.ParentSeq;}path.Reverse();return path;}
  PuzzleState BuildAt(long head,bool verify){
   var lookup=entries.ToDictionary(e=>e.Seq);var saved=snapshots.ToDictionary(s=>s.Seq);var path=new List<JournalEntry>();var seen=new HashSet<long>();long cursor=head;
   while(cursor!=0&&(verify||!saved.ContainsKey(cursor))&&lookup.TryGetValue(cursor,out var e)){if(!seen.Add(cursor))throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");path.Add(e);cursor=e.ParentSeq;}
   var state=cursor==0?new PuzzleState():saved.TryGetValue(cursor,out var snapshot)?snapshot.State:throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");
   path.Reverse();foreach(var e in path){var result=simulation.Commit(state,e.Command);if(!result.Validation.IsValid)throw new InvalidOperationException("REPLAY_VALIDATE_FAILED");state=result.Events.Aggregate(state,T0Simulation.Reduce);
    if(verify&&saved.TryGetValue(e.Seq,out var check)&&PuzzleState.KeepClues(state,check.State.AutoKeptClues).StateHash!=check.State.StateHash)throw new InvalidOperationException("REPLAY_HASH_MISMATCH");}
   return state;
  }
  void Rebuild(){State=BuildAt(HeadSeq,false);PreserveClues();}
  void PreserveClues(){permanentClues.UnionWith(State.AutoKeptClues);State=PuzzleState.KeepClues(State,permanentClues);}
 }
}
