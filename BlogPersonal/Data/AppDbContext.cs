using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets will be added after Models are created
    // public DbSet<User> Users { get; set; }
    // public DbSet<Post> Posts { get; set; }
    // public DbSet<Theme> Themes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationships will be configured after Models are created
    }
}