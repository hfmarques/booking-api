using Core.Domain.Dtos.Booking;
using Core.Features.Booking.Commands;
using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Commands;

public static class DeleteBookingApi
{
    public static void MapDeleteBookingApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapDelete("/{id:long}",
            async (
                [FromServices] ICancelBooking updateBooking,
                [FromServices] ILogger<ICancelBooking> logger,
                long id) =>
            {
                try
                {
                    await updateBooking.Handle(id);

                    return Results.NoContent();
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error cancelling the booking");
                }
            });
    }
}
