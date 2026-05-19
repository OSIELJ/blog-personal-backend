using BlogPersonal.Data;
using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
        => await _context.Posts
            .Include(p => p.Theme)
            .Include(p => p.User)
            .ToListAsync();

    public async Task<Post?> GetByIdAsync(long id)
        => await _context.Posts
            .Include(p => p.Theme)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Post>> GetByTitleAsync(string title)
        => await _context.Posts
            .Include(p => p.Theme)
            .Include(p => p.User)
            .Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();

    public async Task<IEnumerable<Post>> GetByFilterAsync(long? userId, long? themeId)
    {
        var query = _context.Posts
            .Include(p => p.Theme)
            .Include(p => p.User)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.UserId == userId);

        if (themeId.HasValue)
            query = query.Where(p => p.ThemeId == themeId);

        return await query.ToListAsync();
    }

    public async Task<Post> CreateAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post?> UpdateAsync(Post post)
    {
        var existing = await _context.Posts.FindAsync(post.Id);
        if (existing is null) return null;

        _context.Entry(existing).CurrentValues.SetValues(post);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post is null) return false;

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }
}