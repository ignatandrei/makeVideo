using GV.General;
using System.Collections.Concurrent;

namespace GV.Play;

public class PlayOperations
{
    private static ConcurrentDictionary<Guid, VideoJson> data = [];

    public async Task<Guid?> Add(VideoJson vj)
    {
        if (vj == null) return null;
        var id = Guid.NewGuid();
        data[id] = vj;
        return id;

    }

    public string[] GetAll()
    {
        return data.Values.Select(it=>it.scriptName).ToArray();
    }

    public async Task<VideoJson?> GetRegistered(string id)
    {
        await Task.Yield();
        ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
        VideoJson? v = null;
        if (!Guid.TryParse(id, out var guid))
        {
            v= data.Values.FirstOrDefault(x => x.scriptName?.ToLower() == id.ToLower());
            return v;
        }
        
        if (!data.TryGetValue(guid, out var vj)) return null;
        return vj;
    }

}
