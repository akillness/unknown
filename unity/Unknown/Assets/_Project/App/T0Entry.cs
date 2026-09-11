using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Tide.App {
 public sealed class T0Entry:MonoBehaviour {
  IEnumerator Start(){
   Debug.Log("T0_BOOT entry-start");
   DontDestroyOnLoad(gameObject);
   if(!SceneManager.GetSceneByName("ui-root").isLoaded)yield return SceneManager.LoadSceneAsync("ui-root",LoadSceneMode.Additive);
   Debug.Log("T0_BOOT ui-root-loaded");
   if(!SceneManager.GetSceneByName("hub").isLoaded)yield return SceneManager.LoadSceneAsync("hub",LoadSceneMode.Additive);
   Debug.Log("T0_BOOT hub-loaded");
   gameObject.AddComponent<T0GameSession>().Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"));
   Debug.Log("T0_BOOT session-initialized");
  }
 }
}
