using BlogPersonal.Models;

namespace BlogPersonal.Services;

public interface IPostService
{
    Task<IEnumerable<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(long id);
    Task<IEnumerable<Post>> GetByTitleAsync(string title);
    Task<IEnumerable<Post>> GetByFilterAsync(long? userId, long? themeId);
    Task<Post> CreateAsync(Post post);
    Task<Post?> UpdateAsync(Post post);
    Task<bool> DeleteAsync(long id);
}