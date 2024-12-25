namespace GV.APIInteraction;
public class VideoApi : IVideoApi
{
    private readonly string url;
    private IVideoApi apiStorage;
    public VideoApi(string url)
    {
        apiStorage = RestService.For<IVideoApi>(url);
        this.url = url;
    }
    public Task<VideoJson> GetVideo(string id)
    {
        Console.WriteLine($"Getting video from {url}" );
        return apiStorage.GetVideo(id);
    }

    public Task<VideoJson> SendVideoJson(VideoJson data)
    {
        return apiStorage.SendVideoJson(data);
    }
}
