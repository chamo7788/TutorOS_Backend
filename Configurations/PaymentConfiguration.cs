using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments", "public");

        builder.HasIndex(p => p.StudentId)
            .HasDatabaseName("idx_payments_student");

        builder.HasIndex(p => p.FeePeriodId)
            .HasDatabaseName("idx_payments_period");

        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.PaymentStatus)
            .HasDefaultValue("completed");

        builder.Property(p => p.IsHalfMonth)
            .HasDefaultValue(false);

        builder.Property(p => p.PaidAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(p => p.Class)
            .WithMany()
            .HasForeignKey(p => p.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(p => p.FeePeriod)
            .WithMany()
            .HasForeignKey(p => p.FeePeriodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
