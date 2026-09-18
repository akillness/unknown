using System;
using Tide.Presentation;
using Tide.Sim;
using UnityEngine;

namespace Tide.App
{
    public sealed partial class T0GameSession
    {
        public const string M22SeorinDiagnosticArg="--m22-seorin-diagnostic";
        M22EmbodimentProfile embodimentProfile;
        bool embodimentLoaded,embodimentListening,embodimentDiagnostic;
        M22EmbodimentVisual embodiment;
        string embodimentContext;
        M22EmbodimentProfile EmbodimentProfile
        {
            get
            {
                if(!embodimentLoaded)
                {
                    embodimentLoaded=true;embodimentProfile=Resources.Load<M22EmbodimentProfile>("M22Embodiment");
                    embodimentDiagnostic=Array.IndexOf(Environment.GetCommandLineArgs(),M22SeorinDiagnosticArg)>=0;
                    if((embodimentDiagnostic||embodimentProfile!=null&&embodimentProfile.runtimeApproved)&&(embodimentProfile==null||!embodimentProfile.Complete))throw new InvalidOperationException("M22 embodiment requires a complete Resources/M22Embodiment.asset. Run Tide.EditorTools.M22EmbodimentProjectBuilder.Import first.");
                }
                return embodimentProfile;
            }
        }
        bool M22EmbodimentEnabled=>EmbodimentProfile!=null&&EmbodimentProfile.Complete&&(EmbodimentProfile.runtimeApproved||EmbodimentProfile.diagnosticOverride||embodimentDiagnostic);
        // M25: the guide is read from the start screen, so Han Seorin stays on the title stage beneath it.
        bool M22TitleEnabled=>!started&&!OpeningActive&&(overlay==null||overlay==GuideOverlay)&&M22EmbodimentEnabled;
        void ApplyM22TitleStage(Camera camera)
        {
            var profile=EmbodimentProfile;
            camera.transform.SetPositionAndRotation(profile.cameraPosition,Quaternion.LookRotation(profile.lookAt-profile.cameraPosition));
            camera.backgroundColor=profile.background;cameraHorizontalFov=profile.horizontalFov;
            CreateM22Visual(camera,true,null);embodimentContext="title";
        }
        void CreateM22Visual(Camera camera,bool title,M7ReaderStageVisual reader)
        {
            var root=new GameObject(title?"M22 Han Seorin":"M22 Seorin hands");root.transform.SetParent(transform,false);
            embodiment=root.AddComponent<M22EmbodimentVisual>();
            try{embodiment.Initialize(EmbodimentProfile,title,camera,reader);embodiment.Refresh(ReducedMotion);}
            catch{ClearM22Embodiment();throw;}
            if(!embodimentListening){Interface.ScreenChanged+=CancelM22Motion;embodimentListening=true;}
        }
        void RefreshM22Embodiment()
        {
            if(!isActiveAndEnabled||!M22EmbodimentEnabled){ClearM22Embodiment();return;}
            if(shownStage=="seorin"){embodiment?.Refresh(ReducedMotion);return;}
            var profile=EmbodimentProfile;
            string context=null;
            if(started&&!OpeningActive&&overlay==null&&!PatrolActive)
            {
                if(tool=="reader"&&M7ReaderStageActive&&profile.showReaderHands)context="reader";
                else if(tool=="circuit"&&profile.showCircuitHands)context="circuit";
                else if(document!=null&&profile.showRecordHands)context="record:"+document;
            }
            if(context!=embodimentContext)
            {
                ClearM22Embodiment();
                if(context!=null)
                {
                    var camera=Camera.main;if(camera==null)return;
                    CreateM22Visual(camera,false,context=="reader"?m7ReaderVisual.GetComponent<M7ReaderStageVisual>():null);
                    embodimentContext=context;
                }
            }
            embodiment?.Refresh(ReducedMotion);
        }
        void PresentM22Action(PuzzleCommand command,bool durable)
        {
            if(embodiment==null||!started||OpeningActive||overlay!=null||ReducedMotion)return;
            switch(command.CommandId)
            {
                case "LoadRecord":if(!durable)embodiment.Play(M22EmbodimentVisual.Action.Insert);break;
                case "SetOverlayOffset":if(!durable)embodiment.Play(M22EmbodimentVisual.Action.Align);break;
                case "Read":if(!durable)embodiment.Play(M22EmbodimentVisual.Action.Read);break;
                case "ReadOriginal":if(durable)embodiment.Play(M22EmbodimentVisual.Action.ReadOriginal);break;
                case "CiteToBoard":if(durable)embodiment.Play(M22EmbodimentVisual.Action.Seal);break;
            }
        }
        void CancelM22Motion(){embodiment?.Cancel();}
        void ClearM22Embodiment()
        {
            if(embodiment!=null){embodiment.gameObject.SetActive(false);Destroy(embodiment.gameObject);embodiment=null;}
            embodimentContext=null;
        }
        void OnDisable()
        {
            ClearM22Embodiment();
            if(m7ReaderVisual!=null){m7ReaderVisual.SetActive(false);Destroy(m7ReaderVisual);m7ReaderVisual=null;}
            if(Camera.main==null)foreach(var root in hiddenHubRoots??Array.Empty<GameObject>())if(root!=null)root.SetActive(true);
            if(readerPresentationListening&&Interface!=null){Interface.ScreenChanged-=RefreshM7ReaderPresentation;readerPresentationListening=false;}
            if(embodimentListening&&Interface!=null){Interface.ScreenChanged-=CancelM22Motion;embodimentListening=false;}
            ApplyStagePresentation(true);
        }
        void OnEnable(){if(Journal!=null&&!bootFailed)Render();}
    }
}
