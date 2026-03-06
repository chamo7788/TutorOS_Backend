using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("classes", Schema = "public")]
public class Class
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("subject")]
    public string Subject { get; set; } = null!;

    [MaxLength(20)]
    [Column("grade")]
    public string? Grade { get; set; }

    [MaxLength(50)]
    [Column("stream")]
    public string? Stream { get; set; }

    [MaxLength(200)]
    [Column("teacher_name")]
    public string? TeacherName { get; set; }

    [MaxLength(20)]
    [Column("day_of_week")]
    public string? DayOfWeek { get; set; }

    [Column("start_time")]
    public TimeOnly? StartTime { get; set; }

    [Column("end_time")]
    public TimeOnly? EndTime { get; set; }

    [Required]
    [Column("monthly_fee")]
    public decimal MonthlyFee { get; set; }

    [Column("half_month_fee")]
    public decimal? HalfMonthFee { get; set; }

    [MaxLength(200)]
    [Column("location")]
    public string? Location { get; set; }

    [Column("max_students")]
    public int? MaxStudents { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
