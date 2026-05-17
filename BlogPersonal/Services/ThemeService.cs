using BlogPersonal.Models;
using BlogPersonal.Repositories;

namespace BlogPersonal.Services;

public class ThemeService : IThemeService
{
    private readonly IThemeRepository _repository;

    public ThemeService(IThemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Theme>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Theme?> GetByIdAsync(long id)
        => await _repository.GetByIdAsync(id);

    public async Task<IEnumerable<Theme>> GetByDescriptionAsync(string description)
        => await _repository.GetByDescriptionAsync(description);

    public async Task<Theme> CreateAsync(Theme theme)
        => await _repository.CreateAsync(theme);

    public async Task<Theme?> UpdateAsync(Theme theme)
        => await _repository.UpdateAsync(theme);

    public async Task<bool> DeleteAsync(long id)
        => await _repository.DeleteAsync(id);
}