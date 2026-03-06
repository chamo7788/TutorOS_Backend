using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class FeePeriodConfiguration : IEntityTypeConfiguration<FeePeriod>
{
    public void Configure(EntityTypeBuilder<FeePeriod> builder)
    {
        builder.ToTable("fee_periods", "public");

        builder.Property(fp => fp.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(fp => fp.IsActive)
            .HasDefaultValue(true);

        builder.Property(fp => fp.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");
    }
}
