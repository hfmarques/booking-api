using Core.Domain.Entities;
using Core.Repositories;
using Data.Interceptors;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataExtensions
{
    public static void AddServicesFromData(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<UpdateDatabaseEntityInterceptor>();
        services.AddDbContext<DbContext, PostgresDbContext>((sp, options) =>
                options.UseNpgsql(connectionString)
                .AddInterceptors(sp.GetRequiredService<UpdateDatabaseEntityInterceptor>())
            );
        
        services.AddTransient<IQueryRepository<Room>, QueryRepository<Room>>();
        services.AddTransient<ICommandRepository<Room>, CommandRepository<Room>>();
        
        services.AddTransient<IQueryRepository<Hotel>, QueryRepository<Hotel>>();
        services.AddTransient<ICommandRepository<Hotel>, CommandRepository<Hotel>>();
        services.AddTransient<IHotelQueryRepository, HotelQueryRepository>();
        
        services.AddTransient<IQueryRepository<Customer>, QueryRepository<Customer>>();
        services.AddTransient<ICommandRepository<Customer>, CommandRepository<Customer>>();
        services.AddTransient<ICustomerQueryRepository, CustomerQueryRepository>();
    }
}