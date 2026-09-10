using UnityEngine;
using Tide.Data;
namespace Tide.App {
 [CreateAssetMenu(menuName="T0/Runtime config")]
 public sealed class T0RuntimeConfig:ScriptableObject {
  public T0CatalogAsset catalog;
  public TextAsset records,zones,tools,hints,bindings,strings,savePolicy;
 }
}
