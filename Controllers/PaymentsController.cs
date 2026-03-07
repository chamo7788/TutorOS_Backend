using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorOS.Api.DTOs;
using TutorOS.Api.Models;

namespace TutorOS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/payments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetRecentPayments()
    {
        var payments = await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .Include(p => p.FeePeriod)
            .OrderByDescending(p => p.PaidAt)
            .Take(50)
            .Select(p => MapToResponse(p))
            .ToListAsync();

        return Ok(payments);
    }

    // POST: api/payments
    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Validate Context
            var student = await _context.Students.FindAsync(request.StudentId);
            if (student == null || student.Status != "active")
            {
                return NotFound(new { Message = "Active student not found." });
            }

            var cls = await _context.Classes.FindAsync(request.ClassId);
            if (cls == null || cls.Status != "active")
            {
                return NotFound(new { Message = "Active class not found." });
            }

            var feePeriod = await _context.FeePeriods.FindAsync(request.FeePeriodId);
            if (feePeriod == null)
            {
                return NotFound(new { Message = "Fee period not found." });
            }

            // 2. Determine Amount
            decimal paymentAmount = 0;
            bool isHalfMonth = false;

            if (request.PaymentType.ToLower() == "full")
            {
                paymentAmount = cls.MonthlyFee;
            }
            else if (request.PaymentType.ToLower() == "half")
            {
                paymentAmount = cls.HalfMonthFee ?? (cls.MonthlyFee / 2);
                isHalfMonth = true;
            }
            else // "other"
            {
                if (!request.Amount.HasValue)
                {
                    return BadRequest(new { Message = "Amount must be provided for 'other' payment type." });
                }
                paymentAmount = request.Amount.Value;
            }

            // 3. Update StudentFeeStatus
            var feeStatus = await _context.StudentFeeStatuses
                .FirstOrDefaultAsync(sfs => sfs.StudentId == student.Id && 
                                           sfs.ClassId == cls.Id && 
                                           sfs.FeePeriodId == feePeriod.Id);

            if (feeStatus == null)
            {
                feeStatus = new StudentFeeStatus
                {
                    Id = Guid.NewGuid(),
                    StudentId = student.Id,
                    ClassId = cls.Id,
                    FeePeriodId = feePeriod.Id,
                    AmountPaid = 0,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                _context.StudentFeeStatuses.Add(feeStatus);
            }

            feeStatus.AmountPaid += paymentAmount;
            
            // Re-calculate AmountDue. Assuming total due is MonthlyFee for the period.
            // If it's a half month payment, maybe the total due was different? 
            // For now, let's keep it simple: Due = MonthlyFee - Paid.
            feeStatus.AmountDue = cls.MonthlyFee - feeStatus.AmountPaid;
            if (feeStatus.AmountDue < 0) feeStatus.AmountDue = 0;
            
            feeStatus.IsPaid = feeStatus.AmountDue <= 0;
            feeStatus.UpdatedAt = DateTimeOffset.UtcNow;

            // 4. Create Payment Record
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                StudentId = student.Id,
                ClassId = cls.Id,
                FeePeriodId = feePeriod.Id,
                Amount = paymentAmount,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = "completed",
                IsHalfMonth = isHalfMonth,
                PaidAt = DateTimeOffset.UtcNow,
                ReceivedBy = request.ReceivedBy,
                Notes = request.Notes,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // Reload to get joined entities for response
            var createdPayment = await _context.Payments
                .Include(p => p.Student)
                .Include(p => p.Class)
                .Include(p => p.FeePeriod)
                .FirstAsync(p => p.Id == payment.Id);

            return CreatedAtAction(nameof(GetRecentPayments), new { id = payment.Id }, MapToResponse(createdPayment));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { Message = "Error processing payment", Details = ex.Message });
        }
    }

    private static PaymentResponse MapToResponse(Payment payment)
    {
        return new PaymentResponse
        {
            Id = payment.Id,
            StudentId = payment.StudentId,
            StudentName = $"{payment.Student.FirstName} {payment.Student.LastName}",
            ClassId = payment.ClassId,
            ClassName = payment.Class.Name,
            FeePeriodId = payment.FeePeriodId,
            FeePeriodName = payment.FeePeriod.Name,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            PaymentStatus = payment.PaymentStatus,
            IsHalfMonth = payment.IsHalfMonth,
            PaidAt = payment.PaidAt,
            ReceivedBy = payment.ReceivedBy,
            Notes = payment.Notes,
            CreatedAt = payment.CreatedAt
        };
    }
}
