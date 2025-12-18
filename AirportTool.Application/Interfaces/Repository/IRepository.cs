using System.Linq.Expressions;

namespace AirportTool.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        // --- Get ---
        Task<T> GetByIdAsync(int id);
        Task<TResult> GetByIdAsync<TResult>(int id);

        Task<List<T>> GetAllAsync();
        Task<List<TResult>> GetAllAsync<TResult>();

        // --- Add ---
        Task<T> AddAsync(T entity);
        Task<TResult> AddAsync<TSource, TResult>(TSource source);

        // --- Update ---
        Task UpdateAsync(T entity);
        Task UpdateAsync<TSource>(TSource source, int id);

        // --- Delete ---
        Task DeleteAsync(int id);

        // --- Exists ---
        Task<bool> ExistsAsync(int id);

        // --- Find by predicate ---
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<List<TResult>> FindAsync<TResult>(Expression<Func<T, bool>> predicate);
    }
}
