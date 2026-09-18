namespace Tide.UI
{
    // M25 (RFC-CX-M25-20260918): one named type scale for every uGUI text the interface creates.
    // Tiers are ordered so readers can tell them apart by size alone; weight and tone add the second
    // and third cue (colour + form + position, never colour alone — style-guide §2.1 / runbook §5-8).
    // Contract kept by the PlayMode tests: Body > Label > Helper, only Label/Section/Display carry Bold.
    public static class TypeScale
    {
        public const int Display=30;   // opening heading, header title (Bold)
        public const int Title=24;     // overlay / document titles inside the work surface (Bold)
        public const int Body=21;      // reading paragraphs
        public const int Section=18;   // section headings, case-thread card (Bold on paper/ink)
        public const int Label=18;     // action plate labels (Bold)
        public const int Status=17;    // save / commit status line (Bold, ochre)
        public const int Helper=15;    // contextual helper under a label (Normal, muted)
        public const int Meta=14;      // subtitle, footer, chart legends, captions
        public const float LineSpacing=1.15f;
    }
}
