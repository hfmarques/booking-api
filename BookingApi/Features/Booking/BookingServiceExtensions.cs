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
            services.AddTransient<CancelBooking>();
            services.AddTransient<UpdateBooking>();
            services.AddTransient<GetFutureBookings>();
            services.AddTransient<GetActiveBookingsByRoomId>();
            services.AddTransient<IVerifyBookingAvailability, VerifyBookingAvailability>();
            return services;
        }
    }
}