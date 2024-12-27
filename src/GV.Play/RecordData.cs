using GV.General;
using System.Collections.Concurrent;

namespace GV.Play;
public record ScriptID(Guid guid, VideoJson video);

public class PlayOperations
{
    private static ConcurrentDictionary<Guid, VideoJson> data = [];
    private static ConcurrentDictionary<Guid, int> StepsVideo = [];
    public async Task<Guid?> Add(VideoJson vj)
    {
        if (vj == null) return null;
        var id = Guid.NewGuid();
        data[id] = vj;
        return id;

    }
    public async Task<int> IncreaseStep(string idVideo)
    {
        var step = await Step(idVideo);
        var vj = await GetRegistered(idVideo);
        if (vj == null) throw new ArgumentException(idVideo);
        StepsVideo[vj.guid]=step+1;
        return step;

    }
    public async Task<int> Step(string idVideo)
    {
        var vj= await GetRegistered(idVideo);
        if(vj == null) throw new ArgumentException(idVideo);
        if(!StepsVideo.ContainsKey(vj.guid))            
            StepsVideo.AddOrUpdate(vj.guid, -1, (g, lastValue) => lastValue);
        
        return StepsVideo[vj.guid];

    }
    public string[] GetAllScriptNames()
    {
        return data.Values.Select(it=>it.scriptName).ToArray();
    }

    public async Task<ScriptID?> GetRegistered(string id)
    {
        await Task.Yield();
        ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
        ScriptID? ret = null;
        if (!Guid.TryParse(id, out var guid))
        {
            var v= data.Where(x => x.Value.scriptName?.ToLower() == id.ToLower()).ToArray();

            if(v.Length == 0) return null;
            ret = new ScriptID(v[0].Key, v[0].Value);

        }
        
        if (!data.TryGetValue(guid, out var vj)) return null;
        return new ScriptID(guid, vj); 
    }

}
