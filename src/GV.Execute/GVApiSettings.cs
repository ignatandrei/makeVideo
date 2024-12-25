namespace GV.Execute;

public class GVApiSettings
{
    public Dictionary<string, string> https { get; set; } = [];
    public Dictionary<string, string> http { get; set; } = [];

    public string GetUrl()
    {
        if (https.Count==1) return https.First().Value;
        if (http.Count==1) return http.First().Value;
        throw new ArgumentException("No valid url found");
    }
}
