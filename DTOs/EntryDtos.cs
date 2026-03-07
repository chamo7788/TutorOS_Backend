using System;
using System.ComponentModel.DataAnnotations;

namespace TutorOS.Api.DTOs;

public class ScanRequest
{
    // The student's ID or Student Code can be used. Let's accept either.
    // However, usually a QR code contains the GUID or the student code. 
    // Assuming the frontend parses the QR data and sends the ID or code based on standard logic.
    public Guid? StudentId { get; set; }
    
    public string? StudentCode { get; set; }

    [Required]
    public Guid ClassId { get; set; }
}

public class ScanResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    // "green", "yellow", "red"
    public string LightColor { get; set; } = null!;
    
    public ScanStudentDetails? Student { get; set; }
    public ScanClassDetails? Class { get; set; }
}

public class ScanStudentDetails
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string StudentCode { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
}

public class ScanClassDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public TimeOnly? StartTime { get; set; }
}
