using BookingApi.Features.Room.Queries;

namespace BookingApi.Features.Room
{
    public static class RoomServiceExtensions
    {
        public static IServiceCollection AddServicesFromRoom(this IServiceCollection services)
        {
            services.AddTransient<GetRooms>();
            return services;
        }
    }
}