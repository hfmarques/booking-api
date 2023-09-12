using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IQueryRepository<T> where T : DatabaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindSingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
