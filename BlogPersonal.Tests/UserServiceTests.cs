using BlogPersonal.Config;
using BlogPersonal.DTOs;
using BlogPersonal.Models;
using BlogPersonal.Repositories;
using BlogPersonal.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BlogPersonal.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly JwtService _jwtService;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "TestSecretKey2024!LongEnoughForHmacSha256@" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpiresInHours", "1" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _jwtService = new JwtService(configuration);
        _service = new UserService(_repositoryMock.Object, _jwtService);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "User 1", Email = "user1@test.com", Password = "hash1" },
            new() { Id = 2, Name = "User 2", Email = "user2@test.com", Password = "hash2" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        var user = new User { Id = 1, Name = "User 1", Email = "user1@test.com", Password = "hash" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("User 1", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserNotExists_ShouldReturnNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldReturnNull()
    {
        var dto = new UserDto { Name = "User", Email = "existing@test.com", Password = "123456" };
        _repositoryMock.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(true);

        var result = await _service.RegisterAsync(dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailIsNew_ShouldCreateUser()
    {
        var dto = new UserDto { Name = "New User", Email = "new@test.com", Password = "123456" };
        _repositoryMock.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _service.RegisterAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("New User", result.Name);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenCredentialsAreValid_ShouldReturnToken()
    {
        var password = "123456";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { Id = 1, Name = "User", Email = "user@test.com", Password = hashedPassword };

        _repositoryMock.Setup(r => r.GetByEmailAsync("user@test.com")).ReturnsAsync(user);

        var dto = new UserLoginDto { Email = "user@test.com", Password = password };
        var result = await _service.AuthenticateAsync(dto);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task AuthenticateAsync_WhenPasswordIsWrong_ShouldReturnNull()
    {
        var user = new User { Id = 1, Name = "User", Email = "user@test.com", Password = BCrypt.Net.BCrypt.HashPassword("correct") };
        _repositoryMock.Setup(r => r.GetByEmailAsync("user@test.com")).ReturnsAsync(user);

        var dto = new UserLoginDto { Email = "user@test.com", Password = "wrong" };
        var result = await _service.AuthenticateAsync(dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldReturnTrue()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        Assert.True(result);
    }
}