using BlogPersonal.Models;
using BlogPersonal.Repositories;
using BlogPersonal.Services;
using BlogPersonal.Services.IA;
using BlogPersonal.DTOs;
using Moq;

namespace BlogPersonal.Tests;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _repositoryMock;
    private readonly Mock<IIAService> _iaServiceMock;
    private readonly PostService _service;

    public PostServiceTests()
    {
        _repositoryMock = new Mock<IPostRepository>();
        _iaServiceMock = new Mock<IIAService>();
        _service = new PostService(_repositoryMock.Object, _iaServiceMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPosts()
    {
        var posts = new List<Post>
        {
            new() { Id = 1, Title = "Post 1", Content = "Content 1" },
            new() { Id = 2, Title = "Post 2", Content = "Content 2" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(posts);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenPostExists_ShouldReturnPost()
    {
        var post = new Post { Id = 1, Title = "Post 1", Content = "Content 1" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(post);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Post 1", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPostNotExists_ShouldReturnNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Post?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldEnrichPostWithAI()
    {
        var post = new Post { Title = "AI Post", Content = "Some content about technology" };
        var aiResult = new AiResultDto { Summary = "Summary", Tags = "tech", Category = "Tech" };

        _iaServiceMock.Setup(s => s.GenerateSummaryAsync(post.Content)).ReturnsAsync(aiResult);
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Post>())).ReturnsAsync(post);

        var result = await _service.CreateAsync(post);

        Assert.NotNull(result);
        _iaServiceMock.Verify(s => s.GenerateSummaryAsync(post.Content), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenPostExists_ShouldReturnTrue()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenPostNotExists_ShouldReturnFalse()
    {
        _repositoryMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _service.DeleteAsync(99);

        Assert.False(result);
    }

    [Fact]
    public async Task GetByTitleAsync_ShouldReturnMatchingPosts()
    {
        var posts = new List<Post>
        {
            new() { Id = 1, Title = "Tech Post", Content = "Content" }
        };

        _repositoryMock.Setup(r => r.GetByTitleAsync("Tech")).ReturnsAsync(posts);

        var result = await _service.GetByTitleAsync("Tech");

        Assert.Single(result);
    }
}