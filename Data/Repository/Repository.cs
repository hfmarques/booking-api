using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext Context;

    protected Repository(DbContext context)
    {
        Context = context;
    }

    public T? Get(long id)
    {
        return Context.Set<T>().Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return Context.Set<T>().Where(predicate);
    }

    public void Add(T entity)
    {
        Context.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        Context.AddRange(entities);
    }

    public void Remove(T entity)
    {
        Context.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        Context.RemoveRange(entities);
    }
}