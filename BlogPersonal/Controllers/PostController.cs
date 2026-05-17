using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPersonal.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly IPostService _service;

    public PostController(IPostService service)
    {
        _service = service;
    }

    /// <summary>Get all posts.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get post by ID.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var post = await _service.GetByIdAsync(id);
        return post is null ? NotFound() : Ok(post);
    }

    /// <summary>Get posts by title.</summary>
    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title)
        => Ok(await _service.GetByTitleAsync(title));

    /// <summary>Filter posts by user and/or theme.</summary>
    [HttpGet("filter")]
    public async Task<IActionResult> GetByFilter(
        [FromQuery] long? userId,
        [FromQuery] long? themeId)
        => Ok(await _service.GetByFilterAsync(userId, themeId));

    /// <summary>Create a new post.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Post post)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _service.CreateAsync(post);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Update an existing post.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Post post)
    {
        post.Id = id;
        var updated = await _service.UpdateAsync(post);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Delete a post.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}