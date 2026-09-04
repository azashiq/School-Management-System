using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grades");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Score)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(g => g.ExamType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(g => g.ExamDate)
                .IsRequired()
                .HasColumnType("date");

            builder.HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
