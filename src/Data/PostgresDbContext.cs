using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}