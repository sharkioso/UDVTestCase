using System.Text.Json;
using VkNet;
using VkNet.Model;

public class VKService
{
    private readonly VkApi vkApi;
    private readonly long userID;
    private readonly ILogger<VKService> logger;

    public VKService(string accessToken, long userID, ILogger<VKService> logger)
    {
        this.vkApi = new VkApi();
        this.userID = userID;
        this.logger = logger;
        vkApi.Authorize(new ApiAuthParams { AccessToken = accessToken });
    }

    public List<string> GetLastPosts(int count)
    {
        var posts = vkApi.Wall.Get(new WallGetParams
        {
            OwnerId = userID,
            Count = (ulong)count
        });

        return posts.WallPosts
            .Where(p => !string.IsNullOrEmpty(p.Text))
            .Select(p => p.Text)
            .Take(count)
            .ToList();
    }

}