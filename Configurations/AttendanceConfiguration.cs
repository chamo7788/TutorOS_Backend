using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("attendance", "public");

        builder.HasIndex(a => a.ScanDate)
            .HasDatabaseName("idx_attendance_date");

        builder.HasIndex(a => a.StudentId)
            .HasDatabaseName("idx_attendance_student");

        builder.Property(a => a.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(a => a.ScanTime)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.Property(a => a.ScanDate)
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_DATE");

        builder.Property(a => a.IsLate)
            .HasDefaultValue(false);

        builder.Property(a => a.LateMinutes)
            .HasDefaultValue(0);

        builder.Property(a => a.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");
            
        builder.HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(a => a.Class)
            .WithMany()
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
