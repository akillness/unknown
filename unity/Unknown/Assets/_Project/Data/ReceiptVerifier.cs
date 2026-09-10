using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tide.Data
{
    public static class ReceiptVerifier
    {
        private static readonly string[] Required={"beats.json","hints.json","tools.json","zones.json","records.json"};
        private static readonly Regex EmittedUtc=new Regex("\\\"emittedUtc\\\"\\s*:\\s*\\\"[^\\\"]*\\\"",RegexOptions.CultureInvariant);

        public static void Verify(IReadOnlyDictionary<string,byte[]> tables,byte[] receiptBytes,byte[] trustedReceiptBytes)
        {
            if(receiptBytes==null || trustedReceiptBytes==null) throw new InvalidOperationException("Missing receipt");
            string receipt=Encoding.UTF8.GetString(receiptBytes),trusted=Encoding.UTF8.GetString(trustedReceiptBytes);
            // Compare every byte of the producer contract except its explicitly volatile timestamp.
            if(EmittedUtc.Replace(receipt,"")!=EmittedUtc.Replace(trusted,""))
                throw new InvalidOperationException("V-4: receipt contract differs from producer receipt");
            var parsed=JsonUtility.FromJson<ReceiptJson>(receipt);
            if(parsed?.validator==null || parsed.validator.verdict!="PASS" || parsed.validator.exitCode!=0 || parsed.validator.fail!=0)
                throw new InvalidOperationException("V-1: producer validation did not pass");
            if(parsed.scope!="t0" || parsed.tables==null || parsed.tables.Length!=Required.Length ||
                parsed.tables.Select(t=>t.file).Distinct(StringComparer.Ordinal).Count()!=Required.Length ||
                !Required.All(n=>parsed.tables.Any(t=>t.file==n)))
                throw new InvalidOperationException("V-4: incomplete or duplicate T0 table manifest");
            foreach(var table in parsed.tables)
            {
                if(!tables.TryGetValue(table.file,out var bytes) || bytes.LongLength!=table.bytes || Sha256(bytes)!=table.sha256)
                    throw new InvalidOperationException("V-2/V-3: table bytes differ: "+table.file);
            }
        }
        public static string Sha256(byte[] bytes)
        {
            using(var sha=SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-","").ToLowerInvariant();
        }
    }
}

