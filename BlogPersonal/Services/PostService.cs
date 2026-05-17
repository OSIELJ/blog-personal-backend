using BlogPersonal.Models;
using BlogPersonal.Repositories;

namespace BlogPersonal.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
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
        post.CreatedAt = DateTime.UtcNow;
        return await _repository.CreateAsync(post);
    }

    public async Task<Post?> UpdateAsync(Post post)
        => await _repository.UpdateAsync(post);

    public async Task<bool> DeleteAsync(long id)
        => await _repository.DeleteAsync(id);
}