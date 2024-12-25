using System.Diagnostics.CodeAnalysis;

namespace GV.General;

public class Step
{
    public string typeStep { get; set; } = string.Empty;
    public string arg { get; set; } = string.Empty;
    public long DurationSeconds { get; set; } = 0;
    public string SpeakTest { get; set; }=string.Empty;
}
//[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public abstract record newStep(string typeScript, string arg):IParsable<newStep>, IDisposable
{
    public const string esc = "\u001B";
    public int Number { get; set; }
    public abstract Task Execute();
    //public async Task ExecuteAndSpeak()
    //{
    //    await Task.WhenAll(Talk(true), Execute());
    //}
    public abstract Task<bool> InitDefaults();
    public string Description => this.GetType().Name + " " + typeScript + " " + arg;

    public long DurationSeconds { get; set; }
    public string? SpeakTest { get; set; } = null;
    internal async Task Talk(bool speak)
    {
        if (!speak)
        {
            return;
        }
        if(string.IsNullOrWhiteSpace(SpeakTest))
            return;
        Console.WriteLine("TALK:"+SpeakTest);
        await Task.Delay(1000);
        //using SpeechSynthesizer synth = new();

        //var stream = synth.SpeakAsync(SpeakTest);
        //while(!stream.IsCompleted)
        //{
        //    await Task.Delay(1000);
        //}

    }
    public static newStep Parse(string s, IFormatProvider? provider)
    {
        if(TryParse(s,provider, out var value))
            return value;
        throw new ArgumentException("cannot parse to step" + s);
    }

    

    public abstract void Dispose();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out newStep result)
    {
        throw new NotImplementedException();
    }
}
