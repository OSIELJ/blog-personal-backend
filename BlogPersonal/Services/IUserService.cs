using BlogPersonal.DTOs;
using BlogPersonal.Models;

namespace BlogPersonal.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(long id);
    Task<User?> RegisterAsync(UserDto dto);
    Task<User?> UpdateAsync(UserDto dto);
    Task<bool> DeleteAsync(long id);
    Task<UserLoginResponseDto?> AuthenticateAsync(UserLoginDto dto);
}