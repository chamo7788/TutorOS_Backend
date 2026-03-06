using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("enrollments", "public");

        builder.HasIndex(e => new { e.StudentId, e.ClassId })
            .IsUnique();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.EnrolledDate)
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_DATE");

        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.Status)
            .HasDefaultValue("active");
            
        builder.HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(e => e.Class)
            .WithMany()
            .HasForeignKey(e => e.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
