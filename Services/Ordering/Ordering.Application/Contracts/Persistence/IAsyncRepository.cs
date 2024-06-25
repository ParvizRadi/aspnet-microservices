using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderedBy = null,
                string includeString = null,
                bool disableTracking = true
            );

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderedBy = null,
                List<Expression<Func<T, object>>> incluedes = null,
                bool disableTracking = true
            );

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);
    }
}
