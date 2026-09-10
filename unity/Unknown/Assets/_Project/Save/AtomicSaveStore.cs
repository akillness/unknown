using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Tide.Save
{
    public enum SaveStage { BeforeWrite, DuringWrite, BeforeRename, AfterRename }
    public sealed class SaveLoadResult
    {
        public JObject Document { get; internal set; }
        public string Source { get; internal set; }
        public string Failure { get; internal set; }
        public bool Migrated { get; internal set; }
    }
    public sealed class AtomicSaveStore
    {
        public string DirectoryPath { get; }
        public Func<SaveStage,CancellationToken,Task> Injection { get; set; }
        private readonly SemaphoreSlim gate=new SemaphoreSlim(1,1);
        public AtomicSaveStore(string directory) { DirectoryPath=directory; }
        public async Task<bool> WriteAsync(JObject document,string file="save.json",CancellationToken cancellation=default)
        {
            var snapshot=(JObject)document.DeepClone();
            await gate.WaitAsync(cancellation).ConfigureAwait(false);
            try
            {
                return await Task.Run(async ()=>
                {
                    Directory.CreateDirectory(DirectoryPath);
                    string primary=Path.Combine(DirectoryPath,file),temp=primary+".tmp";
                    string backup=file=="save.json"?Path.Combine(DirectoryPath,"save.bak"):primary+".bak";
                    string key=(string)snapshot["commitIdempotencyKey"];
                    if(!string.IsNullOrEmpty(key) && File.Exists(primary))
                    {
                        try { if((string)SaveCodec.Decode(File.ReadAllText(primary))["commitIdempotencyKey"]==key) return false; }
                        catch(InvalidOperationException) { }
                        catch(Newtonsoft.Json.JsonException) { }
                    }
                    try
                    {
                        await Inject(SaveStage.BeforeWrite,cancellation);
                        var bytes=System.Text.Encoding.UTF8.GetBytes(SaveCodec.Encode(snapshot));
                        using(var stream=new FileStream(temp,FileMode.Create,FileAccess.Write,FileShare.None))
                        {
                            int half=bytes.Length/2;
                            stream.Write(bytes,0,half);
                            await Inject(SaveStage.DuringWrite,cancellation);
                            stream.Write(bytes,half,bytes.Length-half); stream.Flush(true);
                        }
                        await Inject(SaveStage.BeforeRename,cancellation);
                        cancellation.ThrowIfCancellationRequested();
                        if(File.Exists(primary)) File.Replace(temp,primary,backup);
                        else File.Move(temp,primary);
                        await Inject(SaveStage.AfterRename,CancellationToken.None);
                        return true;
                    }
                    finally { if(File.Exists(temp)) File.Delete(temp); }
                },cancellation).ConfigureAwait(false);
            }
            finally { gate.Release(); }
        }
        private Task Inject(SaveStage stage,CancellationToken cancellation) =>
            Injection==null?Task.CompletedTask:Injection(stage,cancellation);

        public SaveLoadResult Load(string file="save.json",Action<JObject> validate=null)
        {
            var primary=Path.Combine(DirectoryPath,file);
            bool any=false;
            foreach(var candidate in new[]{file,file=="save.json"?"save.bak":file+".bak","checkpoint.pre-commit.json"})
            {
                var path=Path.Combine(DirectoryPath,candidate);
                if(!File.Exists(path)) continue;
                any=true;
                try
                {
                    var doc=SaveCodec.Decode(File.ReadAllText(path));
                    int version;
                    if(doc["schemaVersion"]?.Type!=JTokenType.Integer || !int.TryParse(doc["schemaVersion"].ToString(),out version) || version<0 || version>1) return new SaveLoadResult{Failure="SAVE_VERSION_REFUSED",Source=candidate};
                    if(version>1) return new SaveLoadResult{Failure="SAVE_VERSION_REFUSED",Source=candidate};
                    if(version<0) throw new InvalidOperationException("SAVE_VERSION_REFUSED");
                    bool migrated=version==0;
                    if(migrated)
                    {
                        var original=path+".v0.bak";
                        if(!File.Exists(original)) File.Copy(path,original);
                        doc["schemaVersion"]=1; doc["storyClock"]="21:00";
                    }
                    validate?.Invoke(doc);
                    return new SaveLoadResult{Document=doc,Source=candidate,Migrated=migrated};
                }
                catch(Exception e) when(e is InvalidOperationException || e is Newtonsoft.Json.JsonException || e is IOException) { }
            }
            return any?new SaveLoadResult{Failure="SAVE_CHECKSUM_FAILED",Source="recovery"}:new SaveLoadResult{Source="empty"};
        }
    }
}

