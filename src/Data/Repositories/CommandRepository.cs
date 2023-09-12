using System.Collections;
using System.Reflection;
using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CommandRepository<T> : ICommandRepository<T> where T : DatabaseEntity
{
    private readonly DbContext _context;
    private readonly HashSet<object> _processedEntities;

    public CommandRepository(DbContext context)
    {
        _context = context;
        _processedEntities = new HashSet<object>();
    }

    public async Task InsertAsync(T entity)
    {
        _processedEntities.Clear();

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        SetUpdateFields(entity);
        _processedEntities.Clear();

        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _context.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
    public async Task BeginTransactionAsync()
    {
        if (!_context.Database.IsInMemory())
            await _context.Database.BeginTransactionAsync();
    }
    public async Task CommitTransactionAsync()
    {
        if (!_context.Database.IsInMemory())
            await _context.Database.CommitTransactionAsync();
    }
    public async Task RollbackTransactionAsync()
    {
        if (!_context.Database.IsInMemory())
            await _context.Database.RollbackTransactionAsync();
    }

    private void SetUpdateFields(object? entity)
    {
        if (entity is not DatabaseEntity || _processedEntities.Contains(entity)) return;

        var properties =
            entity.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p is { CanRead: true, CanWrite: true });

        _processedEntities.Add(entity);

        foreach (var property in properties)
        {
            if (property.Name == nameof(DatabaseEntity.UpdatedAt))
            {
                property.SetValue(entity, DateTime.Now);
            }
            else if (property.PropertyType.IsGenericType &&
                     property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                if (property.GetValue(entity) is not IEnumerable list) continue;

                foreach (var item in list)
                {
                    SetUpdateFields(item);
                }
            }
            else if (!property.PropertyType.IsValueType &&
                     property.PropertyType != typeof(string))
            {
                var childEntity = property.GetValue(entity);

                SetUpdateFields(childEntity);
            }
        }
    }
}