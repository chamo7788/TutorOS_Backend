using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutorOS.Api.DTOs;

public class CreateExamRequest
{
    [Required]
    public Guid ClassId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string ExamType { get; set; } = null!;

    public DateOnly? ExamDate { get; set; }

    [Required]
    public int TotalMarks { get; set; }

    public int? PassMarks { get; set; }

    public int? DurationMinutes { get; set; }

    public int? McqCount { get; set; }

    public int? TheoryCount { get; set; }

    public string? AnswerKey { get; set; }

    public string? TopicMapping { get; set; }
}

public class ExamResponse
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string ExamType { get; set; } = null!;
    public DateOnly? ExamDate { get; set; }
    public int TotalMarks { get; set; }
    public int? PassMarks { get; set; }
    public int? DurationMinutes { get; set; }
}

public class ExamResultResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = null!;
    public string StudentCode { get; set; } = null!;
    public int? McqScore { get; set; }
    public int? TheoryScore { get; set; }
    public int? TotalScore { get; set; }
    public decimal? Percentage { get; set; }
    public int? ClassRank { get; set; }
    public string? Notes { get; set; }
}

public class SubmitResultRequest
{
    [Required]
    public Guid StudentId { get; set; }

    public int? McqScore { get; set; }

    public int? TheoryScore { get; set; }

    public string? McqAnswers { get; set; }

    public string? WeakAreas { get; set; }

    public string? StrongAreas { get; set; }

    public string? Notes { get; set; }

    public string? GradedBy { get; set; }
}
