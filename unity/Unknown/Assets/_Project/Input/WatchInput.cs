using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
namespace Tide.Input {
 public sealed class WatchInput:MonoBehaviour {
  public event Action<int> Navigate; public event Action<Vector2,bool> Adjust;
  public event Action ToolWheel;
  public event Action BeginInteract,EndInteract,Preview,Undo,Redo,Query,Disconnect,Cancel;
  public event Action<string> Overlay; public event Action<int> Tool;
  public bool ToolPanel {get;set;} public bool OverlayActive {get;set;}
  public InputActionAsset Actions {get;private set;}
  public int ContextGeneration {get;private set;}
  public bool LastDeviceIsGamepad {get;private set;} public event Action<bool> DeviceChanged;
  int pressGeneration=-1,previewGeneration=-1;bool previewUndo,armed,navigationStick;float nextNavigation;
  IDisposable deviceListener;
  public bool TextEntryActive {get;private set;}
  public bool GameInputSuspended => TextEntryActive || awaitTextRelease;
  public bool ImeCompositionActive => imeComposing || Time.frameCount <= compositionChangedFrame + 1;
  public event Action TextEntryExitRequested;
  bool awaitTextRelease,imeComposing; int compositionChangedFrame=-100; Keyboard imeKeyboard;
  void OnComposition(UnityEngine.InputSystem.LowLevel.IMECompositionString value){imeComposing=value.Count>0;compositionChangedFrame=Time.frameCount;}
  public void SetTextEntry(bool active){
   if(TextEntryActive==active)return;
   NewContext();TextEntryActive=active;awaitTextRelease=!active;
   if(active){Actions?.FindActionMap("Watch").Disable();imeComposing=false;compositionChangedFrame=-100;}
  }
  void NoteDevice(InputDevice device){bool isPad=device is Gamepad;if(isPad!=LastDeviceIsGamepad){LastDeviceIsGamepad=isPad;DeviceChanged?.Invoke(isPad);}}
  InputActionRebindingExtensions.RebindingOperation rebinding;
  public void Initialize(string json,string overrides=null){
   deviceListener=InputSystem.onAnyButtonPress.Call(control=>NoteDevice(control.device));
   Actions=InputActionAsset.FromJson(json);if(!string.IsNullOrEmpty(overrides))Actions.LoadBindingOverridesFromJson(overrides);
   var map=Actions.FindActionMap("Watch",true);
   map.actionTriggered+=c=>{if(c.phase!=InputActionPhase.Started&&c.phase!=InputActionPhase.Performed)return;NoteDevice(c.control.device);};
   map.FindAction("Navigate").performed+=c=>{navigationStick=c.control.path.Contains("leftStick");Move(c.ReadValue<Vector2>());};
   map.FindAction("Interact").started+=c=>{pressGeneration=ContextGeneration;armed=true;BeginInteract?.Invoke();};
   map.FindAction("Interact").canceled+=c=>{if(armed&&pressGeneration==ContextGeneration)EndInteract?.Invoke();armed=false;};
   map.FindAction("Preview").started+=c=>{previewGeneration=ContextGeneration;previewUndo=Gamepad.current?.leftShoulder.isPressed??false;};
   map.FindAction("Preview").canceled+=c=>{if(previewGeneration!=ContextGeneration)return;if(previewUndo)Undo?.Invoke();else Preview?.Invoke();};
   map.FindAction("UndoRedo").performed+=c=>{if(c.ReadValue<float>()<=0)return;var key=Role(c);bool pad=c.control.device is Gamepad;if(pad){if(Gamepad.current.leftShoulder.isPressed)Redo?.Invoke();else Cancel?.Invoke();}else if(Keyboard.current.ctrlKey.isPressed||Keyboard.current.leftMetaKey.isPressed||Keyboard.current.rightMetaKey.isPressed){if(key=="undo")Undo?.Invoke();else Redo?.Invoke();}};
   map.FindAction("Overlay").performed+=c=>{if(c.ReadValue<float>()<=0)return;var key=Role(c);bool lb=Gamepad.current?.leftShoulder.isPressed??false;Overlay?.Invoke(key=="evidence"?"evidence":key=="hypothesis"?"hypothesis":key=="hints"?"hints":lb?"hypothesis":"evidence");};
   map.FindAction("Query").performed+=c=>{if(c.ReadValue<float>()<=0)return;if(c.control.device is Gamepad&&(Gamepad.current.leftShoulder.isPressed)){Overlay?.Invoke("hints");return;}if(ToolPanel&&!OverlayActive)Query?.Invoke();};
   map.FindAction("Tool").performed+=c=>{if(c.ReadValue<float>()<=0)return;if(OverlayActive)return;var key=Role(c);if(key=="wheel"){if(ToolPanel)Disconnect?.Invoke();else ToolWheel?.Invoke();}else if(key=="next")Tool?.Invoke((Gamepad.current.leftShoulder.isPressed)?-1:0);else if(int.TryParse(key,out var n))Tool?.Invoke(n);};
   map.FindAction("Disconnect").performed+=c=>{if(ToolPanel&&!OverlayActive)Disconnect?.Invoke();};
   map.FindAction("Cancel").performed+=c=>Cancel?.Invoke();map.Enable();
  }
  void Update(){
   if(Actions==null||rebinding!=null)return;
   if(imeKeyboard!=Keyboard.current){if(imeKeyboard!=null)imeKeyboard.onIMECompositionChange-=OnComposition;imeKeyboard=Keyboard.current;if(imeKeyboard!=null)imeKeyboard.onIMECompositionChange+=OnComposition;}
   if(TextEntryActive){
    if(!imeComposing&&Time.frameCount>compositionChangedFrame+1&&((Keyboard.current?.tabKey.wasPressedThisFrame??false)||(Keyboard.current?.escapeKey.wasPressedThisFrame??false)))TextEntryExitRequested?.Invoke();
    return;
   }
   if(awaitTextRelease){
    if(Actions.FindActionMap("Watch").actions.SelectMany(a=>a.controls).Any(control=>control.IsPressed()))return;
    awaitTextRelease=false;NewContext();Actions.FindActionMap("Watch").Enable();return;
   }
   if(Keyboard.current?.tabKey.wasPressedThisFrame??false)Navigate?.Invoke(Keyboard.current.shiftKey.isPressed?-1:1);
   var move=Actions.FindAction("Navigate").ReadValue<Vector2>();if(move.sqrMagnitude>.25f&&Time.unscaledTime>=nextNavigation)Move(move);
  }
  void Move(Vector2 value){if(value.sqrMagnitude<.25f)return;nextNavigation=Time.unscaledTime+.2f;
   bool fine=(Keyboard.current?.shiftKey.isPressed??false)||(Gamepad.current?.leftTrigger.isPressed??false);
   if(ToolPanel&&!OverlayActive&&!navigationStick)Adjust?.Invoke(value,fine);else Navigate?.Invoke(value.y>0||value.x<0?-1:1);
  }
  public void NewContext(){ContextGeneration++;armed=false;pressGeneration=-1;previewGeneration=-1;}
  public string RebindError {get;private set;}
  static string Role(InputAction.CallbackContext c){var index=c.action.GetBindingIndexForControl(c.control);return index>=0?c.action.bindings[index].name:c.control.name;}
  public void Rebind(string actionName,Action<string> completed,int requestedIndex=-1){
   var action=Actions.FindAction(actionName,true);var index=requestedIndex;
   if(index<0)for(int i=0;i<action.bindings.Count;i++)if(!action.bindings[i].isComposite&&action.bindings[i].path.StartsWith("<Keyboard>")){index=i;break;}
   if(index<0){completed?.Invoke(Actions.SaveBindingOverridesAsJson());return;}
   NewContext();RebindError=null;var oldOverride=action.bindings[index].overridePath;bool padBinding=action.bindings[index].path.StartsWith("<Gamepad>");action.Disable();rebinding=action.PerformInteractiveRebinding(index).WithControlsExcluding("Mouse").WithControlsHavingToMatchPath(padBinding?"<Gamepad>":"<Keyboard>").WithCancelingThrough("<Keyboard>/escape")
    .OnCancel(o=>{o.Dispose();rebinding=null;action.Enable();completed?.Invoke(Actions.SaveBindingOverridesAsJson());})
    .OnComplete(o=>{var path=action.bindings[index].effectivePath;bool conflict=path=="<Keyboard>/tab"||path=="<Gamepad>/leftShoulder"||Actions.actionMaps.SelectMany(m=>m.actions).Any(a=>a.bindings.Where((b,i)=>a!=action||i!=index).Any(b=>!b.isComposite&&b.effectivePath==path));if(conflict){if(oldOverride==null)action.RemoveBindingOverride(index);else action.ApplyBindingOverride(index,oldOverride);RebindError="bindingConflict";}o.Dispose();rebinding=null;action.Enable();completed?.Invoke(Actions.SaveBindingOverridesAsJson());});rebinding.Start();
  }
  void OnApplicationFocus(bool focus){if(!focus)NewContext();}
  void OnDestroy(){if(imeKeyboard!=null)imeKeyboard.onIMECompositionChange-=OnComposition;deviceListener?.Dispose();rebinding?.Dispose();Actions?.Disable();if(Actions!=null)Destroy(Actions);}
 }
}
