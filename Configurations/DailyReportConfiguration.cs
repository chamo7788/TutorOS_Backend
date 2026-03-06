using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class DailyReportConfiguration : IEntityTypeConfiguration<DailyReport>
{
    public void Configure(EntityTypeBuilder<DailyReport> builder)
    {
        builder.ToTable("daily_reports", "public");

        builder.HasIndex(dr => dr.ReportDate)
            .IsUnique();

        builder.Property(dr => dr.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(dr => dr.TotalCash)
            .HasDefaultValue(0m);

        builder.Property(dr => dr.TotalOnline)
            .HasDefaultValue(0m);

        builder.Property(dr => dr.TotalStudentsAttended)
            .HasDefaultValue(0);

        builder.Property(dr => dr.UnpaidStudentsCount)
            .HasDefaultValue(0);

        builder.Property(dr => dr.LateStudentsCount)
            .HasDefaultValue(0);

        builder.Property(dr => dr.MaterialsIssuedCount)
            .HasDefaultValue(0);

        builder.Property(dr => dr.GeneratedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");
    }
}
