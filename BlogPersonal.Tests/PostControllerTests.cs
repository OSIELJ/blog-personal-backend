using BlogPersonal.Controllers;
using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogPersonal.Tests;

public class PostControllerTests
{
    private readonly Mock<IPostService> _serviceMock;
    private readonly PostController _controller;

    public PostControllerTests()
    {
        _serviceMock = new Mock<IPostService>();
        _controller = new PostController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Post>());
        var result = await _controller.GetAll();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenPostExists_ShouldReturnOk()
    {
        var post = new Post { Id = 1, Title = "Post", Content = "Content" };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(post);
        var result = await _controller.GetById(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenPostNotExists_ShouldReturnNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Post?)null);
        var result = await _controller.GetById(99);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByTitle_ShouldReturnOk()
    {
        _serviceMock.Setup(s => s.GetByTitleAsync("Tech")).ReturnsAsync(new List<Post>());
        var result = await _controller.GetByTitle("Tech");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByFilter_ShouldReturnOk()
    {
        _serviceMock.Setup(s => s.GetByFilterAsync(null, null)).ReturnsAsync(new List<Post>());
        var result = await _controller.GetByFilter(null, null);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        var post = new Post { Id = 1, Title = "Post", Content = "Content" };
        _serviceMock.Setup(s => s.CreateAsync(post)).ReturnsAsync(post);
        var result = await _controller.Create(post);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task Update_WhenPostExists_ShouldReturnOk()
    {
        var post = new Post { Id = 1, Title = "Updated", Content = "Content" };
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(post);
        var result = await _controller.Update(1, post);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_WhenPostNotExists_ShouldReturnNotFound()
    {
        var post = new Post { Id = 99, Title = "Post", Content = "Content" };
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Post>())).ReturnsAsync((Post?)null);
        var result = await _controller.Update(99, post);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_WhenPostExists_ShouldReturnNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _controller.Delete(1);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WhenPostNotExists_ShouldReturnNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);
        var result = await _controller.Delete(99);
        Assert.IsType<NotFoundResult>(result);
    }
}