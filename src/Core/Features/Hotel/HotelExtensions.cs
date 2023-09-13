using Core.Features.Hotel.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Features.Hotel
{
    public static class HotelExtensions
    {
        public static void AddServicesFromHotel(this IServiceCollection services)
        {
            services.AddTransient<IGetHotels, GetHotels>();
            services.AddTransient<IGetHotelById, IGetHotelById>();
            services.AddTransient<IGetHotelWithRoomsById, IGetHotelWithRoomsById>();
        }
    }
}