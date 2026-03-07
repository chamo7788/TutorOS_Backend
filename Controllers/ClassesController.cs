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
public class ClassesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClassesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/classes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassResponse>>> GetClasses()
    {
        var classes = await _context.Classes
            .Where(c => c.Status == "active")
            .OrderBy(c => c.Name)
            .Select(c => MapToResponse(c))
            .ToListAsync();

        return Ok(classes);
    }

    // GET: api/classes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ClassResponse>> GetClass(Guid id)
    {
        var cls = await _context.Classes.FindAsync(id);

        if (cls == null)
        {
            return NotFound(new { Message = "Class not found" });
        }

        return Ok(MapToResponse(cls));
    }

    // GET: api/classes/{id}/students
    [HttpGet("{id}/students")]
    public async Task<ActionResult<IEnumerable<ClassStudentResponse>>> GetEnrolledStudents(Guid id)
    {
        var classExists = await _context.Classes.AnyAsync(c => c.Id == id);
        if (!classExists)
        {
            return NotFound(new { Message = "Class not found" });
        }

        var students = await _context.Enrollments
            .Include(e => e.Student)
            .Where(e => e.ClassId == id && e.Status == "active" && e.Student.Status == "active")
            .Select(e => new ClassStudentResponse
            {
                EnrollmentId = e.Id,
                StudentId = e.StudentId,
                StudentCode = e.Student.StudentCode,
                FirstName = e.Student.FirstName,
                LastName = e.Student.LastName,
                Phone = e.Student.Phone,
                ParentPhone = e.Student.ParentPhone,
                ProfileImageUrl = e.Student.ProfileImageUrl,
                EnrolledDate = e.EnrolledDate
            })
            .OrderBy(s => s.FirstName).ThenBy(s => s.LastName)
            .ToListAsync();

        return Ok(students);
    }

    // POST: api/classes/{id}/enroll
    [HttpPost("{id}/enroll")]
    public async Task<ActionResult> EnrollStudent(Guid id, [FromBody] EnrollStudentRequest request)
    {
        var cls = await _context.Classes.FindAsync(id);
        if (cls == null || cls.Status != "active")
        {
            return NotFound(new { Message = "Active class not found" });
        }

        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null || student.Status != "active")
        {
            return NotFound(new { Message = "Active student not found" });
        }

        var enrollmentExists = await _context.Enrollments
            .AnyAsync(e => e.ClassId == id && e.StudentId == request.StudentId && e.Status == "active");

        if (enrollmentExists)
        {
            return BadRequest(new { Message = "Student is already enrolled in this class" });
        }

        // Check capacity if applicable
        if (cls.MaxStudents.HasValue)
        {
            var currentEnrolledCount = await _context.Enrollments
                .CountAsync(e => e.ClassId == id && e.Status == "active");
                
            if (currentEnrolledCount >= cls.MaxStudents.Value)
            {
                return BadRequest(new { Message = "Class has reached its maximum capacity" });
            }
        }

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            ClassId = id,
            EnrolledDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "active",
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Student successfully enrolled", EnrollmentId = enrollment.Id });
    }

    private static ClassResponse MapToResponse(Class cls)
    {
        return new ClassResponse
        {
            Id = cls.Id,
            Name = cls.Name,
            Subject = cls.Subject,
            Grade = cls.Grade,
            Stream = cls.Stream,
            TeacherName = cls.TeacherName,
            DayOfWeek = cls.DayOfWeek,
            StartTime = cls.StartTime,
            EndTime = cls.EndTime,
            MonthlyFee = cls.MonthlyFee,
            HalfMonthFee = cls.HalfMonthFee,
            Location = cls.Location,
            MaxStudents = cls.MaxStudents,
            Status = cls.Status,
            CreatedAt = cls.CreatedAt,
            UpdatedAt = cls.UpdatedAt
        };
    }
}
