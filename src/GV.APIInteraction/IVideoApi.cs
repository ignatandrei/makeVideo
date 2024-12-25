
namespace GV.APIInteraction;
public interface IVideoApi
{
    [Get("/api/VideoPlay/Find/{id}")]
    Task<VideoJson> GetVideo(string id);

    [Post("/api/VideoPlay/Register")]
    Task<VideoJson> SendVideoJson(VideoJson data);

}
