using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;
using Tide.UI;
using UnityEngine;

namespace Tide.App
{
    public sealed partial class T0GameSession
    {
        JObject patrolPacket;
        string ConfirmMode=>SignatureActive?(string)settings["c1SignatureConfirmMode"]??(string)signaturePacket["confirmation"]["defaultMode"]:PatrolActive?(string)settings["c1ConfirmMode"]??(string)patrolPacket["confirmation"]["defaultMode"]:(string)settings["confirmMode"];
        string shownStage="hub";
        GameObject patrolVisual;
        GameObject[] hiddenHubRoots;
        Vector3 hubCameraPosition;
        Quaternion hubCameraRotation;
        Color hubCameraBackground;
        CameraClearFlags hubCameraClearFlags;
        float hubCameraFov;
        public bool PatrolActive=>Journal.State.Has("c1:entered");
        public bool PatrolComplete=>Simulation.IsComplete(Journal.State,C1PatrolDefinition.BeatId);
        string PatrolText(string key)=>(string)patrolPacket["localization"]?[key]??key;
        string BeatFor(PuzzleState state)=>C1SignatureDefinition.Has(state,"entered")?C1SignatureDefinition.BeatId:state.Has("c1:entered")?C1PatrolDefinition.BeatId:
            Definition.Beats.FirstOrDefault(b=>b.Id!=C1PatrolDefinition.BeatId && b.Id!=C1SignatureDefinition.BeatId && Simulation.IsAvailable(state,b.Id)&&!Simulation.IsComplete(state,b.Id))?.Id??"t0-b3";
        public void ContinueToPatrol()
        {
            CloseTool();document=null;overlay=null;
            SubmitImmediate(new PuzzleCommand("EnterPatrol"));
        }
        PuzzleCommand PatrolConfirmation()=>new PuzzleCommand("ConfirmPatrol",Definition.Patrol.ConditionId,
            C1PatrolDefinition.Boolean(C1PatrolDefinition.Enabled(Journal.State,"lighting")),
            C1PatrolDefinition.Boolean(C1PatrolDefinition.Enabled(Journal.State,"reader")));
        string PatrolDiagnostic(ValidationResult verdict)=>verdict.Reason.HasValue?Reason(verdict.Reason.Value):
            verdict.DataDiagnostic!=null&&verdict.DataDiagnostic.StartsWith("c1.signature.")?SignatureText(verdict.DataDiagnostic,"작업의 앞 단계를 확인하세요."):verdict.DataDiagnostic!=null&&verdict.DataDiagnostic.StartsWith("c1.patrol.")?PatrolText(verdict.DataDiagnostic):L("unavailable");
        string PatrolConditionText()=>PatrolText((string)patrolPacket["journalCondition"]["labelKey"])+" · "+
            (string)patrolPacket["journalCondition"]["actorDisplayName"]+"\n“"+PatrolText((string)patrolPacket["journalCondition"]["textKey"])+"”";
        string PatrolCaseThread()
        {
            var count=Definition.Patrol.Observations.Count(id=>Journal.State.Has("c1:observed:"+id));
            return "C1 · "+PatrolText("c1.patrol.title")+"\n관찰 "+count+" / "+Definition.Patrol.Observations.Count+
                "\n"+(PatrolComplete?PatrolText("c1.patrol.complete"):
                "다음 행동: "+(count<Definition.Patrol.Observations.Count?"두 기록 확인":
                Journal.State.Get("c1:conditionCandidate")==null?"분기와 출처 조건 확인":"열람과 출처 조건 확정"));
        }
        void PatrolScreen(GameScreen screen)
        {
            screen.Title="C1 · "+PatrolText("c1.patrol.title");
            if(PatrolComplete)
            {
                screen.Body=PatrolText("c1.patrol.complete")+"\n\n"+PatrolConditionText()+
                    "\n\n다음 기록: 겹쳐 붙은 서명지 ";
                screen.Actions.Add(A("continue-c1-signature","서명지철 조사로 이동",ContinueToSignature));
                screen.Actions.Add(A("c1-review",L("evidence"),()=>OpenOverlay("evidence")));
                return;
            }
            var observation=patrolPacket["observations"].FirstOrDefault(o=>(string)o["id"]==document);
            if(observation!=null)
            {
                var id=(string)observation["id"];
                screen.Title=PatrolText((string)observation["labelKey"]);
                screen.Body=(string)observation["description"]+"\n\n출처: "+(string)observation["originId"]+
                    " · "+(string)observation["sourceType"]+"\n직접 기록 · 사본 계보 없음";
                screen.Actions.Add(A("c1-observe-"+id,(Journal.State.Has("c1:observed:"+id)?"✓ ":"")+"관찰 기록",
                    ()=>SubmitImmediate(new PuzzleCommand("ObservePatrol",id))));
                screen.Actions.Add(A("c1-close-observation",L("back"),()=>{document=null;Render();}));return;
            }
            screen.Body=(string)patrolPacket["narrative"]["objective"]+"\n\n"+PatrolText("c1.patrol.sharedSupply")+" → "+
                string.Join(" / ",patrolPacket["branches"].Select(b=>PatrolText((string)b["labelKey"])))+"\n"+
                PatrolText("c1.patrol.preview");
            foreach(var item in patrolPacket["observations"])
            {
                var id=(string)item["id"];screen.Actions.Add(A("c1-open-"+id,
                    (Journal.State.Has("c1:observed:"+id)?"✓ ":"")+PatrolText((string)item["labelKey"]),()=>{document=id;Render();}));
            }
            foreach(var branch in patrolPacket["branches"])
            {
                var id=(string)branch["id"];bool enabled=C1PatrolDefinition.Enabled(Journal.State,id);
                screen.Actions.Add(A("c1-toggle-"+id,PatrolText((string)branch["labelKey"])+" · "+PatrolText(enabled?"c1.patrol.enabled":"c1.patrol.folded"),
                    ()=>SubmitImmediate(new PuzzleCommand("SetPatrolBranch",id,C1PatrolDefinition.Boolean(!enabled)))));
            }
            bool accepted=Journal.State.Get("c1:conditionCandidate")==Definition.Patrol.ConditionId;
            screen.Actions.Add(A("c1-condition",(accepted?"✓ ":"")+PatrolText("c1.patrol.condition.label")+" 확인",
                ()=>SubmitImmediate(new PuzzleCommand("AcknowledgePatrol",Definition.Patrol.ConditionId,C1PatrolDefinition.Boolean(!accepted))),detail:PatrolConditionText()));
            screen.Actions.Add(A("c1-bypass",PatrolText("c1.patrol.bypass"),()=>SubmitImmediate(new PuzzleCommand("RecoverPatrol"))));
            var command=PatrolConfirmation();var verdict=Simulation.Validate(Journal.State,command);
            screen.Actions.Add(A("c1-confirm",PatrolText("c1.patrol.confirm"),()=>RequestConfirm(command),verdict.IsValid,
                verdict.IsValid?PatrolText("c1.patrol.preview"):PatrolDiagnostic(verdict)));
        }
        string PatrolConfirmationText()=>PatrolText("c1.patrol.preview")+"\n"+
            PatrolText("c1.patrol.valid.readerOnly")+"\n\n"+PatrolConditionText()+"\n\n확정하면 제3수문 열람 권한과 출처 조건이 함께 저장됩니다.";
        void ApplyStagePresentation(bool forceHub=false)
        {
            var camera=Camera.main;if(camera==null)return;
            string stage=forceHub||!started||!PatrolActive?"hub":SignatureActive?"signature":"patrol";
            if(stage==shownStage){ApplySignatureVisualState();return;}
            if(shownStage!="hub")
            {
                if(patrolVisual!=null){patrolVisual.SetActive(false);Destroy(patrolVisual);patrolVisual=null;}
                foreach(var item in hiddenHubRoots??new GameObject[0])if(item!=null)item.SetActive(true);
                camera.transform.SetPositionAndRotation(hubCameraPosition,hubCameraRotation);camera.backgroundColor=hubCameraBackground;camera.clearFlags=hubCameraClearFlags;cameraHorizontalFov=hubCameraFov;
            }
            shownStage=stage;if(stage=="hub")return;
            hubCameraPosition=camera.transform.position;hubCameraRotation=camera.transform.rotation;
            hubCameraBackground=camera.backgroundColor;hubCameraClearFlags=camera.clearFlags;hubCameraFov=cameraHorizontalFov;
            var hub=UnityEngine.SceneManagement.SceneManager.GetSceneByName("hub");
            hiddenHubRoots=hub.IsValid()&&hub.isLoaded?hub.GetRootGameObjects().Where(g=>g.activeSelf&&g.GetComponentsInChildren<Renderer>().Length>0).ToArray():new GameObject[0];
            foreach(var item in hiddenHubRoots)item.SetActive(false);
            camera.clearFlags=CameraClearFlags.SolidColor;
            if(stage=="signature")
            {
                var view=Resources.Load<C1SignatureViewSettings>("C1SignatureView");if(view==null){camera.backgroundColor=new Color(.035f,.065f,.075f);return;}
                camera.transform.position=view.cameraPosition;camera.transform.LookAt(view.lookAt);camera.backgroundColor=view.background;cameraHorizontalFov=view.horizontalFov;
                if(view.reader!=null&&(view.runtimeApproved||System.Environment.GetCommandLineArgs().Contains("--c1-signature-diagnostic")))
                {patrolVisual=Instantiate(view.reader,transform);patrolVisual.name="C1 signature reader";ApplySignatureVisualState();}
            }
            else
            {
                var view=Resources.Load<C1ViewSettings>("C1View");if(view==null)return;
                camera.transform.position=view.cameraPosition;camera.transform.LookAt(view.lookAt);camera.backgroundColor=view.background;cameraHorizontalFov=view.horizontalFov;
                if(view.panel!=null&&(view.runtimeApproved||System.Environment.GetCommandLineArgs().Contains("--c1-panel-diagnostic")))
                {patrolVisual=Instantiate(view.panel,transform);patrolVisual.name="C1 static system panel";}
            }
        }
        void ApplySignatureVisualState()
        {
            if(!SignatureActive||patrolVisual==null)return;
            foreach(var item in patrolVisual.GetComponentsInChildren<Transform>(true))
            {
                if(item.name=="STATE_SaltAdhesion")item.gameObject.SetActive(!SignatureHas("separated"));
                if(item.name=="STATE_LowerObscuration")item.gameObject.SetActive(true);
            }
        }
    }
}
