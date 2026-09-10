using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tide.Save
{
    public static class SaveCodec
    {
        public static string Canonical(JToken token)
        {
            if(token is JObject obj) return "{"+string.Join(",",obj.Properties().OrderBy(p=>p.Name,StringComparer.Ordinal)
                .Select(p=>JsonConvert.ToString(p.Name)+":"+Canonical(p.Value)))+"}";
            if(token is JArray arr) return "["+string.Join(",",arr.Select(Canonical))+"]";
            return token.ToString(Formatting.None);
        }
        public static string Hash(string text)
        { using(var sha=SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-","").ToLowerInvariant(); }
        public static string Encode(JObject source)
        {
            var body=(JObject)source.DeepClone(); body.Remove("checksum");
            body["checksum"]=Hash(Canonical(body)); return Canonical(body);
        }
        public static JObject Decode(string text)
        {
            // Keep schema timestamps as strings; DateTime coercion changes checksum spelling.
            JObject obj;using(var reader=new JsonTextReader(new System.IO.StringReader(text)){DateParseHandling=DateParseHandling.None}) obj=JObject.Load(reader); var expected=(string)obj["checksum"];
            var body=(JObject)obj.DeepClone(); body.Remove("checksum");
            if(string.IsNullOrEmpty(expected) || Hash(Canonical(body))!=expected) throw new InvalidOperationException("SAVE_CHECKSUM_FAILED");
            return obj;
        }
    }
}

