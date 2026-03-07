using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutorOS.Api.DTOs;

public class CreateStudentRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(20)]
    public string? ParentPhone { get; set; }

    [MaxLength(200)]
    public string? ParentName { get; set; }

    public string? Address { get; set; }

    [MaxLength(20)]
    public string? Grade { get; set; }

    [MaxLength(50)]
    public string? Stream { get; set; }

    [MaxLength(200)]
    public string? School { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? SpecialNote { get; set; }

    public DateOnly? JoinedDate { get; set; }

    public List<Guid>? ClassIds { get; set; }
}

public class StudentResponse
{
    public Guid Id { get; set; }
    public string StudentCode { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? ParentPhone { get; set; }
    public string? ParentName { get; set; }
    public string? Address { get; set; }
    public string? Grade { get; set; }
    public string? Stream { get; set; }
    public string? School { get; set; }
    public string? QrCodeData { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string Status { get; set; } = null!;
    public string? SpecialNote { get; set; }
    public DateOnly JoinedDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class StudentProgressResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid ExamId { get; set; }
    public string ExamTitle { get; set; } = null!;
    public DateOnly? ExamDate { get; set; }
    public string ExamType { get; set; } = null!;
    
    public string? McqAnswers { get; set; }
    public int? McqScore { get; set; }
    public int? TheoryScore { get; set; }
    public int? TotalScore { get; set; }
    public decimal? Percentage { get; set; }
    public int? ClassRank { get; set; }
    public int? IslandRank { get; set; }
    public string? WeakAreas { get; set; }
    public string? StrongAreas { get; set; }
    
    public DateTimeOffset? GradedAt { get; set; }
    public string? GradedBy { get; set; }
    public string? Notes { get; set; }
}

public class UnpaidStudentResponse
{
    public Guid StudentId { get; set; }
    public string StudentCode { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? ParentPhone { get; set; }
    
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public string Subject { get; set; } = null!;
    
    public Guid FeePeriodId { get; set; }
    public string FeePeriodName { get; set; } = null!;
    
    public decimal? AmountDue { get; set; }
    public DateOnly? DueDate { get; set; }
}
