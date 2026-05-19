using BlogPersonal.Data;
using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Repositories;

public class ThemeRepository : IThemeRepository
{
    private readonly AppDbContext _context;

    public ThemeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Theme>> GetAllAsync()
        => await _context.Themes
            .Include(t => t.Posts)
            .ToListAsync();

    public async Task<Theme?> GetByIdAsync(long id)
        => await _context.Themes
            .Include(t => t.Posts)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<Theme>> GetByDescriptionAsync(string description)
        => await _context.Themes
            .Where(t => t.Description.Contains(description, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();

    public async Task<Theme> CreateAsync(Theme theme)
    {
        _context.Themes.Add(theme);
        await _context.SaveChangesAsync();
        return theme;
    }

    public async Task<Theme?> UpdateAsync(Theme theme)
    {
        var existing = await _context.Themes.FindAsync(theme.Id);
        if (existing is null) return null;

        _context.Entry(existing).CurrentValues.SetValues(theme);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var theme = await _context.Themes.FindAsync(id);
        if (theme is null) return false;

        _context.Themes.Remove(theme);
        await _context.SaveChangesAsync();
        return true;
    }
}