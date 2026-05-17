namespace BlogPersonal.DTOs;

public class UserLoginResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public string Token { get; set; } = string.Empty;
}