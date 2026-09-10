using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Tide.App {
 public sealed class T0Entry:MonoBehaviour {
  IEnumerator Start(){
   DontDestroyOnLoad(gameObject);
   if(!SceneManager.GetSceneByName("ui-root").isLoaded)yield return SceneManager.LoadSceneAsync("ui-root",LoadSceneMode.Additive);
   if(!SceneManager.GetSceneByName("hub").isLoaded)yield return SceneManager.LoadSceneAsync("hub",LoadSceneMode.Additive);
   gameObject.AddComponent<T0GameSession>().Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"));
  }
 }
}
