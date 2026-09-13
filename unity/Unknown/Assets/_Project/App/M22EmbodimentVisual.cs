using System;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Tide.App
{
    // RFC-CX-018: presentation-only clip sampling. The reader owns its stroke clock and pivot.
    public sealed class M22EmbodimentVisual:MonoBehaviour
    {
        public enum Action { None,Insert,Align,Read,ReadOriginal,Seal }
        M22EmbodimentProfile profile;
        Rig character,left,right;
        M7ReaderStageVisual reader;
        Transform handle,support,cameraFrame;
        Vector3 handlePoint,supportPoint;
        Quaternion handleRestRotation;
        bool reducedMotion,title;
        bool applicationPaused,applicationFocused=true;
        Action action;
        float elapsedMs,idleTime;
        double actionStartedAt;
        public Action CurrentAction=>action;
        public Transform RightContactBone=>right?.contact;
        public Vector3 RightContactPosition=>right==null?Vector3.zero:right.ContactPoint(profile.right.contactPoint);
        public Vector3 HandleContactPosition=>handle==null?Vector3.zero:handle.TransformPoint(handlePoint+profile.readerGripOffset);
        public Vector3 LeftContactPosition=>left==null?Vector3.zero:left.ContactPoint(profile.left.contactPoint);
        public Vector3 SupportContactPosition=>support==null?Vector3.zero:support.TransformPoint(supportPoint+profile.readerSupportOffset);
        public Vector3 LeftReadyContactPosition=>SupportContactPosition+cameraFrame.TransformDirection(profile.readerLeftRestOffset);
        public float LeftContactWeight
        {
            get
            {
                if(reader==null||!LeftContactAction)return 0;
                float duration=action==Action.Insert?profile.insertMs:profile.sealMs;
                float contactMs=Mathf.Clamp(action==Action.Insert?profile.insertContactMs:profile.sealContactMs,0,duration);
                if(elapsedMs<contactMs)return Mathf.SmoothStep(0,1,elapsedMs/contactMs);
                if(elapsedMs<=duration)return 1;
                return profile.handRecoveryMs<=0?0:1-Mathf.SmoothStep(0,1,(elapsedMs-duration)/profile.handRecoveryMs);
            }
        }
        bool LeftContactAction=>action==Action.Insert||action==Action.Seal||action==Action.ReadOriginal;

        public void Initialize(M22EmbodimentProfile profile,bool title,Camera camera,M7ReaderStageVisual reader)
        {
            this.profile=profile;this.title=title;this.reader=reader;cameraFrame=camera.transform;
            if(title)
            {
                character=new Rig(profile.character,transform,new[]{profile.idle},null);
                character.root.localPosition=profile.characterPosition;character.root.localRotation=Quaternion.Euler(profile.characterEuler);
                character.Sample(0,0);
                return;
            }
            left=Hand(profile.left);right=Hand(profile.right);
            if(reader!=null)
            {
                handle=Find(reader.CrankPivot,profile.crankHandlePrefix);
                support=Find(reader.transform,profile.readerSupportPrefix);
                if(handle==null||support==null)throw new InvalidOperationException("M22 reader contact missing: crank handle or support mesh");
                handlePoint=MeshCenter(handle);supportPoint=MeshCenter(support);
                handleRestRotation=handle.rotation;
            }
            Cancel();
        }
        Rig Hand(M22EmbodimentProfile.HandRig hand)=>new Rig(hand.prefab,transform,new[]{hand.rest,hand.insert,hand.align,hand.grip,hand.seal},hand.contactBone);
        static Transform Find(Transform root,string prefix)
        {
            if(root==null)return null;
            foreach(var child in root.GetComponentsInChildren<Transform>(true))if(child.name.StartsWith(prefix,StringComparison.Ordinal))return child;
            return null;
        }
        static Vector3 MeshCenter(Transform target)
        {
            var mesh=target.GetComponent<MeshFilter>();
            if(mesh==null||mesh.sharedMesh==null)throw new InvalidOperationException("M22 contact has no mesh: "+target.name);
            return mesh.sharedMesh.bounds.center;
        }
        public void Refresh(bool reducedMotion)
        {
            this.reducedMotion=reducedMotion;
            if(reducedMotion)Cancel();
        }
        public void Play(Action next)
        {
            if(title||reducedMotion||applicationPaused||!applicationFocused||!isActiveAndEnabled)return;
            action=next;elapsedMs=0;actionStartedAt=Time.realtimeSinceStartupAsDouble;
            SampleHands();
        }
        public void Cancel()
        {
            action=Action.None;elapsedMs=0;
            if(character!=null)character.Sample(0,0);
            if(left!=null&&right!=null){left.Sample(0,0);right.Sample(reader==null?0:3,reader==null?0:profile.right.gripPoseTime);PlaceHands();}
        }
        void Update()
        {
            if(reducedMotion||applicationPaused||!applicationFocused)return;
            if(title){idleTime+=Time.unscaledDeltaTime;character?.Sample(0,idleTime%profile.idle.length);return;}
            if(action==Action.None)return;
            elapsedMs=(float)((Time.realtimeSinceStartupAsDouble-actionStartedAt)*1000);
            if(action==Action.Read||action==Action.ReadOriginal)
            {
                if(reader==null||!reader.StrokePlaying){Cancel();return;}
            }
            else if(elapsedMs>=Duration()+(reader!=null&&LeftContactAction?Mathf.Max(0,profile.handRecoveryMs):0)){Cancel();return;}
            SampleHands();
        }
        float Duration()=>action==Action.Insert?profile.insertMs:action==Action.Align?profile.alignMs:profile.sealMs;
        void SampleHands()
        {
            if(left==null||right==null)return;
            int clip=action==Action.Insert?1:action==Action.Align?2:action==Action.Seal||action==Action.ReadOriginal?4:0;
            float duration=action==Action.ReadOriginal?profile.sealMs:Duration();
            if(action==Action.ReadOriginal&&elapsedMs>=profile.sealMs+Mathf.Max(0,profile.handRecoveryMs))clip=0;
            left.Sample(clip,clip==0?0:ClipTime(left.clips[clip],duration));
            // The left inserts/seals while the right retains its authored closed-finger handle grip.
            if(reader!=null)right.Sample(3,profile.right.gripPoseTime);
            else right.Sample(clip,clip==0?0:ClipTime(right.clips[clip],duration));
            PlaceHands();
        }
        float ClipTime(AnimationClip clip,float duration)=>clip.length*(duration<=0?1:Mathf.Clamp01(elapsedMs/duration));
        // Runs after M7ReaderStageVisual.Update: the grip follows the actual pivot, never an imitation angle.
        void LateUpdate(){if(!title&&left!=null&&right!=null)PlaceHands();}
        void PlaceHands()
        {
            if(reader!=null)
            {
                left.root.localScale=Vector3.one*profile.readerLeftScale;right.root.localScale=Vector3.one*profile.readerRightScale;
                // Ready offset uses camera-relative metres, independent of the support mesh's 100x import scale.
                if(support!=null)left.Anchor(Vector3.LerpUnclamped(LeftReadyContactPosition,SupportContactPosition,LeftContactWeight),support.rotation*Quaternion.Euler(profile.readerSupportEuler),profile.left.contactPoint);
                if(handle!=null)right.Anchor(handle.TransformPoint(handlePoint+profile.readerGripOffset),(profile.rotateGripWithHandle?handle.rotation:handleRestRotation)*Quaternion.Euler(profile.readerGripEuler),profile.right.contactPoint);
            }
            else
            {
                Pose(left,profile.foregroundLeft,cameraFrame);Pose(right,profile.foregroundRight,cameraFrame);
            }
        }
        static void Pose(Rig rig,M22EmbodimentProfile.HandPose pose,Transform frame)
        {
            rig.root.localScale=Vector3.one*pose.scale;
            rig.root.SetPositionAndRotation(frame==null?pose.position:frame.TransformPoint(pose.position),(frame==null?Quaternion.identity:frame.rotation)*Quaternion.Euler(pose.euler));
        }
        // Input ownership ends on suspension; resumed frames must not finish or replay an old gesture.
        void OnApplicationPause(bool paused){applicationPaused=paused;if(paused)Cancel();}
        void OnApplicationFocus(bool focused){applicationFocused=focused;if(!focused)Cancel();}
        void OnDisable(){Cancel();Dispose();}
        void OnDestroy(){Dispose();}
        void Dispose(){character?.Dispose();left?.Dispose();right?.Dispose();}

        sealed class Rig
        {
            public readonly Transform root,contact;
            public readonly AnimationClip[] clips;
            PlayableGraph graph;
            AnimationMixerPlayable mixer;
            AnimationClipPlayable[] players;
            int sampledClip=-1;
            float sampledTime;
            public Rig(GameObject prefab,Transform parent,AnimationClip[] clips,string bone)
            {
                this.clips=clips;root=Instantiate(prefab,parent).transform;
                var animator=root.GetComponent<Animator>();if(animator==null)animator=root.gameObject.AddComponent<Animator>();
                animator.applyRootMotion=false;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;animator.fireEvents=false;
                animator.runtimeAnimatorController=null;
                if(bone!=null)
                {
                    foreach(var candidate in root.GetComponentsInChildren<Transform>(true))if(candidate.name==bone){contact=candidate;break;}
                    if(contact==null)throw new InvalidOperationException("M22 contact bone missing: "+bone);
                }
                graph=PlayableGraph.Create("M22 "+prefab.name);graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
                mixer=AnimationMixerPlayable.Create(graph,clips.Length);players=new AnimationClipPlayable[clips.Length];
                for(int i=0;i<clips.Length;i++)
                {
                    players[i]=AnimationClipPlayable.Create(graph,clips[i]);players[i].SetApplyFootIK(false);players[i].SetApplyPlayableIK(false);players[i].SetSpeed(0);
                    graph.Connect(players[i],0,mixer,i);
                }
                AnimationPlayableOutput.Create(graph,"M22 pose",animator).SetSourcePlayable(mixer);graph.Play();
            }
            public void Sample(int index,float time)
            {
                if(!graph.IsValid())return;
                if(index==sampledClip&&time==sampledTime)return;
                if(index!=sampledClip)for(int i=0;i<players.Length;i++)mixer.SetInputWeight(i,i==index?1:0);
                players[index].SetTime(time);graph.Evaluate(0);
                sampledClip=index;sampledTime=time;
            }
            public Vector3 ContactPoint(Vector3 point)=>contact.TransformPoint(point);
            public void Anchor(Vector3 point,Quaternion orientation,Vector3 localPoint)
            {
                root.rotation=orientation*Quaternion.Inverse(Quaternion.Inverse(root.rotation)*contact.rotation);
                root.position+=point-ContactPoint(localPoint);
            }
            public void Dispose(){if(graph.IsValid())graph.Destroy();}
        }
    }
}
