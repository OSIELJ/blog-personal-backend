using BlogPersonal.Controllers;
using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogPersonal.Tests;

public class ThemeControllerTests
{
    private readonly Mock<IThemeService> _serviceMock;
    private readonly ThemeController _controller;

    public ThemeControllerTests()
    {
        _serviceMock = new Mock<IThemeService>();
        _controller = new ThemeController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Theme>());
        var result = await _controller.GetAll();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenThemeExists_ShouldReturnOk()
    {
        var theme = new Theme { Id = 1, Description = "Tech" };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(theme);
        var result = await _controller.GetById(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenThemeNotExists_ShouldReturnNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Theme?)null);
        var result = await _controller.GetById(99);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByDescription_ShouldReturnOk()
    {
        _serviceMock.Setup(s => s.GetByDescriptionAsync("Tech")).ReturnsAsync(new List<Theme>());
        var result = await _controller.GetByDescription("Tech");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        var theme = new Theme { Id = 1, Description = "Tech" };
        _serviceMock.Setup(s => s.CreateAsync(theme)).ReturnsAsync(theme);
        var result = await _controller.Create(theme);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task Update_WhenThemeExists_ShouldReturnOk()
    {
        var theme = new Theme { Id = 1, Description = "Updated" };
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Theme>())).ReturnsAsync(theme);
        var result = await _controller.Update(1, theme);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_WhenThemeNotExists_ShouldReturnNotFound()
    {
        var theme = new Theme { Id = 99, Description = "Theme" };
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Theme>())).ReturnsAsync((Theme?)null);
        var result = await _controller.Update(99, theme);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_WhenThemeExists_ShouldReturnNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _controller.Delete(1);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WhenThemeNotExists_ShouldReturnNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);
        var result = await _controller.Delete(99);
        Assert.IsType<NotFoundResult>(result);
    }
}