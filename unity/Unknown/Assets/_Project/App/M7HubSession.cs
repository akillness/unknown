using System;
using System.Linq;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Tide.App
{
    // RFC-CX-013 lane HubShell: M7 concept-first watchroom shell materials.
    // Gate: profile.runtimeApproved || --m7-hub-diagnostic. Fallback = committed hub scene untouched.
    public sealed partial class T0GameSession
    {
        public const string M7HubLampName="M7 hub lamp";
        M7HubProfile hubProfile;
        bool M7HubEnabled=>hubProfile!=null&&(hubProfile.runtimeApproved||Environment.GetCommandLineArgs().Contains("--m7-hub-diagnostic"));
        public static int M7HubLampCount(Scene scene)=>scene.IsValid()&&scene.isLoaded?scene.GetRootGameObjects().Count(x=>x.name==M7HubLampName):0;
        void BindM7Hub()
        {
            hubProfile=Resources.Load<M7HubProfile>("M7Hub");
            if(!M7HubEnabled)return;
            ApplyM7HubShell(SceneManager.GetSceneByName("hub"),hubProfile);
        }
        // Shared with Editor/M7ProjectBuilder.Capture so the native proof shot uses the exact runtime path. Idempotent.
        public static void ApplyM7HubShell(Scene hub,M7HubProfile profile)
        {
            if(profile==null||!hub.IsValid()||!hub.isLoaded)return;
            var roots=hub.GetRootGameObjects();
            foreach(var root in roots)
            {
                // Approved r03 drawer keeps its committed casing/tray materials; anything not listed in the profile is untouched.
                if(root.name=="T0 approved r03 drawer")continue;
                var material=M7HubMaterialFor(profile,root.name);
                if(material==null)continue;
                foreach(var renderer in root.GetComponentsInChildren<Renderer>(true))renderer.sharedMaterial=material;
            }
            RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=profile.ambient;
            if(roots.Any(x=>x.name==M7HubLampName))return;
            var lamp=new GameObject(M7HubLampName);
            SceneManager.MoveGameObjectToScene(lamp,hub);
            var light=new GameObject("M7 hub lamp light",typeof(Light)).GetComponent<Light>();
            light.transform.SetParent(lamp.transform,false);light.transform.localPosition=profile.lampLocalPosition;
            light.type=LightType.Point;light.color=profile.lampColor;light.intensity=profile.lampIntensity;light.range=profile.lampRange;light.shadows=LightShadows.None;
        }
        static Material M7HubMaterialFor(M7HubProfile profile,string root)
        {
            if(Listed(profile.floorAndWallRoots,root))return profile.floorAndWall;
            if(Listed(profile.workbenchRoots,root))return profile.workbench;
            if(Listed(profile.plateShelfRoots,root))return profile.plateShelf;
            return null;
        }
        static bool Listed(string[] names,string root)=>names!=null&&Array.IndexOf(names,root)>=0;
    }
}
