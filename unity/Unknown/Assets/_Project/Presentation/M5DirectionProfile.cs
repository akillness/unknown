using UnityEngine;
namespace Tide.Presentation {
    [CreateAssetMenu(menuName="Unknown/M5 Direction")]
    public sealed class M5DirectionProfile:ScriptableObject {
        public bool runtimeApproved;
        public Texture2D openingImage;
        public Texture2D sectionSurface;
        public float firstShotSeconds=3.125f;
        public float secondShotSeconds=2.875f;
        public string firstTitle="마지막 당직";
        [TextArea] public string firstCaption="폐국 전날 밤, 마지막 당직을 맡았다.";
        public string secondTitle="목록과 서랍";
        [TextArea] public string secondCaption="목록과 서랍을 대조해, 무엇을 남길지 정한다.";
        public string observeTitle="관찰 · 원본을 펼쳐 확인";
        public string trialTitle="시험 · 선택한 습도";
        public string recordTitle="기록 · 결과와 근거를 따로 확인";
        public string recordPreparation="사본과 가림 표시, 대조 근거를 준비합니다.";
    }
}
