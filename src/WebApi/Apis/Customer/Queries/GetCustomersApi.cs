using Core.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Customer.Queries;

public static class GetCustomersApi
{
    public static void MapGetCustomersApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/",
            async (
                [FromServices] IGetCustomers getCustomers,
                [FromServices] ILogger<IGetCustomers> logger) =>
            {
                try
                {
                    var customers = await getCustomers.Handle();

                    return customers.Count == 0 ? Results.NotFound(new List<Core.Domain.Entities.Customer>()) : Results.Ok(customers);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting customers");
                }
            });
    }
}