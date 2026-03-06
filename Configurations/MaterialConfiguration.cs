using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("materials", "public");

        builder.Property(m => m.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(m => m.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");
            
        builder.HasOne(m => m.Class)
            .WithMany()
            .HasForeignKey(m => m.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
