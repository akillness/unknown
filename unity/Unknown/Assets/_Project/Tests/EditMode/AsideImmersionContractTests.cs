#if TIDE_TEST_FRAMEWORK
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using UnityEngine;
namespace Tide.Tests {
    public sealed class AsideImmersionContractTests {
        static string Fallback(){return (string)JObject.Parse(Resources.Load<TextAsset>("T0Strings").text)["caseObjective"]["ko"];}
        [TestCase("t0-b1")]
        [TestCase("t0-b2")]
        public void MissingAndRedactedEarlyObjectivesUseSafeResourceFallback(string beat){
            var fallback=Fallback();
            var protectedRecords=new[]{"인수 각서","이관 목록"};
            Assert.IsFalse(string.IsNullOrWhiteSpace(fallback),"A missing/redacted objective still needs readable guidance.");
            foreach(var recordName in protectedRecords)StringAssert.DoesNotContain(recordName,fallback);
            var missing=new JObject{["rows"]=new JArray()};
            Assert.AreEqual(fallback,T0GameSession.CaseObjective(missing,beat,protectedRecords,fallback));
            var redacted=new JObject{["rows"]=new JArray(new JObject{["id"]=beat,["objective"]="인수 각서에서 답을 찾아라"})};
            var actual=T0GameSession.CaseObjective(redacted,beat,protectedRecords,fallback);
            Assert.AreEqual(fallback,actual);
            // Content guard on the localized fallback. The assertions above exercise the real C# branch.
            foreach(var spoiler in new[]{"결손","4시간","네 시간","정전","한도연","대조의 밤","서린","H-1","H+3","판 #0"})StringAssert.DoesNotContain(spoiler,actual);
        }
        [Test] public void AuthoredSafeObjectiveStillWinsOverFallback(){
            const string objective="오늘 밤, 무엇이 남을지는 아직 정해지지 않았다.";
            var table=new JObject{["rows"]=new JArray(new JObject{["id"]="t0-b1",["objective"]=objective})};
            Assert.AreEqual(objective,T0GameSession.CaseObjective(table,"t0-b1",new[]{"인수 각서","이관 목록"},Fallback()));
        }
    }
}
#endif
