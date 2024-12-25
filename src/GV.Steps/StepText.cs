namespace GV.Steps;
internal record StepText(string text, string value) : newStep(text, value)
{
    public override bool InitDefaults()
    {
        this.SpeakTest ??= value;
        return true;
    }
    public override void Dispose()
    {
    }
    
    public override async Task Execute()
    {
        await Task.Delay(1000);
        Console.WriteLine(value);
        //await Talk(true);        
        return;
    }
}
