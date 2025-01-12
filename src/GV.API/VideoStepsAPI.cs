
using GV.General;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;

namespace GV.API;

public class VideoStepsAPI : IApi
{
    public void Register(IEndpointRouteBuilder builder)
    {
        var grp = builder.MapGroup("/api/VideoPlay")
            .WithOpenApi()
            .WithTags("ScriptManager")
            ;

        grp.MapPost("/StartScript"
    ,async Task< Results<Ok<string>, BadRequest<string>>> (
             [FromBody]string scriptName,
             [FromServices] PlayOperations service,
             [FromServices] IHubContext<StepsHub> hubContext
             )
             => {
                 var exists = (service.GetRegistered(scriptName) != null);
                 if (!exists) return TypedResults.BadRequest($"cannot find {scriptName}");
                 await hubContext.Clients.All.SendAsync(nameof(StepsHub.Start),"Y"+scriptName);
                 return TypedResults.Ok($"send start script for {scriptName}");
             });
        grp.MapPost("/PlayStep"
    , async Task<Results<Ok<string>, BadRequest<string>>> (
             [FromBody] PlayStep ps,
             [FromServices] PlayOperations service,
             [FromServices] IHubContext<StepsHub> hubContext
             )
             => {
                 var exists = (service.GetRegistered(ps.scriptName) != null);
                 if (!exists) return TypedResults.BadRequest($"cannot find {ps.scriptName}");
                 await hubContext.Clients.All.SendAsync(nameof(StepsHub.SendNextStep), ps);
                 return TypedResults.Ok($"send step script for {ps}");
             });


    }
}
