
namespace GV.API;

public class VideoPlayAPI : IApi
{
    public void Register(IEndpointRouteBuilder builder)
    {
        var grp = builder.MapGroup("/api/VideoPlay");
        grp.MapPost("Register", async (string context) =>
        {
            //var g = await GeneratorVideo.VideoJson.Deserialize(context);
            Guid g= Guid.NewGuid();
            return TypedResults.Ok(g);
        });         
    }
}
public record StepPlay(Guid id, int step);