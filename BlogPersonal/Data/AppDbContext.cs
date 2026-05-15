using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}