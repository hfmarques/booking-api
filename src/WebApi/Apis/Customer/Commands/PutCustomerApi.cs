using Core.Domain.Dtos.Customer;
using Core.Features.Customer.Commands;
using Core.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Customer.Commands;

public static class PutCustomerApi
{
    public static void MapPutCustomerApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPut("/{id:long}",
            async (
                [FromServices] IUpdateCustomer updateCustomer,
                [FromServices] ILogger<IUpdateCustomer> logger,
                long id,
                [FromBody] UpdateCustomerDto dto) =>
            {
                try
                {
                    if (id != dto.Id)
                        return Results.BadRequest("Customer id must be equal");
                    
                    await updateCustomer.Handle(dto);

                    return Results.NoContent();
                }
                catch (ArgumentException e)
                {
                    logger.LogWarning("{Exception}", e.ToString());
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error updating the customer");
                }
            });
    }
}