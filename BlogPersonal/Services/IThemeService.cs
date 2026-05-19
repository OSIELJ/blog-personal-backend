using BlogPersonal.Models;

namespace BlogPersonal.Services;

public interface IThemeService
{
    Task<IEnumerable<Theme>> GetAllAsync();
    Task<Theme?> GetByIdAsync(long id);
    Task<IEnumerable<Theme>> GetByDescriptionAsync(string description);
    Task<Theme> CreateAsync(Theme theme);
    Task<Theme?> UpdateAsync(Theme theme);
    Task<bool> DeleteAsync(long id);
}