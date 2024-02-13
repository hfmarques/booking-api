using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class PostgresDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}