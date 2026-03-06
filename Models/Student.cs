using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorOS.Api.Models;

[Table("students", Schema = "public")]
public class Student
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("student_code")]
    public string StudentCode { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [MaxLength(255)]
    [Column("email")]
    public string? Email { get; set; }

    [MaxLength(20)]
    [Column("phone")]
    public string? Phone { get; set; }

    [MaxLength(20)]
    [Column("parent_phone")]
    public string? ParentPhone { get; set; }

    [MaxLength(200)]
    [Column("parent_name")]
    public string? ParentName { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [MaxLength(20)]
    [Column("grade")]
    public string? Grade { get; set; }

    [MaxLength(50)]
    [Column("stream")]
    public string? Stream { get; set; }

    [MaxLength(200)]
    [Column("school")]
    public string? School { get; set; }

    [Column("qr_code_data")]
    public string? QrCodeData { get; set; }

    [Column("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("special_note")]
    public string? SpecialNote { get; set; }

    [Column("joined_date")]
    public DateOnly JoinedDate { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
