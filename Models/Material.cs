using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("materials", Schema = "public")]
public class Material
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("class_id")]
    public Guid ClassId { get; set; }

    [ForeignKey(nameof(ClassId))]
    public Class Class { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column("material_type")]
    public string MaterialType { get; set; } = null!;

    [Column("issue_date")]
    public DateOnly? IssueDate { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
