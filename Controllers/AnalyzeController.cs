using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnalyzeController : ControllerBase
{
    private readonly VKService vkService;
    private readonly BDContext context;

    public AnalyzeController(VKService vkService, BDContext context)
    {
        this.vkService = vkService;
        this.context = context;
    }

    [HttpPost("analyze-last-posts")]
    public IActionResult AnalyzeLastPosts()
    {
        try
        {
            var posts = vkService.GetLastPosts(5);
            var text = string.Join(" ", posts);

            var LetterStat = text.ToLower()
                .Where(char.IsLetter)
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            context.Analysis.Add(new PostAnalyze
            {
                PostText = text,
                LetterCount = LetterStat
            });
            context.SaveChanges();
            return Ok(LetterStat);
        }
        catch (Exception ex)
        {
            return StatusCode(500,$"Ошибка: {ex.Message}");
        }
    }

}
