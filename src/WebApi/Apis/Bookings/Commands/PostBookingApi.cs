using Core.Domain.Dtos.Booking;
using Core.Features.Booking.Commands;
using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Commands;

public static class PostBookingApi
{
    public static void MapPostBookingApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPost("/",
            async (
                [FromServices] IBookRoom bookRoom,
                [FromServices] ILogger<IBookRoom> logger,
                BookRoomDto dto) =>
            {
                try
                {
                    var booking = await bookRoom.Handle(dto);

                    return Results.Created($"bookings/id/{booking.Id}", booking);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error creating bookings");
                }
            });
    }
}