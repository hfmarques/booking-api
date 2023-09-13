using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class QueryRepository<T> : IQueryRepository<T> where T : DatabaseEntity
{
    private readonly DbContext _context;

    public QueryRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    public async Task<T?> GetByIdAsync(long id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }
    public async Task<T?> FindSingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).SingleOrDefaultAsync();
    }
}