using Employees.Models;
using Microsoft.EntityFrameworkCore;
using Employees.Enums;

namespace Employees.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAuditLog> EmployeeAuditLogs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(50);
                entity.Property(d => d.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DateOfJoining).HasColumnType("date");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.Department)
               .WithMany()
               .HasForeignKey(e => e.DepartmentId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EmployeeAuditLog>(entity =>
            {
                entity.HasKey(a => a.AuditId);

                entity.Property(a => a.Action)
                      .IsRequired();

                entity.Property(a => a.ChangedAt)
                      .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

                entity.HasIndex(e => e.Email);

                entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

                entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(100);

                entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasMaxLength(20);

                entity.Property(u => u.CreatedAt)
                .IsRequired();
            }
            );
        }


    }
}
