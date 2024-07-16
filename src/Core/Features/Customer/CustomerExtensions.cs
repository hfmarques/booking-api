using Core.Features.Customer.Commands;
using Core.Features.Customer.Queries;
using Core.Features.Hotel.Commands;
using Core.Features.Hotel.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Features.Customer
{
    public static class CustomerExtensions
    {
        public static void AddServicesFromCustomer(this IServiceCollection services)
        {
            services.AddTransient<IGetCustomers, GetCustomers>();
            services.AddTransient<IGetCustomerById, GetCustomerById>();
            services.AddTransient<IAddCustomer, AddCustomer>();
            services.AddTransient<IUpdateCustomer, UpdateCustomer>();
        }
    }
}