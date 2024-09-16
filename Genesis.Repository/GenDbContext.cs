using Microsoft.EntityFrameworkCore;

namespace Genie.Counter.DBContext;
public class GenDBContext : DbContext
{
    public GenDBContext(DbContextOptions<GenDBContext> options) : base(options) { }

}