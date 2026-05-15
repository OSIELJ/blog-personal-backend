using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPersonal.Models;

[Table("tb_themes")]
public class Theme
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public ICollection<Post>? Posts { get; set; }
}