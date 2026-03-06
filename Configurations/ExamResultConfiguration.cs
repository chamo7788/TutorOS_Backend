using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TutorOS.Api.Models;

namespace TutorOS.Api.Configurations;

public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
{
    public void Configure(EntityTypeBuilder<ExamResult> builder)
    {
        builder.ToTable("exam_results", "public");

        builder.HasIndex(er => new { er.StudentId, er.ExamId })
            .IsUnique();

        builder.HasIndex(er => er.StudentId)
            .HasDatabaseName("idx_exam_results_student");

        builder.HasIndex(er => er.ExamId)
            .HasDatabaseName("idx_exam_results_exam");

        builder.Property(er => er.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(er => er.GradedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW()");

        builder.Property(er => er.McqAnswers)
            .HasColumnType("jsonb");

        builder.Property(er => er.WeakAreas)
            .HasColumnType("jsonb");

        builder.Property(er => er.StrongAreas)
            .HasColumnType("jsonb");

        builder.HasOne(er => er.Student)
            .WithMany()
            .HasForeignKey(er => er.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(er => er.Exam)
            .WithMany()
            .HasForeignKey(er => er.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
