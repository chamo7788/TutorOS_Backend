using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("daily_reports", Schema = "public")]
public class DailyReport
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("report_date")]
    public DateOnly ReportDate { get; set; }

    [Column("total_cash", TypeName = "decimal(10,2)")]
    public decimal? TotalCash { get; set; }

    [Column("total_online", TypeName = "decimal(10,2)")]
    public decimal? TotalOnline { get; set; }

    [Column("total_students_attended")]
    public int? TotalStudentsAttended { get; set; }

    [Column("unpaid_students_count")]
    public int? UnpaidStudentsCount { get; set; }

    [Column("late_students_count")]
    public int? LateStudentsCount { get; set; }

    [Column("materials_issued_count")]
    public int? MaterialsIssuedCount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("generated_at")]
    public DateTimeOffset GeneratedAt { get; set; }
}
