using Core.Domain.Dtos.Hotel;
using Core.Features.Hotel.Commands;
using Core.Features.Hotel.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Hotel.Commands;

public static class PostHotelApi
{
    public static void MapPostHotelApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPost("/",
            async (
                [FromServices] IAddHotel addHotel,
                [FromServices] ILogger<IAddHotel> logger,
                [FromBody] AddHotelDto dto) =>
            {
                try
                {
                    var hotel = await addHotel.Handle(dto);

                    return Results.Created($"hotels/{hotel.Id}", hotel);
                }
                catch (ArgumentException e)
                {
                    logger.LogWarning("{Exception}", e.ToString());
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error adding the hotel");
                }
            });
    }
}