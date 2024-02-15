using Core.Domain.Dtos.Hotel;
using Core.Features.Hotel.Commands;
using Core.Features.Hotel.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Hotel.Commands;

public static class PutHotelApi
{
    public static void MapPutHotelApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPut("/id/{id:long}",
            async (
                [FromServices] IUpdateHotel updateHotel,
                [FromServices] ILogger<IGetHotels> logger,
                long id,
                [FromBody] UpdateHotelDto dto) =>
            {
                try
                {
                    if (id != dto.Id)
                        return Results.BadRequest("Hotel id must be equal");
                    
                    await updateHotel.Handle(dto);

                    return Results.NoContent();
                }
                catch (ArgumentException e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error updating the hotel");
                }
            });
    }
}