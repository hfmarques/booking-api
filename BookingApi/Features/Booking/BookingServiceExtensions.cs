using BookingApi.Features.Booking.Queries;

namespace BookingApi.Features.Booking
{
    public static class BookingServiceExtensions
    {
        public static IServiceCollection AddServicesFromBooking(this IServiceCollection services)
        {
            services.AddTransient<GetBookings>();
            return services;
        }
    }
}