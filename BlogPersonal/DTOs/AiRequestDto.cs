using System.ComponentModel.DataAnnotations;

namespace BlogPersonal.DTOs;

public class AiRequestDto
{
    [Required]
    public string Text { get; set; } = string.Empty;
}