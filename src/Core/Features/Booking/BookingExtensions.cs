using Core.Features.Booking.Commands;
using Core.Features.Booking.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Features.Booking
{
    public static class BookingExtensions
    {
        public static void AddServicesFromBooking(this IServiceCollection services)
        {
            services.AddTransient<IGetActiveBookingsByRoomId, GetActiveBookingsByRoomId>();
            services.AddTransient<IGetBookingById, GetBookingById>();
            services.AddTransient<IGetBookings, GetBookings>();
            services.AddTransient<IGetUpcomingBookings, GetUpcomingBookings>();

            services.AddTransient<IVerifyBookingOverlapping, VerifyBookingOverlapping>();
            services.AddTransient<IVerifyBookingAvailability, VerifyBookingAvailability>();
            services.AddTransient<IBookRoom, BookRoom>();
            services.AddTransient<IUpdateBooking, UpdateBooking>();
            services.AddTransient<ICancelBooking, CancelBooking>();
        }
    }
}