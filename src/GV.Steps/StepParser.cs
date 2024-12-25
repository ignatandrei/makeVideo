
namespace GV.Steps;
public class StepParser
{
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out newStep result)
    {
        ArgumentNullException.ThrowIfNull(s);
        result = default;
        var split = s.Split(newStep.esc, StringSplitOptions.RemoveEmptyEntries).ToArray();
        if (split.Length != 2)
            return false;
        var text = split[0];
        var typeAndNr = text.Split("_", StringSplitOptions.RemoveEmptyEntries);
        if (typeAndNr.Length != 3) return false;
        var nr = typeAndNr[1];
        var type = typeAndNr[2];
        if (!int.TryParse(nr, out var index)) return false;
        switch (type)
        {
            case "text":
                result = new StepText(split[0], split[1]);
                break;
            case "exec":
                result = new StepExecuteProgram(split[0], split[1]);
                break;
            case "hide":
                result = new StepHide(split[0], split[1]);
                break;
            case "browser":
                result = new StepBrowser(split[0], split[1]);
                break;
            case "tour":
                result = new StartTourVSCode(split[0], split[1]);
                break;
            case "showproj":
                result = new StartProjectVSCode(split[0], split[1]);
                break;
            case "waitseconds":
                result = new StepWaitSeconds(split[0], split[1]);
                break;
            case "stepvscode":
                result = new StepVSCode(split[0], split[1]);
                break;

            default:
                return false;
        }
        result.Number = index;
        return true;
    }
}
