using BlogPersonal.Controllers;
using BlogPersonal.DTOs;
using BlogPersonal.Services.IA;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogPersonal.Tests;

public class IAControllerTests
{
    private readonly Mock<IIAService> _serviceMock;
    private readonly IAController _controller;

    public IAControllerTests()
    {
        _serviceMock = new Mock<IIAService>();
        _controller = new IAController(_serviceMock.Object);
    }

    [Fact]
    public async Task Summarize_WhenTextIsValid_ShouldReturnOk()
    {
        var request = new AiRequestDto { Text = "This is a blog post about technology." };
        var result = new AiResultDto { Summary = "Summary", Tags = "tech", Category = "Technology" };

        _serviceMock.Setup(s => s.GenerateSummaryAsync(request.Text)).ReturnsAsync(result);

        var response = await _controller.Summarize(request);

        Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public async Task Summarize_WhenTextIsEmpty_ShouldReturnBadRequest()
    {
        var request = new AiRequestDto { Text = "" };

        var response = await _controller.Summarize(request);

        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Summarize_WhenTextIsWhitespace_ShouldReturnBadRequest()
    {
        var request = new AiRequestDto { Text = "   " };

        var response = await _controller.Summarize(request);

        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Summarize_ShouldCallIAService()
    {
        var request = new AiRequestDto { Text = "Valid text content" };
        var result = new AiResultDto { Summary = "Summary", Tags = "tag", Category = "Category" };

        _serviceMock.Setup(s => s.GenerateSummaryAsync(request.Text)).ReturnsAsync(result);

        await _controller.Summarize(request);

        _serviceMock.Verify(s => s.GenerateSummaryAsync(request.Text), Times.Once);
    }
}