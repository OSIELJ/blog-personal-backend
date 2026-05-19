using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPersonal.Models;

[Table("tb_posts")]
public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // AI generated fields
    [Column("ai_summary")]
    public string? AiSummary { get; set; }

    [Column("ai_tags")]
    public string? AiTags { get; set; }

    [Column("ai_category")]
    public string? AiCategory { get; set; }

    // Relationships
    public long? ThemeId { get; set; }

    [ForeignKey("ThemeId")]
    public Theme? Theme { get; set; }

    public long? UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}