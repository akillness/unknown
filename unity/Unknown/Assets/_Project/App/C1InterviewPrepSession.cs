using Tide.UI;

namespace Tide.App
{
    // M18 — C1 interview preparation surface. Display-only: it announces that an interview format
    // exists and shows its shape (two anonymous speaker lanes, one neutral comparison prompt, an
    // explicit return). It submits no PuzzleCommand, writes no journal / save / receipt, reveals no
    // hint level and changes no focus beyond the explicit panel and return, so it can neither
    // advance nor leak puzzle state.
    //
    // Authored contract: _workspace/current/presentation/c1-interview-prep-m18.md
    //
    // Disclosure boundary (contract §2): this screen carries no character name, no record/source
    // display name, no exact value or time, no canonical statement, no corrective action and no
    // solution path. It is NOT the canonical c1-b4 dialogue beat and NOT a substitute for it.
    public sealed partial class T0GameSession
    {
        internal const string InterviewPrepOverlay="interviewPrep";
        // Gate (contract §3): only the completed C1 patrol surface offers this, never a fresh save,
        // never mid-patrol, and never once the next beat has been entered.
        public bool InterviewPrepAvailable=>started&&!OpeningActive&&PatrolActive&&PatrolComplete&&!SignatureActive;
        public void OpenInterviewPrep(){if(!InterviewPrepAvailable)return;OpenOverlay(InterviewPrepOverlay);}
        void CloseInterviewPrep(){if(overlay!=InterviewPrepOverlay)return;overlay=null;Render();}
        void InterviewPrepScreen(GameScreen screen)
        {
            screen.Title="C1 · 면접 준비";
            screen.Body=
                "다음 단계에는 면접 절차가 있습니다. 이 화면은 그 형식만 안내합니다.\n\n"+
                "발화자 A · 진술 레인\n"+
                "발화자 B · 진술 레인\n\n"+
                "두 레인을 같은 기준으로 나란히 비교합니다.\n\n"+
                "상대와 진술 내용은 아직 열리지 않았습니다. 이 화면에서는 아무것도 결정하지 않고 기록도 바뀌지 않습니다.";
            screen.Actions.Add(A("c1-interview-prep-back","순찰 요약으로 돌아가기",CloseInterviewPrep));
        }
    }
}
