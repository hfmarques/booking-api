using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Queries;

public static class GetBookingsApi
{
    public static void MapGetBookingsApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/",
            async (
                [FromServices] IGetBookings getBooking,
                [FromServices] ILogger<IGetBookings> logger) =>
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
