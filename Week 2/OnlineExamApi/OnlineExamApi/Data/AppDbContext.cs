using Microsoft.EntityFrameworkCore;
using OnlineExamApi.Models;

namespace OnlineExamApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; }
}