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
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StudentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponse>>> GetStudents([FromQuery] string? search, [FromQuery] Guid? classId)
    {
        var query = _context.Students.AsQueryable();

        // Implicitly filter by active status
        query = query.Where(s => s.Status == "active");

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.ToLower();
            query = query.Where(s => 
                s.FirstName.ToLower().Contains(searchTerm) || 
                s.LastName.ToLower().Contains(searchTerm) || 
                s.StudentCode.ToLower().Contains(searchTerm));
        }

        if (classId.HasValue)
        {
            query = query.Where(s => _context.Enrollments.Any(e => e.StudentId == s.Id && e.ClassId == classId.Value && e.Status == "active"));
        }

        var students = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();

        var response = students.Select(s => MapToResponse(s)).ToList();

        return Ok(response);
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<StudentResponse>> CreateStudent([FromBody] CreateStudentRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Autogenerate unique student_code
            var randomChars = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            var studentCode = $"STU-{randomChars}";

            var student = new Student
            {
                Id = Guid.NewGuid(),
                StudentCode = studentCode,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                ParentPhone = request.ParentPhone,
                ParentName = request.ParentName,
                Address = request.Address,
                Grade = request.Grade,
                Stream = request.Stream,
                School = request.School,
                ProfileImageUrl = request.ProfileImageUrl,
                SpecialNote = request.SpecialNote,
                JoinedDate = request.JoinedDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                Status = "active",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            // Generate qr_code_data
            student.QrCodeData = $"https://app.tutoros.local/students/qr/{student.Id}";

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Enroll in classes if provided
            if (request.ClassIds != null && request.ClassIds.Any())
            {
                var enrollments = request.ClassIds.Select(cId => new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = student.Id,
                    ClassId = cId,
                    EnrolledDate = student.JoinedDate,
                    Status = "active",
                    CreatedAt = DateTimeOffset.UtcNow
                }).ToList();

                _context.Enrollments.AddRange(enrollments);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetStudents), new { search = student.StudentCode }, MapToResponse(student));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // GET: api/students/{studentId}/progress
    [HttpGet("{studentId}/progress")]
    public async Task<ActionResult<IEnumerable<StudentProgressResponse>>> GetStudentProgress(Guid studentId)
    {
        var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
        if (!studentExists)
        {
            return NotFound(new { Message = "Student not found" });
        }

        var progress = await _context.ExamResults
            .Include(er => er.Exam)
            .Where(er => er.StudentId == studentId)
            .OrderByDescending(er => er.Exam.ExamDate)
            .Select(er => new StudentProgressResponse
            {
                Id = er.Id,
                StudentId = er.StudentId,
                ExamId = er.ExamId,
                ExamTitle = er.Exam.Title,
                ExamDate = er.Exam.ExamDate,
                ExamType = er.Exam.ExamType,
                McqAnswers = er.McqAnswers,
                McqScore = er.McqScore,
                TheoryScore = er.TheoryScore,
                TotalScore = er.TotalScore,
                Percentage = er.Percentage,
                ClassRank = er.ClassRank,
                IslandRank = er.IslandRank,
                WeakAreas = er.WeakAreas,
                StrongAreas = er.StrongAreas,
                GradedAt = er.GradedAt,
                GradedBy = er.GradedBy,
                Notes = er.Notes
            })
            .ToListAsync();

        return Ok(progress);
    }

    // GET: api/students/unpaid
    [HttpGet("unpaid")]
    public async Task<ActionResult<IEnumerable<UnpaidStudentResponse>>> GetUnpaidStudents()
    {
        var unpaidStudents = await _context.StudentFeeStatuses
            .Include(sfs => sfs.Student)
            .Include(sfs => sfs.Class)
            .Include(sfs => sfs.FeePeriod)
            .Where(sfs => !sfs.IsPaid && sfs.FeePeriod.IsActive && sfs.Student.Status == "active")
            .Select(sfs => new UnpaidStudentResponse
            {
                StudentId = sfs.StudentId,
                StudentCode = sfs.Student.StudentCode,
                FirstName = sfs.Student.FirstName,
                LastName = sfs.Student.LastName,
                Phone = sfs.Student.Phone,
                ParentPhone = sfs.Student.ParentPhone,
                ClassId = sfs.ClassId,
                ClassName = sfs.Class.Name,
                Subject = sfs.Class.Subject,
                FeePeriodId = sfs.FeePeriodId,
                FeePeriodName = sfs.FeePeriod.Name,
                AmountDue = sfs.AmountDue,
                DueDate = sfs.DueDate
            })
            .OrderBy(u => u.DueDate)
            .ToListAsync();

        return Ok(unpaidStudents);
    }

    private static StudentResponse MapToResponse(Student student)
    {
        return new StudentResponse
        {
            Id = student.Id,
            StudentCode = student.StudentCode,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Phone = student.Phone,
            ParentPhone = student.ParentPhone,
            ParentName = student.ParentName,
            Address = student.Address,
            Grade = student.Grade,
            Stream = student.Stream,
            School = student.School,
            QrCodeData = student.QrCodeData,
            ProfileImageUrl = student.ProfileImageUrl,
            Status = student.Status,
            SpecialNote = student.SpecialNote,
            JoinedDate = student.JoinedDate,
            CreatedAt = student.CreatedAt,
            UpdatedAt = student.UpdatedAt
        };
    }
}
