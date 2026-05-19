using BlogPersonal.Data;
using BlogPersonal.Models;
using BlogPersonal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Tests;

public class ThemeRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddTheme()
    {
        using var context = CreateContext();
        var repository = new ThemeRepository(context);
        var theme = new Theme { Description = "Tech" };

        var result = await repository.CreateAsync(theme);

        Assert.NotNull(result);
        Assert.Equal(1, await context.Themes.CountAsync());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllThemes()
    {
        using var context = CreateContext();
        context.Themes.AddRange(
            new Theme { Description = "Tech" },
            new Theme { Description = "Science" }
        );
        await context.SaveChangesAsync();

        var repository = new ThemeRepository(context);
        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnTheme()
    {
        using var context = CreateContext();
        var theme = new Theme { Description = "Tech" };
        context.Themes.Add(theme);
        await context.SaveChangesAsync();

        var repository = new ThemeRepository(context);
        var result = await repository.GetByIdAsync(theme.Id);

        Assert.NotNull(result);
        Assert.Equal("Tech", result.Description);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        using var context = CreateContext();
        var repository = new ThemeRepository(context);
        var result = await repository.GetByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByDescriptionAsync_ShouldReturnMatchingThemes()
    {
        using var context = CreateContext();
        context.Themes.AddRange(
            new Theme { Description = "Technology" },
            new Theme { Description = "Science" }
        );
        await context.SaveChangesAsync();

        var repository = new ThemeRepository(context);
        var result = await repository.GetByDescriptionAsync("tech");

        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateTheme()
    {
        using var context = CreateContext();
        var theme = new Theme { Description = "Tech" };
        context.Themes.Add(theme);
        await context.SaveChangesAsync();

        theme.Description = "Updated";
        var repository = new ThemeRepository(context);
        var result = await repository.UpdateAsync(theme);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Description);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        using var context = CreateContext();
        var repository = new ThemeRepository(context);
        var theme = new Theme { Id = 99, Description = "Theme" };
        var result = await repository.UpdateAsync(theme);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldRemoveTheme()
    {
        using var context = CreateContext();
        var theme = new Theme { Description = "Tech" };
        context.Themes.Add(theme);
        await context.SaveChangesAsync();

        var repository = new ThemeRepository(context);
        var result = await repository.DeleteAsync(theme.Id);

        Assert.True(result);
        Assert.Equal(0, await context.Themes.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        using var context = CreateContext();
        var repository = new ThemeRepository(context);
        var result = await repository.DeleteAsync(99);
        Assert.False(result);
    }
}