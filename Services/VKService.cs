using System.Text.Json;

public class VKService
{
    private readonly HttpClient client;
    private readonly BDContext BD;
    private readonly ILogger<VKService> logger;

    public VKService(HttpClient client, BDContext BD, ILogger<VKService> logger)
    {
        this.client = client;
        this.BD = BD;
        this.logger = logger;
    }

    public async Task<Dictionary<char, int>> PostAnalyze(string userID, string accessToken)
    {
        logger.LogInformation($"Start analyze for {userID}");
        var post = await GetLastPosts(userID, accessToken);
        var letterCount = CountLetters(post);
        await SaveToBD(userID, letterCount);

        logger.LogInformation($"Analyze for {userID} is complete");
        return letterCount;
    }

    private async Task<List<string>> GetLastPosts(string userID, string accessToken)
    {
        var url = "https://api.vk.com/method/wall.get" +
                  $"?owner_id={userID}" +
                  $"&count=5" +
                  $"&access_token={accessToken}" +
                  $"&v=5.131";
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json);

        return data.RootElement
            .GetProperty("response")
            .GetProperty("Items")
            .EnumerateArray()
            .Select(post => post.GetProperty("text").GetString() ?? "")
            .ToList();
    }

    private Dictionary<char, int> CountLetters(List<string> post)
    {
        var count = new Dictionary<char, int>();
        foreach (var symbol in string.Concat(post).ToLower())
        {
            if (char.IsLetter(symbol))
            {
                count.TryAdd(symbol, 0);
                count[symbol]++;
            }
        }

        return count.OrderBy(pair => pair.Value)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private async Task SaveToBD(string userID, Dictionary<char, int> letterCount)
    {
        var analitic = new PostAnalyze
        {
            UserID = userID,
            AnalyzeDate = DateTime.UtcNow,
            LetterCount = letterCount
        };
        await BD.Analysis.AddAsync(analitic);
        await BD.SaveChangesAsync();


    }

}