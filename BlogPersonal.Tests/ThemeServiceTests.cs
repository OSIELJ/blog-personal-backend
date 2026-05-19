using BlogPersonal.Models;
using BlogPersonal.Repositories;
using BlogPersonal.Services;
using Moq;

namespace BlogPersonal.Tests;

public class ThemeServiceTests
{
    private readonly Mock<IThemeRepository> _repositoryMock;
    private readonly ThemeService _service;

    public ThemeServiceTests()
    {
        _repositoryMock = new Mock<IThemeRepository>();
        _service = new ThemeService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllThemes()
    {
        var themes = new List<Theme>
        {
            new() { Id = 1, Description = "Tech" },
            new() { Id = 2, Description = "Science" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(themes);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenThemeExists_ShouldReturnTheme()
    {
        var theme = new Theme { Id = 1, Description = "Tech" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(theme);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Tech", result.Description);
    }

    [Fact]
    public async Task GetByIdAsync_WhenThemeNotExists_ShouldReturnNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Theme?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedTheme()
    {
        var theme = new Theme { Description = "New Theme" };
        _repositoryMock.Setup(r => r.CreateAsync(theme)).ReturnsAsync(theme);

        var result = await _service.CreateAsync(theme);

        Assert.NotNull(result);
        Assert.Equal("New Theme", result.Description);
    }

    [Fact]
    public async Task UpdateAsync_WhenThemeExists_ShouldReturnUpdatedTheme()
    {
        var theme = new Theme { Id = 1, Description = "Updated" };
        _repositoryMock.Setup(r => r.UpdateAsync(theme)).ReturnsAsync(theme);

        var result = await _service.UpdateAsync(theme);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Description);
    }

    [Fact]
    public async Task DeleteAsync_WhenThemeExists_ShouldReturnTrue()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenThemeNotExists_ShouldReturnFalse()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _service.DeleteAsync(99);

        Assert.False(result);
    }
}