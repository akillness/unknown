#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Sim;
using Tide.UI;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests
{
    public sealed class ReaderComparisonPlayModeTests
    {
        GameObject host;T0GameSession game;string directory;
        const string Plate="rec-plate-standard-hub",Ledger="rec-tide-ledger-bureau";
        [UnitySetUp] public IEnumerator SetUp()
        {
            directory=Path.Combine(Path.GetTempPath(),"reader-compare-"+Guid.NewGuid().ToString("N"));
            host=new GameObject("Reader comparison test");game=host.AddComponent<T0GameSession>();
            game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;
            Click("start");if(game.OpeningActive)Click("intro-skip");
            Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");
            Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);
            Click("decision-written");Click("close-document");Click("load-plate-zero");
            Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");
            Click("offset-left");Click("offset-up");Click("anchor-overlay");
            foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}
            Click("hub-view-reader");Click("open-reader");Click("load-"+Plate);Click("read");yield return Wait(game.FlushSaves());
        }
        [UnityTearDown] public IEnumerator TearDown()
        {
            if(game!=null)yield return Wait(game.FlushSaves());
            UnityEngine.Object.Destroy(host);yield return null;
            if(Directory.Exists(directory))Directory.Delete(directory,true);
        }
        static IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
        void Click(string id)=>Assert.IsTrue(game.Interface.Activate(id),"Missing action "+id);
        ReaderComparisonChart[] Charts()=>host.GetComponentsInChildren<ReaderComparisonChart>();
        void Window(string first,string last)=>Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("SetWindow",value:first,otherValue:last)).IsValid);

        [UnityTest] public IEnumerator PinAndComparisonNavigationLeaveProgressAndSaveUntouched()
        {
            Window("H-1:04","H+3:00");yield return Wait(game.FlushSaves());yield return null;
            string state=game.Journal.State.StateHash;long head=game.Journal.HeadSeq;
            var entries=game.Journal.Entries.Count;var bytes=File.ReadAllBytes(Path.Combine(directory,"save.json"));
            Click("reader-compare-pin");Click("reader-compare-"+Ledger);Click("reader-compare-start-next");
            yield return null;
            var charts=Charts();Assert.AreEqual(2,charts.Length);
            Assert.AreEqual(Ledger,charts[0].Range.RecordId);Assert.AreEqual("H-5:56",charts[0].Range.Start);
            Assert.AreEqual(Plate,charts[1].Range.RecordId);Assert.AreEqual("H-1:04",charts[1].Range.Start);
            Assert.AreEqual("H+3:00",charts[1].Range.End);
            Click("reader-compare-clear");yield return Wait(game.FlushSaves());
            Assert.AreEqual(state,game.Journal.State.StateHash);Assert.AreEqual(head,game.Journal.HeadSeq);
            Assert.AreEqual(entries,game.Journal.Entries.Count);CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")));
        }

        [UnityTest] public IEnumerator PinnedWindowSurvivesReaderEditsAndUndoWithDistinctAxes()
        {
            Window("H-1:04","H+3:00");Click("reader-compare-pin");
            Window("H-4:20","H-3:52");game.Undo();Click("reader-compare-"+Ledger);yield return null;
            var charts=Charts();var current=charts[0].Range;var pinned=charts[1].Range;
            Assert.AreEqual("cm",current.Unit);Assert.AreEqual("kPa",pinned.Unit);
            Assert.AreEqual(42,current.Minimum);Assert.AreEqual(960,current.Maximum);
            Assert.AreEqual(121.2f,pinned.Minimum,.001f);Assert.AreEqual(139.7f,pinned.Maximum,.001f);
            Assert.AreEqual("H-1:04",pinned.Start);Assert.AreEqual("H+3:00",pinned.End);
            Assert.IsFalse(float.IsNaN(pinned.Values[0]));Assert.IsFalse(float.IsNaN(pinned.Values.Last()));
            Assert.IsTrue(pinned.Values.Skip(1).Take(pinned.Values.Length-2).All(float.IsNaN));
            StringAssert.Contains("H-1:00",pinned.Missing);StringAssert.Contains("H+2:56",pinned.Missing);
            var labels=host.GetComponentsInChildren<Text>().Select(t=>t.text).ToArray();
            Assert.IsTrue(labels.Any(t=>t.Contains(current.Title)));Assert.IsTrue(labels.Any(t=>t.Contains(pinned.Title)));
            Assert.IsTrue(labels.Any(t=>t.Contains(current.AxisLabel)));Assert.IsTrue(labels.Any(t=>t.Contains(pinned.AxisLabel)));
        }

        [UnityTest] public IEnumerator UndoToUnavailableRecordClearsItsPinnedGraphWhileEarlierRecordRemainsLoaded()
        {
            game.OpenTool("circuit");
            Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");
            var area=game.Definition.UncoveredAreas[0];
            Click("area-"+area);
            Assert.IsFalse(game.Simulation.IsAvailable(game.Journal.State,"t0-b3"));
            Click("area-"+area);
            game.OpenTool("reader");Click("reader-compare-"+Ledger);Click("reader-compare-pin");
            yield return null;
            Assert.AreEqual(Plate,game.Journal.State.LoadedRecordId);
            Assert.AreEqual(Ledger,Charts().Last().Range.RecordId);
            game.Undo(); // Closing circuit was the last working command; both records remain visible.
            yield return null;
            Assert.IsTrue(game.Simulation.IsAvailable(game.Journal.State,"t0-b3"));
            Assert.AreEqual(2,Charts().Length);
            game.Undo(); // Undo the final area mark without undoing the earlier loaded plate.
            yield return null;
            Assert.IsFalse(game.Simulation.IsAvailable(game.Journal.State,"t0-b3"));
            Assert.IsTrue(game.Simulation.IsAvailable(game.Journal.State,"t0-b2"));
            Assert.AreEqual(Plate,game.Journal.State.LoadedRecordId);
            CollectionAssert.AreEqual(new[]{Plate},Charts().Select(c=>c.Range.RecordId).ToArray(),
                "Unavailable evidence must disappear from the current and pinned comparison views");
            Assert.IsFalse(game.Interface.ActionIds.Contains("reader-compare-"+Ledger));
            game.Redo();yield return null;
            Assert.IsTrue(game.Simulation.IsAvailable(game.Journal.State,"t0-b3"));
            Assert.AreEqual(1,Charts().Length,"Restoring access must not resurrect a cleared presentation pin");
        }

        [UnityTest] public IEnumerator FlatAndSingleSampleWindowsRenderButMissingWindowDoesNotInventSignal()
        {
            Window("H-4:20","H-3:52");yield return null;Canvas.ForceUpdateCanvases();
            var chart=Charts().Single();var mesh=chart.canvasRenderer.GetMesh();
            Assert.AreEqual(106.5f,chart.Range.Minimum,.001f);Assert.AreEqual(chart.Range.Minimum,chart.Range.Maximum);
            Assert.Greater(mesh.vertexCount,0);Assert.Less(mesh.bounds.size.y,5,"A flat signal must not become an invented slope");
            Window("H+3:00","H+3:00");yield return null;Canvas.ForceUpdateCanvases();chart=Charts().Single();mesh=chart.canvasRenderer.GetMesh();
            Assert.Greater(mesh.vertexCount,0,"One real sample beside missing evidence must remain visible");
            Window("H-1:04","H+3:00");yield return null;Canvas.ForceUpdateCanvases();chart=Charts().Single();mesh=chart.canvasRenderer.GetMesh();
            var points=mesh.vertices;var triangles=mesh.triangles;
            for(int i=0;i<triangles.Length;i+=3)
            {
                float a=points[triangles[i]].x,b=points[triangles[i+1]].x,c=points[triangles[i+2]].x;
                Assert.LessOrEqual(Mathf.Max(a,Mathf.Max(b,c))-Mathf.Min(a,Mathf.Min(b,c)),4,
                    "Only the real endpoint marks may render; a segment must not span the missing interval");
            }
            Window("H-1:00","H+2:56");yield return null;Canvas.ForceUpdateCanvases();chart=Charts().Single();mesh=chart.canvasRenderer.GetMesh();
            Assert.IsTrue(float.IsNaN(chart.Range.Minimum));Assert.AreEqual(0,mesh.vertexCount);
        }

        [UnityTest] public IEnumerator DiagnosticComparisonCardsKeepGraphAndReadoutContrastAtLargeText()
        {
            var profile=Resources.Load<Tide.Presentation.M20WorkSurfaceProfile>("M20WorkSurface");
            Assert.IsNotNull(profile);Assert.IsNotNull(profile.workSurface);
            var previous=profile.diagnosticOverride;var approved=profile.runtimeApproved;
            var ink=Charts().Single().color;
            try
            {
                profile.diagnosticOverride=true;
                game.OpenOverlay("settings");for(int i=0;i<5;i++)Click("text-scale");Click("overlay-back");
                foreach(var pinned in new[]{false,true})
                {
                    if(pinned){Click("reader-compare-pin");Click("reader-compare-"+Ledger);}
                    yield return null;yield return null;Canvas.ForceUpdateCanvases();
                    var row=(RectTransform)Charts()[0].transform.parent.parent;
                    Assert.LessOrEqual(row.rect.height,row.GetComponentInParent<ScrollRect>().viewport.rect.height);
                    var caseThread=host.GetComponentsInChildren<Text>().Single(t=>t.name=="CaseThread");
                    var cards=Enumerable.Range(0,row.childCount).Select(row.GetChild).Concat(new[]{caseThread.transform.parent});
                    foreach(var card in cards)
                    {
                        var ground=card.GetComponent<Image>();
                        Assert.IsNotNull(ground,"Nested comparison ink needs a reading ground over the dark diagnostic surface");
                        Assert.IsFalse(ground.raycastTarget,"The contrast correction must not intercept comparison input");
                        var back=new Color(ground.color.r*ground.color.a,ground.color.g*ground.color.a,ground.color.b*ground.color.a).linear;
                        var fore=ink.linear;
                        float dark=.2126f*fore.r+.7152f*fore.g+.0722f*fore.b;
                        float light=.2126f*back.r+.7152f*back.g+.0722f*back.b;
                        Assert.Greater((light+.05f)/(dark+.05f),4f,"Worst-case component-color contrast, not an accessibility certification");
                        foreach(var text in card.GetComponentsInChildren<Text>())Assert.AreEqual(ink,text.color);
                        foreach(var chart in card.GetComponentsInChildren<ReaderComparisonChart>())
                        {Assert.AreEqual(ink,chart.color);Assert.IsTrue(chart.canvasRenderer.hasRectClipping);}
                    }
                }
                Assert.AreEqual(approved,profile.runtimeApproved,"Diagnostic legibility must not promote the candidate");
            }
            finally{profile.diagnosticOverride=previous;}
        }

        [UnityTest] public IEnumerator BothRangesFitTogetherAtLargeTextAndRecoveryRetryClearsPin()
        {
            Window("H-1:04","H+3:00");Click("reader-compare-pin");Click("reader-compare-"+Ledger);
            game.OpenOverlay("settings");for(int i=0;i<5;i++)Click("text-scale");Click("overlay-back");
            Click("reader-compare-"+Ledger);yield return null;yield return null;Canvas.ForceUpdateCanvases();
            Assert.AreEqual(1.5f,game.Interface.TextScale);
            var charts=Charts();Assert.AreEqual(2,charts.Length);
            var row=(RectTransform)charts[0].transform.parent.parent;var scroll=row.GetComponentInParent<ScrollRect>();
            Assert.LessOrEqual(row.rect.height,scroll.viewport.rect.height,"Both complete range cards must fit the same viewport at 150%");
            foreach(var chart in charts)Assert.IsTrue(chart.canvasRenderer.hasRectClipping);
            yield return Wait(game.FlushSaves());game.OpenOverlay("recovery");Click("recovery-retry");
            game.OpenTool("reader");yield return null;
            Assert.AreEqual(1,Charts().Length,"Successful context replacement must clear the presentation pin");
            var replay=game.Journal.Entries.Select(e=>e.Command).ToArray();
            Click("reader-compare-pin");game.OpenOverlay("recovery");Click("recovery-new-slot");
            Click("start");if(game.OpeningActive)Click("intro-skip");
            foreach(var command in replay)Assert.IsTrue(game.SubmitImmediate(command).IsValid);
            game.OpenTool("reader");yield return null;
            Assert.AreEqual(1,Charts().Length,"Returning to the same loaded record in a new slot must not restore its old pin");
        }
    }
}
#endif
