using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class QueryRepository<T>(DbContext context) : IQueryRepository<T> where T : DatabaseEntity
{
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
    }
    public async Task<T?> GetByIdAsync(long id)
    {
        return await context.Set<T>().FindAsync(id);
    }
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(x => !x.IsDeleted).Where(predicate).ToListAsync();
    }
    public async Task<T?> FindSingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(x => !x.IsDeleted).Where(predicate).SingleOrDefaultAsync();
    }
}