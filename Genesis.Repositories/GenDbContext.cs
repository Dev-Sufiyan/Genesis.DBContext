using Microsoft.EntityFrameworkCore;

namespace Genesis.Repositories;
public class GenDBContext : DbContext
{
    public GenDBContext(DbContextOptions<GenDBContext> options) : base(options) { }

}