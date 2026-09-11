using System;
using System.Linq;
using Tide.Presentation;
using Tide.UI;
using UnityEngine;
namespace Tide.App {
    public sealed partial class T0GameSession {
        M5DirectionProfile directionProfile;
        bool openingEligible,openingFinished,openingPaused,openingFocused=true,openingReplay;
        string openingReturnOverlay;
        float openingElapsed;
        int openingCaption;
        public bool OpeningActive {get;private set;}
        public float OpeningElapsed=>openingElapsed;
        public int OpeningCaptionIndex=>openingCaption;
        bool DirectionEnabled=>directionProfile!=null&&(directionProfile.runtimeApproved||Environment.GetCommandLineArgs().Contains("--m5-direction-diagnostic"));
        bool ReducedMotion=>(bool?)(settings?["reducedMotion"])??false;
        void ConfigureOpening(bool noExistingSave){
            CancelOpening();openingFinished=false;
            directionProfile=Resources.Load<M5DirectionProfile>("M5Direction");
            openingEligible=noExistingSave&&Journal.HeadSeq==0&&!Simulation.IsComplete(Journal.State,"t0-b1");
        }
        void BeginOpeningOrStart(){
            if(saveReadOnly){StartGame();return;}
            if(!openingEligible||openingFinished||!DirectionEnabled){StartGame();return;}
            openingReplay=false;BeginOpening();
        }
        void BeginOpening(){OpeningActive=true;openingElapsed=0;openingCaption=0;overlay=null;Render();}
        void ReplayOpening(){if(!DirectionEnabled||SavePending)return;openingReplay=true;openingReturnOverlay=overlay;BeginOpening();}
        void FinishOpening(){
            OpeningActive=false;
            if(openingReplay){openingReplay=false;overlay=openingReturnOverlay;openingReturnOverlay=null;Render();return;}
            openingFinished=true;StartGame();
        }
        void CancelOpening(){OpeningActive=false;openingReplay=false;openingReturnOverlay=null;}
        void UpdateOpening(){
            if(!OpeningActive||openingPaused||!openingFocused||overlay!=null||ReducedMotion)return;
            openingElapsed+=Time.unscaledDeltaTime;
            if(openingElapsed>=directionProfile.firstShotSeconds+directionProfile.secondShotSeconds){FinishOpening();return;}
            int next=openingElapsed>=directionProfile.firstShotSeconds?1:0;
            if(next!=openingCaption){openingCaption=next;Render();}
        }
        void OpeningScreen(GameScreen screen){
            screen.Title=L("title");
            screen.ShowOpening=true;screen.OpeningImage=directionProfile.openingImage;
            screen.OpeningHeading=ReducedMotion?directionProfile.firstTitle:openingCaption==0?directionProfile.firstTitle:directionProfile.secondTitle;
            screen.Body=ReducedMotion?directionProfile.firstCaption+"\n\n"+directionProfile.secondTitle+"\n"+directionProfile.secondCaption:
                openingCaption==0?directionProfile.firstCaption:directionProfile.secondCaption;
            screen.Body+="\n\n관찰 · 시험 · 기록\n자료를 살피고, 조건을 시험하고, 근거를 기록하세요.";
            screen.Actions.Add(A("intro-skip",openingReplay?"돌아가기":ReducedMotion?"작업 시작":"건너뛰고 시작",FinishOpening));
            screen.Actions.Add(A("settings",L("settings"),()=>OpenOverlay("settings")));
            screen.Footer=ControlFooter();screen.Status=null;screen.CaseThread=null;
        }
        void OnApplicationPause(bool value){openingPaused=value;if(value)Watch?.NewContext();}
        void OnApplicationFocus(bool value){openingFocused=value;if(!value)Watch?.NewContext();}
    }
}
