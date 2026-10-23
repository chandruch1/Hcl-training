using CoursesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Learner> Learners => Set<Learner>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.CourseCode);
            entity.Property(c => c.CourseCode).HasMaxLength(20);
            entity.Property(c => c.Title).HasMaxLength(150).IsRequired();
            entity.Property(c => c.Fee).HasPrecision(18, 2);
            entity.Ignore(c => c.AvailableSeats);
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.HasKey(l => l.LearnerId);
            entity.Property(l => l.LearnerName).HasMaxLength(120).IsRequired();
            entity.Property(l => l.Email).HasMaxLength(200).IsRequired();
            entity.HasIndex(l => l.Email).IsUnique();
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId);
            entity.Property(e => e.BaseFee).HasPrecision(18, 2);
            entity.Property(e => e.DiscountFee).HasPrecision(18, 2);
            entity.Property(e => e.FinalFee).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.PaymentStatus).HasConversion<string>().HasMaxLength(30);
            entity.HasOne<Learner>().WithMany().HasForeignKey(e => e.LearnerId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Course>().WithMany().HasForeignKey(e => e.CourseCode).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);
            entity.Property(p => p.Amount).HasPrecision(18, 2);
            entity.Property(p => p.Status).HasConversion<string>().HasMaxLength(30);
            entity.Property(p => p.Mode).HasConversion<string>().HasMaxLength(40);
            entity.HasOne<Enrollment>().WithMany().HasForeignKey(p => p.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
