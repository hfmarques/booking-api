using Core.Domain.Dtos.Booking;
using Core.Features.Booking.Commands;
using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Commands;

public static class PutBookingApi
{
    public static void MapPutBookingApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPut("/{id:long}",
            async (
                [FromServices] IUpdateBooking updateBooking,
                [FromServices] ILogger<IUpdateBooking> logger,
                long id,
                UpdateBookingDto dto) =>
            {
                try
                {
                    if (id != dto.Id)
                        return Results.BadRequest("Ids not match");
                    
                    await updateBooking.Handle(dto);

                    return Results.NoContent();
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error updating the booking");
                }
            });
    }
}