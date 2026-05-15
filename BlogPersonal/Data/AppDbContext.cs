using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Theme> Themes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Post -> Theme (N:1)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Theme)
            .WithMany(t => t.Posts)
            .HasForeignKey(p => p.ThemeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Post -> User (N:1)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}