using BlogPersonal.DTOs;
using BlogPersonal.Models;
using BlogPersonal.Repositories;

namespace BlogPersonal.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<User?> GetByIdAsync(long id)
        => await _repository.GetByIdAsync(id);

    public async Task<User?> RegisterAsync(UserDto dto)
    {
        if (await _repository.ExistsByEmailAsync(dto.Email))
            return null;

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Photo = dto.Photo
        };

        return await _repository.CreateAsync(user);
    }

    public async Task<User?> UpdateAsync(UserDto dto)
    {
        var existing = await _repository.GetByIdAsync(dto.Id);
        if (existing is null) return null;

        existing.Name = dto.Name;
        existing.Email = dto.Email;
        existing.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        existing.Photo = dto.Photo;

        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(long id)
        => await _repository.DeleteAsync(id);

    public async Task<UserLoginResponseDto?> AuthenticateAsync(UserLoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return null;

        return new UserLoginResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Photo = user.Photo,
            Token = string.Empty
        };
    }
}