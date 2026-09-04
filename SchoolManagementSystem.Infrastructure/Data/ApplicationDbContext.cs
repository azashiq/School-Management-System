using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students => Set<Student>();

        public DbSet<Teacher> Teachers => Set<Teacher>();

        public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();

        public DbSet<Subject> Subjects => Set<Subject>();

        public DbSet<Attendance> Attendances => Set<Attendance>();

        public DbSet<Grade> Grades => Set<Grade>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Student>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Teacher>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<ClassRoom>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Subject>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Attendance>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<Grade>().HasQueryFilter(g => !g.IsDeleted);
        }

        public override int SaveChanges()
        {
            ApplySoftDeleteConvention();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDeleteConvention();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplySoftDeleteConvention()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
