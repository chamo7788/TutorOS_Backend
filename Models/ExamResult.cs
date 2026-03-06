using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("exam_results", Schema = "public")]
public class ExamResult
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
    [Column("exam_id")]
    public Guid ExamId { get; set; }

    [ForeignKey(nameof(ExamId))]
    public Exam Exam { get; set; } = null!;

    [Column("mcq_answers", TypeName = "jsonb")]
    public string? McqAnswers { get; set; }

    [Column("mcq_score")]
    public int? McqScore { get; set; }

    [Column("theory_score")]
    public int? TheoryScore { get; set; }

    [Column("total_score")]
    public int? TotalScore { get; set; }

    [Column("percentage", TypeName = "decimal(5,2)")]
    public decimal? Percentage { get; set; }

    [Column("class_rank")]
    public int? ClassRank { get; set; }

    [Column("island_rank")]
    public int? IslandRank { get; set; }

    [Column("weak_areas", TypeName = "jsonb")]
    public string? WeakAreas { get; set; }

    [Column("strong_areas", TypeName = "jsonb")]
    public string? StrongAreas { get; set; }

    [Column("graded_at")]
    public DateTimeOffset? GradedAt { get; set; }

    [MaxLength(200)]
    [Column("graded_by")]
    public string? GradedBy { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }
}
