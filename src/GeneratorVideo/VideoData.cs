﻿
using GV.General;
using System.Windows;

namespace GeneratorVideo;
//[System.Runtime.Versioning.SupportedOSPlatform("windows")]

public class VideoData:IDisposable
{
    VideoJson? vdata = null;
    private readonly string fileName="";

    public VideoData(string fileName)
    {
        this.fileName = fileName;
    }
    public VideoData(VideoJson videoJson)
    {
        this.vdata = videoJson;
    }
    public async Task<bool> Analyze()
    {
        vdata = await VideoJson.DeserializeFromFile(fileName,StepParser.Parse);
        return vdata != null;
        //var steps = JsonSerializer.Deserialize<Dictionary<string, string>>
        //    (data,opt);
        //ArgumentNullException.ThrowIfNull(steps);
        //foreach (var step in steps)
        //{
        //    Step newStep = Step.Parse(step.Key + Step.esc + step.Value, null);
        //    newStep.OriginalFileNameFromWhereTheStepIsComing = fileName;
        //    _steps.Add(newStep);
        //}
        //return _steps.Count;

    }
    public int NrSteps() => vdata?.steps.Length ?? 0;
    public async Task ExecuteStep(int iStep)
    {
        ArgumentNullException.ThrowIfNull(vdata);
        var step = vdata.realSteps[iStep];
        var now = DateTime.Now;                                                                        // 
                                                                                                       //await step.ExecuteAndSpeak();
        await step.Execute();
        var DurationSeconds = DateTime.Now.Subtract(now).TotalSeconds;
        step.DurationSeconds = (long)DurationSeconds;
        Console.WriteLine("Duration:" + step.DurationSeconds);

    }
    public async Task<bool> ExecuteToDetermineDuration()
    {
        if (vdata == null) return false;
        vdata.realSteps = vdata.realSteps.OrderBy(it=>it.Number).ToArray();
        var nr = vdata.realSteps.Length;
        InputSimulator inputSimulator = new InputSimulator();
        long Duration = 0;
        for (var iStep=0;iStep<nr;iStep++) {
            try
            {
                var step = vdata.realSteps[iStep];
                Console.WriteLine($"executing {step.GetType().Name} {step.Number} /{nr}");// + "=>" + step.value);
                await ExecuteStep(iStep);
                Duration += step.DurationSeconds;

                //Console.ReadLine();
                if (iStep == nr - 1)
                {
                    Console.WriteLine("no next step");
                }
                else
                {
                    Console.WriteLine("========>next step " + vdata.realSteps[iStep + 1].Description);
                }


                await Task.Delay(2000);

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error :" + ex.Message +"==>"+ ex.StackTrace);
                return false;
            }
        }
        Console.WriteLine($"Duration Minutes={Duration/60}");
        return true;
    }
    public void Dispose()
    {
        if(vdata == null) return;
        foreach (var step in vdata.realSteps)
        {
            try {
                //Console.WriteLine("disposing " + step.Number);
                step.Dispose(); 
            }
            catch
            {
                Console.WriteLine("Error in dispose");
            }
            
        }   
    }
}
