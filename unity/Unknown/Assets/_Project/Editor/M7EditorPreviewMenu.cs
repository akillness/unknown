#if UNITY_EDITOR
using System.IO;
using System.Linq;
using Tide.App;
using Tide.Presentation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tide.EditorTools
{
    // RFC-CX-013: make "open the project in Unity Hub and press Play" a usable verification path.
    //
    // Two gaps this closes:
    //  1. A fresh Hub open lands on an untitled empty scene (Library/ is gitignored, so there is no
    //     remembered scene on a clean clone). T0Entry lives in boot.unity, so Play did nothing.
    //  2. The M7 gates could only be opened by a player command-line flag or by promotion.
    //
    // Nothing here writes runtimeApproved. Promotion remains Tools/M7/Approve … + a decision-log audit.
    public static class M7EditorPreviewMenu
    {
        const string BootScene = "Assets/_Project/Scenes/boot.unity";
        const string PreviewMenu = "Tools/M7/Editor preview — show M7 candidates in Play mode";
        const string AutoOpenedKey = "Tide.T0.BootSceneAutoOpened";

        [MenuItem(PreviewMenu, priority = 200)]
        static void TogglePreview()
        {
            var next = !EditorPrefs.GetBool(T0GameSession.M7EditorPreviewPref, false);
            EditorPrefs.SetBool(T0GameSession.M7EditorPreviewPref, next);
            Menu.SetChecked(PreviewMenu, next);
            if (next)
                Debug.Log("M7_EDITOR_PREVIEW on — hub shell, UI skin and reader stage render in Editor Play mode. " +
                          "This machine only (EditorPrefs); runtimeApproved stays false and no asset is modified. " +
                          "Turn it off before running PlayMode tests from the Editor GUI: the GateOff contract tests " +
                          "assert the committed look (headless runs ignore this switch via Application.isBatchMode).");
            else
                Debug.Log("M7_EDITOR_PREVIEW off — Play mode shows the committed pre-M7 look.");
        }

        [MenuItem(PreviewMenu, validate = true)]
        static bool ValidateTogglePreview()
        {
            Menu.SetChecked(PreviewMenu, EditorPrefs.GetBool(T0GameSession.M7EditorPreviewPref, false));
            return true;
        }

        [MenuItem("Tools/T0/Open boot scene", priority = 100)]
        public static void OpenBootScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.OpenScene(BootScene);
        }

        [MenuItem("Tools/T0/Play from boot scene", priority = 101)]
        public static void PlayFromBootScene()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            OpenBootScene();
            if (SceneManager_ActiveScenePath() != BootScene) return;
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Tools/M7/Report gate state", priority = 201)]
        public static void ReportGateState()
        {
            var hub = Resources.Load<M7HubProfile>("M7Hub");
            var ui = Resources.Load<M7UiSkinProfile>("M7UiSkin");
            var reader = Resources.Load<M7ReaderStageProfile>("M7ReaderStage");
            Debug.Log("M7_GATE_STATE editorPreview=" + EditorPrefs.GetBool(T0GameSession.M7EditorPreviewPref, false)
                      + " hub{asset=" + (hub != null) + ",approved=" + (hub != null && hub.runtimeApproved) + ",materials=" + (hub != null && hub.floorAndWall != null) + "}"
                      + " ui{asset=" + (ui != null) + ",approved=" + (ui != null && ui.runtimeApproved) + ",paper=" + (ui != null && ui.paperPanel != null) + "}"
                      + " reader{asset=" + (reader != null) + ",approved=" + (reader != null && reader.runtimeApproved) + ",prefab=" + (reader != null && reader.reader != null) + "}"
                      + " — if an asset or texture is missing run Tools/M7/Import all M7 candidates first.");
        }

        // Headless self-check for the Editor-verification wiring. Proves what a batch run can prove:
        // the boot scene exists with its entry point and is build scene 0, the pref round-trips, and the
        // batch-mode guard holds. It cannot prove the interactive Hub open and Play render — that needs a
        // human Editor session.
        public static void VerifyEditorPreviewWiring()
        {
            var problems = new System.Collections.Generic.List<string>();
            if (!File.Exists(BootScene)) problems.Add("missing " + BootScene);
            EditorSceneManager.OpenScene(BootScene);
            var entry = UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                .GetRootGameObjects().Any(g => g.GetComponent<T0Entry>() != null);
            if (!entry) problems.Add("boot scene has no T0Entry root");
            var buildScenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
            if (buildScenes.FirstOrDefault() != BootScene) problems.Add("boot is not build scene 0: " + string.Join(",", buildScenes));

            var original = EditorPrefs.GetBool(T0GameSession.M7EditorPreviewPref, false);
            try
            {
                EditorPrefs.SetBool(T0GameSession.M7EditorPreviewPref, true);
                if (!EditorPrefs.GetBool(T0GameSession.M7EditorPreviewPref, false)) problems.Add("pref does not round-trip");
                if (T0GameSession.M7EditorPreview) problems.Add("batch-mode guard failed: preview reported open in batch mode");
            }
            finally { EditorPrefs.SetBool(T0GameSession.M7EditorPreviewPref, original); }

            Debug.Log("M7_PREVIEW_WIRING " + (problems.Count == 0 ? "OK" : "PROBLEMS: " + string.Join(" | ", problems))
                      + " bootScene=" + BootScene + " entryPoint=" + entry
                      + " buildScene0=" + buildScenes.FirstOrDefault() + " prefKey=" + T0GameSession.M7EditorPreviewPref);
            if (problems.Count > 0) throw new System.InvalidOperationException("M7 preview wiring incomplete: " + string.Join(" | ", problems));
        }

        // A fresh Hub open has no remembered scene. Open boot once per Editor session so Play works
        // immediately; never touch a scene the user is already editing.
        [InitializeOnLoadMethod]
        static void AutoOpenBootSceneOnce()
        {
            if (Application.isBatchMode) return;
            if (SessionState.GetBool(AutoOpenedKey, false)) return;
            SessionState.SetBool(AutoOpenedKey, true);
            EditorApplication.delayCall += () =>
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode) return;
                var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                if (!string.IsNullOrEmpty(active.path) || active.isDirty || active.rootCount > 0) return;
                if (!File.Exists(BootScene)) return;
                EditorSceneManager.OpenScene(BootScene);
                Debug.Log("T0_BOOT_SCENE_OPENED " + BootScene + " — press Play to run. " +
                          "Enable Tools/M7/Editor preview to see the M7 candidates.");
            };
        }

        static string SceneManager_ActiveScenePath() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
    }
}
#endif
