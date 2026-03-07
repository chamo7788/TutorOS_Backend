using System;
using System.ComponentModel.DataAnnotations;

namespace TutorOS.Api.DTOs;

public class ClassResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Grade { get; set; }
    public string? Stream { get; set; }
    public string? TeacherName { get; set; }
    public string? DayOfWeek { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public decimal MonthlyFee { get; set; }
    public decimal? HalfMonthFee { get; set; }
    public string? Location { get; set; }
    public int? MaxStudents { get; set; }
    public string Status { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ClassStudentResponse
{
    public Guid EnrollmentId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentCode { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? ParentPhone { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly EnrolledDate { get; set; }
}

public class EnrollStudentRequest
{
    [Required]
    public Guid StudentId { get; set; }
}
