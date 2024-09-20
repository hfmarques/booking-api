using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Queries;

public static class GetUpcomingBookingsApi
{
    public static void MapGetUpcomingBookingsApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/upcoming/",
            async (
                [FromServices] IGetUpcomingBookings getBooking,
                [FromServices] ILogger<IGetUpcomingBookings> logger) =>
            {
                try
                {
                    var bookings = await getBooking.Handle();

                    return bookings.Count == 0 ? Results.NotFound() : Results.Ok(bookings);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting bookings");
                }
            });
    }
}
