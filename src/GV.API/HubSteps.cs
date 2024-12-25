using GV.General;
using Microsoft.AspNetCore.SignalR;

namespace GV.API;


public class StepsHub : Hub, IStepsHub
{
    public async Task Start(string scriptName)
    {
        PlayStep step = new(scriptName, 0);
        await SendNextStep(step);
    }
    public async Task SendNextStep(PlayStep step)
    {

        await Clients.All.SendAsync(nameof(SendNextStep), step);
    }
}

