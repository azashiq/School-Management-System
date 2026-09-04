using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Data.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(a => a.IsPresent)
                .IsRequired();

            builder.Property(a => a.Remarks)
                .HasMaxLength(300);

            builder.HasIndex(a => new { a.StudentId, a.Date })
                .IsUnique();
        }
    }
}
