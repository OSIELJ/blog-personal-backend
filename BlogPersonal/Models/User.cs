using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogPersonal.Models;

[Table("tb_users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [StringLength(5000)]
    [Column("photo")]
    public string? Photo { get; set; }

    [JsonIgnore]
    public ICollection<Post>? Posts { get; set; }
}