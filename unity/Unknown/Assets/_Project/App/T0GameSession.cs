using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tide.Data;
using Tide.Input;
using Tide.Save;
using Tide.Sim;
using Tide.UI;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Tide.App {
 public sealed class CommitReceipt {
  public string AttemptId,CommandId; public int ContextGeneration; public bool Success;
 }
 public sealed partial class T0GameSession:MonoBehaviour {
  public T0Interface Interface {get;private set;} public WatchInput Watch {get;private set;}
  public CommandJournal Journal {get;private set;} public T0Simulation Simulation {get;private set;}
  public T0Definition Definition {get;private set;} public AtomicSaveStore Store {get;private set;}
  public bool SavePending {get;private set;} public string Surface=>overlay??tool??"shell";
  public event Action<CommitReceipt> tCommitReceipt;
  public int SuccessfulReceipts {get;private set;}
  T0RuntimeConfig config; JObject records,zones,tools,hints,beats,strings,policy,settings;
  string node="hub-view-desk",tool,overlay,document,status="",saveId=Guid.NewGuid().ToString(),createdUtc=DateTime.UtcNow.ToString("O");
  bool started,bootFailed,saveReadOnly; bool phaseStart; int phasePage; int generation,toolIndex; string attemptKey; PuzzleCommand proposed; CancellationTokenSource pendingCancel;
  T0CommitFeedback feedback;Task background=Task.CompletedTask; string selectedArea,saveRootDirectory; readonly Dictionary<string,int> hintLevels=new Dictionary<string,int>(StringComparer.Ordinal); int pendingHintLevel; bool hintLevelsDirty; float lastActivity; float cameraHorizontalFov;
  public void Initialize(T0RuntimeConfig value,string saveDirectory=null){
   config=value; Interface=gameObject.AddComponent<T0Interface>();Interface.Initialize();
   if(EventSystem.current==null){var es=new GameObject("Input event system",typeof(EventSystem));DontDestroyOnLoad(es);var input=es.AddComponent<InputSystemUIInputModule>();input.AssignDefaultActions();input.move=null;input.submit=null;input.cancel=null;}
   try{
    patrolPacket=JObject.Parse(Resources.Load<TextAsset>("C1PatrolContract").text);signaturePacket=JObject.Parse(Resources.Load<TextAsset>("C1SignatureContract").text);Definition=C1SignatureData.Attach(C1PatrolData.Attach(config.catalog.Load(),patrolPacket),signaturePacket);Simulation=new T0Simulation(Definition);
    records=JObject.Parse(config.records.text);zones=JObject.Parse(config.zones.text);tools=JObject.Parse(config.tools.text);hints=JObject.Parse(config.hints.text);beats=config.beats==null?null:JObject.Parse(config.beats.text);strings=JObject.Parse(config.strings.text);policy=JObject.Parse(config.savePolicy.text);
    Journal=new CommandJournal(Simulation,SnapshotInterval);
    cameraHorizontalFov=(float)zones["rows"][0]["viewNodes"].First(v=>(string)v["nodeId"]==node)["cameraPose"]["fovDeg"];
    if(T0DataLoader.CitationBlockers(Definition).Count>0)throw new InvalidOperationException("Canonical citation metadata absent");
    var args=Environment.GetCommandLineArgs();var arg=Array.IndexOf(args,"--t0-save-dir");if(arg>=0&&arg+1<args.Length)saveDirectory=args[arg+1];
    saveRootDirectory=saveDirectory??Path.Combine(Application.persistentDataPath,"saves");Store=new AtomicSaveStore(saveRootDirectory);
    settings=DefaultSettings();
    var settingsPath=Path.Combine(Store.DirectoryPath,"settings.json");if(File.Exists(settingsPath))try{settings=SaveCodec.Decode(File.ReadAllText(settingsPath));}catch(Exception){status="설정을 기본값으로 복구했습니다.";}
    Watch=gameObject.AddComponent<WatchInput>();Watch.Initialize(config.bindings.text,(string)settings["bindings"]);
    Watch.Navigate+=Interface.Navigate;Watch.BeginInteract+=Interface.BeginActivation;Watch.EndInteract+=Interface.EndActivation;
    Watch.ToolWheel+=()=>OpenOverlay("toolWheel");Watch.Adjust+=Adjust;Watch.Tool+=SelectTool;Watch.Overlay+=OpenOverlay;Watch.Query+=Query;Watch.Disconnect+=Disconnect;
    Interface.ReviewCompositionActive=()=>Watch.ImeCompositionActive;Interface.TextEntryChanged+=Watch.SetTextEntry;Watch.TextEntryExitRequested+=Interface.EndReviewEditing;
    Watch.Cancel+=Back;Watch.Undo+=Undo;Watch.Redo+=Redo;Watch.Preview+=Preview;
    var fx=JObject.Parse(Resources.Load<TextAsset>("T0Vfx").text);feedback=gameObject.AddComponent<T0CommitFeedback>();feedback.Initialize((float)fx["duration_ms"]/1000f,((JArray)fx["phases"]).Where(x=>(string)x["id"]!="await_impact").Select(x=>(float)x["start_ms"]).DefaultIfEmpty((float)fx["duration_ms"]).Min()/1000f);
    tCommitReceipt+=receipt=>{if(!C1PatrolDefinition.Handles(receipt.CommandId)&&!C1SignatureDefinition.Handles(receipt.CommandId))feedback.Present(receipt.AttemptId,receipt.Success,(bool)settings["reducedMotion"]);};
    Interface.ScreenChanged+=()=>{Watch.NewContext();feedback.Clear();};
    Watch.DeviceChanged+=_=>Interface.SetFooter(ControlFooter());
    var loaded=Store.Load(validate:doc=>JournalSave.Decode(doc,Simulation,SnapshotInterval));
    if(loaded.Document!=null){try{Journal=JournalSave.Decode(loaded.Document,Simulation,SnapshotInterval);RestoreHintLevels(loaded.Document);saveId=(string)loaded.Document["saveId"];createdUtc=(string)loaded.Document["createdUtc"];if(loaded.Source!="save.json")status=L("recovered")+" "+loaded.Source;}catch(Exception e){status=L("recovery")+" "+e.Message;overlay="recovery";}}
    else if(loaded.Failure!=null){saveReadOnly=true;status=L("recovery")+" "+loaded.Failure;overlay="recovery";}
    ConfigureOpening(loaded.Document==null&&loaded.Failure==null);BindApprovedDrawer();BindM7Hub();lastActivity=Time.unscaledTime;Render();
   }catch(Exception e){bootFailed=true;Debug.LogException(e);Interface.Render(new GameScreen{Title="T0 데이터 확인이 필요합니다",Body=e.Message,Footer="부팅이 중단되었습니다. 저장 파일은 변경하지 않았습니다."});}
  }
  // interaction-rules.md §1-1: two-step is the canonical default confirmation mode.
  public static JObject DefaultSettings()=>new JObject{["language"]="ko",["textScale"]=1.0,["confirmMode"]="two-step",["reducedMotion"]=false,["bindings"]=null};
  int SnapshotInterval=>(int?)policy?["snapshotInterval"]??200;
  void RestoreHintLevels(JObject doc){hintLevels.Clear();if(doc?["progress"]?["hintLevelUsed"] is JObject used)foreach(var p in used.Properties())hintLevels[p.Name]=(int)p.Value;}
  int HintLevel{get=>hintLevels.TryGetValue(CurrentBeat,out var level)?level:0;set=>hintLevels[CurrentBeat]=value;}
  // Hint-level changes during a pending commit are flushed once that commit settles (QA D-M9-10); no save.json race with CommitAsync.
  void SaveHintLevels(){if(SavePending)hintLevelsDirty=true;else QueueSave();}
  void FlushHintLevelsIfDirty(){if(!hintLevelsDirty)return;hintLevelsDirty=false;QueueSave();}
  string L(string key){var lang=(string)settings?["language"]??"ko";return (string)strings?[key]?[lang]??(string)strings?[key]?["ko"]??key;}
  JObject Record(string id)=>records["rows"].OfType<JObject>().First(r=>(string)r["recordId"]==id);
  string Name(string id)=>Definition.Records.ContainsKey(id)?(string)Record(id)["displayNameKo"]:id;
  string CurrentBeat=>BeatFor(Journal.State);
  ViewAction A(string id,string label,Action action,bool enabled=true,string detail=null,float hold=0)=>new ViewAction{Id=id,Label=label,Activate=()=>{lastActivity=Time.unscaledTime;action();},Enabled=enabled,Detail=detail,HoldSeconds=hold};
  void Update(){UpdateOpening();if(!bootFailed&&started)UpdateReviewNotesAvailability();if(!bootFailed&&started&&!OpeningActive&&overlay==null&&document==null&&Time.unscaledTime-lastActivity>float.Parse((string)tools["knobs"]["idleHintOfferSeconds"]["value"],CultureInfo.InvariantCulture)){status=L("hintOffer");lastActivity=Time.unscaledTime;Render();}}
  // Resume status follows the actual state (D-M9-16): the t0-b1 welcome only fits a fresh or intake-stage save.
  string ResumeStatus()=>SignatureActive?(SignatureComplete?"저장된 대조 기록을 복원했습니다.":"저장된 서명지 작업을 복원했습니다."):PatrolActive?"저장된 순찰 기록을 복원했습니다.":Simulation.IsComplete(Journal.State,"t0-b3")?L("caseReview"):Simulation.IsComplete(Journal.State,"t0-b1")?L("resumeInProgress"):L("welcome");
  public void StartGame(){CancelOpening();if(saveReadOnly){overlay="recovery";Render();return;}started=true;overlay=null;document=null;status=ResumeStatus();Render();}
  public void GoNode(string id){CloseTool();node=id;document=null;overlay=null;var camera=Camera.main;var target=zones["rows"][0]["viewNodes"].First(v=>(string)v["nodeId"]==id);if(camera!=null){var pos=target["cameraPose"]["pos"];var look=target["cameraPose"]["lookAt"];camera.transform.position=Point(pos);camera.transform.LookAt(Point(look));cameraHorizontalFov=(float)target["cameraPose"]["fovDeg"];}Render();}
  Transform approvedDrawer;
  Renderer[] approvedDrawerRenderers;
  void LateUpdate(){if(!bootFailed){ApplySceneViewport();InteractWithApprovedDrawer();}}
  void BindApprovedDrawer(){
   var hub=SceneManager.GetSceneByName("hub");if(!hub.IsValid()||!hub.isLoaded)return;
   var drawer=hub.GetRootGameObjects().FirstOrDefault(x=>x.name=="T0 approved r03 drawer");if(drawer==null)return;
   approvedDrawer=drawer.transform;
   approvedDrawerRenderers=drawer.GetComponentsInChildren<Renderer>();
  }
  void InteractWithApprovedDrawer(){
   var mouse=Mouse.current;
   if(!started||OpeningActive||PatrolActive||overlay!=null||tool!=null||document!=null||approvedDrawer==null||mouse==null||!mouse.leftButton.wasPressedThisFrame)return;
   var camera=Camera.main;var point=mouse.position.ReadValue();
   if(camera==null||!camera.pixelRect.Contains(point))return;
   if(EventSystem.current!=null){
    var hits=new List<RaycastResult>();
    EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current){position=point},hits);
    if(hits.Any(result=>result.module is GraphicRaycaster))return;
   }
   var ray=camera.ScreenPointToRay(point);var frustum=GeometryUtility.CalculateFrustumPlanes(camera);float nearest=float.PositiveInfinity;
   foreach(var renderer in approvedDrawerRenderers){
    if(!CameraCanRender(renderer,camera,frustum))continue;
    if(renderer.bounds.IntersectRay(ray,out var distance)&&distance<=camera.farClipPlane)nearest=Mathf.Min(nearest,distance);
   }
   if(float.IsPositiveInfinity(nearest)||Physics.Raycast(ray,nearest,camera.cullingMask,QueryTriggerInteraction.Ignore))return;
   // Bounds are a conservative visibility guard; approved r03 meshes stay non-readable.
   foreach(var renderer in FindObjectsByType<Renderer>(FindObjectsSortMode.None)){
    if(renderer.transform.IsChildOf(approvedDrawer)||!CameraCanRender(renderer,camera,frustum))continue;
    if(renderer.bounds.IntersectRay(ray,out var distance)&&distance<nearest)return;
   }
   lastActivity=Time.unscaledTime;GoNode("hub-view-drawer");
  }
  static bool CameraCanRender(Renderer renderer,Camera camera,Plane[] frustum)=>renderer!=null&&renderer.enabled&&!renderer.forceRenderingOff&&renderer.gameObject.activeInHierarchy&&renderer.shadowCastingMode!=UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly&&(camera.cullingMask&(1<<renderer.gameObject.layer))!=0&&GeometryUtility.TestPlanesAABB(frustum,renderer.bounds);
  void ApplySceneViewport(){var camera=Camera.main;if(camera==null||Interface==null||cameraHorizontalFov<=0)return;var viewport=Interface.SceneViewport;if(viewport.width<=0||viewport.height<=0||Screen.height<=0)return;camera.rect=viewport;camera.aspect=Screen.width*viewport.width/(Screen.height*viewport.height);camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView(cameraHorizontalFov,camera.aspect);}
  static Vector3 Point(JToken p)=>new Vector3((float)p["x"],(float)p["z"],(float)p["y"]);
  void CloseTool(){if(tool=="circuit")SubmitImmediate(new PuzzleCommand("CloseTool","circuit"),false);tool=null;}
  public void OpenTool(string id){if(OpeningActive)return;if(!started)return;if(PatrolActive){document=null;overlay=null;tool=null;Render();return;}CloseTool();document=null;overlay=null;tool=id;if(id=="circuit"){var v=SubmitImmediate(new PuzzleCommand("OpenTool","circuit"),false);if(!v.IsValid){tool=null;status=L("finishIntake");}}Render();}
  void SelectTool(int index){if(index>0)toolIndex=index-1;else toolIndex=(toolIndex+(index<0?5:1))%6;var ids=new[]{"circuit","reader","alignment","routing","corrosion","seal"};OpenTool(ids[Mathf.Clamp(toolIndex,0,5)]);}
  // hint-system.md §2: revealedLevel[beatId] survives close/reopen — no reset on overlay entry.
  public void OpenOverlay(string id){if(OpeningActive&&id!="settings")return;overlay=id;Render();}
  void Query(){if(OpeningActive)return;if(tool=="circuit")OpenOverlay("coverageQuery");}
  void Disconnect(){if(OpeningActive)return;if(tool=="reader"&&Journal.State.LoadedRecordId!=null)RequestConfirm(new PuzzleCommand("CiteToBoard",Journal.State.LoadedRecordId));}
  public void Back(){if(OpeningActive){if(overlay!=null){overlay=null;Render();}else FinishOpening();return;}if(SavePending){CancelPending();QueueSave();}if(overlay=="reviewNotes"){CloseReviewNotes();return;}if(overlay!=null){overlay=null;proposed=null;Render();return;}if(document!=null){document=null;Render();return;}if(tool=="circuit"&&Simulation.CircuitMode(Journal.State)=="Overlaying"){SubmitImmediate(new PuzzleCommand("Cancel"));return;}CloseTool();Render();}
  public ValidationResult SubmitImmediate(PuzzleCommand command,bool render=true){
   if(command.CommandId=="ConfirmPatrol"||command.CommandId=="ConfirmSignature")return ValidationResult.InvalidData("C1_REQUIRES_ATOMIC_COMMIT");
   CancelPending();var verdict=Journal.Submit(command);status=verdict.IsValid?"":PatrolDiagnostic(verdict);
   if(verdict.IsValid)QueueSave();if(render)Render();return verdict;
  }
  void CancelPending(){if(!SavePending)return;generation++;pendingCancel?.Cancel();SavePending=false;attemptKey=null;FlushHintLevelsIfDirty();}
  JObject SaveDocument(CommandJournal journal,string key){var doc=JournalSave.Encode(journal,key,(int)policy["entryCap"],(int)policy["byteCap"],saveId,createdUtc,hintLevels);doc["beatId"]=BeatFor(journal.State);return doc;}
  void QueueSave(){if(saveReadOnly)return;var candidate=Journal.Clone();var key=Guid.NewGuid().ToString();background=Persist(candidate,key);}
  async Task Persist(CommandJournal journal,string key){try{await Store.WriteAsync(SaveDocument(journal,key));reviewAvailabilityMayChange=true;}catch(Exception e){Debug.LogWarning("T0 autosave: "+e.Message);status=L("saveFailed");}}
  public void Undo(){if(OpeningActive)return;CancelPending();if(Journal.Undo()){status=L("undone");QueueSave();}overlay=null;Render();}
  public void Redo(){if(OpeningActive)return;CancelPending();if(Journal.Redo()){status=L("redone");QueueSave();}overlay=null;Render();}
  public void RequestConfirm(PuzzleCommand command){
   if(SavePending)return;proposed=command;attemptKey=null;var v=Simulation.Validate(Journal.State,command);if(!v.IsValid){status=PatrolDiagnostic(v);Render();return;}
   overlay=ConfirmMode=="two-step"?"preview":"confirm";Render();
  }
  public async Task<bool> CommitAsync(PuzzleCommand command){
   if(SavePending||saveReadOnly)return false;var candidate=Journal.Clone();var valid=candidate.Submit(command);if(!valid.IsValid){status=PatrolDiagnostic(valid);Render();return false;}
   var key=attemptKey??(attemptKey=Guid.NewGuid().ToString());var attempt=++generation;pendingCancel=new CancellationTokenSource();SavePending=true;proposed=command;overlay="pending";Render();var receiptContext=Watch.ContextGeneration;
   try{
    await Store.WriteAsync(SaveDocument(Journal.Clone(),key+"-checkpoint"),"checkpoint.pre-commit.json",pendingCancel.Token);
    await Store.WriteAsync(SaveDocument(candidate,key),"save.json",pendingCancel.Token);
    if(attempt!=generation||pendingCancel.IsCancellationRequested)return false;
    Journal=candidate;SavePending=false;attemptKey=null;overlay=null;status=L("saved");bool present=Watch.ContextGeneration==receiptContext;FlushHintLevelsIfDirty();Render();
    if(present){SuccessfulReceipts++;tCommitReceipt?.Invoke(new CommitReceipt{AttemptId=key,CommandId=command.CommandId,ContextGeneration=Watch.ContextGeneration,Success=true});}return true;
   }catch(Exception e){if(attempt!=generation)return false;SavePending=false;overlay="saveFailure";status=L("saveFailed");Debug.LogWarning("T0 commit: "+e.Message);FlushHintLevelsIfDirty();Render();return false;}
  }
  void Preview(){if(OpeningActive)return;if(overlay!=null)return;if(tool=="reader"&&Journal.State.LoadedRecordId!=null)SubmitImmediate(new PuzzleCommand("Read"));else if(tool=="circuit"){var area=Definition.UncoveredAreas.FirstOrDefault(a=>"area-"+a==Interface.CurrentFocusId);if(area!=null)SubmitImmediate(new PuzzleCommand("ToggleUncovered",area));}}
  void Adjust(Vector2 delta,bool fine){if(OpeningActive)return;if(tool=="circuit"&&Simulation.CircuitMode(Journal.State)=="Overlaying"){
   var o=Definition.Overlay;if(fine&&!o.FineGridStep.HasValue){status=L("fineUnavailable");Render();return;}double step=fine?o.FineGridStep.Value:o.GridStep;
   double x=double.Parse(Journal.State.Get("overlayX"),CultureInfo.InvariantCulture),y=double.Parse(Journal.State.Get("overlayY"),CultureInfo.InvariantCulture);
   if(Mathf.Abs(delta.x)>Mathf.Abs(delta.y))x+=Math.Sign(delta.x)*step;else y+=Math.Sign(delta.y)*step;
   SubmitImmediate(new PuzzleCommand("SetOverlayOffset",value:x.ToString(CultureInfo.InvariantCulture),otherValue:y.ToString(CultureInfo.InvariantCulture)));
  }else Interface.Navigate(delta.y>0||delta.x<0?-1:1);}
  string Reason(ReasonCode code)=>L("reason."+code);
  void SaveSettings(){Directory.CreateDirectory(Store.DirectoryPath);var path=Path.Combine(Store.DirectoryPath,"settings.json");var temp=path+".tmp";File.WriteAllText(temp,SaveCodec.Encode(settings));if(File.Exists(path))File.Replace(temp,path,path+".bak");else File.Move(temp,path);Render();}
  string ControlFooter()=>L(Watch!=null&&Watch.LastDeviceIsGamepad?"controlsPad":"controls");
  float HoldSeconds=>settings["holdSeconds"]==null?float.Parse((string)tools["knobs"]["commitHoldSeconds"]["value"],CultureInfo.InvariantCulture):(float)settings["holdSeconds"];
  void CycleHoldDuration(){var option=JObject.Parse(Resources.Load<TextAsset>("HoldOptions").text);float min=(float)option["minSeconds"],max=(float)option["maxSeconds"],step=(float)option["stepSeconds"];settings["holdSeconds"]=HoldSeconds>=max?min:Math.Round(HoldSeconds+step,1);SaveSettings();}
  public void SetConfirmMode(string mode){settings[SignatureActive?"c1SignatureConfirmMode":PatrolActive?"c1ConfirmMode":"confirmMode"]=mode;SaveSettings();}
  public void Render(){
   if(bootFailed||strings==null)return;Interface.TextScale=(float?)settings["textScale"]??1f;
   var s=new GameScreen{Title=L("title"),Subtitle="21:00 · "+(started?CurrentBeat:L("startTitle")),Status=status,Footer=ControlFooter()};
   Watch.ToolPanel=tool!=null&&!OpeningActive;Watch.OverlayActive=overlay!=null;
   if(OpeningActive)OpeningScreen(s);
   else if(!started){s.Body=L("intro");s.Actions.Add(A("start",L("start"),BeginOpeningOrStart));s.Actions.Add(A("settings",L("settings"),()=>OpenOverlay("settings")));}
   else{
    if(!PatrolActive)foreach(var n in zones["rows"][0]["viewNodes"]){string id=(string)n["nodeId"];s.Navigation.Add(A(id,(string)n["label"],()=>GoNode(id)));}
    s.Toolbar.Add(A("undo",L("undo"),Undo));s.Toolbar.Add(A("redo",L("redo"),Redo));s.Toolbar.Add(A("evidence",L("evidence"),()=>OpenOverlay("evidence")));s.Toolbar.Add(A("hints",L("hints"),()=>OpenOverlay("hints")));s.Toolbar.Add(A("settings",L("settings"),()=>OpenOverlay("settings")));s.Toolbar.Add(A("back",L("back"),Back));
    if(Journal.CoarseUndo)s.Footer=L("coarseUndo")+" · "+s.Footer;
    if(overlay==null){if(SignatureActive)SignatureScreen(s);else if(PatrolActive)PatrolScreen(s);else if(document!=null)DocumentScreen(s);else if(tool=="circuit")CircuitScreen(s);else if(tool=="reader")ReaderScreen(s);else if(tool!=null){s.Body=L("stub");s.Actions.Add(A("close-tool",L("back"),Back));}else ShellScreen(s);}
   }
   if(overlay!=null){if(!started||OpeningActive)s.Toolbar.Add(A("back",L("back"),Back));s.ShowOpening=false;s.OpeningImage=null;s.Actions.Clear();OverlayScreen(s);if(started&&(overlay=="evidence"||overlay=="hypothesis"))s.Actions.Insert(0,A("open-review-notes","검토 노트 · 대조 메모",OpenReviewNotes));}
   if(!PatrolActive&&Simulation.IsComplete(Journal.State,"t0-b3"))s.Subtitle+=" · "+L("complete");
   s.CaseThread=OpeningActive||overlay=="reviewNotes"?null:CaseThreadText();
   if(DirectionEnabled&&SignatureActive&&!OpeningActive){s.ShowDirection=true;s.SectionSurface=directionProfile.sectionSurface;foreach(var action in s.Actions.Concat(s.Toolbar))action.DirectionCategory=SignatureDirectionCategory(action.Id);}
   ApplyM7UiSkin(s);Interface.Render(s);ApplyStagePresentation();ApplySceneViewport();
  }
  // Projection only: required pins are progress, but the existing full predicate owns completion.
  string CaseThreadText(){
   if(SignatureActive)return SignatureCaseThread();
            if(PatrolActive)return PatrolCaseThread();
   var requirements=Definition.Beats.Single(b=>b.Id=="t0-b3").Requirements;
   var pins=requirements.Where(r=>r.Type=="citationPinned").ToArray();
   int count=pins.Count(r=>Journal.State.Has("citation:"+r.RecordId));
   bool complete=Simulation.IsComplete(Journal.State,"t0-b3");
   string detail=complete?L("caseComplete"):L(count==pins.Length?"caseCheckPins":"caseMedia");
   return L("caseTitle")+" · "+string.Format(L("caseProgress"),count,pins.Length)+"\n"+CaseObjective(beats,CurrentBeat,Definition.Records.Keys.Select(Name),L("caseObjective"))+"\n"+detail+"\n"+L("caseNext")+CaseThreadNext();
  }
  string CaseThreadNext(){
   var state=Journal.State;
   if(SavePending)return L("caseWait");
   if(Simulation.IsComplete(state,"t0-b3"))return L("caseReview");
   if(!Simulation.IsComplete(state,"t0-b1"))return L("caseRead");
   if(!Simulation.IsComplete(state,"t0-b2"))return L("caseAlign");
   return L("casePin");
  }
  // S-D guided teaching (RFC-CX-011). Objective text is data-owned; a record display name inside it
  // would violate the CaseThread disclosure contract, so such rows fall back to the fixed objective.
  public static string CaseObjective(JObject beatsTable,string beatId,IEnumerable<string> recordNames,string fallback){
   var objective=(string)beatsTable?["rows"]?.FirstOrDefault(r=>(string)r["id"]==beatId)?["objective"];
   if(string.IsNullOrEmpty(objective)||recordNames.Any(objective.Contains))return fallback;
   return objective;
  }
  string GuidedTeachingText(string toolId){
   if(SignatureActive||PatrolActive||beats==null)return null;
   var row=((JArray)tools["rows"]).OfType<JObject>().FirstOrDefault(r=>(string)r["toolId"]==toolId);
   var beat=(string)row?["introBeatId"];if(beat==null||beat!=CurrentBeat)return null;
   var teaching=beats["rows"]?.FirstOrDefault(b=>(string)b["id"]==beat)?["toolTeaching"];
   if(teaching==null||!teaching.Any(t=>(string)t["tool"]==toolId&&(string)t["mode"]=="guided"))return null;
   var hint=(string)hints["rows"].FirstOrDefault(h=>(string)h["beatId"]==beat&&(int)h["level"]==1)?["sourceTextKo"];
   if(hint==null)return null;
   int remaining=Definition.Beats.Single(b=>b.Id==beat).Requirements.Count(r=>!Simulation.IsSatisfied(Journal.State,r));
   return L("teachingIntro")+"\n"+hint+"\n"+string.Format(L("teachingRemaining"),remaining);
  }
  // S-C pure preview diff (RFC-CX-011): sentences describing what an applied command changes.
  public static List<string> PreviewDifferenceSentences(PuzzleState current,PuzzleState candidate,Func<string,string> text,Func<string,string> name){
   var lines=new List<string>();
   foreach(var fact in candidate.FactSnapshot.Where(f=>!current.Has(f))){
    if(fact.StartsWith("citation:",StringComparison.Ordinal)){
     var id=fact.Substring(9);
     lines.Add(string.Format(text("previewFactCitation"),name(id),candidate.ValueSnapshot.TryGetValue("citationStart:"+id,out var start)?start:"?",candidate.ValueSnapshot.TryGetValue("citationEnd:"+id,out var end)?end:"?"));
    }else if(fact.StartsWith("copy:",StringComparison.Ordinal))lines.Add(string.Format(text("previewFactCopy"),name(fact.Substring(5))));
    else if(!fact.StartsWith("kept:",StringComparison.Ordinal))lines.Add(string.Format(text("previewFactGeneric"),fact));
   }
   foreach(var pair in candidate.ReadCounts.OrderBy(p=>p.Key,StringComparer.Ordinal)){
    int before=current.ReadCount(pair.Key);
    if(pair.Value!=before)lines.Add(string.Format(text("previewReadCount"),name(pair.Key),before,pair.Value));
   }
   return lines;
  }
  void ShellScreen(GameScreen s){
   if(Simulation.IsComplete(Journal.State,"t0-b3"))s.Actions.Add(A("continue-c1","C1 · 두 개의 필적으로 이동",ContinueToPatrol));
   s.Body=L("room")+"\n"+L("intakeProgress")+" "+(Simulation.IsComplete(Journal.State,"t0-b1")?L("done"):L("inProgress"));
   if(node=="hub-view-desk"){
    s.Actions.Add(A("handover",Name("rec-handover-brief"),()=>{document="rec-handover-brief";Render();}));
    s.Actions.Add(A("transfer",Name("rec-transfer-list"),()=>{document="rec-transfer-list";Render();}));
    s.Actions.Add(A("load-plate-zero",L("placePlate"),()=>SubmitImmediate(new PuzzleCommand("PlaceInSlot","workbench-standing-slot","plate-zero"))));
   }else if(node=="hub-view-drawer")s.Actions.Add(A("inspect-plate-zero",L("plateZero"),()=>{status=L("plateZeroDetail");Render();}));
   else if(node=="hub-view-circuitmap")s.Actions.Add(A("open-circuit",L("circuit"),()=>OpenTool("circuit")));
   else if(node=="hub-view-reader")s.Actions.Add(A("open-reader",L("reader"),()=>OpenTool("reader")));
   else if(node=="hub-view-plateshelf")foreach(var r in Definition.Records.Values.Where(r=>r.VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b)))){var id=r.Id;s.Actions.Add(A("record-"+id,Name(id),()=>{document=id;Render();}));}
   else if(node=="hub-view-sealdesk")s.Body=L("sealOutsideT0");
  }
  void DocumentScreen(GameScreen s){
   var r=Record(document);s.Title=Name(document);s.Body=(string)settings["language"]=="en"?L("koFallback"):"";
   foreach(var line in (r["lines"] as JArray??new JArray())){var id=(string)line["lineId"];s.Actions.Add(A("line-"+id,(Journal.State.Has("line:"+document+":"+id)?"✓ ":"")+(string)line["text"],()=>SubmitImmediate(new PuzzleCommand("ViewLine",document,id))));}
   foreach(var row in (r["rows"] as JArray??new JArray())){var id=(string)row["rowId"];if(id=="tl-r4")continue;var text=(string)row["item"]+" · "+(string)row["note"];s.Actions.Add(A("row-"+id,(Journal.State.Has("row:"+document+":"+id)?"✓ ":"")+text,()=>SubmitImmediate(new PuzzleCommand("ViewRow",document,id))));}
   if(document=="rec-transfer-list")foreach(var value in new[]{"written","blank"}){var choice=value;s.Actions.Add(A("decision-"+choice,L(choice),()=>SubmitImmediate(new PuzzleCommand("RecordDecision","transfer-list-entry",choice))));}
   if(r["samples"]!=null)s.Actions.Add(A("load-document",L("loadReader"),()=>{var id=document;OpenTool("reader");SubmitImmediate(new PuzzleCommand("LoadRecord",id));}));
   s.Actions.Add(A("close-document",L("back"),Back));
  }
  void CircuitScreen(GameScreen s){
   var mode=Simulation.CircuitMode(Journal.State);s.Title=L("circuit");var teaching=GuidedTeachingText("circuit");s.Body=(teaching==null?"":teaching+"\n")+L("circuitInstructions")+"\n"+L("state")+": "+mode;
   if(mode=="Tracing"){
    foreach(var id in Definition.SystemIds){var sys=id;s.Actions.Add(A("trace-"+sys,L("trace"),()=>SubmitImmediate(new PuzzleCommand("TraceSystem",sys))));}
    s.Actions.Add(A("begin-overlay",L("beginOverlay"),()=>SubmitImmediate(new PuzzleCommand("BeginOverlay"))));
   }else if(mode=="Overlaying"){
    var o=Definition.Overlay;float x=float.Parse(Journal.State.Get("overlayX"),CultureInfo.InvariantCulture),y=float.Parse(Journal.State.Get("overlayY"),CultureInfo.InvariantCulture);
    s.AnchorTargets=o.Anchors.Select(a=>new Vector2((float)a.TargetX,(float)a.TargetY)).ToArray();s.AnchorOverlay=o.Anchors.Select(a=>new Vector2((float)a.OverlayX+x,(float)a.OverlayY+y)).ToArray();s.AnchorLabels=o.Anchors.Select(a=>a.Label).ToArray();s.Body+="\n"+L("offset")+" ("+x+", "+y+")";
    s.Actions.Add(A("offset-left","← "+L("left"),()=>Adjust(Vector2.left,false)));s.Actions.Add(A("offset-right","→ "+L("right"),()=>Adjust(Vector2.right,false)));s.Actions.Add(A("offset-up","↑ "+L("up"),()=>Adjust(Vector2.up,false)));s.Actions.Add(A("offset-down","↓ "+L("down"),()=>Adjust(Vector2.down,false)));
    s.Actions.Add(A("reset-overlay",L("resetOverlay"),()=>SubmitImmediate(new PuzzleCommand("SetOverlayOffset",value:o.InitialX.ToString(CultureInfo.InvariantCulture),otherValue:o.InitialY.ToString(CultureInfo.InvariantCulture)))));
    s.Actions.Add(A("anchor-overlay",L("anchorOverlay"),()=>SubmitImmediate(new PuzzleCommand("AnchorOverlay")),o.Aligned(x,y)));
   }else foreach(var area in Definition.UncoveredAreas){var a=area;var number=Definition.UncoveredAreas.ToList().IndexOf(a)+1;s.Actions.Add(A("area-"+a,(Journal.State.Has("area:"+a)?"✓ ":"")+L("outdoorArea")+" "+number,()=>SubmitImmediate(new PuzzleCommand("ToggleUncovered",a))));s.Actions.Add(A("area-evidence-"+a,L("attachEvidence")+" "+number,()=>{selectedArea=a;OpenOverlay("areaEvidence");}));}
  }
  void ReaderScreen(GameScreen s){
   s.Title=L("reader");var id=Journal.State.LoadedRecordId;var teaching=GuidedTeachingText("reader");var teachingPrefix=teaching==null?"":teaching+"\n";
   if(id==null){s.Body=teachingPrefix+L("chooseRecord");foreach(var r in Definition.Records.Values.Where(r=>r.Phases.Count>0&&r.VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b)))){var rid=r.Id;s.Actions.Add(A("load-"+rid,Name(rid),()=>SubmitImmediate(new PuzzleCommand("LoadRecord",rid))));}return;}
   var record=Record(id);s.Body=teachingPrefix+Name(id)+"\n"+L("reads")+": "+Journal.State.ReadCount(id)+" / "+Definition.ReadBudget+" · "+(Journal.State.Has("copy:"+id)?L("copyKept"):L("copyOnRead"));
   var samples=(JArray)record["samples"];s.Chart=samples.Select(x=>(string)x["state"]=="missing"?float.NaN:(float?)(x["pressure"]??x["tideHeight"])??float.NaN).ToArray();s.ChartLabel=(string)samples[0]["phase"]+" · "+(string)samples[samples.Count/2]["phase"]+" · "+(string)samples[samples.Count-1]["phase"]+"\n"+L("chartLegend");
   s.Actions.Add(A("read",L("read"),()=>SubmitImmediate(new PuzzleCommand("Read"))));s.Actions.Add(A("read-original",L("readOriginal"),()=>RequestConfirm(new PuzzleCommand("ReadOriginal")),Journal.State.Has("copy:"+id)&&Journal.State.ReadCount(id)<Definition.ReadBudget));
   var phases=Definition.Records[id].Phases;var start=Journal.State.Get("windowStart:"+id)??phases[0];var end=Journal.State.Get("windowEnd:"+id)??phases[phases.Count-1];s.Body+="\n"+L("window")+": "+start+" → "+end;
   s.Actions.Add(A("pick-start",L("pickStart"),()=>{phaseStart=true;phasePage=phases.ToList().IndexOf(start)/30;OpenOverlay("phasePicker");}));s.Actions.Add(A("pick-end",L("pickEnd"),()=>{phaseStart=false;phasePage=phases.ToList().IndexOf(end)/30;OpenOverlay("phasePicker");}));
   s.Actions.Add(A("start-prev",L("startEarlier"),()=>ShiftWindow(id,true,-1)));s.Actions.Add(A("start-next",L("startLater"),()=>ShiftWindow(id,true,1)));s.Actions.Add(A("end-prev",L("endEarlier"),()=>ShiftWindow(id,false,-1)));s.Actions.Add(A("end-next",L("endLater"),()=>ShiftWindow(id,false,1)));
   s.Actions.Add(A("cite",L("cite"),()=>RequestConfirm(new PuzzleCommand("CiteToBoard",id)),!SavePending));
   s.Actions.Add(A("select-record",L("chooseRecord"),()=>OpenOverlay("readerEvidence")));
  }
  void ShiftWindow(string id,bool start,int delta){var phases=Definition.Records[id].Phases;var first=Journal.State.Get("windowStart:"+id)??phases[0];var last=Journal.State.Get("windowEnd:"+id)??phases[phases.Count-1];int index=phases.ToList().IndexOf(start?first:last);var next=phases[Mathf.Clamp(index+delta,0,phases.Count-1)];SubmitImmediate(new PuzzleCommand("SetWindow",value:start?next:first,otherValue:start?last:next));}
  void RecoverySlots(GameScreen s){
   if(!Directory.Exists(saveRootDirectory))return;int index=0;
   foreach(var directory in Directory.GetDirectories(saveRootDirectory,"recovery-*").OrderBy(d=>d)){
    var store=new AtomicSaveStore(directory);var loaded=store.Load(validate:d=>JournalSave.Decode(d,Simulation,SnapshotInterval));if(loaded.Document==null)continue;
    var path=directory;var label=L("recoverySlot")+" "+(++index)+" · "+(string)loaded.Document["createdUtc"];
    s.Actions.Add(A("select-slot-"+index,label,()=>{var next=new AtomicSaveStore(path);var validated=next.Load(validate:d=>JournalSave.Decode(d,Simulation,SnapshotInterval));if(validated.Document==null){status=L("recovery");Render();return;}Journal=JournalSave.Decode(validated.Document,Simulation,SnapshotInterval);RestoreHintLevels(validated.Document);Store=next;saveId=(string)validated.Document["saveId"];createdUtc=(string)validated.Document["createdUtc"];saveReadOnly=false;ConfigureOpening(false);overlay=null;status=L("recovered");Render();}));
   }
  }
  void OverlayScreen(GameScreen s){
   if(overlay=="reviewNotes"){ReviewNotesScreen(s);return;}
   if(SignatureOverlay(s))return;
            if(PatrolActive&&overlay=="toolWheel"){s.Body="C1 · "+PatrolText("c1.patrol.preview");s.Actions.Add(A("c1-circuit",PatrolText("c1.patrol.title"),()=>{overlay=null;document=null;Render();}));s.Actions.Add(A("overlay-back",L("back"),Back));return;}
   s.Title=L(overlay);s.Body="";
   if(overlay=="toolWheel"){foreach(var id in new[]{"circuit","reader","alignment","routing","corrosion","seal"}){var selected=id;s.Actions.Add(A("wheel-"+id,L("action."+id),()=>OpenTool(selected)));}}
   else if(overlay=="coverageQuery"){s.Body=L("outsideCoverage");}
   else if(overlay=="phasePicker"){var id=Journal.State.LoadedRecordId;var phases=Definition.Records[id].Phases;s.Body=L("window");s.Actions.Add(A("phase-prev",L("previousPage"),()=>{phasePage=Math.Max(0,phasePage-1);Render();}));s.Actions.Add(A("phase-next",L("nextPage"),()=>{phasePage=Math.Min((phases.Count-1)/30,phasePage+1);Render();}));foreach(var phase in phases.Skip(phasePage*30).Take(30)){var selected=phase;var sample=Record(id)["samples"].First(x=>(string)x["phase"]==selected);var reading=(string)sample["state"]=="missing"?L("missing"):((string)sample["state"]=="flat"?L("flat")+" · ":"")+(sample["pressure"]??sample["tideHeight"]).ToString();s.Actions.Add(A("phase-"+selected,selected+" · "+reading,()=>{var first=Journal.State.Get("windowStart:"+id)??phases[0];var last=Journal.State.Get("windowEnd:"+id)??phases[phases.Count-1];var v=SubmitImmediate(new PuzzleCommand("SetWindow",value:phaseStart?selected:first,otherValue:phaseStart?last:selected),false);if(v.IsValid)overlay=null;Render();}));}}
   else if(overlay=="settings"){if(DirectionEnabled&&!OpeningActive)s.Actions.Add(A("intro-replay","도입 안내 다시 보기",ReplayOpening,!SavePending));if(!OpeningActive)s.Actions.Add(A("saved-slots",L("savedSlots"),()=>OpenOverlay("recovery")));
    s.Actions.Add(A("language",L("language")+": "+(string)settings["language"],()=>{settings["language"]=(string)settings["language"]=="ko"?"en":"ko";SaveSettings();}));
    s.Actions.Add(A("text-scale",L("textScale")+": "+settings["textScale"],()=>{double scale=(double)settings["textScale"];settings["textScale"]=scale>=1.5?1:Math.Round(scale+.1,1);SaveSettings();}));
    s.Actions.Add(A("reduced-motion",L("reducedMotion")+": "+settings["reducedMotion"],()=>{settings["reducedMotion"]=!(bool)settings["reducedMotion"];SaveSettings();}));
    s.Actions.Add(A("hold-duration",L("holdDuration")+": "+HoldSeconds.ToString("0.0",CultureInfo.InvariantCulture)+" s",CycleHoldDuration));
    foreach(var mode in new[]{"confirm-dialog","hold","two-step"}){var m=mode;s.Actions.Add(A("confirm-mode-"+m,L(m)+(ConfirmMode==m?" ✓":""),()=>SetConfirmMode(m)));}
    foreach(var action in Watch.Actions.FindActionMap("Watch").actions)for(int i=0;i<action.bindings.Count;i++){if(action.bindings[i].isComposite)continue;var name=action.name;var binding=i;s.Actions.Add(A("rebind-"+name+"-"+i,L("action."+name)+" · "+action.GetBindingDisplayString(i),()=>{status=L("pressKey");Render();Watch.Rebind(name,json=>{settings["bindings"]=json;if(Watch.RebindError!=null)status=L(Watch.RebindError);SaveSettings();},binding);}));}
   }else if(overlay=="areaEvidence"){
    if(selectedArea==null)selectedArea=Definition.UncoveredAreas[0];s.Body=L("evidenceRequirement");
    foreach(var r in Definition.Records.Values.Where(r=>r.VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b)))){var rid=r.Id;s.Actions.Add(A("attach-"+rid,(Journal.State.Has("evidence:"+selectedArea+":"+rid)?"✓ ":"")+Name(rid)+" · "+ReviewMediaName(r.SourceType),()=>SubmitImmediate(new PuzzleCommand("AttachAreaEvidence",selectedArea,rid))));}
   }else if(overlay=="readerEvidence"){
    foreach(var r in Definition.Records.Values.Where(r=>r.Phases.Count>0&&r.VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b)))){var rid=r.Id;s.Actions.Add(A("choose-"+rid,Name(rid)+" · "+ReviewMediaName(r.SourceType),()=>{overlay=null;SubmitImmediate(new PuzzleCommand("LoadRecord",rid));}));}
   }else if(overlay=="evidence"||overlay=="hypothesis"){
    s.Body=L("evidenceIntro");if(PatrolActive){foreach(var observation in patrolPacket["observations"].Where(o=>Journal.State.Has("c1:observed:"+(string)o["id"])))s.Body+="\n"+(string)observation["description"]+" · "+ReviewMediaName((string)observation["sourceType"]);if(PatrolComplete)s.Body+="\n"+PatrolConditionText();}foreach(var id in Journal.State.AutoKeptClues)s.Body+="\n✓ "+id;
    foreach(var id in Definition.Records.Keys.Where(id=>Journal.State.Has("citation:"+id)))s.Body+="\n"+Name(id)+" · "+Journal.State.Get("citationStart:"+id)+" → "+Journal.State.Get("citationEnd:"+id);
   }else if(overlay=="hints"){
    var rows=(SignatureActive?new JArray(signaturePacket["narrative"]["hints"].Select((h,i)=>new JObject{["beatId"]=C1SignatureDefinition.BeatId,["level"]=i+1,["sourceTextKo"]=(string)h})):PatrolActive?new JArray(patrolPacket["narrative"]["hints"].Select((h,i)=>new JObject{["beatId"]=C1PatrolDefinition.BeatId,["level"]=i+1,["sourceTextKo"]=(string)h})):hints["rows"]).Where(h=>(string)h["beatId"]==CurrentBeat).OrderBy(h=>(int)h["level"]).ToArray();s.Body=L("hintFree");foreach(var row in rows.Where(h=>(int)h["level"]<=HintLevel))s.Body+="\n"+(string)row["sourceTextKo"];
    // The spoiler gate is data-driven: the next row's warnsBeforeReveal decides; synthesized C1 rows
    // carry no field, so entering level 3 falls back to the warning (hint-system.md H-R3).
    if(HintLevel<rows.Length)s.Actions.Add(A("hint-next",L("nextHint"),()=>{var next=rows[HintLevel];bool warns=next["warnsBeforeReveal"]!=null?(bool)next["warnsBeforeReveal"]:(int)next["level"]==3;if(warns){pendingHintLevel=(int)next["level"];overlay="hintWarning";}else{HintLevel=(int)next["level"];SaveHintLevels();}Render();}));
   }else if(overlay=="hintWarning"){s.Body=L("hintSpoiler");s.Actions.Add(A("hint-reveal",L("reveal"),()=>{overlay="hints";HintLevel=pendingHintLevel;SaveHintLevels();Render();}));}
   else if(overlay=="pending"){s.Body=L("saving");s.Actions.Add(A("pending-undo",L("undo"),Undo));}
   else if(overlay=="saveFailure"){s.Body=L("saveFailed");s.Actions.Add(A("retry-save",L("retry"),()=>{_ = CommitAsync(proposed);}));}
   else if(overlay=="confirm"||overlay=="preview"){
    if(proposed?.CommandId=="ConfirmSignature")s.Body=SignatureConfirmationText();
    else if(proposed?.CommandId=="ConfirmPatrol")s.Body=PatrolConfirmationText();
    else{
     s.Body=L("confirmExplanation");
     if(proposed!=null&&Simulation.Validate(Journal.State,proposed).IsValid){
      var differences=PreviewDifferenceSentences(Journal.State,Simulation.Preview(Journal.State,proposed),L,Name);
      if(differences.Count>0)s.Body=L("previewChanged")+"\n"+string.Join("\n",differences)+"\n"+L("previewUndoNote")+"\n"+s.Body;
     }
    }
    if(proposed!=null){var v=Simulation.Validate(Journal.State,proposed);if(!v.IsValid)s.Body+="\n"+(PatrolDiagnostic(v));}
    if(overlay=="preview"&&ConfirmMode=="two-step")s.Actions.Add(A("preview-next",L("next"),()=>{overlay="confirm";Render();}));
    else s.Actions.Add(A("confirm-submit",L("confirm"),()=>{_ = CommitAsync(proposed);},proposed!=null&&Simulation.Validate(Journal.State,proposed).IsValid,hold:ConfirmMode=="hold"?HoldSeconds:0));
   }else if(overlay=="recovery"){s.Body=status;RecoverySlots(s);s.Actions.Add(A("recovery-new-slot",L("newSlot"),()=>{Store=new AtomicSaveStore(Path.Combine(saveRootDirectory,"recovery-"+Guid.NewGuid().ToString("N")));saveId=Guid.NewGuid().ToString();createdUtc=DateTime.UtcNow.ToString("O");Journal=new CommandJournal(Simulation,SnapshotInterval);hintLevels.Clear();saveReadOnly=false;started=false;document=null;tool=null;ConfigureOpening(true);overlay=null;status=L("oldSavePreserved");Render();}));s.Actions.Add(A("recovery-retry",L("retry"),()=>{var loaded=Store.Load();if(loaded.Document!=null)try{Journal=JournalSave.Decode(loaded.Document,Simulation,SnapshotInterval);RestoreHintLevels(loaded.Document);saveReadOnly=false;saveId=(string)loaded.Document["saveId"];createdUtc=(string)loaded.Document["createdUtc"];ConfigureOpening(false);overlay=null;status=L("recovered");}catch(Exception e){status=e.Message;}Render();}));}
   s.Actions.Add(A("overlay-back",L("back"),()=>{overlay=null;Render();}));
  }
  public Task FlushSaves()=>Task.WhenAll(background,reviewBackground);
  void OnDestroy(){pendingCancel?.Cancel();ApplyStagePresentation(true);}
 }
}
