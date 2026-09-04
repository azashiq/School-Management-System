using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(s => s.RegistrationNumber)
                .IsUnique();

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(s => s.Address)
                .HasMaxLength(300);

            builder.Property(s => s.Gender)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(s => s.ClassRoom)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Attendances)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
