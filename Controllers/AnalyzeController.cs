using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnalyzeController : ControllerBase
{
    private readonly VKService vkService;

    public AnalyzeController(VKService vkService) => this.vkService = vkService;

    [HttpPost]
    public async Task<IActionResult> Analyze([FromBody] AnalyzeRequest request)
    {
        var result = await vkService.PostAnalyze(request.UserId, request.AccessToken);
        return Ok(result);
    }
}

public class AnalyzeRequest
{
    public string UserId { get; set; }
    public string AccessToken { get; set; }
}