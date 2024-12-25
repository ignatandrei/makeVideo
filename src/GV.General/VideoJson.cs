using System.Text.Json;

namespace GV.General;
public class VideoJson
{
    public string scriptName { get; set; }= string.Empty;
    public Step[] steps { get; set; } = [];

    public newStep[] realSteps=[];
    public async static Task<VideoJson?> DeserializeFromFile(string fileName)
    {
        var json = await File.ReadAllTextAsync(fileName);
        return await DeserializeFromString(json);
    }
    public async static Task<VideoJson?> DeserializeFromString(string json)
    {
        
        var opt = new JsonSerializerOptions(JsonSerializerOptions.Default);
        opt.AllowTrailingCommas = true;
        var data = JsonSerializer.Deserialize<VideoJson>(json, opt);
        if (data == null) return null;
        List<newStep> steps = new List<newStep>();
        var esc = newStep.esc; 
        for(var i = 0; i < data.steps.Length; i++)
        {
            var step = data.steps[i];
            var newStep1= newStep.Parse("step_"+ i + "_"+step.typeStep + esc + step.arg, null);
            if (newStep1 == null) continue;
            newStep1.DurationSeconds = step.DurationSeconds;
            newStep1.SpeakTest ??= step.SpeakTest;
            newStep1.Number = (i+1);
            await newStep1.InitDefaults();
            steps.Add(newStep1);
        }
        data.realSteps= steps.ToArray();
        return data;
    }
}
