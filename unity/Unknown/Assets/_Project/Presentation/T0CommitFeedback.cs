using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Tide.Presentation {
 public sealed class T0CommitFeedback:MonoBehaviour {
  readonly HashSet<string> consumed=new HashSet<string>();
  InkMark mark;float elapsed,duration,impact;bool playing,paused;
  public int PresentedCount {get;private set;}
  public float PresentationElapsed=>elapsed;
  public bool IsPlaying=>playing;
  public bool IsMarkVisible=>mark!=null&&mark.enabled;
  public void Initialize(float durationSeconds,float impactSeconds){
   duration=durationSeconds;impact=impactSeconds;
   var canvas=new GameObject("Confirmed ink layer",typeof(Canvas),typeof(CanvasScaler));canvas.transform.SetParent(transform,false);canvas.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;canvas.GetComponent<Canvas>().sortingOrder=30;
   var go=new GameObject("Receipt ink",typeof(RectTransform),typeof(InkMark));go.transform.SetParent(canvas.transform,false);var rect=go.GetComponent<RectTransform>();rect.anchorMin=rect.anchorMax=new Vector2(.955f,.15f);rect.sizeDelta=new Vector2(48,42);mark=go.GetComponent<InkMark>();mark.color=new Color(23f/255,50f/255,56f/255);mark.raycastTarget=false;mark.enabled=false;
  }
  public void Present(string attemptId,bool success,bool reducedMotion){
   if(!success||string.IsNullOrEmpty(attemptId)||!consumed.Add(attemptId))return;
   Clear();if(reducedMotion)return;elapsed=0;playing=true;PresentedCount++;
  }
  public void Clear(){playing=false;if(mark!=null)mark.enabled=false;}
  void Update(){if(!playing||paused)return;elapsed+=Time.deltaTime;mark.enabled=elapsed>=impact&&elapsed<duration;if(elapsed>=duration)Clear();}
  void OnApplicationPause(bool value){paused=value;}
 }
 public sealed class InkMark:MaskableGraphic {
  protected override void OnPopulateMesh(VertexHelper helper){helper.Clear();var rect=rectTransform.rect;Stroke(helper,new Vector2(rect.xMin+7,0),new Vector2(-3,rect.yMin+9));Stroke(helper,new Vector2(-3,rect.yMin+9),new Vector2(rect.xMax-6,rect.yMax-6));}
  void Stroke(VertexHelper h,Vector2 a,Vector2 b){var n=new Vector2(-(b-a).y,(b-a).x).normalized*1.5f;int first=h.currentVertCount;foreach(var p in new[]{a-n,a+n,b+n,b-n})h.AddVert(p,color,Vector2.zero);h.AddTriangle(first,first+1,first+2);h.AddTriangle(first,first+2,first+3);}
 }
}
