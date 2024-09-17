using Genesis.Models.DTO;

namespace Genesis.Repositories
{
    public interface IRepositoriesBase<T> where T : class
    {
        Task<IEnumerable<T>> GetRecordsAsync(List<SearchParam> searchParams);
        Task<T> GetByPKAsync(object keyValue);
        Task<bool> IsEntityExistAsync(T entity);
        Task AddAsync(T entity);
        Task SaveRecordsAsync(IEnumerable<T> records);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteByPKAsync(object keyValue);
    }
}