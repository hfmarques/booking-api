using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Queries;

public static class GetBookingByIdApi
{
    public static void MapGetBookingByIdApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}",
            async (
                [FromServices] IGetBookingById getBooking,
                [FromServices] ILogger<IGetBookingById> logger,
                long id) =>
            {
                try
                {
                    var booking = await getBooking.Handle(id);

                    return booking == null ? Results.NotFound() : Results.Ok(booking);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting booking");
                }
            });
    }
}
