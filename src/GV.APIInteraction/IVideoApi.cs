
namespace GV.APIInteraction;
public interface IVideoApi
{
    [Get("/api/VideoPlay/Find/{id}")]
    Task<string> GetVideo(string id);

    [Post("/api/VideoPlay/Register")]
    Task<string> SendVideoJson(VideoJson data);

}
