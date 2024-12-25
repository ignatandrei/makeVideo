
namespace GV.APIInteraction;
public interface IVideoApi
{
    [Get("/api/VideoPlay/Find/{id}")]
    Task<VideoJson> GetVideo(int id);

    [Post("/api/VideoPlay/Register/{id}")]
    Task<VideoJson> SendVideoJson(int id);

}
