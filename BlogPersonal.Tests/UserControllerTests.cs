using BlogPersonal.Controllers;
using BlogPersonal.DTOs;
using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogPersonal.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _serviceMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _serviceMock = new Mock<IUserService>();
        _controller = new UserController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<User>());
        var result = await _controller.GetAll();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenUserExists_ShouldReturnOk()
    {
        var user = new User { Id = 1, Name = "User", Email = "user@test.com", Password = "hash" };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);
        var result = await _controller.GetById(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenUserNotExists_ShouldReturnNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((User?)null);
        var result = await _controller.GetById(99);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Register_WhenEmailIsNew_ShouldReturnCreated()
    {
        var dto = new UserDto { Name = "User", Email = "new@test.com", Password = "123456" };
        var user = new User { Id = 1, Name = "User", Email = "new@test.com", Password = "hash" };
        _serviceMock.Setup(s => s.RegisterAsync(dto)).ReturnsAsync(user);
        var result = await _controller.Register(dto);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task Register_WhenEmailExists_ShouldReturnConflict()
    {
        var dto = new UserDto { Name = "User", Email = "existing@test.com", Password = "123456" };
        _serviceMock.Setup(s => s.RegisterAsync(dto)).ReturnsAsync((User?)null);
        var result = await _controller.Register(dto);
        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task Update_WhenUserExists_ShouldReturnOk()
    {
        var dto = new UserDto { Id = 1, Name = "Updated", Email = "user@test.com", Password = "123456" };
        var user = new User { Id = 1, Name = "Updated", Email = "user@test.com", Password = "hash" };
        _serviceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(user);
        var result = await _controller.Update(1, dto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Update_WhenUserNotExists_ShouldReturnNotFound()
    {
        var dto = new UserDto { Id = 99, Name = "User", Email = "user@test.com", Password = "123456" };
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<UserDto>())).ReturnsAsync((User?)null);
        var result = await _controller.Update(99, dto);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_WhenUserExists_ShouldReturnNoContent()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _controller.Delete(1);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WhenUserNotExists_ShouldReturnNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);
        var result = await _controller.Delete(99);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Login_WhenCredentialsValid_ShouldReturnOk()
    {
        var dto = new UserLoginDto { Email = "user@test.com", Password = "123456" };
        var response = new UserLoginResponseDto { Id = 1, Name = "User", Email = "user@test.com", Token = "token" };
        _serviceMock.Setup(s => s.AuthenticateAsync(dto)).ReturnsAsync(response);
        var result = await _controller.Login(dto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Login_WhenCredentialsInvalid_ShouldReturnUnauthorized()
    {
        var dto = new UserLoginDto { Email = "user@test.com", Password = "wrong" };
        _serviceMock.Setup(s => s.AuthenticateAsync(dto)).ReturnsAsync((UserLoginResponseDto?)null);
        var result = await _controller.Login(dto);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}