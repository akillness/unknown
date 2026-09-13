using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using Tide.Sim;
using UnityEngine;

namespace Tide.App
{
    // RFC-CX-013 lane ReaderStage: M7 optical reader 3D stage (mesh, record set, lighting, crank motion).
    // Gate: profile.runtimeApproved || --m7-reader-diagnostic. Fallback = hub stays shown, no stage change.
    // Camera is posed once on entry and never touched again (presentation/cinematic-gameplay-m6.md:11); C1GameSession restores it on exit.
    public sealed partial class T0GameSession
    {
        public const string M7ReaderVisualName="M7 optical reader";
        public const string M7ReaderLampName="M7 reader lamp";
        public const string M7ReaderFillName="M7 reader fill";
        M7ReaderStageProfile readerStageProfile;
        bool readerStageProfileLoaded;
        GameObject m7ReaderVisual;
        bool readerPresentationListening;
        M7ReaderStageProfile ReaderStageProfile
        {
            get
            {
                if(!readerStageProfileLoaded){readerStageProfileLoaded=true;readerStageProfile=Resources.Load<M7ReaderStageProfile>("M7ReaderStage");}
                return readerStageProfile;
            }
        }
        bool M7ReaderGate=>ReaderStageProfile!=null&&(ReaderStageProfile.runtimeApproved||Environment.GetCommandLineArgs().Contains("--m7-reader-diagnostic"));
        // True when the reader stage should be shown instead of the hub (stage=="reader").
        bool M7ReaderStageEnabled=>tool=="reader"&&!PatrolActive&&M7ReaderGate;
        public bool M7ReaderStageActive=>shownStage=="reader";
        // Called by ApplyStagePresentation after hub roots are hidden and camera state saved.
        void ApplyM7ReaderStage(Camera camera)
        {
            if(m7ReaderVisual!=null){m7ReaderVisual.SetActive(false);Destroy(m7ReaderVisual);m7ReaderVisual=null;}
            var profile=ReaderStageProfile;if(profile==null)return;
            camera.transform.position=profile.cameraPosition;camera.transform.LookAt(profile.lookAt);camera.backgroundColor=profile.background;cameraHorizontalFov=profile.horizontalFov;
            m7ReaderVisual=new GameObject(M7ReaderVisualName);
            m7ReaderVisual.transform.SetParent(transform,false);
            if(profile.reader!=null){var reader=Instantiate(profile.reader,m7ReaderVisual.transform);reader.name=profile.reader.name;reader.transform.localPosition=profile.readerLocalPosition;}
            if(profile.recordSet!=null){var records=Instantiate(profile.recordSet,m7ReaderVisual.transform);records.name=profile.recordSet.name;records.transform.localPosition=profile.recordSetLocalPosition;}
            M7ReaderLight(M7ReaderLampName,profile.lampColor,profile.lampIntensity,profile.lampRange,profile.lampOffset);
            M7ReaderLight(M7ReaderFillName,profile.fillColor,profile.fillIntensity,profile.fillRange,profile.fillOffset);
            var pivot=m7ReaderVisual.GetComponentsInChildren<Transform>(true).FirstOrDefault(t=>t.name==profile.crankPivotName);
            var vfxAsset=Resources.Load<TextAsset>("T0ReaderVfx");JObject vfx=null;
            if(vfxAsset!=null)try{vfx=JObject.Parse(vfxAsset.text);}catch(Newtonsoft.Json.JsonException){vfx=null;}
            m7ReaderVisual.AddComponent<M7ReaderStageVisual>().Initialize(()=>started&&tool=="reader"&&!PatrolActive&&M7ReaderStageActive,()=>ReducedMotion,pivot,vfx,SuppressHubLights(),ClearM22Embodiment);
            if(!readerPresentationListening){Interface.ScreenChanged+=RefreshM7ReaderPresentation;readerPresentationListening=true;}
        }
        // RFC-CX-017: only a successful deliberate read may move the crank; render/navigation is not an action.
        private void PresentReaderAction(PuzzleCommand command)
        {
            if(command.CommandId!="Read"&&command.CommandId!="ReadOriginal")return;
            if(m7ReaderVisual!=null)m7ReaderVisual.GetComponent<M7ReaderStageVisual>().PlayStroke();
        }
        void RefreshM7ReaderPresentation()
        {
            if(m7ReaderVisual!=null)m7ReaderVisual.GetComponent<M7ReaderStageVisual>().RefreshPresentation();
        }
        // RFC-CX-016: ApplyStagePresentation hides hub roots by "activeSelf && has a Renderer", so the committed
        // "Watch room lamp" (a Light with no Renderer) kept lighting the reader stage. Measured: 5.5-5.7% of the scene
        // viewport clipped to 255 on all channels, and lowering the stage lamp 2.2 -> 1.25 moved it only 0.18 points,
        // because a directional light has no distance falloff for the profile lamp to compete with. The stage now owns
        // its own light list; every foreign enabled light is disabled for the duration and restored by
        // M7ReaderStageVisual.OnDisable (before deferred destruction or same-frame re-entry).
        Light[] SuppressHubLights()
        {
            var hub=UnityEngine.SceneManagement.SceneManager.GetSceneByName("hub");
            if(!hub.IsValid()||!hub.isLoaded)return new Light[0];
            var foreign=hub.GetRootGameObjects().SelectMany(root=>root.GetComponentsInChildren<Light>(true))
                .Where(light=>light.enabled&&!light.transform.IsChildOf(m7ReaderVisual.transform)).ToArray();
            foreach(var light in foreign)light.enabled=false;
            return foreign;
        }
        void M7ReaderLight(string lightName,Color color,float intensity,float range,Vector3 offset)
        {
            var light=new GameObject(lightName,typeof(Light)).GetComponent<Light>();
            light.transform.SetParent(m7ReaderVisual.transform,false);light.transform.localPosition=offset;
            light.type=LightType.Point;light.color=color;light.intensity=intensity;light.range=range;light.shadows=LightShadows.None;
        }
    }

