using BlogPersonal.DTOs;
using BlogPersonal.Services.IA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPersonal.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class IAController : ControllerBase
{
    private readonly IIAService _iaService;

    public IAController(IIAService iaService)
    {
        _iaService = iaService;
    }

    /// <summary>
    /// Receives post text and returns AI-generated summary, category and tags.
    /// </summary>
    [HttpPost("summarize")]
    public async Task<IActionResult> Summarize([FromBody] AiRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest(new { message = "Text cannot be empty." });

        var result = await _iaService.GenerateSummaryAsync(request.Text);
        return Ok(result);
    }
}