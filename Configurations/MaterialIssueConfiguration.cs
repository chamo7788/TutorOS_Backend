using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class MaterialIssueConfiguration : IEntityTypeConfiguration<MaterialIssue>
{
    public void Configure(EntityTypeBuilder<MaterialIssue> builder)
    {
        builder.ToTable("material_issues", "public");

        builder.HasIndex(mi => new { mi.StudentId, mi.MaterialId })
            .IsUnique();

        builder.Property(mi => mi.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(mi => mi.IssuedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.HasOne(mi => mi.Student)
            .WithMany()
            .HasForeignKey(mi => mi.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(mi => mi.Material)
            .WithMany()
            .HasForeignKey(mi => mi.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
