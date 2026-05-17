using System.ComponentModel.DataAnnotations;

namespace BlogPersonal.DTOs;

public class UserDto
{
    public long Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;

    public string? Photo { get; set; }
}