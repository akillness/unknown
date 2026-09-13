using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.UI;

namespace Tide.App
{
    public sealed partial class T0GameSession
    {
        string comparisonRecord,comparisonLoadedRecord,comparisonStart,comparisonEnd;
        string comparisonJournalStart,comparisonJournalEnd;
        bool comparisonResetScroll;
        ReaderComparisonRange pinnedReaderRange;

        void ResetReaderComparison()
        {
            comparisonRecord=comparisonLoadedRecord=comparisonStart=comparisonEnd=null;
            comparisonJournalStart=comparisonJournalEnd=null;
            pinnedReaderRange=null;
            comparisonResetScroll=true;
        }

        void SelectReaderComparison(string id)
        {
            comparisonRecord=id;
            var phases=Definition.Records[id].Phases;
            comparisonStart=comparisonJournalStart=Journal.State.Get("windowStart:"+id)??phases[0];
            comparisonEnd=comparisonJournalEnd=Journal.State.Get("windowEnd:"+id)??phases[phases.Count-1];
            comparisonResetScroll=true;
        }

        void ClearUnavailableReaderPin()
        {
            if(pinnedReaderRange!=null&&
                !Definition.Records[pinnedReaderRange.RecordId].VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b)))
            {
                pinnedReaderRange=null;
                comparisonResetScroll=true;
            }
        }

        void ReaderComparisonScreen(GameScreen screen)
        {
            var loaded=Journal.State.LoadedRecordId;
            if(loaded==null)return;
            if(comparisonRecord==null||comparisonLoadedRecord!=loaded||
                !Definition.Records[comparisonRecord].VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b)))
                SelectReaderComparison(loaded);
            comparisonLoadedRecord=loaded;
            var phases=Definition.Records[comparisonRecord].Phases;
            var start=Journal.State.Get("windowStart:"+comparisonRecord)??phases[0];
            var end=Journal.State.Get("windowEnd:"+comparisonRecord)??phases[phases.Count-1];
            // Window edits and Undo preserve a pinned window only while its record remains visible.
            if(start!=comparisonJournalStart||end!=comparisonJournalEnd)SelectReaderComparison(comparisonRecord);
            var current=ReaderRange(comparisonRecord,comparisonStart,comparisonEnd);
            screen.ReaderComparison=new ReaderComparisonView { Current=current,Pinned=pinnedReaderRange,
                Relationship=pinnedReaderRange==null?null:
                    (Definition.ResolveRoot(comparisonRecord)==Definition.ResolveRoot(pinnedReaderRange.RecordId)?"같은 원본":"다른 원본")+
                    (Definition.Records[comparisonRecord].SourceType==Definition.Records[pinnedReaderRange.RecordId].SourceType?" · 같은 매체":" · 다른 매체") };
            screen.Chart=null;
            screen.ResetWorkScroll|=comparisonResetScroll;comparisonResetScroll=false;
            screen.Navigation.Add(A("reader-compare-pin","비교창 고정 · 현재 선택",()=>{pinnedReaderRange=current;comparisonResetScroll=true;Render();}));
            screen.Navigation.Add(A("reader-compare-clear","비교창 고정 해제",()=>{pinnedReaderRange=null;comparisonResetScroll=true;Render();},pinnedReaderRange!=null));
            foreach(var record in Definition.Records.Values.Where(r=>r.Phases.Count>0&&r.VisibleAt.Any(b=>Simulation.IsAvailable(Journal.State,b))))
            {
                var id=record.Id;
                screen.Navigation.Add(A("reader-compare-"+id,"비교만 선택 · "+Name(id),()=>{SelectReaderComparison(id);Render();}));
            }
            screen.Navigation.Add(A("reader-compare-start-prev","비교 시작 앞 샘플",()=>ShiftReaderComparison(true,-1),comparisonStart!=phases[0]));
            screen.Navigation.Add(A("reader-compare-start-next","비교 시작 뒤 샘플",()=>ShiftReaderComparison(true,1),comparisonStart!=comparisonEnd));
            screen.Navigation.Add(A("reader-compare-end-prev","비교 끝 앞 샘플",()=>ShiftReaderComparison(false,-1),comparisonEnd!=comparisonStart));
            screen.Navigation.Add(A("reader-compare-end-next","비교 끝 뒤 샘플",()=>ShiftReaderComparison(false,1),comparisonEnd!=phases[phases.Count-1]));
            screen.Body="비교창은 보기 전용입니다. 읽기·인용 대상: "+Name(loaded)+"\n"+screen.Body;
        }

        void ShiftReaderComparison(bool start,int delta)
        {
            var phases=Definition.Records[comparisonRecord].Phases;
            int first=phases.ToList().IndexOf(comparisonStart),last=phases.ToList().IndexOf(comparisonEnd);
            if(start)comparisonStart=phases[Math.Max(0,Math.Min(last,first+delta))];
            else comparisonEnd=phases[Math.Max(first,Math.Min(phases.Count-1,last+delta))];
            comparisonResetScroll=true;
            Render();
        }

        ReaderComparisonRange ReaderRange(string id,string start,string end)
        {
            var record=Record(id);
            var all=(JArray)record["samples"];
            string channel=all[0]["pressure"]!=null?"pressure":"tideHeight";
            var metadata=((JArray)(record["channels"]??record["columns"]))
                .First(x=>(string)(x["channelId"]??x["columnId"])==channel);
            int first=0,last=all.Count-1;
            for(int i=0;i<all.Count;i++)
            {
                if((string)all[i]["phase"]==start)first=i;
                if((string)all[i]["phase"]==end)last=i;
            }
            var values=new float[last-first+1];var times=new float[values.Length];
            var missing=new List<string>();int missingStart=-1;
            float min=float.PositiveInfinity,max=float.NegativeInfinity;
            for(int i=first;i<=last;i++)
            {
                var sample=all[i];int n=i-first;
                float value=(string)sample["state"]=="missing"?float.NaN:(float?)sample[channel]??float.NaN;
                if(float.IsInfinity(value))value=float.NaN;
                values[n]=value;times[n]=(float)sample["t"];
                if(float.IsNaN(value)){if(missingStart<0)missingStart=i;}
                else
                {
                    min=Math.Min(min,value);max=Math.Max(max,value);
                    if(missingStart>=0){missing.Add(ReaderMissingSpan(all,missingStart,i-1));missingStart=-1;}
                }
            }
            if(missingStart>=0)missing.Add(ReaderMissingSpan(all,missingStart,last));
            string medium=ReviewMediaName(Definition.Records[id].SourceType);
            if(!string.IsNullOrEmpty(Definition.Records[id].CopiedFrom))medium+=" · 사본";
            return new ReaderComparisonRange { RecordId=id,Title=Name(id),Source=medium,Start=start,End=end,
                Channel=(string)metadata["ko"],Unit=(string)metadata["unit"],Values=values,Times=times,
                Minimum=float.IsInfinity(min)?float.NaN:min,Maximum=float.IsInfinity(max)?float.NaN:max,
                Missing=missing.Count==0?"결손 샘플 없음":"결손 샘플: "+string.Join("; ",missing),
                StepMinutes=(int?)(record["resolutionMin"]??record["entryRange"]?["stepMin"])??0 };
        }

        static string ReaderMissingSpan(JArray samples,int first,int last)
        {
            string start=(string)samples[first]["phase"],end=(string)samples[last]["phase"];
            return first==last?start:start+" → "+end;
        }
    }
}
