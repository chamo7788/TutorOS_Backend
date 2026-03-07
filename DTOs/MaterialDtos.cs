using System;
using System.ComponentModel.DataAnnotations;

namespace TutorOS.Api.DTOs;

public class MaterialResponse
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string MaterialType { get; set; } = null!; // Tute, Paper, etc.
    public DateOnly? IssueDate { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class IssueMaterialRequest
{
    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid MaterialId { get; set; }

    [MaxLength(200)]
    public string? IssuedBy { get; set; }

    public string? Notes { get; set; }
}
