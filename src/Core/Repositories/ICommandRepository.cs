using Core.Domain.Entities;

namespace Core.Repositories;

public interface ICommandRepository<in T> where T : DatabaseEntity
{
    Task InsertAsync(T entity);
    Task InsertRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}