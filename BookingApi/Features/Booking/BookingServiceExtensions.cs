using BookingApi.Features.Booking.Commands;
using BookingApi.Features.Booking.Queries;

namespace BookingApi.Features.Booking
{
    public static class BookingServiceExtensions
    {
        public static IServiceCollection AddServicesFromBooking(this IServiceCollection services)
        {
            services.AddTransient<GetBookings>();
            services.AddTransient<GetBookingById>();
            services.AddTransient<BookRoom>();
            services.AddTransient<CancelBook>();
            services.AddTransient<UpdateBook>();
            services.AddTransient<GetFutureBookings>();
            services.AddTransient<GetActiveBookingsByRoomId>();
            return services;
        }
    }
}