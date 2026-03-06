using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students", "public");

        builder.HasIndex(s => s.StudentCode)
            .IsUnique();

        builder.HasIndex(s => s.QrCodeData)
            .IsUnique();

        builder.Property(s => s.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.JoinedDate)
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_DATE");

        builder.Property(s => s.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.Property(s => s.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(s => s.Status)
            .HasDefaultValue("active");
    }
}
