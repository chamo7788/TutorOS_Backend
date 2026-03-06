using Microsoft.EntityFrameworkCore;
using TutorOS.Api.Models;

namespace TutorOS.Api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Class> Classes { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<MaterialIssue> MaterialIssues { get; set; } = null!;
    public DbSet<FeePeriod> FeePeriods { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<StudentFeeStatus> StudentFeeStatuses { get; set; } = null!;
    public DbSet<Exam> Exams { get; set; } = null!;
    public DbSet<ExamResult> ExamResults { get; set; } = null!;
    public DbSet<Staff> Staff { get; set; } = null!;
    public DbSet<DailyReport> DailyReports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}