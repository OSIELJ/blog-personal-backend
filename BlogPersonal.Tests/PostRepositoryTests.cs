using BlogPersonal.Data;
using BlogPersonal.Models;
using BlogPersonal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Tests;

public class PostRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddPost()
    {
        using var context = CreateContext();
        var repository = new PostRepository(context);
        var post = new Post { Title = "Post", Content = "Content" };

        var result = await repository.CreateAsync(post);

        Assert.NotNull(result);
        Assert.Equal(1, await context.Posts.CountAsync());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPosts()
    {
        using var context = CreateContext();
        context.Posts.AddRange(
            new Post { Title = "Post1", Content = "Content1" },
            new Post { Title = "Post2", Content = "Content2" }
        );
        await context.SaveChangesAsync();

        var repository = new PostRepository(context);
        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnPost()
    {
        using var context = CreateContext();
        var post = new Post { Title = "Post", Content = "Content" };
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        var repository = new PostRepository(context);
        var result = await repository.GetByIdAsync(post.Id);

        Assert.NotNull(result);
        Assert.Equal("Post", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        using var context = CreateContext();
        var repository = new PostRepository(context);
        var result = await repository.GetByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByTitleAsync_ShouldReturnMatchingPosts()
    {
        using var context = CreateContext();
        context.Posts.AddRange(
            new Post { Title = "Tech Post", Content = "Content" },
            new Post { Title = "Science Post", Content = "Content" }
        );
        await context.SaveChangesAsync();

        var repository = new PostRepository(context);
        var result = await repository.GetByTitleAsync("tech");

        Assert.Single(result);
    }

    [Fact]
    public async Task GetByFilterAsync_ByUserId_ShouldReturnFilteredPosts()
    {
        using var context = CreateContext();
        var user = new User { Name = "User", Email = "user@test.com", Password = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        context.Posts.AddRange(
            new Post { Title = "Post1", Content = "Content", UserId = user.Id },
            new Post { Title = "Post2", Content = "Content" }
        );
        await context.SaveChangesAsync();

        var repository = new PostRepository(context);
        var result = await repository.GetByFilterAsync(user.Id, null);

        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdatePost()
    {
        using var context = CreateContext();
        var post = new Post { Title = "Post", Content = "Content" };
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        post.Title = "Updated";
        var repository = new PostRepository(context);
        var result = await repository.UpdateAsync(post);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        using var context = CreateContext();
        var repository = new PostRepository(context);
        var post = new Post { Id = 99, Title = "Post", Content = "Content" };
        var result = await repository.UpdateAsync(post);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldRemovePost()
    {
        using var context = CreateContext();
        var post = new Post { Title = "Post", Content = "Content" };
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        var repository = new PostRepository(context);
        var result = await repository.DeleteAsync(post.Id);

        Assert.True(result);
        Assert.Equal(0, await context.Posts.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        using var context = CreateContext();
        var repository = new PostRepository(context);
        var result = await repository.DeleteAsync(99);
        Assert.False(result);
    }
}