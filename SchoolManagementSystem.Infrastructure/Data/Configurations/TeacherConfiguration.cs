using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.EmployeeId)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(t => t.EmployeeId)
                .IsUnique();

            builder.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(t => t.Qualification)
                .HasMaxLength(150);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.Phone)
                .HasMaxLength(20);
        }
    }
}
