using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("enrollments", Schema = "public")]
public class Enrollment
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
    [Column("class_id")]
    public Guid ClassId { get; set; }

    [ForeignKey(nameof(ClassId))]
    public Class Class { get; set; } = null!;

    [Column("enrolled_date")]
    public DateOnly EnrolledDate { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
