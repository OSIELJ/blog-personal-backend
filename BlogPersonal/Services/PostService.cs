using BlogPersonal.Models;
using BlogPersonal.Repositories;
using BlogPersonal.Services.IA;

namespace BlogPersonal.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;
    private readonly IIAService _iaService;

    public PostService(IPostRepository repository, IIAService iaService)
    {
        _repository = repository;
        _iaService = iaService;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Post?> GetByIdAsync(long id)
        => await _repository.GetByIdAsync(id);

    public async Task<IEnumerable<Post>> GetByTitleAsync(string title)
        => await _repository.GetByTitleAsync(title);

    public async Task<IEnumerable<Post>> GetByFilterAsync(long? userId, long? themeId)
        => await _repository.GetByFilterAsync(userId, themeId);

    public async Task<Post> CreateAsync(Post post)
    {
        try
        {
            var aiResult = await _iaService.GenerateSummaryAsync(post.Content);
            post.AiSummary = aiResult.Summary;
            post.AiTags = aiResult.Tags;
            post.AiCategory = aiResult.Category;
        }
        catch
        {
            // AI failed, save post without enrichment
        }

        post.CreatedAt = DateTime.UtcNow;
        return await _repository.CreateAsync(post);
    }

    public async Task<Post?> UpdateAsync(Post post)
        => await _repository.UpdateAsync(post);

    public async Task<bool> DeleteAsync(long id)
        => await _repository.DeleteAsync(id);
}