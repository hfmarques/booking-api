using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataExtensions
{
    public static void AddServicesFromData(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DbContext, PostgresDbContext>(options => options.UseNpgsql(connectionString));
    }
}