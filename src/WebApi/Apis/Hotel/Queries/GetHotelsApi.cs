using Core.Features.Hotel.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Hotel.Queries;

public static class GetHotelsApi
{
    public static void MapGetHotelsApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/",
            async (
                [FromServices] IGetHotels getHotels,
                [FromServices] ILogger<IGetHotels> logger) =>
            {
                try
                {
                    var hotels = await getHotels.Handle();

                    return hotels.Count == 0 ? Results.NotFound(new List<Core.Domain.Entities.Hotel>()) : Results.Ok(hotels);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting hotels");
                }
            });
    }
}
