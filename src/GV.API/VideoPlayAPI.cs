using GV.General;

namespace GV.API;

public class VideoPlayAPI : IApi
{
    public async void Register(IEndpointRouteBuilder builder)
    {
        await Task.Yield();
        var grp = builder.MapGroup("/api/VideoPlay");
        
        grp.MapPost("/Register"
            ,async Task<Results<Ok<Guid>,InternalServerError<string>>> (
                     [FromBody] string  context,
                     [FromServices] PlayOperations service
                     )
                     => {
                         ArgumentNullException.ThrowIfNullOrWhiteSpace(context);
                         var g = await service.Add(context);
                         if(g == null) return TypedResults.InternalServerError("Invalid JSON");
                         if(context == null) return TypedResults.InternalServerError("Invalid JSON");
                         return TypedResults.Ok(g.Value);
                     });
        grp.MapGet("/Find/{id}", async Task<Results<Ok<VideoJson>, InternalServerError<string>>> (
                     [FromRouteAttribute] string id,
                     [FromServices] PlayOperations service
                     )
                     => {
                         ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
                         var g = await service.GetRegistered(id);
                         if (g == null) return TypedResults.InternalServerError("Invalid id "+ id);
                         return TypedResults.Ok(g);
                     });

    }
}
public record StepPlay(Guid id, int step);