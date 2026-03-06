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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}