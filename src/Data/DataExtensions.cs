using Core.Domain.Entities;
using Core.Repositories;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataExtensions
{
    public static void AddServicesFromData(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DbContext, PostgresDbContext>(options => options.UseNpgsql(connectionString));
        
        services.AddTransient<IQueryRepository<Room>, QueryRepository<Room>>();
        services.AddTransient<ICommandRepository<Room>, CommandRepository<Room>>();
        
        services.AddTransient<IQueryRepository<Hotel>, QueryRepository<Hotel>>();
        services.AddTransient<ICommandRepository<Hotel>, CommandRepository<Hotel>>();
        services.AddTransient<IHotelQueryRepository, HotelQueryRepository>();
    }
}