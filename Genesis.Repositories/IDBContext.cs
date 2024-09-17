using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Genesis.Repositories
{
    public interface IDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        Task SaveChangesAsync();
        PropertyInfo GetPrimaryKeyProperty<T>() where T : class;
        void Update<T>(T extEntity, T newEntity) where T : class;
    }
}