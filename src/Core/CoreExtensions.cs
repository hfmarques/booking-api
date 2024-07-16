using Core.Features.Customer;
using Core.Features.Hotel;
using Core.Features.Room;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreExtensions
{
    public static void AddServicesFromCore(this IServiceCollection services)
    {
        services.AddServicesFromRoom();
        services.AddServicesFromHotel();
        services.AddServicesFromCustomer();
    }
}