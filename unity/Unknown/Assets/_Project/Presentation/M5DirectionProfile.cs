using UnityEngine;
namespace Tide.Presentation {
    [CreateAssetMenu(menuName="Unknown/M5 Direction")]
    public sealed class M5DirectionProfile:ScriptableObject {
        public bool runtimeApproved;
        public Texture2D openingImage;
        public Texture2D sectionSurface;
        public float firstShotSeconds=3.125f;
        public float secondShotSeconds=2.875f;
        public string firstTitle="기록 앞에서";
        [TextArea] public string firstCaption="원본을 펼쳐 내용을 살펴봅니다.";
        public string secondTitle="직접 확인하고, 기록하기";
        [TextArea] public string secondCaption="도구로 확인한 결과를 근거와 함께 기록합니다.";
        public string observeTitle="관찰 · 원본을 펼쳐 확인";
        public string trialTitle="시험 · 선택한 습도";
        public string recordTitle="기록 · 결과와 근거를 따로 확인";
        public string recordPreparation="사본과 가림 표시, 대조 근거를 준비합니다.";
    }
}
