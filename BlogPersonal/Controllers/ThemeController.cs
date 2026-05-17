using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPersonal.Controllers;

[ApiController]
[Route("api/themes")]
[Authorize]
public class ThemeController : ControllerBase
{
    private readonly IThemeService _service;

    public ThemeController(IThemeService service)
    {
        _service = service;
    }

    /// <summary>Get all themes.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get theme by ID.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var theme = await _service.GetByIdAsync(id);
        return theme is null ? NotFound() : Ok(theme);
    }

    /// <summary>Get themes by description.</summary>
    [HttpGet("description/{description}")]
    public async Task<IActionResult> GetByDescription(string description)
        => Ok(await _service.GetByDescriptionAsync(description));

    /// <summary>Create a new theme.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Theme theme)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _service.CreateAsync(theme);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Update an existing theme.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Theme theme)
    {
        theme.Id = id;
        var updated = await _service.UpdateAsync(theme);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Delete a theme.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}