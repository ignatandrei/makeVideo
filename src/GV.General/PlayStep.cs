using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.General;
public record PlayStep(string scriptName, int Step)
{
}
public interface IStepsHub
{
    Task SendNextStep(PlayStep step);
    Task Start(string scriptName);

    
}
