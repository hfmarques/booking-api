using WebApi.Apis.Customer.Commands;
using WebApi.Apis.Customer.Queries;

namespace WebApi.Apis.Customer;

public static class CustomerApiMap
{
    public static void AddApisFromCustomers(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers")
            .WithTags("Customer");

        app.MapGetCustomersApi(group);
        app.MapGetCustomerByIdApi(group);
        app.MapPostCustomerApi(group);
        app.MapPutCustomerApi(group);
    }
}