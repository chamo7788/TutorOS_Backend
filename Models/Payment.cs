using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("payments", Schema = "public")]
public class Payment
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

    [Required]
    [Column("amount", TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("payment_method")]
    public string PaymentMethod { get; set; } = null!;

    [MaxLength(20)]
    [Column("payment_status")]
    public string PaymentStatus { get; set; } = "completed";

    [Column("is_half_month")]
    public bool IsHalfMonth { get; set; }

    [MaxLength(100)]
    [Column("transaction_reference")]
    public string? TransactionReference { get; set; }

    [Column("paid_at")]
    public DateTimeOffset PaidAt { get; set; }

    [MaxLength(200)]
    [Column("received_by")]
    public string? ReceivedBy { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
