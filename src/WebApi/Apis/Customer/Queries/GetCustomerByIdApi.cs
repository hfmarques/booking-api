using Core.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Customer.Queries;

public static class GetCustomerByIdApi
{
    public static void MapGetCustomerByIdApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}",
            async (
                [FromServices] IGetCustomerById getCustomerById,
                [FromServices] ILogger<IGetCustomerById> logger,
                long id) =>
            {
                try
                {
                    var customer = await getCustomerById.Handle(id);

                    return customer == null ? Results.NotFound() : Results.Ok(customer);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting customer");
                }
            });
    }
}
