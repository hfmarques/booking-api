using WebApi.Apis.Bookings.Queries;

namespace WebApi.Apis.Bookings;

public static class BookingsApiMap
{
    public static void AddApisFromBookings(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bookings")
            .WithTags("Bookings");

        app.MapGetBookingsApi(group);
        app.MapGetBookingByIdApi(group);
        app.MapGetUpcomingBookingsApi(group);
        app.MapGetActiveBookingsByRoomIdApi(group);
    }
}