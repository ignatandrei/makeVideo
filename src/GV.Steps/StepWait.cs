﻿namespace  GV.Steps;
internal record StepWaitSeconds(string text,string value): newStep(text, value)
{
    public override async Task Execute()
    {
        var nr = int.Parse(value);
        await Task.Delay(nr*100);
    }
    public override void Dispose()
    {
    }
    public override bool InitDefaults()
    {
        this.SpeakTest ??= "";
        return true;
    }
}
