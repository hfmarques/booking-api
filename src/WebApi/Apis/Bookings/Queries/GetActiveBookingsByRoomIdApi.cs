using Core.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Bookings.Queries;

public static class GetActiveBookingsByRoomIdApi
{
    public static void MapGetActiveBookingsByRoomIdApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/active/{roomId:long}",
            async (
                [FromServices] IGetActiveBookingsByRoomId getBooking,
                [FromServices] ILogger<IGetActiveBookingsByRoomId> logger,
                long roomId) =>
            {
                try
                {
                    var bookings = await getBooking.Handle(roomId);

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
