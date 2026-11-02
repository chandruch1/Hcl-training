using Microsoft.EntityFrameworkCore;

namespace MovieTicketBooking.Data
{
    /// <summary>
    /// The central EF Core DbContext for the Movie Ticket Booking system.
    /// Entities (DbSet properties), Fluent API relationships, and constraints
    /// will be added in Phase 2 and Phase 3.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Constructor receives DbContextOptions from DI container.
        // This is how EF Core knows which database provider and connection string to use.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for all eight entities will be added in Phase 2.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configurations for relationships, indexes, and constraints
            // will be added in Phase 3.
        }
    }
}

