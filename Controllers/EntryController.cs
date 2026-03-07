using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorOS.Api.DTOs;
using TutorOS.Api.Models;

namespace TutorOS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EntryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/entry/scan
    [HttpPost("scan")]
    public async Task<ActionResult<ScanResponse>> ProcessScan([FromBody] ScanRequest request)
    {
        if (!request.StudentId.HasValue && string.IsNullOrWhiteSpace(request.StudentCode))
        {
            return BadRequest(new { Message = "StudentId or StudentCode must be provided." });
        }

        // 1. Identify Class
        var cls = await _context.Classes.FindAsync(request.ClassId);
        if (cls == null || cls.Status != "active")
        {
            return Ok(new ScanResponse
            {
                Success = false,
                Message = "Active class not found.",
                LightColor = "red"
            });
        }

        var classDetails = new ScanClassDetails
        {
            Id = cls.Id,
            Name = cls.Name,
            Subject = cls.Subject,
            StartTime = cls.StartTime
        };

        // 2. Identify Student
        Student? student = null;
        if (request.StudentId.HasValue)
        {
            student = await _context.Students.FindAsync(request.StudentId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(request.StudentCode))
        {
            student = await _context.Students.FirstOrDefaultAsync(s => s.StudentCode == request.StudentCode);
        }

        if (student == null || student.Status != "active")
        {
            return Ok(new ScanResponse
            {
                Success = false,
                Message = "Active student not found.",
                LightColor = "red",
                Class = classDetails
            });
        }

        var studentDetails = new ScanStudentDetails
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            StudentCode = student.StudentCode,
            ProfileImageUrl = student.ProfileImageUrl
        };

        // 3. Verify Enrollment
        var isEnrolled = await _context.Enrollments.AnyAsync(e => 
            e.StudentId == student.Id && 
            e.ClassId == cls.Id && 
            e.Status == "active");

        if (!isEnrolled)
        {
            return Ok(new ScanResponse
            {
                Success = false,
                Message = "Student is not enrolled in this class.",
                LightColor = "red",
                Student = studentDetails,
                Class = classDetails
            });
        }

        // 4. Check for Recent Duplicate Scans (Debouncing)
        var recentScanWindow = DateTimeOffset.UtcNow.AddMinutes(-5);
        var recentScan = await _context.Attendances.AnyAsync(a => 
            a.StudentId == student.Id && 
            a.ClassId == cls.Id && 
            a.ScanTime >= recentScanWindow);

        if (recentScan)
        {
             return Ok(new ScanResponse
            {
                Success = true,
                Message = "Duplicate scan ignored. Student already marked present recently.",
                LightColor = "green", // Maintain green for rapid rescans
                Student = studentDetails,
                Class = classDetails
            });
        }

        // 5. Verify Fee Status
        var currentFeePeriod = await _context.FeePeriods
            .Where(fp => fp.IsActive)
            .OrderByDescending(fp => fp.StartDate)
            .FirstOrDefaultAsync();

        string lightColor = "green";
        string responseMessage = "Entry granted.";
        
        if (currentFeePeriod != null)
        {
            var feeStatus = await _context.StudentFeeStatuses.FirstOrDefaultAsync(sfs => 
                sfs.StudentId == student.Id && 
                sfs.ClassId == cls.Id && 
                sfs.FeePeriodId == currentFeePeriod.Id);

            if (feeStatus != null && !feeStatus.IsPaid)
            {
                if (feeStatus.DueDate.HasValue && feeStatus.DueDate.Value < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    lightColor = "red";
                    responseMessage = "Entry denied. Overdue fees.";
                }
                else
                {
                    lightColor = "yellow";
                    responseMessage = "Entry granted. Fees are due soon.";
                }
            }
        }

        // If red due to fees, we don't proceed with marking attendance usually, but let's decide:
        // For hardware, if red, gate doesn't open. Record a failed attempt? The requirements say "record attendance and dictate light color".
        // Let's record the scan regardless, but mark status based on success.
        var attendanceStatus = (lightColor == "red") ? "denied" : "present";

        // 6. Process Lateness
        bool isLate = false;
        int lateMinutes = 0;

        if (lightColor != "red" && cls.StartTime.HasValue)
        {
            // Get local time of scan (assuming server is UTC, we compare time components directly for now, or use configured timezone)
            var currentLocalTime = TimeOnly.FromDateTime(DateTime.Now); // Warning: depends on server timezone. Should Ideally pass timezone or use UTC offsets.
            var gracePeriodMinutes = 15;
            
            var expectedStart = cls.StartTime.Value;
            var timeDiff = currentLocalTime - expectedStart;

            if (timeDiff.TotalMinutes > gracePeriodMinutes)
            {
                isLate = true;
                lateMinutes = (int)timeDiff.TotalMinutes;
                
                // If it was green, late makes it yellow. If already yellow, keep yellow.
                if (lightColor == "green")
                {
                    lightColor = "yellow";
                    responseMessage = $"Entry granted. Student is {lateMinutes} minutes late.";
                }
                else
                {
                    responseMessage += $" Also, student is {lateMinutes} minutes late.";
                }
            }
        }

        // 7. Record Attendance
        var attendance = new Attendance
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            ClassId = cls.Id,
            ScanTime = DateTimeOffset.UtcNow,
            ScanDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = attendanceStatus,
            IsLate = isLate,
            LateMinutes = lateMinutes,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        return Ok(new ScanResponse
        {
            Success = lightColor != "red",
            Message = responseMessage,
            LightColor = lightColor,
            Student = studentDetails,
            Class = classDetails
        });
    }
}
