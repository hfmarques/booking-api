using Core.Features.Room.Commands;
using Core.Features.Room.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Features.Room
{
    public static class RoomExtensions
    {
        public static void AddServicesFromRoom(this IServiceCollection services)
        {
            services.AddTransient<IGetRooms, GetRooms>();
            services.AddTransient<IGetRoomById, GetRoomById>();
            services.AddTransient<IAddRoom, AddRoom>();
            services.AddTransient<IUpdateRoom, UpdateRoom>();
        }
    }
}