using GV.General;
using GV.Steps;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace GV.API;

public class VideoPlayAPI : IApi
{
    public async void Register(IEndpointRouteBuilder builder)
    {
        await Task.Yield();
        var grp = builder.MapGroup("/api/VideoPlay");
        
        grp.MapPost("/Register"
            ,async Task<Results<Ok<string>, BadRequest<string>>> (
                     VideoJson  context,
                     [FromServices] PlayOperations service
                     )
                     => {
                         if (context == null) return TypedResults.BadRequest("Invalid JSON");
                         ArgumentNullException.ThrowIfNull(context);
                         var g = await service.Add(context);
                         return TypedResults.Ok(g.Value.ToString());
                     });

        grp.MapGet("/All", ([FromServices] PlayOperations service) =>
        {
            var data = service.GetAll();
            return TypedResults.Ok(data);
        });
        grp.MapGet("/Find/{id}", async Task<Results<Ok<string>, InternalServerError<string>>> (
                     [FromRouteAttribute] string id,
                     [FromServices] PlayOperations service
                     )
                     => {
                         ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
                         var g = await service.GetRegistered(id);
                         if (g == null) return TypedResults.InternalServerError("Invalid id "+ id);
                         return TypedResults.Ok(g.SerializeMe());
                     });

    }
}
public record StepPlay(Guid id, int step);

public class DeserializeVideoJson : VideoJson, IParsable<DeserializeVideoJson>
{
    public static DeserializeVideoJson Parse(string s, IFormatProvider? provider)
    {
        var video = VideoJson.DeserializeFromString(s, StepParser.Parse);
        if (video == null) throw new ArgumentException("cannot parse to step" + s);
        DeserializeVideoJson ret = new();
        ret.scriptName = video.scriptName;
        ret.steps = video.steps;
        ret.realSteps = video.realSteps;
        return ret;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out DeserializeVideoJson result)
    {
        throw new NotImplementedException();
    }
}