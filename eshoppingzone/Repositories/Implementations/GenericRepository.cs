using EshoppingZone.Data;
using EshoppingZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EshoppingZone.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<T> _set;

        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

        public virtual async Task<IReadOnlyList<T>> ListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeString = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _set;
            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual void Update(T entity) => _set.Update(entity);
        public virtual void Delete(T entity) => _set.Remove(entity);
        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
