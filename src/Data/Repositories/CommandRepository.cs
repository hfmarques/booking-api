using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CommandRepository<T>(DbContext context) : ICommandRepository<T>
    where T : DatabaseEntity
{

    public async Task AddAsync(T entity)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        context.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        context.RemoveRange(entities);
        await context.SaveChangesAsync();
    }
    public async Task BeginTransactionAsync()
    {
        if (!context.Database.IsInMemory())
            await context.Database.BeginTransactionAsync();
    }
    public async Task CommitTransactionAsync()
    {
        if (!context.Database.IsInMemory())
            await context.Database.CommitTransactionAsync();
    }
    public async Task RollbackTransactionAsync()
    {
        if (!context.Database.IsInMemory())
            await context.Database.RollbackTransactionAsync();
    }
}