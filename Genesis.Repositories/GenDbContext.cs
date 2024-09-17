using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Genesis.Repositories;
public class GenDBContext : DbContext
{
    public GenDBContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public new int SaveChanges()
    {
        return base.SaveChanges();
    }

    public void Update<T>(T extEntity, T newEntity) where T : class
    {
        base.Entry(extEntity).CurrentValues.SetValues(newEntity);
    }

    public Task SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public PropertyInfo GetPrimaryKeyProperty<T>() where T : class
    {
        var entityType = Model.FindEntityType(typeof(T));
        var keyProperty = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();
        return keyProperty?.PropertyInfo
            ?? throw new InvalidOperationException("No key property defined for the entity.");
    }
}