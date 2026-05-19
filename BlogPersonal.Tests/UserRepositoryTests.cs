using BlogPersonal.Data;
using BlogPersonal.Models;
using BlogPersonal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Tests;

public class UserRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddUser()
    {
        using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = new User { Name = "User", Email = "user@test.com", Password = "hash" };

        var result = await repository.CreateAsync(user);

        Assert.NotNull(result);
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Name = "User1", Email = "user1@test.com", Password = "hash" },
            new User { Name = "User2", Email = "user2@test.com", Password = "hash" }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnUser()
    {
        using var context = CreateContext();
        var user = new User { Name = "User", Email = "user@test.com", Password = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var result = await repository.GetByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal("User", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        using var context = CreateContext();
        var repository = new UserRepository(context);
        var result = await repository.GetByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenExists_ShouldReturnUser()
    {
        using var context = CreateContext();
        var user = new User { Name = "User", Email = "user@test.com", Password = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var result = await repository.GetByEmailAsync("user@test.com");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task ExistsByEmailAsync_WhenExists_ShouldReturnTrue()
    {
        using var context = CreateContext();
        context.Users.Add(new User { Name = "User", Email = "user@test.com", Password = "hash" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var result = await repository.ExistsByEmailAsync("user@test.com");

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByEmailAsync_WhenNotExists_ShouldReturnFalse()
    {
        using var context = CreateContext();
        var repository = new UserRepository(context);
        var result = await repository.ExistsByEmailAsync("notexists@test.com");
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateUser()
    {
        using var context = CreateContext();
        var user = new User { Name = "User", Email = "user@test.com", Password = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        user.Name = "Updated";
        var repository = new UserRepository(context);
        var result = await repository.UpdateAsync(user);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldRemoveUser()
    {
        using var context = CreateContext();
        var user = new User { Name = "User", Email = "user@test.com", Password = "hash" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var result = await repository.DeleteAsync(user.Id);

        Assert.True(result);
        Assert.Equal(0, await context.Users.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        using var context = CreateContext();
        var repository = new UserRepository(context);
        var result = await repository.DeleteAsync(99);
        Assert.False(result);
    }
}