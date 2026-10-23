Appusing System;

public class AppdbContext : DbContext
{
	public AppdbContext(DbContextOptions <AppdbContext> option) : base (option)
	{
	}

	public DbSet <Course> { get; set; }
}
