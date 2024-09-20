using Core.Features.Hotel.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Hotel.Queries;

public static class GetHotelByIdApi
{
    public static void MapGetHotelByIdApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}",
            async (
                [FromServices] IGetHotelById getHotelById,
                [FromServices] ILogger<IGetHotelById> logger,
                long id) =>
            {
                try
                {
                    var hotel = await getHotelById.Handle(id);

                    return hotel == null ? Results.NotFound() : Results.Ok(hotel);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting hotel");
                }
            });
    }
}