    // Crank stroke driver on the "M7 optical reader" root. Self-destroys once the session leaves the reader stage.
    // Stroke = forward to crank_stroke_deg over crank_forward_ms, hold crank_hold_ms, return over crank_return_ms (Resources/T0ReaderVfx.json),
    // on an unscaled monotonic clock anchored at the accepted action, not the preceding frame's duration.
    public sealed class M7ReaderStageVisual:MonoBehaviour
    {
        Func<bool> stillActive,reducedMotion;
        Transform pivot;
        Quaternion restRotation;
        float forwardMs,holdMs,returnMs,strokeDeg,elapsedMs,angle;
        double strokeStartedAt;
        bool strokePlaying;
        bool applicationPaused,applicationFocused=true;
        Light[] suppressed=Array.Empty<Light>();
        Action stageDeparted;
        public Transform CrankPivot=>pivot;
        public bool StrokePlaying=>strokePlaying;
        public float CrankAngleDeg=>angle;
        public void Initialize(Func<bool> stillActive,Func<bool> reducedMotion,Transform pivot,JObject vfx,Light[] suppressedHubLights=null,Action stageDeparted=null)
        {
            this.stillActive=stillActive;this.reducedMotion=reducedMotion;this.pivot=pivot;
            this.stageDeparted=stageDeparted;
            restRotation=pivot==null?Quaternion.identity:pivot.localRotation;
            suppressed=suppressedHubLights??Array.Empty<Light>();
            forwardMs=Ms(vfx,"crank_forward_ms");holdMs=Ms(vfx,"crank_hold_ms");returnMs=Ms(vfx,"crank_return_ms");
            strokeDeg=vfx==null?0:(float?)vfx["crank_stroke_deg"]??0;
            SetAngle(0);
        }
        void OnApplicationPause(bool paused){applicationPaused=paused;if(paused)CancelStroke();}
        void OnApplicationFocus(bool focused){applicationFocused=focused;if(!focused)CancelStroke();}
        // Release synchronously: a replacement stage must not borrow lights until this owner returns them.
        void OnDisable()
        {
            CancelStroke();
            foreach(var light in suppressed)if(light!=null)light.enabled=true;
            stageDeparted?.Invoke();stageDeparted=null;
            suppressed=Array.Empty<Light>();
        }
        static float Ms(JObject vfx,string key)=>vfx==null?0:Mathf.Max(0,(float?)vfx[key]??0);
        public void RefreshPresentation()
        {
            if(!isActiveAndEnabled)return;
            if(stillActive==null||!stillActive()){gameObject.SetActive(false);Destroy(gameObject);return;}
            if(reducedMotion!=null&&reducedMotion())CancelStroke();
        }
        void Update()
        {
            RefreshPresentation();
            if(!strokePlaying)return;
            elapsedMs=(float)((Time.realtimeSinceStartupAsDouble-strokeStartedAt)*1000);
            if(elapsedMs<forwardMs)SetAngle(strokeDeg*Ease(elapsedMs/forwardMs));
            else if(elapsedMs<forwardMs+holdMs)SetAngle(strokeDeg);
            else if(elapsedMs<forwardMs+holdMs+returnMs)SetAngle(strokeDeg*(1-Ease((elapsedMs-forwardMs-holdMs)/returnMs)));
            else CancelStroke();
        }
        public void PlayStroke()
        {
            RefreshPresentation();
            if(!isActiveAndEnabled||applicationPaused||!applicationFocused||pivot==null||reducedMotion!=null&&reducedMotion())return;
            elapsedMs=0;strokeStartedAt=Time.realtimeSinceStartupAsDouble;strokePlaying=forwardMs+holdMs+returnMs>0;
            SetAngle(0);
        }
        static float Ease(float value)=>value*value*(3-2*value);
        void CancelStroke()
        {
            strokePlaying=false;elapsedMs=0;SetAngle(0);
        }
        void SetAngle(float value)
        {
            angle=value;
            if(pivot!=null)pivot.localRotation=restRotation*Quaternion.AngleAxis(value,Vector3.right);
        }
    }
}
