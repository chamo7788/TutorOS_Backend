using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class StudentFeeStatusConfiguration : IEntityTypeConfiguration<StudentFeeStatus>
{
    public void Configure(EntityTypeBuilder<StudentFeeStatus> builder)
    {
        builder.ToTable("student_fee_status", "public");

        builder.HasIndex(sfs => new { sfs.StudentId, sfs.ClassId, sfs.FeePeriodId })
            .IsUnique();

        builder.HasIndex(sfs => sfs.StudentId)
            .HasDatabaseName("idx_fee_status_student");

        builder.Property(sfs => sfs.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(sfs => sfs.IsPaid)
            .HasDefaultValue(false);

        builder.Property(sfs => sfs.AmountPaid)
            .HasDefaultValue(0m);

        builder.Property(sfs => sfs.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.HasOne(sfs => sfs.Student)
            .WithMany()
            .HasForeignKey(sfs => sfs.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(sfs => sfs.Class)
            .WithMany()
            .HasForeignKey(sfs => sfs.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(sfs => sfs.FeePeriod)
            .WithMany()
            .HasForeignKey(sfs => sfs.FeePeriodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
