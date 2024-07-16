using Core.Domain.Dtos.Customer;
using Core.Features.Customer.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Customer.Commands;

public static class PostCustomerApi
{
    public static void MapPostCustomerApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPost("/",
            async (
                [FromServices] IAddCustomer addCustomer,
                [FromServices] ILogger<IAddCustomer> logger,
                [FromBody] AddCustomerDto dto) =>
            {
                try
                {
                    var customer = await addCustomer.Handle(dto);

                    return Results.Created($"customers/id/{customer.Id}", customer);
                }
                catch (ArgumentException e)
                {
                    logger.LogWarning("{Exception}", e.ToString());
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error adding the customer");
                }
            });
    }
}