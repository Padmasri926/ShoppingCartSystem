using System.Linq.Expressions;

namespace EshoppingZone.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null,
                                         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                         string? includeString = null,
                                         bool disableTracking = true);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
    }
}