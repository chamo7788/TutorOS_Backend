using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("exams", Schema = "public")]
public class Exam
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
    [Column("exam_type")]
    public string ExamType { get; set; } = null!;

    [Column("exam_date")]
    public DateOnly? ExamDate { get; set; }

    [Required]
    [Column("total_marks")]
    public int TotalMarks { get; set; }

    [Column("pass_marks")]
    public int? PassMarks { get; set; }

    [Column("duration_minutes")]
    public int? DurationMinutes { get; set; }

    [Column("mcq_count")]
    public int? McqCount { get; set; }

    [Column("theory_count")]
    public int? TheoryCount { get; set; }

    [Column("answer_key", TypeName = "jsonb")]
    public string? AnswerKey { get; set; }

    [Column("topic_mapping", TypeName = "jsonb")]
    public string? TopicMapping { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
