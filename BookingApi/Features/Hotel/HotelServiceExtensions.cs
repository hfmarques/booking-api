using BookingApi.Features.Hotel.Queries;

namespace BookingApi.Features.Hotel
{
    public static class HotelServiceExtensions
    {
        public static IServiceCollection AddServicesFromHotel(this IServiceCollection services)
        {
            services.AddTransient<GetHotels>();
            services.AddTransient<GetHotelRooms>();
            services.AddTransient<GetHotelById>();
            return services;
        }
    }
}