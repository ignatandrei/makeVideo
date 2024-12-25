using GV.General;

namespace GV.API;

public class VideoPlayAPI : IApi
{
    public async void Register(IEndpointRouteBuilder builder)
    {
        await Task.Yield();
        var grp = builder.MapGroup("/api/VideoPlay");
        
        grp.MapPost("/Register"
            ,async Task<Results<Ok<Guid>, BadRequest<string>>> (
                     VideoJson  context,
                     [FromServices] PlayOperations service
                     )
                     => {
                         if (context == null) return TypedResults.BadRequest("Invalid JSON");
                         ArgumentNullException.ThrowIfNull(context);
                         var g = await service.Add(context);
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