using System;
using System.ComponentModel.DataAnnotations;

namespace TutorOS.Api.DTOs;

public class ProcessPaymentRequest
{
    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid ClassId { get; set; }

    [Required]
    public Guid FeePeriodId { get; set; }

    [Required]
    [MaxLength(20)]
    public string PaymentType { get; set; } = "full"; // full, half, other

    public decimal? Amount { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "cash";

    public string? ReceivedBy { get; set; }
    public string? Notes { get; set; }
}

public class PaymentResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = null!;
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public Guid FeePeriodId { get; set; }
    public string FeePeriodName { get; set; } = null!;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public bool IsHalfMonth { get; set; }
    public DateTimeOffset PaidAt { get; set; }
    public string? ReceivedBy { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
