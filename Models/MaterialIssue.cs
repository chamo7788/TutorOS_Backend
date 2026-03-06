using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("material_issues", Schema = "public")]
public class MaterialIssue
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("student_id")]
    public Guid StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; } = null!;

    [Required]
    [Column("material_id")]
    public Guid MaterialId { get; set; }

    [ForeignKey(nameof(MaterialId))]
    public Material Material { get; set; } = null!;

    [Column("issued_at")]
    public DateTimeOffset IssuedAt { get; set; }

    [MaxLength(200)]
    [Column("issued_by")]
    public string? IssuedBy { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }
}
