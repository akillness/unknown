#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

// The editor selects compatible versions; Packages/packages-lock.json records them.
public static class PackageBootstrap
{
    private static AddAndRemoveRequest request;

    public static void Run()
    {
        request = Client.AddAndRemove(new[]
        {
            "com.unity.inputsystem", "com.unity.ugui", "com.unity.localization",
            "com.unity.test-framework", "com.unity.render-pipelines.universal"
        }, new[] { "com.unity.multiplayer.center" });
        EditorApplication.update += Poll;
    }

    private static void Poll()
    {
        if (!request.IsCompleted) return;
        EditorApplication.update -= Poll;
        if (request.Status != StatusCode.Success)
        {
            Debug.LogError("Required package resolution failed: " + request.Error?.message);
            EditorApplication.Exit(1);
            return;
        }
        Debug.Log("Required package resolution completed; inspect packages-lock.json.");
        EditorApplication.Exit(0);
    }
}
#endif
