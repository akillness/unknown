using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
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
            m7ReaderVisual.AddComponent<M7ReaderStageVisual>().Initialize(()=>M7ReaderStageActive,()=>phasePage*2+(phaseStart?1:0),()=>ReducedMotion,pivot,vfx,SuppressHubLights());
        }
        // RFC-CX-016: ApplyStagePresentation hides hub roots by "activeSelf && has a Renderer", so the committed
        // "Watch room lamp" (a Light with no Renderer) kept lighting the reader stage. Measured: 5.5-5.7% of the scene
        // viewport clipped to 255 on all channels, and lowering the stage lamp 2.2 -> 1.25 moved it only 0.18 points,
        // because a directional light has no distance falloff for the profile lamp to compete with. The stage now owns
        // its own light list; every foreign enabled light is disabled for the duration and restored by
        // M7ReaderStageVisual.OnDestroy (the same hook that runs on self-destruct, re-entry and session teardown).
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
    // on Time.unscaledDeltaTime. reducedMotion: every phase uses reduced_motion_ms; <=0 snaps to the rest pose without animating.
    public sealed class M7ReaderStageVisual:MonoBehaviour
    {
        Func<bool> stillActive;Func<int> strokeKey;Func<bool> reducedMotion;
        Transform pivot;
        float forwardMs,holdMs,returnMs,strokeDeg,reducedMs;
        bool initialised;int lastKey;Coroutine stroke;float angle;
        Light[] suppressed=new Light[0];
        // Foreign hub lights the stage borrowed exclusivity from. Public so a test can assert the restore contract.
        public IReadOnlyList<Light> SuppressedHubLights=>suppressed;
        public Transform CrankPivot=>pivot;
        public bool StrokePlaying=>stroke!=null;
        public float CrankAngleDeg=>angle;
        public void Initialize(Func<bool> stillActive,Func<int> strokeKey,Func<bool> reducedMotion,Transform pivot,JObject vfx,Light[] suppressedHubLights=null)
        {
            this.stillActive=stillActive;this.strokeKey=strokeKey;this.reducedMotion=reducedMotion;this.pivot=pivot;
            suppressed=suppressedHubLights??new Light[0];
            forwardMs=Ms(vfx,"crank_forward_ms");holdMs=Ms(vfx,"crank_hold_ms");returnMs=Ms(vfx,"crank_return_ms");reducedMs=Ms(vfx,"reduced_motion_ms");
            strokeDeg=vfx==null?0:(float?)vfx["crank_stroke_deg"]??0;
            SetAngle(0);
        }
        // Runs on self-destruct (Update), on stage re-entry (ApplyM7ReaderStage destroys the old visual) and on
        // session teardown, so a suppressed hub light can never outlive the stage that borrowed it.
        void OnDestroy()
        {
            foreach(var light in suppressed)if(light!=null)light.enabled=true;
            suppressed=new Light[0];
        }
        static float Ms(JObject vfx,string key)=>vfx==null?0:(float?)vfx[key]??0;
        void Update()
        {
            if(stillActive==null||!stillActive()){gameObject.SetActive(false);Destroy(gameObject);return;}
            var key=strokeKey==null?0:strokeKey();
            if(!initialised){initialised=true;lastKey=key;PlayStroke();return;}
            if(key==lastKey)return;
            lastKey=key;PlayStroke();
        }
        void PlayStroke()
        {
            if(stroke!=null){StopCoroutine(stroke);stroke=null;}
            if(pivot==null)return;
            bool reduced=reducedMotion!=null&&reducedMotion();
            if(reduced&&reducedMs<=0){SetAngle(0);return;}
            stroke=StartCoroutine(Stroke(reduced?reducedMs:forwardMs,reduced?reducedMs:holdMs,reduced?reducedMs:returnMs));
        }
        IEnumerator Stroke(float forward,float hold,float back)
        {
            float duration=forward/1000f,elapsed=0;
            while(elapsed<duration){elapsed+=Time.unscaledDeltaTime;SetAngle(strokeDeg*Mathf.Clamp01(elapsed/duration));yield return null;}
            SetAngle(strokeDeg);
            duration=hold/1000f;elapsed=0;
            while(elapsed<duration){elapsed+=Time.unscaledDeltaTime;yield return null;}
            duration=back/1000f;elapsed=0;
            while(elapsed<duration){elapsed+=Time.unscaledDeltaTime;SetAngle(strokeDeg*(1-Mathf.Clamp01(elapsed/duration)));yield return null;}
            SetAngle(0);stroke=null;
        }
        void SetAngle(float value)
        {
            angle=value;
            if(pivot!=null)pivot.localRotation=value==0?Quaternion.identity:Quaternion.AngleAxis(value,Vector3.right);
        }
    }
}
