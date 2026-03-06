using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("student_fee_status", Schema = "public")]
public class StudentFeeStatus
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

    [Required]
    [Column("fee_period_id")]
    public Guid FeePeriodId { get; set; }

    [ForeignKey(nameof(FeePeriodId))]
    public FeePeriod FeePeriod { get; set; } = null!;

    [Column("is_paid")]
    public bool IsPaid { get; set; }

    [Column("amount_due", TypeName = "decimal(10,2)")]
    public decimal? AmountDue { get; set; }

    [Column("amount_paid", TypeName = "decimal(10,2)")]
    public decimal AmountPaid { get; set; }

    [Column("due_date")]
    public DateOnly? DueDate { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
