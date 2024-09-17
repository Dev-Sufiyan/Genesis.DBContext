using Microsoft.EntityFrameworkCore;

namespace Genesis.Repositories;
public class GenDBContext : DbContext
{
    public GenDBContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}